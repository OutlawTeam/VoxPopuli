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
        internal void GenerateChunk()
        {
            
            for (int j = 0; j < GlobalVariable.CHUNK_HEIGHT; j++)
            {
                for (int i = 0; i < GlobalVariable.CHUNK_SIZE; i++)
                {
                    for (int k = 0; k < GlobalVariable.CHUNK_SIZE; k++)
                    {

                        if (j > 300)
                        {
                            Blocks[carray.treedto1d(i, j, k)] = 0;
                        }
                        else if (j == 300)
                        {

                            Blocks[carray.treedto1d(i, j, k)] = 1;
                        }
                        else if (j < 300 && j > 275)
                        {
                            Blocks[carray.treedto1d(i, j, k)] = 1;
                        }
                        else
                        {
                            Blocks[carray.treedto1d(i, j, k)] = 3;
                        }
                    }
                }
            }
        }
    }
}
