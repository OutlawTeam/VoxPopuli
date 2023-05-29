/**
 * Chunk Manager sever side implementation
 * Copyrights Florian Pfeiffer
 * Author Florian Pfeiffer
 */
using OpenTK.Mathematics;

namespace VoxPopuliLibrary.Engine.World
{
    internal partial class ServerChunkManager
    {
        internal Dictionary<Vector3i, Chunk> clist = new Dictionary<Vector3i, Chunk>();
        internal List<Vector3i> ChunkToBeAdded = new List<Vector3i>();
        internal void Update()
        {
            ChunkToBeAdded.Clear();
            foreach (Chunk chunk in clist.Values)
            {
                if (chunk.Used == false)
                {
                    clist.Remove(chunk.Position);
                }
                chunk.Used = false;
            }
            foreach (Player.Player player in ServerWorldManager.world.GetPlayerFactoryServer().List.Values)
            {
                int minx = (int)(player.Position.X / 16) - ServerWorldManager.world.LoadDistance;
                int miny = (int)(player.Position.Y / 16) - ServerWorldManager.world.VerticalLoadDistance;
                int minz = (int)(player.Position.Z / 16) - ServerWorldManager.world.LoadDistance;
                int maxx = (int)(player.Position.X / 16) + ServerWorldManager.world.LoadDistance;
                int maxy = (int)(player.Position.Y / 16) + ServerWorldManager.world.VerticalLoadDistance;
                int maxz = (int)(player.Position.Z / 16) + ServerWorldManager.world.LoadDistance;
                for (int x = minx; x <= maxx; x++)
                {
                    for (int y = miny; y <= maxy; y++)
                    {
                        for (int z = minz; z <= maxz; z++)
                        {
                            if (!clist.TryGetValue(new Vector3i(x, y, z), out Chunk Nothing))
                            {
                                ChunkToBeAdded.Add(new Vector3i(x, y, z));
                            }
                            else
                            {
                                Nothing.Used = true;
                            }
                        }
                    }
                }
            }
            foreach (Vector3i pos in ChunkToBeAdded)
            {
                if (!clist.TryGetValue(pos, out Chunk Nothing))
                {
                    CreateChunk(pos);
                }
            }
        }
    }
}
