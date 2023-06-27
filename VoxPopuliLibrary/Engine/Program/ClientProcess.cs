/**
 * Client main logic
 * Copyrights Florian Pfeiffer
 * Author Florian Pfeiffer
 **/
using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;
using VoxPopuliLibrary.Engine.API;
using VoxPopuliLibrary.Engine.API.GUI;
using VoxPopuliLibrary.Engine.API.Input;
using VoxPopuliLibrary.Engine.Debug;
using VoxPopuliLibrary.Engine.GraphicEngine;
using VoxPopuliLibrary.Engine.GraphicEngine.FontRenderer;
using VoxPopuliLibrary.Engine.Init;
using VoxPopuliLibrary.Engine.ModdingSystem;
using VoxPopuliLibrary.Engine.Network;
using VoxPopuliLibrary.Engine.World;

namespace VoxPopuliLibrary.Engine.Program
{
    /// <summary>
    /// Game Window and Process
    /// </summary>
    public class Window : GameWindow
    {
        //Debug menu
        private bool DebugMenu = false;

        //Server thread 
        internal static Thread ServerLocalThread;

        // Créer une instance de Stopwatch
        Stopwatch RenderProfiler = new Stopwatch();
        Stopwatch UpdateProfiler = new Stopwatch();
        Stopwatch NetworkProfiler = new Stopwatch();
        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }
        protected override void OnLoad()
        {
            base.OnLoad();
            ModManager.LoadMods();
            ModManager.Init();
            RessourceManager.RessourceManager.LoadRessourcesClient();
            DebugSystem.Init(ClientSize);
            RenderSystem.Init(this);
            Engine.API.API.Init(this);
            MenuGUiManager.UpdateScale( Size.X , Size.Y);
            ClientEngineInit.Init();
            ClientNetwork.Init();
            ClientNetwork.Update();
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            RenderProfiler.Start();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
            //
            //
            if (ClientWorldManager.Initialized)
            {
                ClientWorldManager.RenderWorld();
            }

            

            //
            //Debug
            DebugSystem.Update(this, (float)e.Time);
            if (DebugMenu == true)
            {
                DebugSystem.DebugMenu();
            }
            UIManager.Render();
            
            DebugSystem.RenderDebug();
            DebugSystem.Render();

            ImGuiController.CheckGLError("End of frame");
            TextMaster.render();

            SwapBuffers();
            RenderProfiler.Stop();
            DebugSystem.RenderTime = RenderProfiler.ElapsedMilliseconds;
            RenderProfiler.Reset();
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {

            UpdateProfiler.Start();
            base.OnUpdateFrame(e);
            //FontManager.Update(Size.X,Size.Y);
            NetworkProfiler.Start();
            ClientNetwork.Update();
            NetworkProfiler.Stop();
            DebugSystem.NetworkTime = NetworkProfiler.ElapsedMilliseconds;
            NetworkProfiler.Reset();
            var input = KeyboardState;
            var mouse = MouseState;
            InputSystem.Update(input, mouse, e);
            if (input.IsKeyPressed(Keys.Escape))
            {
                if (InputSystem.Grab == false)
                {
                    CursorState = CursorState.Grabbed;
                    InputSystem.Grab = true;
                }
                else
                {
                    CursorState = CursorState.Normal;
                    InputSystem.Grab = false;
                }
            }
            if (input.IsKeyPressed(Keys.F1))
            {
                if (DebugMenu == false)
                {
                    DebugMenu = true;
                }
                else
                {
                    DebugMenu = false;
                }
            }
            if (ClientWorldManager.Initialized)
            {
                ClientWorldManager.UpdateWorld();
            }
            //MenuManager.Update();
            UpdateProfiler.Stop();
            DebugSystem.UpdateTime = UpdateProfiler.ElapsedMilliseconds;
            UpdateProfiler.Reset();
        }
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
            MenuGUiManager.UpdateScale(e.Width, e.Height);
            Renderer.UpdateResize(Size.X, Size.Y);
            if (ClientWorldManager.Initialized)
            {
                if (ClientWorldManager.world.LocalPlayerExist())
                {
                    ClientWorldManager.world.LocalPlayerCamera().AspectRatio = Size.X / (float)Size.Y;
                }
            }
            // Configurer le framebuffer avec 4 samples
            int colorBuffer;
            GL.GenRenderbuffers(1, out colorBuffer);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, colorBuffer);
            GL.RenderbufferStorageMultisample(RenderbufferTarget.Renderbuffer, 4, RenderbufferStorage.Rgba32f, e.Size.X, e.Size.Y);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, RenderbufferTarget.Renderbuffer, colorBuffer);

            int depthBuffer;
            GL.GenRenderbuffers(1, out depthBuffer);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, depthBuffer);
            GL.RenderbufferStorageMultisample(RenderbufferTarget.Renderbuffer, 4, RenderbufferStorage.DepthComponent24, e.Size.X, e.Size.Y);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, depthBuffer);
            DebugSystem.Resize(ClientSize);
        }
        protected override void OnTextInput(TextInputEventArgs e)
        {
            base.OnTextInput(e);
            DebugSystem.Char((char)e.Unicode);
        }
    }
}