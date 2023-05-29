using VoxPopuliLibrary.Engine.Maths;

namespace VoxPopuliLibrary.Engine.World
{
    internal partial class Chunk
    {
        internal ushort GetBlock(int x, int y, int z)
        {
            try
            {
                return Blocks[Carray.TreetoOne(x, y, z)];
            }
            catch
            {
                Console.WriteLine(x + ":" + y + ":" + z);
                throw new Exception("Block coordinate was supperior to chunk size or height.");
            }
        }
        internal void SetBlock(int x, int y, int z, ushort id)
        {
            Blocks[Carray.TreetoOne(x, y, z)] = id;
        }
    }
}
