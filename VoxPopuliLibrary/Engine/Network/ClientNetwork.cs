﻿/**
 * Client Network Manager
 * Copyrights Florian Pfeiffer
 * Author Florian Pfeiffer
 * */
using LiteNetLib;
using LiteNetLib.Utils;
using VoxPopuliLibrary.Engine.API;
using VoxPopuliLibrary.Engine.Player;
using VoxPopuliLibrary.Engine.World;

namespace VoxPopuliLibrary.Engine.Network
{
    /// <summary>
    /// The network manager for client
    /// </summary>
    public static class ClientNetwork
    {
        static EventBasedNetListener listener = new EventBasedNetListener();
        internal static NetManager client = new NetManager(listener);
        internal static NetDataWriter message = new NetDataWriter();
        private static NetPacketProcessor packetProcessor;
        internal static NetPeer Server;
        internal static string ServerAPIVersion = "NotConnected";
        internal static string ServerEngineVersion = "NotConnected";
        internal static void Init()
        {
            client.DisconnectTimeout = 10000;
            client.Start();
            packetProcessor = new NetPacketProcessor();
            //
            //Network
            //
            //
            //API
            //
            packetProcessor.RegisterNestedType<ServerInitialPacket>();
            packetProcessor.RegisterNestedType<ClientInitialPacket>();
            packetProcessor.SubscribeNetSerializable<ServerInitialPacket>(HandleInitialPacket);
            

            listener.PeerConnectedEvent += (server) =>
            {
                Server = server;
                ClientWorldManager.InitWorld();
                //
                //ChunkManager
                //
                packetProcessor.RegisterNestedType<OneBlockChangeDemand>();
                packetProcessor.SubscribeNetSerializable<ServerChunkData, NetPeer>(ClientWorldManager.world.GetChunkManagerClient().HandleChunk);
                packetProcessor.SubscribeNetSerializable<OneBlockChange, NetPeer>(ClientWorldManager.world.GetChunkManagerClient().HandleChunkUpdate);
                packetProcessor.SubscribeNetSerializable<UnloadChunk, NetPeer>(ClientWorldManager.world.GetChunkManagerClient().HandleChunkUnload);
                //
                //Player
                //
                packetProcessor.RegisterNestedType<PlayerControl>();
                packetProcessor.RegisterNestedType<PlayerPositionTP>();
                packetProcessor.SubscribeNetSerializable<PlayerData, NetPeer>(ClientWorldManager.world.GetPlayerFactoryClient().HandleData);
                packetProcessor.SubscribeNetSerializable<PlayerSpawn, NetPeer>(ClientWorldManager.world.GetPlayerFactoryClient().HandleSpawn);
                packetProcessor.SubscribeNetSerializable<PlayerSpawnLocal, NetPeer>(ClientWorldManager.world.GetPlayerFactoryClient().HandleLocalPlayer);
                packetProcessor.SubscribeNetSerializable<PlayerDeco, NetPeer>(ClientWorldManager.world.GetPlayerFactoryClient().HandleDeco);

                ClientInitialPacket packet = new ClientInitialPacket{ Name = Account.Name};


                SendPacket(packet,DeliveryMethod.ReliableOrdered);
            };
            listener.NetworkReceiveEvent += (fromPeer, dataReader, deliveryMethod, Nothing) =>
            {
                packetProcessor.ReadAllPackets(dataReader,fromPeer);
                dataReader.Recycle();
            };
        }
        /// <summary>
        /// Send a packet to the server
        /// </summary>
        /// <typeparam name="T">A struct who implement INetSerialize</typeparam>
        /// <param name="packet">The packet variable</param>
        /// <param name="deliveryMethod">DeliveryMethod</param>
        public static void SendPacket<T>(T packet, DeliveryMethod deliveryMethod) where T : INetSerializable
        {
            if (Server != null)
            {
                message.Reset();
                packetProcessor.WriteNetSerializable(message, ref packet);
                Server.Send(message, deliveryMethod);
            }
        }
        internal static void HandleInitialPacket(ServerInitialPacket data)
        {
            ServerEngineVersion = data.EngineVersion;
            ServerAPIVersion = data.APIVersion;
        }
        internal static void Connect(string ip, int port)
        {
            client.Connect(ip, port, message);
        }
        internal static void Update()
        {
            client.PollEvents();
        }
    }
}