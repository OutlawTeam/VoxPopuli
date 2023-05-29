/**
 * Client Network Manager
 * Copyrights Florian Pfeiffer
 * Author Florian Pfeiffer
 * */
using LiteNetLib;
using LiteNetLib.Utils;
using VoxPopuliLibrary.Engine.World;

namespace VoxPopuliLibrary.Engine.Network
{
    public class ClientNetwork
    {
        //Message listener
        static EventBasedNetListener listener = new EventBasedNetListener();
        //Client
        internal static NetManager client = new NetManager(listener);
        //Client Id
        public static int id;
        //Server peer
        public static NetPeer Server;
        public static string ServerGameVersion = "NotConnected";
        public static string ServerEngineVersion = "NotConnected";
        /// <summary>
        /// Init Network Manager
        /// </summary>
        public static void Init()
        {
            client.DisconnectTimeout = 10000;
            client.Start();
            listener.PeerConnectedEvent += (server) =>
            {
                Server = server;
                ClientWorldManager.InitWorld();
            };
            listener.NetworkReceiveEvent += (fromPeer, dataReader, deliveryMethod, Nothing) =>
            {
                ushort messageType = dataReader.GetUShort();
                if (ClientWorldManager.Initialized)
                {
                    switch ((NetworkProtocol)messageType)
                    {
                        case NetworkProtocol.ChunkData:
                            ClientWorldManager.world.GetChunkManagerClient().HandleChunk(dataReader, fromPeer);
                            break;
                        case NetworkProtocol.PlayerSpawnToClient:
                            ClientWorldManager.world.GetPlayerFactoryClient().HandleSpawn(dataReader, fromPeer);
                            break;
                        case NetworkProtocol.ChunkOneBlockChange:
                            ClientWorldManager.world.GetChunkManagerClient().HandleChunkUpdate(dataReader);
                            break;
                        case NetworkProtocol.PlayerPosition:
                            ClientWorldManager.world.GetPlayerFactoryClient().HandleData(dataReader, fromPeer);
                            break;
                        case NetworkProtocol.PlayerDeco:
                            ClientWorldManager.world.GetPlayerFactoryClient().HandleDeco(dataReader, fromPeer);
                            break;
                        case NetworkProtocol.PlayerLocal:
                            ClientWorldManager.world.GetPlayerFactoryClient().AddLocalPlayer(dataReader, fromPeer);
                            break;
                        case NetworkProtocol.ServerVersionSend:
                            HandleVersion(dataReader, fromPeer);
                            break;
                        default:
                            // handle unknown value
                            break;
                    }
                }
                dataReader.Recycle();
            };
        }
        internal static void HandleVersion(NetDataReader data, NetPeer peer)
        {
            ServerEngineVersion = data.GetString();
            ServerGameVersion = data.GetString();
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