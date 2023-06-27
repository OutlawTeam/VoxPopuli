/**
 * Player implementation for server side
 * Copyrights Florian Pfeiffer
 * Author Florian Pfeiffer
 */
using LiteNetLib;
using OpenTK.Mathematics;
using System.Net.Sockets;
using VoxPopuliLibrary.Engine.Network;
using VoxPopuliLibrary.Engine.World;

namespace VoxPopuliLibrary.Engine.Player
{
    internal class ServerPlayerFactory
    {
        internal Dictionary<ushort, Player> List = new Dictionary<ushort, Player>();
        internal void AddPlayer(ushort clientId, NetPeer peer)
        {
            Player temp = new Player(new Vector3d(0.5,ServerWorldManager.world.WorldGen.GetOrigin()+1,0.5), clientId, false, false);
            foreach (Player otherPlayer in List.Values)
            {
                if (otherPlayer.ClientID != peer.Id)
                {
                    PlayerSpawn packet = new PlayerSpawn { ClientID = otherPlayer.ClientID, Position = otherPlayer.Position };
                    ServerNetwork.SendPacket(packet, peer, DeliveryMethod.ReliableOrdered);
                }
            }
            List.Add(clientId, temp);
            PlayerSpawnLocal packets = new PlayerSpawnLocal { ClientID = temp.ClientID, Position = temp.Position };
            ServerNetwork.SendPacket(packets, peer, DeliveryMethod.ReliableOrdered);

            PlayerSpawn packety = new PlayerSpawn { ClientID = temp.ClientID, Position = temp.Position };
            ServerNetwork.SendPacketToAllWithoutOnePeer(packety,peer, DeliveryMethod.ReliableOrdered);
            if(ServerWorldManager.world.GetChunkManagerServer().clist.TryGetValue((Vector3i)temp.Position/16,out var ch))
            {
                ch.PlayerInChunk.Add(temp);
                ServerChunkData chunkData = new ServerChunkData();
                chunkData.importance = 0;
                chunkData.x = ch.Position.X;
                chunkData.y = ch.Position.Y;
                chunkData.z = ch.Position.Z;
                chunkData.data = new ChunkData { data = ch.Blocks, pal = ch.ChunkPalette };
                ServerNetwork.SendPacket(chunkData, peer, DeliveryMethod.ReliableOrdered);
            }
            else
            {
                var chs =ServerWorldManager.world.GetChunkManagerServer().CreateChunk((Vector3i)temp.Position / 16);
                chs.PlayerInChunk.Add(temp);
                ServerChunkData chunkData = new ServerChunkData();
                chunkData.importance = 0;
                chunkData.x = chs.Position.X;
                chunkData.y = chs.Position.Y;
                chunkData.z = chs.Position.Z;
                chunkData.data = new ChunkData { data = chs.Blocks, pal = chs.ChunkPalette };
                ServerNetwork.SendPacket(chunkData, peer, DeliveryMethod.ReliableOrdered);
            }
        }
        internal void RemovePlayer(ushort clientId)
        {
            PlayerDeco packet = new PlayerDeco {  ClientID = clientId };
            ServerNetwork.SendPacketToAll(packet, DeliveryMethod.ReliableOrdered);
            List.Remove(clientId);
        }
        internal void Update(float DT)
        {
            foreach (Player player in List.Values)
            {
                player.UpdateServer(DT);
            }
        }
        internal void SendData()
        {
            foreach (Player player in List.Values)
            {
                player.SendData();
            }
        }

        internal void HandleControl(PlayerControl data, NetPeer peer)
        {
            if (List.TryGetValue((ushort)peer.Id, out Player player))
            {
                player.forward = data.Forward;
                player.backward = data.Backward;
                player.right = data.Right;
                player.left = data.Left;
                player.space = data.Space;
                player.shift = data.Shift;
                player.control = data.Control;
                player.Rotation = data.Rotation;
                player.Front = data.Front;
                player.Right = data.CRight;
                player.Elevation = data.Elevation;
                player.Fly = data.Fly;
                player.UpdateServer(data.Dt);
            }
        }
        internal void HandlePos(PlayerPositionTP data, NetPeer peer)
        {
            if (List.TryGetValue((ushort)peer.Id, out Player player))
            {
                player.Position = data.Position;
            }
        }
    }
}
