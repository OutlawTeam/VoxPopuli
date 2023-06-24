/**
 * Entity render system
 * Copyrights Florian Pfeiffer
 * Author Florian Pfeiffer
 * */
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using VoxPopuliLibrary.Engine.Debug;
using VoxPopuliLibrary.Engine.GraphicEngine.FontRenderer;
using VoxPopuliLibrary.Engine.Physics;
using VoxPopuliLibrary.Engine.World;

namespace VoxPopuliLibrary.Engine.GraphicEngine
{
    internal static class RenderSystem
    {
        /// <summary>
        /// Init Graphic
        /// </summary>
        internal static void Init(GameWindow window)
        {
            window.CursorState = CursorState.Grabbed;
            GL.ClearColor(0.39f, 0.58f, 0.92f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
            
            // Configurer le framebuffer avec 4 samples
            int colorBuffer;
            GL.GenRenderbuffers(1, out colorBuffer);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, colorBuffer);
            GL.RenderbufferStorageMultisample(RenderbufferTarget.Renderbuffer, 4, RenderbufferStorage.Rgba32f, window.Size.X, window.Size.Y);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, RenderbufferTarget.Renderbuffer, colorBuffer);

            int depthBuffer;
            GL.GenRenderbuffers(1, out depthBuffer);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, depthBuffer);
            GL.RenderbufferStorageMultisample(RenderbufferTarget.Renderbuffer, 4, RenderbufferStorage.DepthComponent24, window.Size.X, window.Size.Y);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, depthBuffer);
            GL.Enable(EnableCap.Multisample);
            TextMaster.Init();
        }

        internal static void RenderDebugBox(DebugBox box, Vector3 pos)
        {
            // Envoi des matrices view et projection au shader
            RessourceManager.RessourceManager.GetShader("Debug").SetMatrix4("view", ClientWorldManager.world.GetPlayerFactoryClient().LocalPlayer._Camera.GetViewMatrix());
            RessourceManager.RessourceManager.GetShader("Debug").SetMatrix4("projection", ClientWorldManager.world.GetPlayerFactoryClient().LocalPlayer._Camera.GetProjectionMatrix());
            RessourceManager.RessourceManager.GetShader("Debug").SetMatrix4("model", Matrix4.Zero + Matrix4.CreateTranslation(new Vector3(pos.X, pos.Y, pos.Z)));
            RessourceManager.RessourceManager.GetShader("Debug").SetVector4("colors", box.Color);
            RessourceManager.RessourceManager.GetShader("Debug").Use();
            GL.BindVertexArray(box.VAO);
            GL.DrawElements(PrimitiveType.Lines, 24, DrawElementsType.UnsignedInt, 0);
        }
    }
}
