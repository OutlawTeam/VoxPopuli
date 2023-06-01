/**
 * Chunk Manager sever side implementation
 * Copyrights Florian Pfeiffer
 * Author Florian Pfeiffer
 */
using OpenTK.Mathematics;
using VoxPopuliLibrary.Engine.Network;
using LiteNetLib;
using VoxPopuliLibrary.Engine.Maths;
using System.Net.Sockets;

namespace VoxPopuliLibrary.Engine.World
{
    internal partial class ServerChunkManager
    {
        internal Dictionary<Vector3i, Chunk> clist = new Dictionary<Vector3i, Chunk>();
        internal Queue<ServerChunkData> ChunkToBeSend = new Queue<ServerChunkData>();
        internal void Update()
        {
            for(int x= 0;x <=10;x++)
            {
                if(ChunkToBeSend.TryDequeue(out ServerChunkData packet))
                {
                    ServerNetwork.SendPacket(packet, packet.peer, DeliveryMethod.ReliableOrdered);
                }
            }
            foreach (Player.Player player in ServerWorldManager.world.GetPlayerFactoryServer().List.Values)
            {
                int minx = (int)(player.Position.X / 16) - ServerWorldManager.world.LoadDistance;
                int miny = (int)(player.Position.Y / 16) - ServerWorldManager.world.VerticalLoadDistance;
                int minz = (int)(player.Position.Z / 16) - ServerWorldManager.world.LoadDistance;
                int maxx = (int)(player.Position.X / 16) + ServerWorldManager.world.LoadDistance;
                int maxy = (int)(player.Position.Y / 16) + ServerWorldManager.world.VerticalLoadDistance;
                int maxz = (int)(player.Position.Z / 16) + ServerWorldManager.world.LoadDistance;
                for (int x = minx; x <= maxx; x++)
                {
                    for (int y = miny; y <= maxy; y++)
                    {
                        for (int z = minz; z <= maxz; z++)
                        {
                            if (!clist.TryGetValue(new Vector3i(x, y, z), out Chunk Nothing))
                            {
                                Chunk tempChunk = new Chunk(new Vector3i(x, y, z));
                                tempChunk.Used = true;
                                tempChunk.PlayerInChunk.Add(player);
                                clist.Add(new Vector3i(x, y, z), tempChunk);
                                ServerChunkData chunkData = new ServerChunkData();
                                chunkData.x = x;
                                chunkData.y = y;
                                chunkData.z = z;
                                chunkData.data = new ChunkData { data = tempChunk.Blocks, pal = tempChunk.ChunkPalette };
                                chunkData.peer = ServerNetwork.server.GetPeerById(player.ClientID);
                                ChunkToBeSend.Enqueue(chunkData);
                            }
                            else
                            {
                                Nothing.Used = true;
                                Nothing.PlayerInChunk.Add(player);
                            }
                        }
                    }
                }
            }
            Vector3i[] keys = clist.Keys.ToArray();
            foreach(Vector3i key in keys)
            {
                Chunk ch = clist[key];
                if(!ch.Used)
                {
                    UnloadChunk packet = new UnloadChunk {x= key.X,y=key.Y,z=key.Z };
                    foreach(Player.Player play in ch.PlayerInChunk)
                    {
                        ServerNetwork.SendPacket(packet,ServerNetwork.server.GetPeerById(play.ClientID),
                            DeliveryMethod.ReliableOrdered);
                    }
                    clist.Remove(key);
                }
                ch.PlayerInChunk.Clear();
                ch.Used = false;
            }
        }
        internal bool GetBlock(int x, int y, int z, out string id)
        {
            (Vector3i cpos, Vector3i bpos) = Coord.GetVoxelCoord(x, y, z);
            if (clist.TryGetValue(cpos, out Chunk ch))
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
