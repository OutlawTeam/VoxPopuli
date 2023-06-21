using OpenTK.Mathematics;

namespace VoxPopuliLibrary.Engine.Maths
{
    internal static class Coord
    {
        internal static (Vector3i, Vector3i) GetVoxelCoord(int x, int y, int z)
        {
            Vector3i cpos;
            Vector3i bpos;
            int blockX = Math.Abs(x % 16);
            int blockY = Math.Abs(y % 16);
            int blockZ = Math.Abs(z % 16);

            if (x < 0 && z >= 0 && y >= 0)
            {
                cpos = new Vector3i(x / 16 - (x % 16 == 0 ? 0 : 1), y / 16, z / 16);
                bpos = new Vector3i(blockX == 0 ? 0 : 16 - blockX, blockY, blockZ);
            }
            else if (x >= 0 && z < 0 && y >= 0)
            {
                cpos = new Vector3i(x / 16, y / 16, z / 16 - (z % 16 == 0 ? 0 : 1));
                bpos = new Vector3i(blockX, blockY, blockZ == 0 ? 0 : 16 - blockZ);
            }
            else if (x < 0 && z < 0 && y >= 0)
            {
                cpos = new Vector3i(x / 16 - (x % 16 == 0 ? 0 : 1), y / 16, z / 16 - (z % 16 == 0 ? 0 : 1));
                bpos = new Vector3i(blockX == 0 ? 0 : 16 - blockX, blockY, blockZ == 0 ? 0 : 16 - blockZ);
            }
            else if (x < 0 && z >= 0 && y < 0)
            {
                cpos = new Vector3i(x / 16 - (x % 16 == 0 ? 0 : 1), y / 16 - (y % 16 == 0 ? 0 : 1), z / 16);
                bpos = new Vector3i(blockX == 0 ? 0 : 16 - blockX, blockY == 0 ? 0 : 16 - blockY, blockZ);
            }
            else if (x >= 0 && z < 0 && y < 0)
            {
                cpos = new Vector3i(x / 16, y / 16 - (y % 16 == 0 ? 0 : 1), z / 16 - (z % 16 == 0 ? 0 : 1));
                bpos = new Vector3i(blockX, blockY == 0 ? 0 : 16 - blockY, blockZ == 0 ? 0 : 16 - blockZ);
            }
            else if (x < 0 && z < 0 && y < 0)
            {
                cpos = new Vector3i(x / 16 - (x % 16 == 0 ? 0 : 1), y / 16 - (y % 16 == 0 ? 0 : 1), z / 16 - (z % 16 == 0 ? 0 : 1));
                bpos = new Vector3i(blockX == 0 ? 0 : 16 - blockX, blockY == 0 ? 0 : 16 - blockY, blockZ == 0 ? 0 : 16 - blockZ);
            }
            else if (x >= 0 && z >= 0 && y < 0)
            {
                cpos = new Vector3i(x / 16 , y / 16 - (y % 16 == 0 ? 0 : 1), z / 16);
                bpos = new Vector3i(blockX , blockY == 0 ? 0 : 16 - blockY, blockZ );
            }
            else
            {
                cpos = new Vector3i(x / 16, y / 16, z / 16);
                bpos = new Vector3i(blockX, blockY, blockZ);
            }

            return (cpos, bpos);
        }

    }
}
