using OpenTK.Mathematics;
using VoxPopuliLibrary.Engine.Maths;

namespace VoxPopuliLibrary.Engine.World
{
    internal partial class Chunk
    {
        internal string GetBlock(int x, int y, int z)
        {
            try
            {
                byte id = Blocks[Carray.TreetoOne(x, y, z)];
                return ChunkPalette.GetBlock(id);

            }
            catch
            {
                Console.WriteLine(x + ":" + y + ":" + z);
                throw new Exception("Block coordinate was supperior to chunk size or height.");
            }
        }
        internal void SetBlock(int x, int y, int z, string id)
        {
            if (ChunkPalette.ContainBlock(id))
            {
                Blocks[Carray.TreetoOne(x, y, z)] = ChunkPalette.GetBlockId(id);
            }
            else
            {
                Blocks[Carray.TreetoOne(x, y, z)] = ChunkPalette.AddBlock(id);
            }
        }
    }
}

