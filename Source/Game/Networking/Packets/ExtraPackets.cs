using Framework.Collections;
using Framework.Constants;
using Framework.Database;
using Game.Entities;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Game.Networking.Packets
{
    public class GetAccountCharacterListRequest : ClientPacket
    {
        public GetAccountCharacterListRequest(WorldPacket packet) : base(packet) { }

        public override void Read()
        {
            Token = _worldPacket.ReadUInt32();
        }

        public uint Token;
    }

    public class GetAccountCharacterListResult : ServerPacket
    {
        public GetAccountCharacterListResult() : base(ServerOpcodes.GetAccountCharacterListResult) { }

        public override void Write()
        {
            _worldPacket.WriteUInt32(Token);
            _worldPacket.WriteUInt32((uint)CharacterList.Count);

            _worldPacket.ResetBitPos();
            _worldPacket.WriteBit(false); // unknown bit

            foreach (var entry in CharacterList)
                entry.WriteEntry(_worldPacket);
        }

        public uint Token = 0;
        public List<AccountCharacterListEntry> CharacterList = new();
    }

    public class AccountCharacterListEntry
    {
        public void WriteEntry(WorldPacket packet)
        {
            packet.WritePackedGuid(AccountId);
            packet.WritePackedGuid(CharacterGuid);
            packet.WriteUInt32(RealmVirtualAddress);
            packet.WriteUInt8((byte)Race);
            packet.WriteUInt8((byte)Class);
            packet.WriteUInt8((byte)Sex);
            packet.WriteUInt8(Level);

            packet.WriteUInt64(LastLoginUnixSec);

            // Unk for 1.14.1+ is not written for 1.14.0

            packet.ResetBitPos();
            packet.WriteBits(Name.GetByteCount(), 6);
            packet.WriteBits(RealmName.GetByteCount(), 9);
            packet.FlushBits();

            packet.WriteString(Name);
            packet.WriteString(RealmName);
        }

        public ObjectGuid AccountId;
        public ObjectGuid CharacterGuid;
        public uint RealmVirtualAddress;
        public Race Race;
        public Class Class;
        public Gender Sex;
        public byte Level;
        public ulong LastLoginUnixSec;
        public string Name = "";
        public string RealmName = "";
    }

    public class QueuedMessagesEnd : ClientPacket
    {
        public QueuedMessagesEnd(WorldPacket packet) : base(packet) { }
        public override void Read() { }
    }

    public class BattlePayGetProductList : ClientPacket
    {
        public BattlePayGetProductList(WorldPacket packet) : base(packet) { }
        public override void Read() { }
    }

    public class BattlePayGetPurchaseList : ClientPacket
    {
        public BattlePayGetPurchaseList(WorldPacket packet) : base(packet) { }
        public override void Read() { }
    }
}
