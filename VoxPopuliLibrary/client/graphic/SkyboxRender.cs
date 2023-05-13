using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using VoxPopuliLibrary.common.ecs.client;

namespace VoxPopuliLibrary.client.graphic.renderer
{ 
    internal static class SkyboxRender
    {
        static CubeMapTexture Skybox;
        static SkyBoxModel SkyboxModel;
        internal static void InitSkyBox()
        {
            List<string> faces = new List<string>
                {
                    "Textures/cubemap/right.png",
                    "Textures/cubemap/left.png",
                    "Textures/cubemap/bottom.png",
                    "Textures/cubemap/top.png",
                    "Textures/cubemap/front.png",
                    "Textures/cubemap/back.png"
                };
            Skybox = CubeMapTexture.LoadCubeMap(faces);
            SkyboxModel = new SkyBoxModel(SkyboxMesh.SkyboxVertices);
        }
        internal static void RenderSkyBox(Matrix4 View, Matrix4 Projection)
        {
            GL.Disable(EnableCap.DepthTest);
            GlobalVariable.SkyBoxShader.Use();
            GlobalVariable.SkyBoxShader.SetMatrix4("projection", Projection);
            GlobalVariable.SkyBoxShader.SetMatrix4("view", View);
            GL.BindVertexArray(SkyboxModel.Vao);
            Skybox.Use(TextureUnit.Texture0);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            GL.BindVertexArray(0);
            GL.Enable(EnableCap.DepthTest);
        }
        internal static CubeMapTexture GetSkyboxTexture()
        {
            return Skybox;
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
