using LiteNetLib;
using LiteNetLib.Utils;
using VoxPopuliLibrary.Engine.API;
using VoxPopuliLibrary.Engine.Player;
using VoxPopuliLibrary.Engine.World;
namespace VoxPopuliLibrary.Engine.Network
{
    /// <summary>
    /// The network manager for server
    /// </summary>
    public static class ServerNetwork
    {
        internal static NetDataWriter message= new NetDataWriter();
        internal static NetManager server;
        internal static readonly NetPacketProcessor PacketProcessor = new NetPacketProcessor();
        internal static void StartServer(int Port)
        {
            EventBasedNetListener listener = new EventBasedNetListener();
            server = new NetManager(listener) {DisconnectTimeout = 10000};
            //
            //API
            //
            PacketProcessor.RegisterNestedType<ServerInitialPacket>();
            PacketProcessor.RegisterNestedType<ClientInitialPacket>();
            PacketProcessor.SubscribeNetSerializable<ClientInitialPacket,NetPeer>(ClientInitialPacket);
            //
            //ChunkManager
            //
            PacketProcessor.RegisterNestedType<OneBlockChange>();
            PacketProcessor.RegisterNestedType<ServerChunkData>();
            PacketProcessor.RegisterNestedType<UnloadChunk>();
            PacketProcessor.SubscribeNetSerializable<OneBlockChangeDemand,NetPeer>(
                ServerWorldManager.world.GetChunkManagerServer().HandleBlockChange);
            //
            //Player
            //
            PacketProcessor.RegisterNestedType<PlayerData>();
            PacketProcessor.RegisterNestedType<PlayerSpawn>();
            PacketProcessor.RegisterNestedType<PlayerSpawnLocal>();
            PacketProcessor.RegisterNestedType<PlayerDeco>();
            PacketProcessor.SubscribeNetSerializable<PlayerControl, NetPeer>(ServerWorldManager.world.GetPlayerFactoryServer().HandleControl);
            PacketProcessor.SubscribeNetSerializable<PlayerPositionTP, NetPeer>(ServerWorldManager.world.GetPlayerFactoryServer().HandlePos);

            listener.ConnectionRequestEvent += request=>
            {
                request.Accept();
            };
            listener.PeerConnectedEvent += peer =>
            {
                Console.WriteLine("A player was connectected: {0}", peer.EndPoint); // Show peer ip
                ServerWorldManager.world.GetPlayerFactoryServer().AddPlayer((ushort)peer.Id, peer);       // Send with reliability
                ServerInitialPacket packet = new ServerInitialPacket
                {
                    EngineVersion = API.Version.EngineVersion,
                    APIVersion = API.Version.APIVersion
                };
                SendPacket(packet, peer, DeliveryMethod.ReliableOrdered);
            };
            listener.NetworkReceiveEvent += (fromPeer, dataReader, deliveryMethod, Nothing) =>
            {
                PacketProcessor.ReadAllPackets(dataReader,fromPeer);
                dataReader.Recycle();
            };
            listener.PeerDisconnectedEvent += (peer, disconnectInfo) =>
            {
                Console.WriteLine("A player was deconnectected: {0} , for this reason {1}", peer.EndPoint, disconnectInfo.Reason); // Show peer ip
                ServerWorldManager.world.GetPlayerFactoryServer().RemovePlayer((ushort)peer.Id);
            };
            server.Start(Port);
        }
        /// <summary>
        /// Send packet to a specific client
        /// </summary>
        /// <typeparam name="T">A struct who implement INetSerialize</typeparam>
        /// <param name="packet">The packet</param>
        /// <param name="peer">The peer</param>
        /// <param name="deliveryMethod">The DeliveryMethod</param>
        public static void SendPacket<T>(T packet, NetPeer peer, DeliveryMethod deliveryMethod) where T : INetSerializable
        {
            if (peer != null)
            {
                message.Reset();
                PacketProcessor.WriteNetSerializable(message, ref packet);
                peer.Send(message, deliveryMethod);
            }
        }
        /// <summary>
        /// Send packet to all client
        /// </summary>
        /// <typeparam name="T">A struct who implement INetSerialize</typeparam>
        /// <param name="packet">The packet</param>
        /// <param name="deliveryMethod">The DeliveryMethod</param>
        public static void SendPacketToAll<T>(T packet, DeliveryMethod deliveryMethod) where T : INetSerializable
        {
            message.Reset();
            PacketProcessor.WriteNetSerializable(message, ref packet);
            server.SendToAll(message, deliveryMethod);
        }
        /// <summary>
        /// Send a packet to all client exclude one
        /// </summary>
        /// <typeparam name="T">A struct who implement INetSerialize</typeparam>
        /// <param name="packet">The packet</param>
        /// <param name="peer">The excluded peer</param>
        /// <param name="deliveryMethod">The DeliveryMethod</param>
        public static void SendPacketToAllWithoutOnePeer<T>(T packet, NetPeer peer,DeliveryMethod deliveryMethod) where T : INetSerializable
        {
            message.Reset();
            PacketProcessor.WriteNetSerializable(message, ref packet);
            server.SendToAll(message, deliveryMethod,peer);
        }
        internal static void ClientInitialPacket(ClientInitialPacket data, NetPeer peer)
        {
            if(ServerWorldManager.world.GetPlayerFactoryServer().List.TryGetValue((ushort)peer.Id,out Player.Player play))
            {
                play.Name = data.Name;
            }
        }
        internal static void Update()
        {
            server.PollEvents();
        }
    }
}
