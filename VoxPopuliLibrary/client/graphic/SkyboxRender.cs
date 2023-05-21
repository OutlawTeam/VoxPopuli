using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using VoxPopuliLibrary.client.ressource;
using VoxPopuliLibrary.common.ecs.client;

namespace VoxPopuliLibrary.client.graphic.renderer
{ 
    internal static class SkyboxRender
    {
        static SkyBoxModel SkyboxModel = new SkyBoxModel(SkyboxMesh.SkyboxVertices);
        internal static void RenderSkyBox(Matrix4 View, Matrix4 Projection)
        {
            GL.Disable(EnableCap.DepthTest);
            RessourceManager.GetShader("Cubemap").Use();
            RessourceManager.GetShader("Cubemap").SetMatrix4("projection", Projection);
            RessourceManager.GetShader("Cubemap").SetMatrix4("view", View);
            GL.BindVertexArray(SkyboxModel.Vao);
            RessourceManager.GetCubeMapTexture("Base").Use(TextureUnit.Texture0);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            GL.BindVertexArray(0);
            GL.Enable(EnableCap.DepthTest);
        }
    }
    internal static class SkyboxMesh
    {
        static internal float[] SkyboxVertices = {
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

