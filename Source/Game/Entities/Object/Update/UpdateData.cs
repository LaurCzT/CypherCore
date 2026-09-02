// Copyright (c) CypherCore <http://github.com/CypherCore> All rights reserved.
// Licensed under the GNU GENERAL PUBLIC LICENSE. See LICENSE file in the project root for full license information.

using Framework.IO;
using Game.Networking;
using Game.Networking.Packets;
using System.Collections.Generic;

namespace Game.Entities
{
    public class UpdateData
    {
        int MapId;
        uint BlockCount;
        List<ObjectGuid> destroyGUIDs = new();
        List<ObjectGuid> outOfRangeGUIDs = new();
        List<ByteBuffer> _blocks = new();

        public UpdateData(int mapId)
        {
            MapId = mapId;
        }

        public void AddDestroyObject(ObjectGuid guid)
        {
            destroyGUIDs.Add(guid);
        }

        public void AddOutOfRangeGUID(List<ObjectGuid> guids)
        {
            outOfRangeGUIDs.AddRange(guids);
        }

        public void AddOutOfRangeGUID(ObjectGuid guid)
        {
            outOfRangeGUIDs.Add(guid);
        }

        public void AddUpdateBlock(ByteBuffer block)
        {
            // Store blocks separately so we can split large update packets
            _blocks.Add(block);
            ++BlockCount;
        }

        public bool BuildPacket(out UpdateObject packet)
        {
            var packets = new List<UpdateObject>();
            BuildPackets(out packets);
            if (packets.Count == 0)
            {
                packet = new UpdateObject();
                return false;
            }

            packet = packets[0];
            return true;
        }

        public void BuildPackets(out List<UpdateObject> packets)
        {
            packets = new List<UpdateObject>();

            // If no blocks, return empty list
            if (BlockCount == 0 && outOfRangeGUIDs.Count == 0 && destroyGUIDs.Count == 0)
                return;

            const int MaxPayload = 300 * 1024; // 300 KB per packet payload target

            int blockIndex = 0;
            int totalBlocks = _blocks.Count;

            bool firstPacket = true;
            while (blockIndex < totalBlocks || firstPacket)
            {
                WorldPacket buffer = new();

                // include destroy/outOfRange only in first packet
                if (firstPacket)
                {
                    if (buffer.WriteBit(!outOfRangeGUIDs.Empty() || !destroyGUIDs.Empty()))
                    {
                        buffer.WriteUInt16((ushort)destroyGUIDs.Count);
                        buffer.WriteInt32(destroyGUIDs.Count + outOfRangeGUIDs.Count);

                        foreach (var destroyGuid in destroyGUIDs)
                            buffer.WritePackedGuid(destroyGuid);

                        foreach (var outOfRangeGuid in outOfRangeGUIDs)
                            buffer.WritePackedGuid(outOfRangeGuid);
                    }
                }
                else
                {
                    // write zero flags for subsequent packets
                    buffer.WriteBit(false);
                }

                // accumulate blocks until MaxPayload reached
                ByteBuffer temp = new();
                int currentSize = 0;
                int addedBlocks = 0;

                while (blockIndex < totalBlocks)
                {
                    var bdata = _blocks[blockIndex].GetData();
                    if (currentSize + bdata.Length > MaxPayload && addedBlocks > 0)
                        break;

                    temp.WriteBytes(bdata);
                    currentSize += bdata.Length;
                    ++blockIndex;
                    ++addedBlocks;
                }

                buffer.WriteInt32(currentSize);
                if (currentSize > 0)
                    buffer.WriteBytes(temp.GetData());

                UpdateObject pkt = new();
                pkt.NumObjUpdates = (uint)addedBlocks;
                pkt.MapID = (ushort)MapId;
                pkt.Data = buffer.GetData();
                // Diagnostic logging for packet sizes
                Log.outDebug(LogFilter.Network, $"UpdateData.BuildPackets: created packet #{packets.Count} map={MapId} blocks={addedBlocks} payloadSize={currentSize}");
                packets.Add(pkt);

                firstPacket = false;

                // safety: break if nothing was added to prevent infinite loop
                if (addedBlocks == 0)
                    break;
            }
        }

        public void Clear()
        {
            _blocks.Clear();
            destroyGUIDs.Clear();
            outOfRangeGUIDs.Clear();
            BlockCount = 0;
            MapId = 0;
        }

        public bool HasData() { return BlockCount > 0 || !outOfRangeGUIDs.Empty() || !destroyGUIDs.Empty(); }

        public List<ObjectGuid> GetOutOfRangeGUIDs() { return outOfRangeGUIDs; }

        public void SetMapId(ushort mapId) { MapId = mapId; }
    }
}
