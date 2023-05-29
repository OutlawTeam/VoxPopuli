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
        internal void ChangeChunk(Vector3d blockp, ushort block)
        {
            (Vector3i cpos, Vector3i bpos) = Maths.Coord.GetVoxelCoord((int)blockp.X, (int)blockp.Y, (int)blockp.Z);
            var message = new NetDataWriter();
            message.Put(Convert.ToUInt16(NetworkProtocol.ChunkOneBlockChangeDemand));
            message.Put(cpos.X);
            message.Put(cpos.Y);
            message.Put(cpos.Z);
            message.Put(bpos.X);
            message.Put(bpos.Y);
            message.Put(bpos.Z);
            message.Put(block);
            if (ClientNetwork.client.FirstPeer != null)
            {
                ClientNetwork.client.FirstPeer.Send(message, DeliveryMethod.ReliableUnordered);
            }
        }
        internal bool GetBlock(int x, int y, int z, out ushort id)
        {
            (Vector3i cpos, Vector3i bpos) = Maths.Coord.GetVoxelCoord(x, y, z);
            if (Clist.TryGetValue(cpos, out Chunk ch))
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
