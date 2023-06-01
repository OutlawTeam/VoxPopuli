using LiteNetLib;
using LiteNetLib.Utils;
using OpenTK.Mathematics;
using VoxPopuliLibrary.Engine.Network;

namespace VoxPopuliLibrary.Engine.World
{
    internal partial class ClientChunkManager
    {
        internal Chunk GetChunk(int x, int y, int z)
        {
            if (Clist.TryGetValue(new Vector3i(x, y, z), out Chunk ch))
            {
                return ch;
            }
            else
            {
                throw new Exception("Chunk is not loaded");
            }
        }
        internal void ChangeChunk(Vector3d blockp, string block)
        {
            (Vector3i cpos, Vector3i bpos) = Maths.Coord.GetVoxelCoord((int)blockp.X, (int)blockp.Y, (int)blockp.Z);
            OneBlockChangeDemand packet = new OneBlockChangeDemand
            {
                BlockID = block,
                cx = cpos.X,
                cy = cpos.Y,
                cz = cpos.Z,
                bx = bpos.X,
                by = bpos.Y,
                bz = bpos.Z
            };
            ClientNetwork.SendPacket(packet, DeliveryMethod.ReliableOrdered);
        }
        internal bool GetBlock(int x, int y, int z, out string id)
        {
            (Vector3i cpos, Vector3i bpos) = Maths.Coord.GetVoxelCoord(x, y, z);
            if (Clist.TryGetValue(cpos, out Chunk ch))
            {

                id = ch.GetBlock(bpos.X, bpos.Y, bpos.Z);
                return true;
            }
            else
            {
                id = "air";
                return false;
            }
        }
    }
}
