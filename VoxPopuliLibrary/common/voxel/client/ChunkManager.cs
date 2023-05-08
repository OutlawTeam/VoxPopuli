/**
 * ChunkManager for client side
 * Copyrights Florian Pfeiffer
 * Author Florian Pfeiffer
 */
using K4os.Compression.LZ4;
using LiteNetLib;
using LiteNetLib.Utils;
using OpenTK.Mathematics;
using VoxPopuliLibrary.client;
using VoxPopuliLibrary.client.graphic;
using VoxPopuliLibrary.client.graphic.renderer;
using VoxPopuliLibrary.common.voxel.common;
using VoxPopuliLibrary.client;
using VoxPopuliLibrary.common.physic;
using VoxPopuliLibrary.common.voxel.common;
using VoxPopuliLibrary.common.ecs;
using VoxPopuliLibrary.common.ecs.client;

namespace VoxPopuliLibrary.common.voxel.client
{
    public static class Chunk_Manager
    {
        public static Dictionary<Vector2i, Chunk> clist = new Dictionary<Vector2i, Chunk>();
        private static int chunk_fra = 0;
        public static void Update(Shader _shader, Vector3d pos)
        {
            chunk_fra = 0;
            loadchunk(pos);
            loadchunkmesh(_shader);
        }
        internal static void ClearAllChunk()
        {
            clist.Clear();
        }
        static void loadchunk(Vector3d Position)
        {
            int minx = (int)(Position.X / 16) - GlobalVariable.Distance;
            int minz = (int)(Position.Z / 16) - GlobalVariable.Distance;
            int maxx = (int)(Position.X / 16) + GlobalVariable.Distance;
            int maxz = (int)(Position.Z / 16) + GlobalVariable.Distance;
            ClearNotUsedChunk();
            for (int x = minx; x <= maxx; x++)
            {
                for (int z = minz; z <= maxz; z++)
                {
                    if (!clist.TryGetValue(new Vector2i(x, z), out Chunk fsdfgris))
                    {
                        var message = new NetDataWriter();
                        message.Put(Convert.ToUInt16(network.NetworkProtocol.ChunkDemand));
                        message.Put(x);
                        message.Put(z);
                        if (VoxPopuliLibrary.client.network.Network.client.FirstPeer != null)
                        {
                            VoxPopuliLibrary.client.network.Network.client.FirstPeer.Send(message, DeliveryMethod.ReliableUnordered);
                        }
                    }
                }
            }
        }
        static void ClearNotUsedChunk()
        {
            foreach (Chunk chunk in clist.Values)
            {
                foreach (Player player in PlayerFactory.PlayerList.Values)
                {
                    int minx = (int)(player.Position.X / 16) - GlobalVariable.Distance;
                    int minz = (int)(player.Position.Z / 16) - GlobalVariable.Distance;
                    int maxx = (int)(player.Position.X / 16) + GlobalVariable.Distance;
                    int maxz = (int)(player.Position.Z / 16) + GlobalVariable.Distance;

                    if (!(chunk.Position.X >= minx && chunk.Position.X <= maxx && chunk.Position.Y >= minz && chunk.Position.Y <= maxz))
                    {
                        clist.Remove(chunk.Position);
                    }
                }
            }
        }
        static void loadchunkmesh(Shader _shader)
        {
            foreach (Chunk ch in clist.Values)
            {
                if (ch.Changed == true && clist.ContainsKey(new Vector2i(ch.Position.X + 1, ch.Position.Y)) && clist.ContainsKey(new Vector2i(ch.Position.X - 1, ch.Position.Y)) && clist.ContainsKey(new Vector2i(ch.Position.X, ch.Position.Y + 1)) && clist.ContainsKey(new Vector2i(ch.Position.X, ch.Position.Y - 1)))

                {
                    ch.GenerateMesh();
                }
            }
        }
        public static void RenderChunk(Vector3 campos)
        {
            int minx = (int)(campos.X / 16) - GlobalVariable.Distance;
            int minz = (int)(campos.Z / 16) - GlobalVariable.Distance;
            int maxx = (int)(campos.X / 16) + GlobalVariable.Distance;
            int maxz = (int)(campos.Z / 16) + GlobalVariable.Distance;

            for (int x = minx; x <= maxx; x++)
            {
                for (int z = minz; z <= maxz; z++)
                {
                    if (clist.TryGetValue(new Vector2i(x, z), out Chunk ch))
                    {
                        ch.Render();
                    }
                }
            }
        }
        public static Chunk getchunk(int x, int y)
        {
            if (clist.TryGetValue(new Vector2i(x, y), out Chunk ch))
            {
                return ch;
            }
            else
            {
                throw new Exception("The demanded chunk not exist in client memory.");
            }
        }
        public static void ChangeChunk(Vector3d blockp, ushort block)
        {
            (Vector2i cpos, Vector3i bpos) = math.Coord.GetVoxelCoord(blockp);
            var message = new NetDataWriter();
            message.Put(Convert.ToUInt16(network.NetworkProtocol.ChunkOneBlockChangeDemand));
            message.Put(cpos.X);
            message.Put(cpos.Y);
            message.Put(bpos.X);
            message.Put(bpos.Y);
            message.Put(bpos.Z);
            message.Put(block);
            if (VoxPopuliLibrary.client.network.Network.client.FirstPeer != null)
            {
                VoxPopuliLibrary.client.network.Network.client.FirstPeer.Send(message, DeliveryMethod.ReliableUnordered);
            }
        }
        public static void HandleChunk(NetDataReader data, NetPeer peer)
        {
            Vector2i cpos = new Vector2i(data.GetInt(), data.GetInt());
            byte[] blocks = data.GetRemainingBytes();

            blocks = LZ4Pickler.Unpickle(blocks);
            ushort[] block = Utils.bytestoints(blocks);
            if (!clist.TryGetValue(cpos, out Chunk vtff))
            {
                clist.Add(cpos, new Chunk(block, cpos));
            }
        }
        public static async void HandleChunkUpdate(NetDataReader data, NetPeer peer)
        {
            ushort block = data.GetUShort();
            Vector2i cpos = new Vector2i(data.GetInt(), data.GetInt());
            Vector3i cpos2 = new Vector3i(data.GetInt(), data.GetInt(), data.GetInt());
            if (clist.TryGetValue(cpos, out Chunk ch))
            {
                ch.SetBlock(cpos2.X, cpos2.Y, cpos2.Z, block);
                ch.Changed = true;
            }
            if (clist.TryGetValue(new Vector2i(cpos.X + 1, cpos.Y), out Chunk ch1))
            {
                ch1.Changed = true;
            }
            if (clist.TryGetValue(new Vector2i(cpos.X - 1, cpos.Y), out Chunk ch2))
            {
                ch2.Changed = true;
            }
            if (clist.TryGetValue(new Vector2i(cpos.X, cpos.Y - 1), out Chunk ch3))
            {
                ch3.Changed = true;
            }
            if (clist.TryGetValue(new Vector2i(cpos.X, cpos.Y + 1), out Chunk ch4))
            {
                ch4.Changed = true;
            }
        }
        internal static bool GetBlock(Vector3d blockp, out ushort id)
        {
            (Vector2i cpos, Vector3i bpos) = math.Coord.GetVoxelCoord(blockp);
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
        internal static bool GetBlock(int x,int y,int z, out ushort id)
        {
            (Vector2i cpos,Vector3i bpos) =math.Coord.GetVoxelCoord(x,y,z);
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
    public static class Utils
    {
        public static ushort[] bytestoints(byte[] input)
        {
            var size = input.Length / sizeof(short);
            var ints = new ushort[size];
            for (var index = 0; index < size; index++)
            {
                ints[index] = BitConverter.ToUInt16(input, index * sizeof(ushort));
            }
            return ints;
        }
    }
}
 