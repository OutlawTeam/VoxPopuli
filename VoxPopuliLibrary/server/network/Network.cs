using LiteNetLib;
using LiteNetLib.Utils;
using System.Diagnostics;
using VoxPopuliLibrary.common.ecs.server;
using VoxPopuliLibrary.common.network;
using VoxPopuliLibrary.common.voxel.common;
using VoxPopuliLibrary.common.voxel.server;

namespace VoxPopuliLibrary.server.network
{
    internal static class Network
    {
        internal static NetDataWriter message;
        internal static NetManager server;
        internal static readonly NetPacketProcessor _netPacketProcessor = new NetPacketProcessor();

        static internal void StartServer(int Port)
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
            };
            listener.NetworkReceiveEvent += (fromPeer, dataReader, deliveryMethod) =>
            {
                ushort messageType = dataReader.GetUShort();
                switch ((NetworkProtocol)messageType)
                {
                    case NetworkProtocol.ChunkDemand:
                        Chunk_Manager.HandleChunk(dataReader, fromPeer);
                        break;
                    case NetworkProtocol.ChunkOneBlockChangeDemand:
                        Chunk_Manager.HandleBlockChange(dataReader, fromPeer);
                        break;
                    case NetworkProtocol.PlayerSendControl:
                        PlayerFactory.HandleControl(dataReader, fromPeer);
                        break;
                    case NetworkProtocol.PlayerClientSendPos:
                        PlayerFactory.HandlePos(dataReader,fromPeer);
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
    }
}
