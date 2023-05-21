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
using VoxPopuliLibrary.common.ecs;
using VoxPopuliLibrary.common.ecs.client;
using VoxPopuliLibrary.common.voxel.common;

namespace VoxPopuliLibrary.common.voxel.client
{
    internal static class ChunkManager
    {
        internal static Dictionary<Vector2i, Chunk> Clist = new Dictionary<Vector2i, Chunk>();
        /* internal static ConcurrentQueue<Chunk> ChunkMeshToGenerate = new ConcurrentQueue<Chunk>();

         private static int numThreads = 0; // nombre actuel de threads en cours d'exécution
         private static object threadLock = new object(); // verrou pour gérer l'accès à numThreads
         private static Thread[] threads;*/
        internal static void Update(Vector3d pos)
        {
            GetChunk(pos);
            GenerateChunksMesh();
        }
        internal static void ClearAllChunk()
        {
            Clist.Clear();
        }/*
        internal static void AddChunkToQueue(Chunk chunk)
        {
            ChunkMeshToGenerate.Enqueue(chunk);
        }*/

        static void GetChunk(Vector3d Position)
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
                    if (!Clist.TryGetValue(new Vector2i(x, z), out Chunk fsdfgris))
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
            foreach (Chunk chunk in Clist.Values)
            {
                foreach (Player player in PlayerFactory.PlayerList.Values)
                {
                    int minx = (int)(player.Position.X / 16) - GlobalVariable.Distance;
                    int minz = (int)(player.Position.Z / 16) - GlobalVariable.Distance;
                    int maxx = (int)(player.Position.X / 16) + GlobalVariable.Distance;
                    int maxz = (int)(player.Position.Z / 16) + GlobalVariable.Distance;

                    if (!(chunk.Position.X >= minx && chunk.Position.X <= maxx && chunk.Position.Y >= minz && chunk.Position.Y <= maxz))
                    {
                        Clist.Remove(chunk.Position, out Chunk nothing);
                    }
                }
            }
        }
        internal static void GenerateChunksMesh()
        {
            foreach (Chunk chunk in Clist.Values)
            {
                if (chunk.Changed == true && Clist.ContainsKey(new Vector2i(chunk.Position.X + 1, chunk.Position.Y)) &&
                Clist.ContainsKey(new Vector2i(chunk.Position.X - 1, chunk.Position.Y)) &&

                Clist.ContainsKey(new Vector2i(chunk.Position.X, chunk.Position.Y + 1)) &&
                Clist.ContainsKey(new Vector2i(chunk.Position.X, chunk.Position.Y - 1)))

                {
                    chunk.GenerateMesh();
                }
                //AddChunkToQueue(chunk);
            }/*
            while (ChunkMeshToGenerate.Count > 0 && numThreads < GlobalVariable.maxThreads)
            {
                lock (threadLock)
                {
                    numThreads++;
                }

                Thread thread = new Thread(new ThreadStart(ThreadedGenerateMeshes));
                thread.Start();
            }*/

        }/*
        private static void ThreadedGenerateMeshes()
        {
            Chunk chunk;
            while (ChunkMeshToGenerate.TryDequeue(out chunk))
            {
                chunk.GenerateMesh();

                lock (threadLock)
                {
                    numThreads--;
                }
            }
        }*/
        internal static void RenderChunk(Vector3 campos)
        {
            int minx = (int)(campos.X / 16) - GlobalVariable.Distance;
            int minz = (int)(campos.Z / 16) - GlobalVariable.Distance;
            int maxx = (int)(campos.X / 16) + GlobalVariable.Distance;
            int maxz = (int)(campos.Z / 16) + GlobalVariable.Distance;

            for (int x = minx; x <= maxx; x++)
            {
                for (int z = minz; z <= maxz; z++)
                {
                    if (Clist.TryGetValue(new Vector2i(x, z), out Chunk ch))
                    {
                        ch.Render();
                    }
                }
            }
        }
        internal static Chunk GetChunk(int x, int y)
        {
            if (Clist.TryGetValue(new Vector2i(x, y), out Chunk ch))
            {
                return ch;
            }
            else
            {
                throw new Exception("Chunk is not loaded");
            }
        }
        internal static void ChangeChunk(Vector3d blockp, ushort block)
        {
            (Vector2i cpos, Vector3i bpos) = math.Coord.GetVoxelCoord((int)blockp.X, (int)blockp.Y, (int)blockp.Z);
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
        internal static void HandleChunk(NetDataReader data, NetPeer peer)
        {
            Vector2i cpos = new Vector2i(data.GetInt(), data.GetInt());
            byte[] blocks = data.GetRemainingBytes();

            blocks = LZ4Pickler.Unpickle(blocks);
            ushort[] block = Utils.BytesToInts(blocks);
            if (!Clist.TryGetValue(cpos, out Chunk vtff))
            {
                Clist.Add(cpos, new Chunk(block, cpos));
            }
        }
        internal static async void HandleChunkUpdate(NetDataReader data)
        {
            ushort block = data.GetUShort();
            Vector2i cpos = new Vector2i(data.GetInt(), data.GetInt());
            Vector3i cpos2 = new Vector3i(data.GetInt(), data.GetInt(), data.GetInt());
            if (Clist.TryGetValue(cpos, out Chunk ch))
            {
                ch.SetBlock(cpos2.X, cpos2.Y, cpos2.Z, block);
                ch.Changed = true;
            }
            if (Clist.TryGetValue(new Vector2i(cpos.X + 1, cpos.Y), out Chunk ch1))
            {
                ch1.Changed = true;
            }
            if (Clist.TryGetValue(new Vector2i(cpos.X - 1, cpos.Y), out Chunk ch2))
            {
                ch2.Changed = true;
            }
            if (Clist.TryGetValue(new Vector2i(cpos.X, cpos.Y - 1), out Chunk ch3))
            {
                ch3.Changed = true;
            }
            if (Clist.TryGetValue(new Vector2i(cpos.X, cpos.Y + 1), out Chunk ch4))
            {
                ch4.Changed = true;
            }
        }
        internal static bool GetBlock(int x, int y, int z, out ushort id)
        {
            (Vector2i cpos, Vector3i bpos) = math.Coord.GetVoxelCoord(x, y, z);
            if (Clist.TryGetValue(new Vector2i(cpos.X, cpos.Y), out Chunk ch))
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
        internal static bool GetBlockChunkRelative(int x, int y, int z, out ushort id)
        {
            (Vector2i cpos, Vector3i bpos) = math.Coord.GetVoxelCoordChunkRelative(x, y, z);
            if (Clist.TryGetValue(new Vector2i(cpos.X, cpos.Y), out Chunk ch))
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
    internal static class Utils
    {
        internal static ushort[] BytesToInts(byte[] input)
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
