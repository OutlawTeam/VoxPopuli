/**
 * Chunk Manager sever side implementation
 * Copyrights Florian Pfeiffer
 * Author Florian Pfeiffer
 */
using K4os.Compression.LZ4;
using LiteNetLib;
using LiteNetLib.Utils;
using OpenTK.Mathematics;
using VoxPopuliLibrary.client;
using VoxPopuliLibrary.common.ecs;
using VoxPopuliLibrary.common.ecs.server;
using VoxPopuliLibrary.common.utils;
using VoxPopuliLibrary.common.voxel.common;
using VoxPopuliLibrary.server.network;

namespace VoxPopuliLibrary.common.voxel.server
{
    public static class ChunkManager
    {
        internal static Dictionary<Vector2i, Chunk> clist = new Dictionary<Vector2i, Chunk>();
        internal static List<Vector2i> ChunkToBeAdded = new List<Vector2i>();
        internal static Chunk tempChunk;
        private static void SendChunk(Chunk chunk, NetPeer peer)
        {
            byte[] data = utils.conintbyte(chunk.Blocks);
            data = LZ4Pickler.Pickle(data);
            Network.message.Reset();
            //id of message
            Network.message.Put(Convert.ToUInt16(network.NetworkProtocol.ChunkData));
            Network.message.Put(chunk.Position.X);
            Network.message.Put(chunk.Position.Y);
            Network.message.Put(data);
            peer.Send(Network.message, DeliveryMethod.ReliableOrdered);
        }
        internal static Chunk CreateChunk(Vector2i pos)
        {
            tempChunk = new Chunk(pos);
            clist.Add(pos, tempChunk);
            return tempChunk;
        }
        internal static void SendChunkatall(Chunk chunk, Vector2 pos)
        {
            byte[] data = utils.conintbyte(chunk.Blocks);
            data = LZ4Pickler.Pickle(data);
            Network.message.Reset();
            //id of message
            Network.message.Put(Convert.ToUInt16(network.NetworkProtocol.ChunkData));
            Network.message.Put(chunk.Position.X);
            Network.message.Put(chunk.Position.Y);
            Network.message.Put(data);
            Network.server.SendToAll(Network.message, DeliveryMethod.ReliableOrdered);
        }
        private static void SendChunkUpdate(ushort id, Vector3i bpos, Vector2i cpos)
        {
            Network.message.Reset();
            Network.message.Put(Convert.ToUInt16(network.NetworkProtocol.ChunkOneBlockChange));
            Network.message.Put(id);
            Network.message.Put(cpos.X);
            Network.message.Put(cpos.Y);
            Network.message.Put(bpos.X);
            Network.message.Put(bpos.Y);
            Network.message.Put(bpos.Z);
            Network.server.SendToAll(Network.message, DeliveryMethod.ReliableOrdered);
        }
        public static void HandleChunk(NetPacketReader args, NetPeer peer)
        {
            Vector2i cpos = new Vector2i(args.GetInt(), args.GetInt());
            if (clist.TryGetValue(cpos, out Chunk tempchunk))
            {
                SendChunk(tempchunk, peer);
            }
            else
            {
                SendChunk(CreateChunk(cpos), peer);
            }
        }
        internal static void Update()
        {
            ChunkToBeAdded.Clear();
            foreach (Chunk chunk in clist.Values)
            {
                if (chunk.Used = false)
                {
                    clist.Remove(chunk.Position);
                }
                chunk.Used = false;
            }
            foreach (Player player in PlayerFactory.List.Values)
            {
                int minx = (int)(player.Position.X / 16) - GlobalVariable.RenderDistance;
                int minz = (int)(player.Position.Z / 16) - GlobalVariable.RenderDistance;
                int maxx = (int)(player.Position.X / 16) + GlobalVariable.RenderDistance;
                int maxz = (int)(player.Position.Z / 16) + GlobalVariable.RenderDistance;
                for (int x = minx; x <= maxx; x++)
                {
                    for (int z = minz; z <= maxz; z++)
                    {
                        if (!clist.TryGetValue(new Vector2i(x, z), out Chunk Nothing))
                        {
                            ChunkToBeAdded.Add(new Vector2i(x, z));
                        }
                        else
                        {
                            Nothing.Used = true;
                        }
                    }
                }
            }
            foreach (Vector2i pos in ChunkToBeAdded)
            {
                if (!clist.TryGetValue(pos, out Chunk Nothing))
                {
                    CreateChunk(pos);
                }
            }
        }
        public static void HandleBlockChange(NetDataReader data, NetPeer peer)
        {
            Vector2i cpos = new Vector2i(data.GetInt(), data.GetInt());
            Vector3i cpos2 = new Vector3i(data.GetInt(), data.GetInt(), data.GetInt());
            ushort block = data.GetUShort();
            if (clist.TryGetValue(cpos, out tempChunk))
            {
                tempChunk.Blocks[carray.treedto1d(cpos2.X, cpos2.Y, cpos2.Z)] = block;
                SendChunkUpdate(block, cpos2, cpos);
            }
        }
        internal static bool GetBlock(int x, int y, int z, out ushort id)
        {
            (Vector2i cpos, Vector3i bpos) = math.Coord.GetVoxelCoord(x, y, z);
            if (clist.TryGetValue(new Vector2i(cpos.X, cpos.Y), out Chunk ch))
            {
                if (bpos.Y >= 0 && bpos.Y < GlobalVariable.CHUNK_HEIGHT)
                {
                    id = ch.GetBlock(bpos.X, bpos.Y, bpos.Z);
                    return true;
                }
                else
                {
                    id = 0;
                    return false;
                }
            }
            else
            {
                id = 0;
                return false;
            }
        }
    }
    public static class utils
    {
        public static byte[] conintbyte(ushort[] intArray)
        {
            byte[] result = new byte[intArray.Length * sizeof(ushort)];
            Buffer.BlockCopy(intArray, 0, result, 0, result.Length);
            return result;
        }
    }
}
