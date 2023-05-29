using K4os.Compression.LZ4;
using LiteNetLib;
using LiteNetLib.Utils;
using OpenTK.Mathematics;
using VoxPopuliLibrary.Engine.Network;

namespace VoxPopuliLibrary.Engine.World
{
    internal partial class ServerChunkManager
    {
        internal void HandleBlockChange(NetDataReader data, NetPeer peer)
        {
            Vector3i cpos = new Vector3i(data.GetInt(), data.GetInt(), data.GetInt());
            Vector3i cpos2 = new Vector3i(data.GetInt(), data.GetInt(), data.GetInt());
            ushort block = data.GetUShort();
            if (clist.TryGetValue(cpos, out Chunk tempChunk))
            {
                tempChunk.SetBlock(cpos2.X, cpos2.Y, cpos2.Z, block);
                SendChunkUpdate(block, cpos2, cpos);
            }
            else
            {
                tempChunk = new Chunk(cpos);
                clist.Add(cpos, tempChunk);
                clist[cpos].SetBlock(cpos2.X, cpos2.Y, cpos2.Z, block);
            }
        }
        internal void SendChunkatall(Chunk chunk)
        {
            byte[] data = Utils.IntsToBytes(chunk.Blocks);
            data = LZ4Pickler.Pickle(data);
            ServerNetwork.message.Reset();
            //id of message
            ServerNetwork.message.Put(Convert.ToUInt16(NetworkProtocol.ChunkData));
            ServerNetwork.message.Put(chunk.Position.X);
            ServerNetwork.message.Put(chunk.Position.Y);
            ServerNetwork.message.Put(chunk.Position.Z);
            ServerNetwork.message.Put(data);
            ServerNetwork.server.SendToAll(ServerNetwork.message, DeliveryMethod.ReliableOrdered);
        }
        private void SendChunkUpdate(ushort id, Vector3i bpos, Vector3i cpos)
        {
            ServerNetwork.message.Reset();
            ServerNetwork.message.Put(Convert.ToUInt16(NetworkProtocol.ChunkOneBlockChange));
            ServerNetwork.message.Put(id);
            ServerNetwork.message.Put(cpos.X);
            ServerNetwork.message.Put(cpos.Y);
            ServerNetwork.message.Put(cpos.Z);
            ServerNetwork.message.Put(bpos.X);
            ServerNetwork.message.Put(bpos.Y);
            ServerNetwork.message.Put(bpos.Z);
            ServerNetwork.server.SendToAll(ServerNetwork.message, DeliveryMethod.ReliableOrdered);
        }
        internal void HandleChunk(NetPacketReader args, NetPeer peer)
        {
            Vector3i cpos = new Vector3i(args.GetInt(), args.GetInt(), args.GetInt());
            if (clist.TryGetValue(cpos, out Chunk tempchunk))
            {
                SendChunk(tempchunk, peer);
            }
            else
            {
                SendChunk(CreateChunk(cpos), peer);
            }
        }
        private void SendChunk(Chunk chunk, NetPeer peer)
        {
            byte[] data = Utils.IntsToBytes(chunk.Blocks);
            data = LZ4Pickler.Pickle(data);
            ServerNetwork.message.Reset();
            //id of message
            ServerNetwork.message.Put(Convert.ToUInt16(NetworkProtocol.ChunkData));
            ServerNetwork.message.Put(chunk.Position.X);
            ServerNetwork.message.Put(chunk.Position.Y);
            ServerNetwork.message.Put(chunk.Position.Z);
            ServerNetwork.message.Put(data);
            peer.Send(ServerNetwork.message, DeliveryMethod.ReliableOrdered);
        }
    }
}
