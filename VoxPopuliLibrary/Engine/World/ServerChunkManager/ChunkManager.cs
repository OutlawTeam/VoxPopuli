/**
 * Chunk Manager sever side implementation
 * Copyrights Florian Pfeiffer
 * Author Florian Pfeiffer
 */
using OpenTK.Mathematics;
using VoxPopuliLibrary.Engine.Network;
using LiteNetLib;
using VoxPopuliLibrary.Engine.Maths;
using VoxPopuliLibrary.Engine.Player;

namespace VoxPopuliLibrary.Engine.World
{
    internal partial class ServerChunkManager
    {
        internal Dictionary<Vector3i, Chunk> clist = new Dictionary<Vector3i, Chunk>();
        internal Queue<ServerChunkData> ChunkToBeSend = new Queue<ServerChunkData>();
        internal void Update()
        {
            for (int x = 0; x <= 10; x++)
            {
                if (ChunkToBeSend.TryDequeue(out ServerChunkData packet))
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
                                if (!Nothing.PlayerInChunk.Contains(player))
                                {
                                    Nothing.Used = true;
                                    Nothing.PlayerInChunk.Add(player);
                                    ServerChunkData chunkData = new ServerChunkData();
                                    chunkData.x = x;
                                    chunkData.y = y;
                                    chunkData.z = z;
                                    chunkData.data = new ChunkData { data = Nothing.Blocks, pal = Nothing.ChunkPalette };
                                    chunkData.peer = ServerNetwork.server.GetPeerById(player.ClientID);
                                    ChunkToBeSend.Enqueue(chunkData);
                                }
                                else
                                {
                                    Nothing.Used = true;
                                }
                            }
                        }
                    }
                }
            }
            Vector3i[] keys = clist.Keys.ToArray();
            foreach (Vector3i key in keys)
            {
                Chunk ch = clist[key];
                for (int i = 0; i < ch.PlayerInChunk.Count; i++)
                {
                    Player.Player play = ch.PlayerInChunk[i];
                    int minx = (int)(play.Position.X / 16) - ServerWorldManager.world.LoadDistance;
                    int miny = (int)(play.Position.Y / 16) - ServerWorldManager.world.VerticalLoadDistance;
                    int minz = (int)(play.Position.Z / 16) - ServerWorldManager.world.LoadDistance;
                    int maxx = (int)(play.Position.X / 16) + ServerWorldManager.world.LoadDistance;
                    int maxy = (int)(play.Position.Y / 16) + ServerWorldManager.world.VerticalLoadDistance;
                    int maxz = (int)(play.Position.Z / 16) + ServerWorldManager.world.LoadDistance;
                    if (!(ch.Position.X >= minx && ch.Position.Y >= miny && ch.Position.Z >= minz
                        && ch.Position.X <= maxx && ch.Position.Y <= maxy && ch.Position.Z <= maxz)
                        && ch.PlayerInChunk.Contains(play))
                    {
                        UnloadChunk packet = new UnloadChunk { x = key.X, y = key.Y, z = key.Z };
                        ch.PlayerInChunk.Remove(play);
                        ServerNetwork.SendPacket(packet, ServerNetwork.server.GetPeerById(play.ClientID),
                            DeliveryMethod.ReliableOrdered);
                    }
                }
                if (!ch.Used || ch.PlayerInChunk.Count == 0)
                {

                    clist.Remove(key);
                }
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
        internal void HandleBlockChange(OneBlockChangeDemand data, NetPeer peer)
        {

            if (clist.TryGetValue(new Vector3i(data.cx, data.cy, data.cz), out Chunk tempChunk))
            {
                tempChunk.SetBlock(data.bx, data.by, data.bz, data.BlockID);
                OneBlockChange packet = new OneBlockChange
                {
                    cx = data.cx,
                    cy = data.cy,
                    cz = data.cz,
                    bx = data.bx,
                    by = data.by,
                    bz = data.bz,
                    BlockID = data.BlockID
                };
                foreach(Player.Player play in tempChunk.PlayerInChunk)
                {
                    ServerNetwork.SendPacket(packet, ServerNetwork.server.GetPeerById( play.ClientID), DeliveryMethod.ReliableOrdered);
                }
                
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
