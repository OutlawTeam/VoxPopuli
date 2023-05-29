/**
 * Debug Menu 
 * Copyrights Florian Pfeiffer
 * Author Florian Pfeiffer
 * */
using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using VoxPopuliLibrary.client.config;
using VoxPopuliLibrary.common.voxel.client;
using VoxPopuliLibrary.common.voxel.common;
using VoxPopuliLibrary.Engine.client.graphic;
using VoxPopuliLibrary.Engine.common;
using VoxPopuliLibrary.Engine.common.ecs;
using VoxPopuliLibrary.Engine.server.program;

namespace VoxPopuliLibrary.Engine.client.debug
{
    public static class DebugSystem
    {
        // progiller variable
        static internal double RenderTime = 0;
        static internal double UpdateTime = 0;
        static internal double MeshGenerationTime = 0;
        static internal double ClearTime = 0;
        static internal double ChunkRenderTime = 0;
        //Menu visiblity bool
        static bool PlayerMenu;
        static bool VoxelMenu;
        static bool NetMenu;
        static bool GraphicMenu;
        static bool PhysicMenu;
        //Voxel Debug Variables
        static int bx, by, bz;
        static int blockid;
        static bool DebugChunk;
        static DebugBox ChunkBox = new DebugBox(new Vector3d(16, 16, 16), new Vector4(1f, 1f, 0f, 1f));
        //Player Debug Variables
        static float px = 0, py = 300, pz = 0;
        //Render Debug Variables
        static bool WireFrameView;
        //Network debug variable 
        static string ip = "localhost";
        static int port = 23482;
        //physic debug variable
        internal static bool ShowAABB;
        //Imgui Controller
        static ImGuiController Controller;

        static Thread ServerLocalThread = new Thread(Program.Main);

        /// <summary>
        /// Function who draw debug menu
        /// </summary>
        public static void DebugMenu()
        {
            #region BaseMenu
            ImGui.BeginMainMenuBar();
            if (ImGui.BeginMenu("Game"))
            {
                ImGui.EndMenu();
            }
            if (ImGui.BeginMenu("Windows"))
            {
                if (ImGui.MenuItem("PlayerMenu")) { PlayerMenu = true; }
                if (ImGui.MenuItem("Voxel Menu")) { VoxelMenu = true; }
                if (ImGui.MenuItem("Network Menu")) { NetMenu = true; }
                if (ImGui.MenuItem("Graphic Menu")) { GraphicMenu = true; }
                if (ImGui.MenuItem("Physic Menu")) { PhysicMenu = true; }
                ImGui.EndMenu();
            }
            if (ImGui.BeginMenu("Profiling"))
            {
                ImGui.Text("Profiler");
                ImGui.Text($"VoxPopuli average {1000.0f / ImGui.GetIO().Framerate:0.##} ms/frame ({ImGui.GetIO().Framerate:0.#} FPS)");
                ImGui.Text("Rendering time :" + RenderTime + "ms");
                ImGui.Text("Update time :" + UpdateTime + "ms");
                ImGui.Text("Clear time :" + ClearTime + "ms");
                ImGui.Text("Chunk rendering time :" + ChunkRenderTime + "ms");
                ImGui.Text("Mesh generation time :" + MeshGenerationTime + "ms");
                ImGui.EndMenu();
            }
            if (ImGui.BeginMenu("Info"))
            {
                ImGui.Text("Game Version:");
                ImGui.Text("Client: " + common.Version.VersionNumber);
                ImGui.Text("Server: " + network.Network.ServerVersion);
                ImGui.EndMenu();
            }
            ImGui.EndMainMenuBar();
            #endregion

            #region Player
            if (PlayerMenu == true)
            {
                PlayerMenuDraw();
            }
            #endregion

            #region VoxelMenu
            if (VoxelMenu == true)
            {
                VoxelMenuDraw();
            }
            #endregion

            #region NetworkMenu
            if (NetMenu == true)
            {
                NetMenuDraw();
            }
            #endregion

            #region GraphicMenu
            if (GraphicMenu == true)
            {
                GraphicMenuDraw();
            }
            #endregion

            #region PhysicMenu
            if (PhysicMenu == true)
            {
                PhysicMenuDraw();
            }
            #endregion
        }
        /// <summary>
        /// Entity debug menu
        /// </summary>
        static void PlayerMenuDraw()
        {
            ImGui.Begin("Debug Menu || Player");
            if (ImGui.Button("Close", new System.Numerics.Vector2(20, 20)))
            {
                PlayerMenu = false;
            }
            ImGui.SeparatorText("Local Player");
            if (PlayerFactory.LocalPlayerExist)
            {
                ImGui.Text($"Player Position: {PlayerFactory.LocalPlayer.Position}");
                ImGui.SliderFloat("FOV", ref PlayerFactory.LocalPlayer._Camera._fov, MathHelper.PiOver3, MathHelper.PiOver2);
                ImGui.InputInt("Selected Block", ref PlayerFactory.LocalPlayer.SelectedBlock);
                ImGui.Checkbox("Fly", ref PlayerFactory.LocalPlayer.Fly);
                ImGui.InputFloat("x", ref px);
                ImGui.InputFloat("y", ref py);
                ImGui.InputFloat("z", ref pz);
                if (ImGui.Button("Teleport"))
                {
                    PlayerFactory.LocalPlayer.Position = new Vector3(px, py, pz);
                    PlayerFactory.LocalPlayer.SendPos();
                }
            }
            ImGui.SeparatorText("Player Factory");
            ImGui.Text("Player count :" + PlayerFactory.PlayerList.Count.ToString());
            ImGui.End();
        }
        /// <summary>
        /// Voxel debug menu
        /// </summary>
        static void VoxelMenuDraw()
        {
            ImGui.Begin("Debug Menu || Voxel");
            if (ImGui.Button("Close", new System.Numerics.Vector2(20, 20)))
            {
                VoxelMenu = false;
            }
            ImGui.Text($"Number of chunk: {ChunkManager.Clist.Count}");
            ImGui.SliderInt("Render Distance:", ref GlobalVariable.RenderDistance, 2, 32);
            ImGui.SliderInt("Vertical Render Distance:", ref GlobalVariable.RenderDistanceVertical, 2, 10);
            ImGui.InputInt("Block x:", ref bx);
            ImGui.InputInt("Block y:", ref by);
            ImGui.InputInt("Block z:", ref bz);
            ImGui.InputInt("Replace block id:", ref blockid);
            if (ImGui.Button("Send chunk modification"))
                ChunkManager.ChangeChunk(new Vector3i(bx, by, bz), (ushort)blockid);
            ImGui.Separator();
            ImGui.Checkbox("ChunkDebug", ref DebugChunk);

            ImGui.End();
        }
        /// <summary>
        /// Net debug menu
        /// </summary>
        static void NetMenuDraw()
        {
            ImGui.Begin("Debug Menu || Network");
            if (ImGui.Button("Close", new System.Numerics.Vector2(20, 20)))
            {
                NetMenu = false;
            }
            ImGui.InputText("Ip", ref ip, 100);
            ImGui.InputInt("Port", ref port);
            if (ImGui.Button("Connect"))
            {
                network.Network.Connect(ip, port);
            }
            ImGui.Separator();
            if (ImGui.Button("StartLocalServer"))
            {
                ServerLocalThread.Start();
            }
            ImGui.End();
        }
        /// <summary>
        /// Graphic debug menu
        /// </summary>
        static void GraphicMenuDraw()
        {
            ImGui.Begin("Debug Menu || Graphic");
            if (ImGui.Button("Close", new System.Numerics.Vector2(20, 20)))
            {
                GraphicMenu = false;
            }
            ImGui.Separator();
            ImGui.Text("Chunks graphics informations");
            ImGui.Text("Chunks which have mesh update");
            ImGui.Text(ChunkManager.ChunkMeshUpdated.ToString());
            ImGui.Text("Chunks rendered");
            ImGui.Text(ChunkManager.ChunkRendered.ToString());
            if (ImGui.Button("Debug Mesh"))
            {
                if (WireFrameView == true)
                {
                    WireFrameView = false;
                    GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                }
                else if (WireFrameView == false)
                {
                    WireFrameView = true;
                    GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                }
            }
            ImGui.End();
        }
        /// <summary>
        /// Physic debug menu
        /// </summary>
        static void PhysicMenuDraw()
        {
            ImGui.Begin("Debug Menu || Physic");
            if (ImGui.Button("Close", new System.Numerics.Vector2(20, 20)))
            {
                PhysicMenu = false;
            }
            ImGui.Checkbox("AABB view", ref ShowAABB);
            ImGui.End();
        }
        internal static void RenderDebug()
        {
            if (ShowAABB)
            {
                //Show player aabb
                foreach (var entity in PlayerFactory.PlayerList.Values)
                {
                    RenderSystem.RenderDebugBox(new DebugBox(
                        new Vector3d(entity.EntityWidth, entity.EntityHeight, entity.EntityWidth),
                        new Vector4(1f, 0f, 0f, 1f)),
                        new Vector3((float)entity.Coll.x1, (float)entity.Coll.y1, (float)entity.Coll.z1));
                }
            }
            if (DebugChunk)
            {
                foreach (Chunk ch in ChunkManager.Clist.Values)
                {
                    if (ch.Empty == false && ch.VerticeCount != 0)
                    {
                        RenderSystem.RenderDebugBox(ChunkBox, new Vector3(ch.Position.X * 16, ch.Position.Y * 16, ch.Position.Z * 16));
                    }
                }
            }
        }
        /// <summary>
        /// Init Debug Menu system
        /// </summary>
        /// <param name="ClientSize">Client window size</param>
        public static void Init(Vector2i ClientSize)
        {
            Controller = new ImGuiController(ClientSize.X, ClientSize.Y);
            WireFrameView = false;
        }
        /// <summary>
        /// Update Controller
        /// </summary>
        /// <param name="window">Main window</param>
        /// <param name="deltaSecond"> delta time</param>
        public static void Update(GameWindow window, float deltaSecond)
        {
            Controller.Update(window, deltaSecond);
        }
        /// <summary>
        /// Render Imgui
        /// </summary>
        public static void Render()
        {
            Controller.Render();
        }
        /// <summary>
        /// Update Controller size
        /// </summary>
        /// <param name="ClientSize">Window size</param>
        public static void Resize(Vector2i ClientSize)
        {
            Controller.WindowResized(ClientSize.X, ClientSize.Y); ;
        }
        /// <summary>
        /// Get Text Input
        /// </summary>
        /// <param name="Char">Char</param>
        public static void Char(char Char)
        {
            Controller.PressChar(Char);
        }
    }
}
