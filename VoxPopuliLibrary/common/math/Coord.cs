using OpenTK.Mathematics;
namespace VoxPopuliLibrary.common.math
{
    internal static class Coord
    {
        internal static (Vector2i, Vector3i) GetVoxelCoord(int x, int y, int z)
        {
            Vector2i cpos;
            Vector3i bpos;
            if (x < 0 && z >= 0)
            {
                if (x % 16 == 0)
                {
                    cpos = new Vector2i(x / 16, z / 16);
                }else
                {
                    cpos = new Vector2i(x / 16 - 1, z / 16);
                }
                if(Math.Abs(x)%16 == 0)
                {
                    bpos = new Vector3i(0, y, Math.Abs(z % 16));
                }else
                {
                    bpos = new Vector3i(Math.Abs(x % 16 + 16), y, Math.Abs(z % 16));
                }
                
            }
            else if ((x >= 0 && z < 0))
            {
                if (z % 16 == 0)
                {
                    cpos = new Vector2i(x / 16, z / 16);
                }else
                {
                    cpos = new Vector2i(x / 16, z / 16 - 1);
                }
                if (Math.Abs(z) % 16 == 0)
                {
                    bpos = new Vector3i(Math.Abs(x % 16), y, 0);
                }else
                {
                    bpos = new Vector3i(Math.Abs(x % 16), y, Math.Abs(z % 16 + 16));
                }
            }
            else if (x < 0 && z < 0)
            {
                if (x % 16 == 0 && z % 16 != 0)
                {
                    cpos = new Vector2i(x / 16, z / 16 - 1);
                }
                else if (x % 16 != 0 && z % 16 == 0)
                {
                    cpos = new Vector2i(x / 16 - 1, z / 16);
                }
                else if (x % 16 == 0 && z % 16 == 0)
                {
                    cpos = new Vector2i(x / 16, z / 16);
                }
                else
                {
                    cpos = new Vector2i(x / 16 - 1, z / 16 - 1);
                }
                    
                if(Math.Abs(z) % 16 == 0 && Math.Abs(x) %16!=0)
                {
                    bpos = new Vector3i(Math.Abs(x % 16 + 16), y, 0);
                }else if(Math.Abs(z) %16!=0 && Math.Abs(x) %16==0)
                {
                    bpos = new Vector3i(0, y, Math.Abs(z % 16 + 16));
                }else if(Math.Abs(z) %16==0 && Math.Abs(x) %16==0)
                {
                    bpos = new Vector3i(0, y, 0);
                }
                else
                {
                    bpos = new Vector3i(Math.Abs(x % 16 + 16), y, Math.Abs(z % 16 + 16));
                }
            }
            else
            {
                cpos = new Vector2i(x / 16, z / 16);
                bpos = new Vector3i(Math.Abs(x % 16), y, Math.Abs(z % 16));
            }
            return (cpos, bpos);
        }
        internal static (Vector2i, Vector3i) GetVoxelCoordChunkRelative(int x, int y, int z)
        {
            Vector2i cpos;
            Vector3i bpos;
            if (x < 0 && z >= 0)
            {
                if (x % 16 == 0)
                {
                    cpos = new Vector2i(x / 16, z / 16);
                }
                else
                {
                    cpos = new Vector2i(x / 16 - 1, z / 16);
                }
                if (Math.Abs(x) % 16 == 0)
                {
                    bpos = new Vector3i(0, y, Math.Abs(z % 16));
                }
                else
                {
                    bpos = new Vector3i(Math.Abs(x % 16), y, Math.Abs(z % 16));
                }

            }
            else if ((x >= 0 && z < 0))
            {
                if (x % 16 == 0)
                {
                    cpos = new Vector2i(x / 16, z / 16);
                }
                else
                {
                    cpos = new Vector2i(x / 16, z / 16 - 1);
                }
                if (Math.Abs(z) % 16 == 0)
                {
                    bpos = new Vector3i(Math.Abs(x % 16), y, 0);
                }
                else
                {
                    bpos = new Vector3i(Math.Abs(x % 16), y, Math.Abs(z % 16));
                }
            }
            else if (x < 0 && z < 0)
            {
                if (x % 16 == 0 && z % 16 != 0)
                {
                    cpos = new Vector2i(x / 16, z / 16 - 1);
                }
                else if (x % 16 != 0 && z % 16 == 0)
                {
                    cpos = new Vector2i(x / 16 - 1, z / 16);
                }
                else if (x % 16 == 0 && z % 16 == 0)
                {
                    cpos = new Vector2i(x / 16, z / 16);
                }
                else
                {
                    cpos = new Vector2i(x / 16 - 1, z / 16 - 1);
                }

                if (Math.Abs(z) % 16 == 0 && Math.Abs(x) % 16 != 0)
                {
                    bpos = new Vector3i(Math.Abs(x % 16), y, 0);
                }
                else if (Math.Abs(z) % 16 != 0 && Math.Abs(x) % 16 == 0)
                {
                    bpos = new Vector3i(0, y, Math.Abs(z % 16));
                }
                else if (Math.Abs(z) % 16 == 0 && Math.Abs(x) % 16 == 0)
                {
                    bpos = new Vector3i(0, y, 0);
                }
                else
                {
                    bpos = new Vector3i(Math.Abs(x % 16), y, Math.Abs(z % 16));
                }
            }
            else
            {
                cpos = new Vector2i(x / 16, z / 16);
                bpos = new Vector3i(Math.Abs(x % 16), y, Math.Abs(z % 16));
            }
            return (cpos, bpos);
        }
    }
}
