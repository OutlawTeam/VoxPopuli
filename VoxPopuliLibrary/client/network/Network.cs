/**
 * Client Network Manager
 * Copyrights Florian Pfeiffer
 * Author Florian Pfeiffer
 * */
using LiteNetLib;
using LiteNetLib.Utils;
using VoxPopuliLibrary.common.ecs.client;
using VoxPopuliLibrary.common.network;
using VoxPopuliLibrary.common.voxel.client;
namespace VoxPopuliLibrary.client.network
{
    public class Network
    {
        //Message listener
        static EventBasedNetListener listener = new EventBasedNetListener();
        //Client
        internal static NetManager client = new NetManager(listener);
        //Client Id
        public static int id;
        //Server peer
        public static NetPeer Server;
        public static string ServerVersion = "NotConnected";

        internal static void ServerDisconnectSequence()
        {
            PlayerFactory.LocalPlayer = default;
            PlayerFactory.LocalPlayerExist = false;
            PlayerFactory.PlayerList.Clear();
            ChunkManager.ClearAllChunk();
        }

        /// <summary>
        /// Init Network Manager
        /// </summary>
        public static void Init()
        {
            client.DisconnectTimeout = 60000;
            client.Start();
            listener.PeerConnectedEvent += (server) =>
            {
                Server = server;
            };

            listener.NetworkReceiveEvent += (fromPeer, dataReader, deliveryMethod) =>
            {

                ushort messageType = dataReader.GetUShort();
                switch ((NetworkProtocol)messageType)
                {
                    case NetworkProtocol.ChunkData:
                        ChunkManager.HandleChunk(dataReader, fromPeer);
                        break;
                    case NetworkProtocol.PlayerSpawnToClient:
                        PlayerFactory.HandleSpawn(dataReader, fromPeer);
                        break;
                    case NetworkProtocol.ChunkOneBlockChange:
                        ChunkManager.HandleChunkUpdate(dataReader);
                        break;
                    case NetworkProtocol.PlayerPosition:
                        PlayerFactory.HandleData(dataReader, fromPeer);
                        break;
                    case NetworkProtocol.PlayerDeco:
                        PlayerFactory.HandleDeco(dataReader, fromPeer);
                        break;
                    case NetworkProtocol.PlayerLocal:
                        PlayerFactory.AddLocalPlayer(dataReader, fromPeer);
                        break;
                    case NetworkProtocol.ServerVersionSend:
                        HandleVersion(dataReader, fromPeer);
                        break;
                    default:
                        // handle unknown value
                        break;
                }
                dataReader.Recycle();
            };
        }
        internal static void HandleVersion(NetDataReader data, NetPeer peer)
        {
            ServerVersion = data.GetString();
        }
        /// <summary>
        /// Connect the client to the server
        /// </summary>
        /// <param name="ip">Ip of the Server</param>
        public static void Connect(string ip, int port)
        {
            client.Connect(ip, port, "");
        }
        public static void DeConnect()
        {
            if (Server != null)
            {
                Server.Disconnect();
            }
        }
        /// <summary>
        /// Update Network Manager
        /// </summary>
        public static void Update()
        {
            client.PollEvents();
        }
    }
}