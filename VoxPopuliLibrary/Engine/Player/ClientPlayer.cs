/**
 * Player implementation for client side
 * Copyrights Florian Pfeiffer
 * Author Florian Pfeiffer
 */
using LiteNetLib;
using LiteNetLib.Utils;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace VoxPopuliLibrary.Engine.Player
{
    internal class ClientPlayerFactory
    {
        internal readonly Dictionary<ushort, Player> PlayerList = new Dictionary<ushort, Player>();
        internal Player LocalPlayer;
        internal bool LocalPlayerExist = false;
        internal void AddPlayer(ushort clientId, Vector3d position, bool Local)
        {
            Player temp = new Player(position, clientId, Local, true);
            if (Local)
            {
                LocalPlayer = temp;
                LocalPlayerExist = true;
            }
            PlayerList.Add(clientId, temp);
        }
        internal void Update(float DT, KeyboardState Keyboard, MouseState Mouse, bool Grabed)
        {
            if (LocalPlayerExist)
            {
                LocalPlayer.UpdateClient(DT, Keyboard, Mouse, Grabed);
            }
        }
        internal void Render()
        {
            foreach (var player in PlayerList.Values)
            {
                if (player != LocalPlayer)
                {
                    player.Render();
                }
            }
        }
        internal void HandleSpawn(PlayerSpawn data, NetPeer peer)
        {
            AddPlayer(data.ClientID, data.Position, false);
        }
        internal void HandleLocalPlayer(PlayerSpawnLocal data, NetPeer peer)
        {
            AddPlayer(data.ClientID,data.Position, true);
        }
        internal void HandleData(PlayerData data, NetPeer peer)
        {
            if (PlayerList.TryGetValue(data.ClientID, out Player player))
            {
                if (player != LocalPlayer)
                {
                    player.Rotation = data.Rotation;
                }
                player.Position = data.Position;
            }
        }
        internal void HandleDeco(PlayerDeco data, NetPeer peer)
        { 
            PlayerList.Remove(data.ClientID);
        }
    }
}
