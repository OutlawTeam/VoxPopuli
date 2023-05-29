using LiteNetLib;
using LiteNetLib.Utils;
using VoxPopuliLibrary.Engine.World;

namespace VoxPopuliLibrary.Engine.Network
{
    internal static class ServerNetwork
    {
        internal static NetDataWriter message;
        internal static NetManager server;
        internal static readonly NetPacketProcessor _netPacketProcessor = new NetPacketProcessor();

        internal static void StartServer(int Port)
        {
            message = new NetDataWriter();
            EventBasedNetListener listener = new EventBasedNetListener();
            server = new NetManager(listener);
            server.DisconnectTimeout = 60000;


            listener.ConnectionRequestEvent += request =>
            {
                request.Accept();
            };
            listener.PeerConnectedEvent += peer =>
            {
                Console.WriteLine("A player was connectected: {0}", peer.EndPoint); // Show peer ip
                ServerWorldManager.world.GetPlayerFactoryServer().AddPlayer((ushort)peer.Id, peer);       // Send with reliability
                SendVersion(peer);
            };
            listener.NetworkReceiveEvent += (fromPeer, dataReader, deliveryMethod, Nothing) =>
            {
                ushort messageType = dataReader.GetUShort();
                switch ((NetworkProtocol)messageType)
                {
                    case NetworkProtocol.ChunkDemand:
                        ServerWorldManager.world.GetChunkManagerServer().HandleChunk(dataReader, fromPeer);
                        break;
                    case NetworkProtocol.ChunkOneBlockChangeDemand:
                        ServerWorldManager.world.GetChunkManagerServer().HandleBlockChange(dataReader, fromPeer);
                        break;
                    case NetworkProtocol.PlayerSendControl:
                        ServerWorldManager.world.GetPlayerFactoryServer().HandleControl(dataReader, fromPeer);
                        break;
                    case NetworkProtocol.PlayerClientSendPos:
                        ServerWorldManager.world.GetPlayerFactoryServer().HandlePos(dataReader, fromPeer);
                        break;
                    default:
                        // handle unknown value
                        break;
                }
                dataReader.Recycle();
            };
            listener.PeerDisconnectedEvent += (peer, disconnectInfo) =>
            {
                Console.WriteLine("A player was deconnectected: {0} , for this reason {1}", peer.EndPoint, disconnectInfo.Reason); // Show peer ip
                ServerWorldManager.world.GetPlayerFactoryServer().RemovePlayer((ushort)peer.Id);
            };
            server.Start(Port);
        }
        internal static void Update()
        {
            server.PollEvents();
        }
        internal static void SendVersion(NetPeer peer)
        {
            NetDataWriter message = new NetDataWriter();
            message.Put(Convert.ToUInt16(NetworkProtocol.ServerVersionSend));
            message.Put(API.Version.EngineVersion);
            message.Put(API.Version.GameVersion);
            peer.Send(message, DeliveryMethod.ReliableOrdered);
        }
    }
}
