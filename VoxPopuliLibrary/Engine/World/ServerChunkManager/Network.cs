using K4os.Compression.LZ4;
using LiteNetLib;
using LiteNetLib.Utils;
using OpenTK.Mathematics;
using System.Buffers;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using VoxPopuliLibrary.Engine.Network;
using static OpenTK.Graphics.OpenGL.GL;

namespace VoxPopuliLibrary.Engine.World
{ 
    internal partial class ServerChunkManager
    {
        internal void HandleBlockChange(OneBlockChangeDemand data, NetPeer peer)
        {
            
            if (clist.TryGetValue(new Vector3i(data.cx, data.cy, data.cz), out Chunk tempChunk))
            {
                tempChunk.SetBlock(data.bx, data.cy, data.bz, data.BlockID);
                OneBlockChange packet = new OneBlockChange {cx = data.cx,cy = data.cy,cz =data.cz,
                bx = data.bx,by = data.by,bz = data.bz,BlockID = data.BlockID};
                ServerNetwork.SendPacketToAll(packet,DeliveryMethod.ReliableUnordered);
            }
            else
            {
                tempChunk = new Chunk(new Vector3i(data.cx, data.cy, data.cz));
                clist.Add(new Vector3i(data.cx, data.cy, data.cz), tempChunk);
                clist[new Vector3i(data.cx, data.cy, data.cz)].SetBlock(data.bx, data.by, data.bz, data.BlockID);
            }
        }
    }
}
