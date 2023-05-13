/**
 * Player implementation for client side
 * Copyrights Florian Pfeiffer
 * Author Florian Pfeiffer
 */
using LiteNetLib;
using LiteNetLib.Utils;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace VoxPopuliLibrary.common.ecs.client
{
    public static class PlayerFactory
    {
        internal static readonly Dictionary<ushort, Player> PlayerList = new Dictionary<ushort, Player>();
        internal static Player LocalPlayer;
        internal static bool LocalPlayerExist = false;
        internal static void AddPlayer(ushort clientId, Vector3d position, bool Local)
        {
            Player temp = new Player(position, clientId, Local, true);
            if (Local)
            {
                LocalPlayer = temp;
                LocalPlayerExist = true;
            }
            PlayerList.Add(clientId, temp);
        }
        internal static void Update(float DT, KeyboardState Keyboard, MouseState Mouse,bool Grabed)
        {
            if (LocalPlayerExist)
            {
                LocalPlayer.UpdateClient(DT, Keyboard, Mouse,Grabed);
            }
        }
        internal static void Render()
        {
            foreach (var player in PlayerList.Values)
            {
                if (player != LocalPlayer)
                {
                    player.Render();
                }
            }
        }
        internal static void HandleSpawn(NetDataReader data, NetPeer peer)
        {
            ushort id = data.GetUShort();
            Vector3d position = new Vector3d(data.GetDouble(), data.GetDouble(), data.GetDouble());
            AddPlayer(id, position, false);
        }
        internal static void AddLocalPlayer(NetDataReader data, NetPeer peer)
        {
            ushort id = data.GetUShort();
            Vector3d position = new Vector3d(data.GetDouble(), data.GetDouble(), data.GetDouble());
            AddPlayer(id, position, true);
        }
        internal static void HandleData(NetDataReader data, NetPeer peer)
        {
            ushort id = data.GetUShort();
            if (PlayerList.TryGetValue(id, out Player player))
            {
                double x = data.GetDouble();
                double y = data.GetDouble();
                double z = data.GetDouble();
                Vector3 rotation = new Vector3(data.GetFloat(), data.GetFloat(), data.GetFloat());
                if (player != LocalPlayer)
                {
                    player.Rotation = rotation;
                }
                player.Position = new Vector3d(x, y, z);
            }
        }
        internal static void HandleDeco(NetDataReader data, NetPeer peer)
        {
            ushort id = data.GetUShort();
            PlayerList.Remove(id);
        }

    }
}
