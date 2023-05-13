using LiteNetLib;
using LiteNetLib.Utils;
using VoxPopuliLibrary.common.ecs.server;
using VoxPopuliLibrary.common.network;
using VoxPopuliLibrary.common.voxel.server;

namespace VoxPopuliLibrary.server.network
{
    internal static class Network
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
                PlayerFactory.AddPlayer(((ushort)peer.Id), peer);       // Send with reliability
                SendVersion(peer);

            };
            listener.NetworkReceiveEvent += (fromPeer, dataReader, deliveryMethod) =>
            {
                ushort messageType = dataReader.GetUShort();
                switch ((NetworkProtocol)messageType)
                {
                    case NetworkProtocol.ChunkDemand:
                        ChunkManager.HandleChunk(dataReader, fromPeer);
                        break;
                    case NetworkProtocol.ChunkOneBlockChangeDemand:
                        ChunkManager.HandleBlockChange(dataReader, fromPeer);
                        break;
                    case NetworkProtocol.PlayerSendControl:
                        PlayerFactory.HandleControl(dataReader, fromPeer);
                        break;
                    case NetworkProtocol.PlayerClientSendPos:
                        PlayerFactory.HandlePos(dataReader, fromPeer);
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
                PlayerFactory.RemovePlayer((ushort)peer.Id);
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
            message.Put(common.Version.VersionNumber);
            peer.Send(message, DeliveryMethod.ReliableOrdered);
        }
    }
}
