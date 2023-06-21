using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using System.Diagnostics;
using VoxPopuliLibrary.Engine.API.Input;
using VoxPopuliLibrary.Engine.GraphicEngine;
using VoxPopuliLibrary.Engine.Player;

namespace VoxPopuliLibrary.Engine.World
{
    internal class World
    {
        public int Seed;
        public WorldGenerator WorldGen;
        public int CHUNK_SIZE { get { return 16; } }
        public int Height;

        internal int VerticalRenderDistance = 5;
        internal int RenderDistance = 7;
        internal int LoadDistance { get { return RenderDistance + 3; } }
        internal int VerticalLoadDistance { get { return VerticalRenderDistance + 3; } }

        Stopwatch frameWatch = Stopwatch.StartNew();
        Stopwatch LowUpdate = Stopwatch.StartNew();
        float dt = 1f / 60f;

        private ClientChunkManager ChunkManagerClient;
        private ServerChunkManager ChunkManagerServer;

        private Player.ClientPlayerFactory PlayerFactoryClient;
        private Player.ServerPlayerFactory PlayerFactoryServer;

        bool Client;
        internal World(bool _Client, string WorldPath)
        {
            Client = _Client;
            if (Client)
            {
                ChunkManagerClient = new ClientChunkManager();
                PlayerFactoryClient = new Player.ClientPlayerFactory();
            }
            else
            {
                Height = 512;
                Seed = new Random().Next(int.MinValue, int.MaxValue);
                WorldGen = new WorldGenerator(Seed);
                ChunkManagerServer = new ServerChunkManager();
                PlayerFactoryServer = new Player.ServerPlayerFactory();
            }
        }
        public bool GetBlock(int x, int y, int z, out string id)
        {
            if (Client)
            {
                return ChunkManagerClient.GetBlock(x, y, z, out id);
            }
            else
            {
                return ChunkManagerServer.GetBlock(x, y, z, out id);
            }
        }
        internal void Update()
        {
            if (Client)
            {
                if (PlayerFactoryClient.LocalPlayerExist)
                {
                    ChunkManagerClient.Update(PlayerFactoryClient.LocalPlayer.Position);
                }
                PlayerFactoryClient.Update((float)InputSystem.FrameEvent.Time,
                    InputSystem.Keyboard, InputSystem.Mouse, InputSystem.Grab);
            }
            else
            {
                frameWatch.Restart();
                ChunkManagerServer.Update();
                if (LowUpdate.ElapsedMilliseconds > 25)
                {
                    PlayerFactoryServer.SendData();
                    LowUpdate.Restart();
                }
            }
        }
        internal void Render()
        {
            if (PlayerFactoryClient.LocalPlayerExist)
            {
                SkyboxRender.RenderSkyBox(PlayerFactoryClient.LocalPlayer._Camera.GetViewMatrix(),
                    PlayerFactoryClient.LocalPlayer._Camera.GetProjectionMatrix());
                RessourceManager.RessourceManager.GetShader("Chunk").SetMatrix4("view", PlayerFactoryClient.LocalPlayer._Camera.GetViewMatrix());
                RessourceManager.RessourceManager.GetShader("Chunk").SetMatrix4("projection", PlayerFactoryClient.LocalPlayer._Camera.GetProjectionMatrix());
                GL.Enable(EnableCap.CullFace);
                ChunkManagerClient.RenderChunk((Vector3)PlayerFactoryClient.LocalPlayer.Position);
                GL.Disable(EnableCap.CullFace);
            }
            //Entity
            PlayerFactoryClient.Render();
        }
        internal Camera LocalPlayerCamera()
        {
            return PlayerFactoryClient.LocalPlayer._Camera;
        }
        internal bool LocalPlayerExist()
        {
            return PlayerFactoryClient.LocalPlayerExist;
        }
        internal ClientPlayerFactory GetPlayerFactoryClient()
        {
            return PlayerFactoryClient;
        }
        internal ClientChunkManager GetChunkManagerClient()
        {
            return ChunkManagerClient;
        }
        internal ServerPlayerFactory GetPlayerFactoryServer()
        {
            return PlayerFactoryServer;
        }
        internal ServerChunkManager GetChunkManagerServer()
        {
            return ChunkManagerServer;
        }
    }
}
