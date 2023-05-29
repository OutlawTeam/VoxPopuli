using OpenTK.Mathematics;
using VoxPopuliLibrary.Engine.Maths;

namespace VoxPopuliLibrary.Engine.World
{
    internal partial class ServerChunkManager
    {
        internal Chunk CreateChunk(Vector3i pos)
        {
            Chunk tempChunk = new Chunk(pos);
            clist.Add(pos, tempChunk);
            return tempChunk;
        }
        internal bool GetBlock(int x, int y, int z, out ushort id)
        {
            (Vector3i cpos, Vector3i bpos) = Coord.GetVoxelCoord(x, y, z);
            if (clist.TryGetValue(cpos, out Chunk ch))
            {
                id = ch.GetBlock(bpos.X, bpos.Y, bpos.Z);
                return true;
            }
            else
            {
                id = 0;
                return false;
            }
        }
    }
}
