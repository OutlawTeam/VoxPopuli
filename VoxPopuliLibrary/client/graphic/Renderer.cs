/**
 * Entity render system
 * Copyrights Florian Pfeiffer
 * Author Florian Pfeiffer
 * */
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using VoxPopuliLibrary.client.debug;
using VoxPopuliLibrary.common.ecs.client;

namespace VoxPopuliLibrary.client.graphic.renderer
{
    public static class RenderSystem
    {
        /// <summary>
        /// Init Graphic
        /// </summary>
        public static void Init(GameWindow window)
        {
            window.CursorState = CursorState.Grabbed;
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Multisample);
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
        }

        internal static void RenderDebugBox(DebugBox box, Vector3 pos)
        {
            // Envoi des matrices view et projection au shader
            GlobalVariable.DebugShader.SetMatrix4("view", PlayerFactory.LocalPlayer._Camera.GetViewMatrix());
            GlobalVariable.DebugShader.SetMatrix4("projection", PlayerFactory.LocalPlayer._Camera.GetProjectionMatrix());
            GlobalVariable.DebugShader.SetMatrix4("model", Matrix4.Zero + Matrix4.CreateTranslation(new Vector3(pos.X, pos.Y, pos.Z)));
            GlobalVariable.DebugShader.SetVector4("colors", box.Color);
            GlobalVariable.DebugShader.Use();
            GL.BindVertexArray(box.VAO);
            GL.DrawElements(PrimitiveType.Lines, 24, DrawElementsType.UnsignedInt, 0);
        }
    }
}
