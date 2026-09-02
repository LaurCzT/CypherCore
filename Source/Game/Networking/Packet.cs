// Copyright (c) CypherCore <http://github.com/CypherCore> All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE. See LICENSE file in the project root for full license information.

using Framework.Constants;
using Framework.IO;
using Game.Entities;
using System;

namespace Game.Networking
{
    public abstract class ClientPacket : IDisposable
    {
        protected ClientPacket(WorldPacket worldPacket)
        {
            _worldPacket = worldPacket;
        }

        public abstract void Read();

        public void Dispose()
        {
            _worldPacket.Dispose();
        }

        public ClientOpcodes GetOpcode() { return (ClientOpcodes)_worldPacket.GetOpcode(); }

        public void LogPacket(WorldSession session)
        {
            string sender = session != null ? session.GetPlayerInfo() : "Unknown IP";
            Log.outDebug(LogFilter.Network, $"Received ClientOpcode: {GetOpcode()} From: {sender}");
        }

        protected WorldPacket _worldPacket;
    }

    public abstract class ServerPacket
    {
        protected ServerPacket(ServerOpcodes opcode)
        {
            connectionType = ConnectionType.Realm;
            _worldPacket = new WorldPacket(opcode);
        }

        protected ServerPacket(ServerOpcodes opcode, ConnectionType type = ConnectionType.Realm)
        {
            connectionType = type;
            _worldPacket = new WorldPacket(opcode);
        }

        protected void TakeBufferAndDestroy(WorldPacket packet)
        {
            buffer = packet.GetData();
            packet.Dispose();
        }

        public void Clear()
        {
            _worldPacket.Clear();
            buffer = null;
        }

        public ServerOpcodes GetOpcode()
        {
            return (ServerOpcodes)_worldPacket.GetOpcode();
        }

        public byte[] GetData()
        {
            return buffer;
        }

        public void UpdateData()
        {
            buffer = _worldPacket.GetData();
        }

        public void LogPacket(WorldSession session)
        {
            string receiver = session != null ? session.GetPlayerInfo() : string.Empty;
            Log.outDebug(LogFilter.Network, $"Sent ServerOpcode: {GetOpcode()} To: {receiver}");
        }

        public abstract void Write();

        public void WritePacketData()
        {
            if (buffer != null)
                return;

            // Several senders pre-serialise by calling Write() directly, so one packet
            // object can be handed to many players without re-encoding it per recipient
            // (ChatPacketSender, MultiplePacketSender, the quest info response, and the
            // loot / party-kill / world-state builders all do this).
            //
            // Write() fills _worldPacket but does NOT publish `buffer`, so this method
            // used to see buffer == null and call Write() a SECOND time -- appending a
            // duplicate payload to the same stream. The packet went out at exactly twice
            // its real length with the tail repeated, e.g. SMSG_CHAT at 108 bytes instead
            // of 54, and the client discarded it. Publish what is already there instead.
            if (_worldPacket.GetSize() > 0)
            {
                buffer = _worldPacket.GetData();
                _worldPacket.Dispose();
                return;
            }

            Write();

            buffer = _worldPacket.GetData();
            _worldPacket.Dispose();
        }

        public ConnectionType GetConnection() { return connectionType; }

        byte[] buffer;
        ConnectionType connectionType;
        protected WorldPacket _worldPacket;
    }

    public class WorldPacket : ByteBuffer
    {
        public WorldPacket(ServerOpcodes opcode = ServerOpcodes.None)
        {
            this.opcode = (uint)opcode;
        }

        public WorldPacket(ServerOpcodes opcode, byte[] data) : base(data, true)
        {
            this.opcode = (uint)opcode;
        }

        public WorldPacket(byte[] data) : base(data)
        {
            opcode = ReadUInt16();
        }

        public ObjectGuid ReadPackedGuid()
        {
            var loLength = ReadUInt8();
            var hiLength = ReadUInt8();
            var low = ReadPackedInt64(loLength);
            return new ObjectGuid(ReadPackedInt64(hiLength), low);
        }

        private long ReadPackedInt64(byte length)
        {
            if (length == 0)
                return 0;

            long guid = 0;

            for (var i = 0; i < 8; i++)
                if ((1 << i & length) != 0)
                    guid |= (long)ReadUInt8() << (i * 8);

            return guid;
        }

        public Position ReadPosition()
        {
            return new Position(ReadFloat(), ReadFloat(), ReadFloat());
        }

        public void WritePackedGuid(ObjectGuid guid)
        {
            if (guid.IsEmpty())
            {
                WriteUInt8(0);
                WriteUInt8(0);
                return;
            }

            byte lowMask, highMask;
            byte[] lowPacked, highPacked;

            var loSize = PackInt64(guid.GetLowValue(), out lowMask, out lowPacked);
            var hiSize = PackInt64(guid.GetHighValue(), out highMask, out highPacked);

            WriteUInt8(lowMask);
            WriteUInt8(highMask);
            WriteBytes(lowPacked, loSize);
            WriteBytes(highPacked, hiSize);
        }

        public void WritePackedInt64(long guid)
        {
            byte mask;
            byte[] packed;
            var packedSize = PackInt64(guid, out mask, out packed);

            WriteUInt8(mask);
            WriteBytes(packed, packedSize);
        }

        int PackInt64(long value, out byte mask, out byte[] result)
        {
            int resultSize = 0;
            mask = 0;
            result = new byte[8];

            for (byte i = 0; value != 0; ++i)
            {
                if ((value & 0xFF) != 0)
                {
                    mask |= (byte)(1 << i);
                    result[resultSize++] = (byte)(value & 0xFF);
                }

                value = (long)((ulong)value >> 8);
            }

            return resultSize;
        }

        public void WriteBytes(WorldPacket data)
        {
            FlushBits();
            WriteBytes(data.GetData());
        }

        public void WriteXYZ(Position pos)
        {
            if (pos == null)
                return;

            float x, y, z;
            pos.GetPosition(out x, out y, out z);
            WriteFloat(x);
            WriteFloat(y);
            WriteFloat(z);
        }
        public void WriteXYZO(Position pos)
        {
            float x, y, z, o;
            pos.GetPosition(out x, out y, out z, out o);
            WriteFloat(x);
            WriteFloat(y);
            WriteFloat(z);
            WriteFloat(o);
        }

        public uint GetOpcode() { return opcode; }

        public ServerTime GetReceivedTime() { return m_receivedTime; }
        public void SetReceiveTime(ServerTime receivedTime) { m_receivedTime = receivedTime; }

        uint opcode;
        ServerTime m_receivedTime; // only set for a specific set of opcodes, for performance reasons.
    }

    public class PacketHeader
    {
        public int Size;
        public byte[] Tag = new byte[12];

        public void Read(byte[] buffer)
        {
            Size = BitConverter.ToInt32(buffer, 0);
            Buffer.BlockCopy(buffer, 4, Tag, 0, 12);
        }

        public void Write(ByteBuffer byteBuffer)
        {
            byteBuffer.WriteInt32(Size);
            byteBuffer.WriteBytes(Tag, 12);
        }

        public bool IsValidSize() { return Size < 0x40000; }
    }
}
