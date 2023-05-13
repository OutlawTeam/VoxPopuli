/**
 * Client main logic
 * Copyrights Florian Pfeiffer
 * Author Florian Pfeiffer
 **/
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;
using VoxPopuliLibrary.client.debug;
using VoxPopuliLibrary.client.graphic.renderer;
using VoxPopuliLibrary.common.ecs.client;

namespace VoxPopuliLibrary.client
{
    /// <summary>
    /// Game Window and Process
    /// </summary>
    public class Window : GameWindow
    {
        //Grabed state
        private bool Grabed = true;
        //Debug menu
        private bool DebugMenu = false;
        //20 Game Logic timer
        Stopwatch updateTimer;
        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }
        protected override void OnLoad()
        {
            base.OnLoad();
            DebugSystem.Init(ClientSize);
            RenderSystem.Init(this);
            GlobalVariable.LoadClient();
            network.Network.Init();
            network.Network.Update();
            common.voxel.common.AllBlock.init();
            SkyboxRender.InitSkyBox();
            updateTimer = new Stopwatch();
            updateTimer.Start();
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
            if (PlayerFactory.LocalPlayerExist)
            {
                SkyboxRender.RenderSkyBox(PlayerFactory.LocalPlayer._Camera.GetViewMatrix(),
                    PlayerFactory.LocalPlayer._Camera.GetProjectionMatrix()) ;
                GlobalVariable.VoxelShader.SetMatrix4("view", PlayerFactory.LocalPlayer._Camera.GetViewMatrix());
                GlobalVariable.VoxelShader.SetMatrix4("projection", PlayerFactory.LocalPlayer._Camera.GetProjectionMatrix());
                common.voxel.client.ChunkManager.RenderChunk((Vector3)PlayerFactory.LocalPlayer.Position);
                PlayerFactory.LocalPlayer.RenderPlayerUtils();

            }
            //Entity
            PlayerFactory.Render();
            //Debug
            DebugSystem.Update(this, (float)e.Time);
            if (DebugMenu == true)
            {
                DebugSystem.DebugMenu();
            }
            DebugSystem.RenderDebug();
            DebugSystem.Render();
            ImGuiController.CheckGLError("End of frame");
            SwapBuffers();
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            if (PlayerFactory.LocalPlayerExist)
            {
                common.voxel.client.ChunkManager.Update(PlayerFactory.LocalPlayer.Position);
            }
            network.Network.Update();
            var input = KeyboardState;
            var mouse = MouseState;
            PlayerFactory.Update((float)e.Time, KeyboardState, MouseState);
            if (input.IsKeyPressed(Keys.Escape))
            {
                if (Grabed == false)
                {
                    CursorState = CursorState.Grabbed;
                    Grabed = true;
                }
                else
                {
                    CursorState = CursorState.Normal;
                    Grabed = false;
                }
            }
            if (input.IsKeyPressed(Keys.Insert))
            {
                if (DebugMenu == false)
                {
                    DebugMenu = true;
                    CursorState = CursorState.Grabbed;
                }
                else
                {
                    DebugMenu = false;
                    CursorState = CursorState.Normal;
                }
            }
        }
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
        }
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);

            if (PlayerFactory.LocalPlayerExist)
            {
                PlayerFactory.LocalPlayer._Camera.AspectRatio = Size.X / (float)Size.Y;
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