using VoxPopuliLibrary.Engine.Maths;

namespace VoxPopuliLibrary.Engine.World
{
    internal partial class Chunk
    {
        internal void GenerateChunk()
        {

            for (int y = 0; y < ServerWorldManager.world.CHUNK_SIZE; y++)
            {
                for (int x = 0; x < ServerWorldManager.world.CHUNK_SIZE; x++)
                {
                    for (int z = 0; z < ServerWorldManager.world.CHUNK_SIZE; z++)
                    {
                        if (Position.Y >= 18)
                        {
                            if (Position.Y == 18 && y == 12)
                            {
                                Blocks[Carray.TreetoOne(x, y, z)] = 1;
                            }
                            else if (Position.Y == 18 && y < 12)
                            {
                                Blocks[Carray.TreetoOne(x, y, z)] = 1;
                            }
                            else
                            {
                                Blocks[Carray.TreetoOne(x, y, z)] = 0;
                            }
                        }
                        else
                        {
                            Blocks[Carray.TreetoOne(x, y, z)] = 3;
                        }
                    }
                }
            }
        }
    }
}
