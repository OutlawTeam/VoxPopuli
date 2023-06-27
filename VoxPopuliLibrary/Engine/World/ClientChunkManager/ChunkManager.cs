/**
 * ChunkManager for client side
 * Copyrights Florian Pfeiffer
 * Author Florian Pfeiffer
 */
using OpenTK.Mathematics;
using System.Collections.Concurrent;
using System.Diagnostics;
using VoxPopuliLibrary.Engine.Debug;
namespace VoxPopuliLibrary.Engine.World
{
    internal partial class ClientChunkManager
    {
        internal Dictionary<Vector3i, Chunk> Clist = new();
        internal ConcurrentQueue<Chunk> ChunkToBeMesh = new();
        internal ConcurrentQueue<Chunk> ChunkToGenerateOG = new();
        internal int ChunkRendered = 0;
        private Stopwatch RenderProfiler = new();
        private List<Thread> meshBuildThreads;
        private int threadCount = 8;  
        public ClientChunkManager()
        {
            meshBuildThreads = new List<Thread>();
            for (int i = 0; i < threadCount; i++)
            {
                var thread = new Thread(GenerateChunksMesh);
                thread.Name = "MeshBuilder_" + i;
                thread.Start();
                meshBuildThreads.Add(thread);
            }
        }
        internal void Update()
        {
            while(ChunkToGenerateOG.Count > 0) 
            {
                if(ChunkToGenerateOG.TryDequeue(out var chunk))
                {
                    chunk.GenerateOG();
                }
            }
        }
        internal void GenerateChunksMesh()
        {
            while(true)
            {
                if (ChunkToBeMesh.TryDequeue(out var chunk))
                {
                    if (chunk.Changed == true &&
                    Clist.ContainsKey(new Vector3i(chunk.Position.X + 1, chunk.Position.Y, chunk.Position.Z)) &&
                    Clist.ContainsKey(new Vector3i(chunk.Position.X - 1, chunk.Position.Y, chunk.Position.Z)) &&
                    Clist.ContainsKey(new Vector3i(chunk.Position.X, chunk.Position.Y + 1, chunk.Position.Z)) &&
                    Clist.ContainsKey(new Vector3i(chunk.Position.X, chunk.Position.Y - 1, chunk.Position.Z)) &&
                    Clist.ContainsKey(new Vector3i(chunk.Position.X, chunk.Position.Y, chunk.Position.Z - 1)) &&
                    Clist.ContainsKey(new Vector3i(chunk.Position.X, chunk.Position.Y, chunk.Position.Z + 1)) &&
                    Clist.ContainsKey(new Vector3i(chunk.Position.X + 1, chunk.Position.Y + 1, chunk.Position.Z)) &&
                    Clist.ContainsKey(new Vector3i(chunk.Position.X - 1, chunk.Position.Y + 1, chunk.Position.Z)) &&
                    Clist.ContainsKey(new Vector3i(chunk.Position.X + 1, chunk.Position.Y - 1, chunk.Position.Z)) &&
                    Clist.ContainsKey(new Vector3i(chunk.Position.X - 1, chunk.Position.Y - 1, chunk.Position.Z)) &&
                    Clist.ContainsKey(new Vector3i(chunk.Position.X + 1, chunk.Position.Y, chunk.Position.Z + 1)) &&
                    Clist.ContainsKey(new Vector3i(chunk.Position.X - 1, chunk.Position.Y, chunk.Position.Z + 1)) &&
                    Clist.ContainsKey(new Vector3i(chunk.Position.X + 1, chunk.Position.Y, chunk.Position.Z - 1)) &&
                    Clist.ContainsKey(new Vector3i(chunk.Position.X - 1, chunk.Position.Y, chunk.Position.Z - 1)) &&
                    Clist.ContainsKey(new Vector3i(chunk.Position.X, chunk.Position.Y + 1, chunk.Position.Z + 1)) &&
                    Clist.ContainsKey(new Vector3i(chunk.Position.X, chunk.Position.Y - 1, chunk.Position.Z + 1)) &&
                    Clist.ContainsKey(new Vector3i(chunk.Position.X, chunk.Position.Y + 1, chunk.Position.Z - 1)) &&
                    Clist.ContainsKey(new Vector3i(chunk.Position.X, chunk.Position.Y - 1, chunk.Position.Z - 1)) &&
                    Clist.ContainsKey(new Vector3i(chunk.Position.X + 1, chunk.Position.Y + 1, chunk.Position.Z + 1)) &&
                    Clist.ContainsKey(new Vector3i(chunk.Position.X - 1, chunk.Position.Y + 1, chunk.Position.Z + 1)) &&
                    Clist.ContainsKey(new Vector3i(chunk.Position.X + 1, chunk.Position.Y - 1, chunk.Position.Z + 1)) &&
                    Clist.ContainsKey(new Vector3i(chunk.Position.X - 1, chunk.Position.Y - 1, chunk.Position.Z + 1)) &&
                    Clist.ContainsKey(new Vector3i(chunk.Position.X + 1, chunk.Position.Y + 1, chunk.Position.Z - 1)) &&
                    Clist.ContainsKey(new Vector3i(chunk.Position.X - 1, chunk.Position.Y + 1, chunk.Position.Z - 1)) &&
                    Clist.ContainsKey(new Vector3i(chunk.Position.X + 1, chunk.Position.Y - 1, chunk.Position.Z - 1)) &&
                    Clist.ContainsKey(new Vector3i(chunk.Position.X - 1, chunk.Position.Y - 1, chunk.Position.Z - 1)) &&
                    chunk.Empty == false)
                    {
                        chunk.GenerateMesh();
                        ChunkToGenerateOG.Enqueue(chunk);
                    }
                    else
                    {
                        chunk.Changed = true;
                        ChunkToBeMesh.Enqueue(chunk);
                    }
                }
            }
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
                            if (ch.Empty == false && ch.VerticeCount != 0 && ch.Changed ==false) 
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
    }
}
