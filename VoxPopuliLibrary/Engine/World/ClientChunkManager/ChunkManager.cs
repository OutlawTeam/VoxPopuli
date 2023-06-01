/**
 * ChunkManager for client side
 * Copyrights Florian Pfeiffer
 * Author Florian Pfeiffer
 */
using OpenTK.Mathematics;
using System.Diagnostics;
using VoxPopuliLibrary.Engine.Debug;
namespace VoxPopuliLibrary.Engine.World
{
    internal partial class ClientChunkManager
    {
        internal Dictionary<Vector3i, Chunk> Clist = new();
        internal List<Chunk> ChunkMesh = new();
        internal int ChunkMeshUpdated = 0;
        internal int ChunkRendered = 0;
        private Stopwatch MeshProfiler = new();
        private Stopwatch RenderProfiler = new();
        internal void Update(Vector3d pos)
        {
            GenerateChunksMesh();
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
    }
}
