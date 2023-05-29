/**
 * ChunkManager for client side
 * Copyrights Florian Pfeiffer
 * Author Florian Pfeiffer
 */
using LiteNetLib;
using LiteNetLib.Utils;
using OpenTK.Mathematics;
using System.Diagnostics;
using VoxPopuliLibrary.Engine.Debug;
using VoxPopuliLibrary.Engine.Network;

namespace VoxPopuliLibrary.Engine.World
{
    internal partial class ClientChunkManager
    {
        internal Dictionary<Vector3i, Chunk> Clist = new();
        internal List<Chunk> ChunkMesh = new();
        internal int ChunkMeshUpdated = 0;
        internal int ChunkRendered = 0;
        internal int ChunkDemand = 0;
        private Stopwatch MeshProfiler = new();
        private Stopwatch ClearProfiler = new();
        private Stopwatch RenderProfiler = new();
        internal void Update(Vector3d pos)
        {
            DemandChunk(pos);
            GenerateChunksMesh();
        }
        internal void ClearAllChunk()
        {
            Clist.Clear();
        }
        void ClearNotUsedChunk()
        {
            ClearProfiler.Start();
            int minx = (int)(ClientWorldManager.world.GetPlayerFactoryClient().LocalPlayer.Position.X / 16) - ClientWorldManager.world.LoadDistance;
            int miny = (int)(ClientWorldManager.world.GetPlayerFactoryClient().LocalPlayer.Position.Y / 16) - ClientWorldManager.world.VerticalLoadDistance;
            int minz = (int)(ClientWorldManager.world.GetPlayerFactoryClient().LocalPlayer.Position.Z / 16) - ClientWorldManager.world.LoadDistance;
            int maxx = (int)(ClientWorldManager.world.GetPlayerFactoryClient().LocalPlayer.Position.X / 16) + ClientWorldManager.world.LoadDistance;
            int maxy = (int)(ClientWorldManager.world.GetPlayerFactoryClient().LocalPlayer.Position.Y / 16) + ClientWorldManager.world.VerticalLoadDistance;
            int maxz = (int)(ClientWorldManager.world.GetPlayerFactoryClient().LocalPlayer.Position.Z / 16) + ClientWorldManager.world.LoadDistance;

            Dictionary<Vector3i, Chunk> newClist = new();

            foreach (KeyValuePair<Vector3i, Chunk> entry in Clist)
            {
                Vector3i position = entry.Key;
                Chunk chunk = entry.Value;

                if (position.X >= minx && position.X <= maxx &&
                    position.Y >= miny && position.Y <= maxy &&
                    position.Z >= minz && position.Z <= maxz)
                {
                    newClist[position] = chunk;
                }
            }

            Clist = newClist;
            ClearProfiler.Stop();
            DebugSystem.ClearTime = ClearProfiler.ElapsedMilliseconds;
            ClearProfiler.Reset();
        }

        internal void GenerateChunksMesh()
        {
            MeshProfiler.Start();
            // Créer un tableau de tâches pour chaque chunk
            ChunkMeshUpdated = 0;
            for (int i = ChunkMesh.Count - 1; i >= 0; i--)
            {
                Chunk chunk = ChunkMesh[i];
                if (chunk.Changed == true &&
                Clist.ContainsKey(new Vector3i(chunk.Position.X + 1, chunk.Position.Y, chunk.Position.Z)) &&
                Clist.ContainsKey(new Vector3i(chunk.Position.X - 1, chunk.Position.Y, chunk.Position.Z)) &&
                Clist.ContainsKey(new Vector3i(chunk.Position.X, chunk.Position.Y + 1, chunk.Position.Z)) &&
                Clist.ContainsKey(new Vector3i(chunk.Position.X, chunk.Position.Y - 1, chunk.Position.Z)) &&
                Clist.ContainsKey(new Vector3i(chunk.Position.X, chunk.Position.Y, chunk.Position.Z - 1)) &&
                Clist.ContainsKey(new Vector3i(chunk.Position.X, chunk.Position.Y, chunk.Position.Z + 1)) &&
                chunk.Empty == false)

                {
                    chunk.GenerateMesh();
                    ChunkMesh.RemoveAt(i);
                    ChunkMeshUpdated++;
                }
            }
            MeshProfiler.Stop();
            DebugSystem.MeshGenerationTime = MeshProfiler.ElapsedMilliseconds;
            MeshProfiler.Reset();
        }
        internal void RenderChunk(Vector3 campos)
        {
            RenderProfiler.Start();
            ChunkRendered = 0;
            int minx = (int)(campos.X / 16) - ClientWorldManager.world.RenderDistance;
            int miny = (int)(campos.Y / 16) - ClientWorldManager.world.VerticalRenderDistance;
            int minz = (int)(campos.Z / 16) - ClientWorldManager.world.RenderDistance;
            int maxx = (int)(campos.X / 16) + ClientWorldManager.world.RenderDistance;
            int maxy = (int)(campos.Y / 16) + ClientWorldManager.world.VerticalRenderDistance;
            int maxz = (int)(campos.Z / 16) + ClientWorldManager.world.RenderDistance;

            for (int x = minx; x <= maxx; x++)
            {
                for (int y = miny; y <= maxy; y++)
                {
                    for (int z = minz; z <= maxz; z++)
                    {
                        if (Clist.TryGetValue(new Vector3i(x, y, z), out Chunk ch))
                        {
                            if (ch.Empty == false && ch.VerticeCount != 0)
                            {
                                ch.Render();
                                ChunkRendered++;
                            }
                        }
                    }
                }
            }
            RenderProfiler.Stop();
            DebugSystem.ChunkRenderTime = RenderProfiler.ElapsedMilliseconds;
            RenderProfiler.Reset();
        }
        void DemandChunk(Vector3d Position)
        {
            ChunkDemand = 0;
            int minx = (int)(Position.X / 16) - ClientWorldManager.world.LoadDistance;
            int miny = (int)(Position.Y / 16) - ClientWorldManager.world.VerticalLoadDistance;
            int minz = (int)(Position.Z / 16) - ClientWorldManager.world.LoadDistance;
            int maxx = (int)(Position.X / 16) + ClientWorldManager.world.LoadDistance;
            int maxy = (int)(Position.Y / 16) + ClientWorldManager.world.VerticalLoadDistance;
            int maxz = (int)(Position.Z / 16) + ClientWorldManager.world.LoadDistance;
            ClearNotUsedChunk();
            for (int x = minx; x <= maxx; x++)
            {
                for (int y = miny; y <= maxy; y++)
                {
                    for (int z = minz; z <= maxz; z++)
                    {
                        if (!Clist.TryGetValue(new Vector3i(x, y, z), out _))
                        {
                            var message = new NetDataWriter();
                            message.Put(Convert.ToUInt16(NetworkProtocol.ChunkDemand));
                            message.Put(x);
                            message.Put(y);
                            message.Put(z);
                            if (ClientNetwork.Server != null && ChunkDemand <= 40)
                            {
                                ClientNetwork.Server.Send(message, DeliveryMethod.ReliableUnordered);
                                ChunkDemand++;
                            }
                        }
                    }
                }
            }
        }
    }

}
