/**
 * Player implementation for server side
 * Copyrights Florian Pfeiffer
 * Author Florian Pfeiffer
 */
using LiteNetLib;
using LiteNetLib.Utils;
using OpenTK.Mathematics;
namespace VoxPopuliLibrary.Engine.Player
{
    internal class ServerPlayerFactory
    {
        internal Dictionary<ushort, Player> List = new Dictionary<ushort, Player>();
        internal void AddPlayer(ushort clientId, NetPeer peer)
        {
            Player temp = new Player(new Vector3d(0, 302, 0), clientId, false, false);
            foreach (Player otherPlayer in List.Values)
            {
                if (otherPlayer.ClientID != peer.Id)
                {
                    SendPlayersToClient(otherPlayer, peer);
                }
            }
            List.Add(clientId, temp);
            SendPlayerToLocal(temp, peer);
            SendPlayerToAll(temp, peer);
        }
        internal void RemovePlayer(ushort clientId)
        {
            SendPlayersDecoToClient(clientId);
            List.Remove(clientId);
        }
        public void SendPlayersDecoToClient(ushort ClientId)
        {
            NetDataWriter message = new NetDataWriter();
            message.Put(Convert.ToUInt16(Network.NetworkProtocol.PlayerDeco));
            message.Put(ClientId);
            Network.ServerNetwork.server.SendToAll(message, DeliveryMethod.ReliableOrdered);
        }
        public void SendPlayersToClient(Player et, NetPeer peer)
        {
            NetDataWriter message = new NetDataWriter();
            message.Put(Convert.ToUInt16(Network.NetworkProtocol.PlayerSpawnToClient));
            message.Put(et.ClientID);
            message.Put(et.Position.X);
            message.Put(et.Position.Y);
            message.Put(et.Position.Z);
            peer.Send(message, DeliveryMethod.ReliableUnordered);
        }
        public void SendPlayerToAll(Player et, NetPeer peer)
        {
            NetDataWriter message = new NetDataWriter();
            message.Put(Convert.ToUInt16(Network.NetworkProtocol.PlayerSpawnToClient));
            message.Put(et.ClientID);
            message.Put(et.Position.X);
            message.Put(et.Position.Y);
            message.Put(et.Position.Z);
            Network.ServerNetwork.server.SendToAll(message, DeliveryMethod.ReliableOrdered, peer);
        }
        public void SendPlayerToLocal(Player et, NetPeer peer)
        {
            NetDataWriter message = new NetDataWriter();
            message.Put(Convert.ToUInt16(Network.NetworkProtocol.PlayerLocal));
            message.Put(et.ClientID);
            message.Put(et.Position.X);
            message.Put(et.Position.Y);
            message.Put(et.Position.Z);
            peer.Send(message, DeliveryMethod.ReliableOrdered);
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

        internal void HandleControl(NetDataReader data, NetPeer peer)
        {
            if (List.TryGetValue((ushort)peer.Id, out Player player))
            {
                player.forward = data.GetBool();
                player.backward = data.GetBool();
                player.right = data.GetBool();
                player.left = data.GetBool();
                player.space = data.GetBool();
                player.shift = data.GetBool();
                player.control = data.GetBool();
                player.Rotation = new Vector3(data.GetFloat(), data.GetFloat(), data.GetFloat());
                player.Front = new Vector3(data.GetFloat(), data.GetFloat(), data.GetFloat());
                player.Right = new Vector3(data.GetFloat(), data.GetFloat(), data.GetFloat());
                player.Elevation = data.GetFloat();
                player.Fly = data.GetBool();
            }
        }
        internal void HandlePos(NetDataReader data, NetPeer peer)
        {
            if (List.TryGetValue((ushort)peer.Id, out Player player))
            {
                player.Position = new Vector3d(data.GetDouble(), data.GetDouble(), data.GetDouble());
            }
        }
    }
}
