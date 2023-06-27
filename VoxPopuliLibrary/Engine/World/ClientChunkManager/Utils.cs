using LiteNetLib;
using LiteNetLib.Utils;
using OpenTK.Mathematics;
using VoxPopuliLibrary.Engine.Network;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VoxPopuliLibrary.Engine.World
{
    internal partial class ClientChunkManager
    {
        internal string GetBlockForMesh(Vector3i bpos , Vector3i cpos)
        {
            Vector3i Rpos = cpos;
            if(bpos.X > 15)
            {
                Rpos.X += 1;
                bpos.X = 0;
            }else if(bpos.X <0)
            { 
                Rpos.X -= 1;
                bpos.X = 15;
            }
            if (bpos.Y > 15)
            {
                Rpos.Y += 1;
                bpos.Y= 0;
            }
            else if (bpos.Y < 0)
            {
                Rpos.Y -= 1;
                bpos.Y = 15;
            }
            if (bpos.Z > 15)
            {
                Rpos.Z += 1;
                bpos.Z = 0;
            }
            else if (bpos.Z < 0)
            {
                Rpos.Z -= 1;
                bpos.Z = 15;
            }
            try
            {
                return GetChunk(Rpos.X,Rpos.Y,Rpos.Z).GetBlock(bpos.X,bpos.Y,bpos.Z);
            }catch(Exception ex)
            {
                return "air";
            }
        }
        internal Chunk GetChunk(int x, int y, int z)
        {
            if (Clist.TryGetValue(new Vector3i(x, y, z), out Chunk ch))
            {
                return ch;
            }
            else
            {
                byte zero = 0;
                return new Chunk(Enumerable.Repeat(zero, 4096).ToArray(), new Palette(), new Vector3i(x, y, z));
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
