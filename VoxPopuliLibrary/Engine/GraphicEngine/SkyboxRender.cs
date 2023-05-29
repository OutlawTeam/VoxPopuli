using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
namespace VoxPopuliLibrary.Engine.GraphicEngine
{
    internal static class SkyboxRender
    {
        static SkyBoxModel SkyboxModel = new SkyBoxModel(SkyboxMesh.SkyboxVertices);
        internal static void RenderSkyBox(Matrix4 View, Matrix4 Projection)
        {
            GL.Disable(EnableCap.DepthTest);
            RessourceManager.RessourceManager.GetShader("Cubemap").Use();
            RessourceManager.RessourceManager.GetShader("Cubemap").SetMatrix4("projection", Projection);
            RessourceManager.RessourceManager.GetShader("Cubemap").SetMatrix4("view", View);
            GL.BindVertexArray(SkyboxModel.Vao);
            RessourceManager.RessourceManager.GetCubeMapTexture("Base").Use(TextureUnit.Texture0);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            GL.BindVertexArray(0);
            GL.Enable(EnableCap.DepthTest);
        }
    }
    internal static class SkyboxMesh
    {
        internal static float[] SkyboxVertices = {
            // positions          
			-1.0f,  1.0f, -1.0f,
            -1.0f, -1.0f, -1.0f,
             1.0f, -1.0f, -1.0f,
             1.0f, -1.0f, -1.0f,
             1.0f,  1.0f, -1.0f,
            -1.0f,  1.0f, -1.0f,

            -1.0f, -1.0f,  1.0f,
            -1.0f, -1.0f, -1.0f,
            -1.0f,  1.0f, -1.0f,
            -1.0f,  1.0f, -1.0f,
            -1.0f,  1.0f,  1.0f,
            -1.0f, -1.0f,  1.0f,

             1.0f, -1.0f, -1.0f,
             1.0f, -1.0f,  1.0f,
             1.0f,  1.0f,  1.0f,
             1.0f,  1.0f,  1.0f,
             1.0f,  1.0f, -1.0f,
             1.0f, -1.0f, -1.0f,

            -1.0f, -1.0f,  1.0f,
            -1.0f,  1.0f,  1.0f,
             1.0f,  1.0f,  1.0f,
             1.0f,  1.0f,  1.0f,
             1.0f, -1.0f,  1.0f,
            -1.0f, -1.0f,  1.0f,

            -1.0f,  1.0f, -1.0f,
             1.0f,  1.0f, -1.0f,
             1.0f,  1.0f,  1.0f,
             1.0f,  1.0f,  1.0f,
            -1.0f,  1.0f,  1.0f,
            -1.0f,  1.0f, -1.0f,

            -1.0f, -1.0f, -1.0f,
            -1.0f, -1.0f,  1.0f,
             1.0f, -1.0f, -1.0f,
             1.0f, -1.0f, -1.0f,
            -1.0f, -1.0f,  1.0f,
             1.0f, -1.0f,  1.0f
        };
    }
}

