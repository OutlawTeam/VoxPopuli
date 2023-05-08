using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxPopuliLibrary.common.math
{
    internal static class Coord
    {
        internal static (Vector2i,Vector3i) GetVoxelCoord(Vector3d pos)
        {
            Vector2i cpos;
            if (pos.X < 0 && pos.Z >= 0)
            {
                cpos = new Vector2i((int)(pos.X / 16) - 1, (int)(pos.Z / 16));
            }
            else if ((pos.X >= 0 && pos.Z < 0))
            {
                cpos = new Vector2i((int)(pos.X / 16), (int)(pos.Z / 16) - 1);
            }
            else if (pos.X < 0 && pos.Z < 0)
            {
                cpos = new Vector2i((int)(pos.X / 16) - 1, (int)(pos.Z / 16) - 1);
            }
            else
            {
                cpos = new Vector2i((int)(pos.X / 16), (int)(pos.Z / 16));
            }
            Vector3i bpos = new Vector3i(Math.Abs((int)pos.X % 16), (int)pos.Y  , Math.Abs((int)pos.Z % 16));
            return(cpos,bpos);
        }
        internal static (Vector2i,Vector3i) GetVoxelCoord(int x,int y,int z)
        {
            Vector2i cpos;
            if (x < 0 && z >= 0)
            {
                cpos = new Vector2i((int)(x / 16) - 1, (int)(z / 16));
            }
            else if ((x >= 0 && z < 0))
            {
                cpos = new Vector2i((int)(x / 16), (int)(z / 16) - 1);
            }
            else if (x < 0 && z < 0)
            {
                cpos = new Vector2i((int)(x / 16) - 1, (int)(z / 16) - 1);
            }
            else
            {
                cpos = new Vector2i((int)(x / 16), (int)(z / 16));
            }
            Vector3i bpos = new Vector3i(Math.Abs((int)x % 16), (int)y, Math.Abs((int)z % 16));
            return(cpos,bpos);
        }
    }
}
