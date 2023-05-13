using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxPopuliLibrary.client;
using VoxPopuliLibrary.common.utils;

namespace VoxPopuliLibrary.common.voxel.common
{
    internal partial class Chunk
    {
        internal ushort GetBlock(int x, int y, int z)
        {
            try
            {
                return Blocks[carray.treedto1d(x, y, z)];
            }
            catch
            {
                Console.WriteLine(x + ":" + y + ":" + z);
                throw new Exception("Block coordinate was supperior to chunk size or height.");
            }
        }
        internal void SetBlock(int x, int y, int z, ushort id)
        {
            Blocks[x + GlobalVariable.CHUNK_SIZE * (y + GlobalVariable.CHUNK_HEIGHT * z)] = id;
        }
    }
}
