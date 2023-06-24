using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
namespace VoxPopuliLibrary.Engine.GraphicEngine
{
    internal static partial class Renderer
    {
        internal static Matrix4 MVP = Matrix4.Identity;
        private static int VAO = GL.GenVertexArray();
        private static int VBO = GL.GenBuffer();
        private static Shader GUI = new Shader("assets/engine/shaders/GUI.vert", "assets/engine/shaders/GUI.frag");
        private static Texture Base = Texture.LoadFromFile("assets/engine/textures/gui/base.png");
        public static void UpdateResize(int w,int h)
        {
            MVP = Matrix4.CreateOrthographicOffCenter(
                0.0f,
                w,
                h,
                0.0f,
                -1.0f,
                1.0f);
            GUI.SetMatrix4NoTranspose("projection",MVP);
        }
        public static void RenderRec(int x,int y,int w,int h,Vector4 col)
        {
            float[] Vertices = new float[]
            {
                x,y,0,0,col.X,col.Y,col.Z,col.W,
                x+w,y,1,0,col.X,col.Y,col.Z,col.W,
                x+w,y+h,1,1,col.X,col.Y, col.Z,col.W,
                x,y+h,0,1,col.X,col.Y, col.Z,col.W
            };
            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(float), Vertices, BufferUsageHint.StaticDraw);
           
            int vertexLocation = 0;
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            int UVLocation = 1;
            GL.EnableVertexAttribArray(UVLocation);
            GL.VertexAttribPointer(UVLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 2 * sizeof(float));
            int ColorLocation = 2;
            GL.EnableVertexAttribArray(ColorLocation);
            GL.VertexAttribPointer(ColorLocation, 4, VertexAttribPointerType.Float, false, 8 * sizeof(float), 4 * sizeof(float));
            GUI.Use();
            Base.Use(TextureUnit.Texture0);
            GL.Disable(EnableCap.DepthTest);
            GL.DrawArrays(PrimitiveType.TriangleFan, 0, 4);
            GL.Enable(EnableCap.DepthTest);
            Vertices = null;
        }
        public static void RenderImage(string image, int x, int y, int w, int h)
        {

            Vector4 col = new Vector4(1);


            float[] Vertices = new float[]
            {
                x,y,0,0,col.X,col.Y,col.Z,col.W,
                x+w,y,1,0,col.X,col.Y,col.Z,col.W,
                x+w,y+h,1,1,col.X,col.Y, col.Z,col.W,
                x,y+h,0,1,col.X,col.Y, col.Z,col.W
            };
            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(float), Vertices, BufferUsageHint.StaticDraw);

            int vertexLocation = 0;
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            int UVLocation = 1;
            GL.EnableVertexAttribArray(UVLocation);
            GL.VertexAttribPointer(UVLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 2 * sizeof(float));
            int ColorLocation = 2;
            GL.EnableVertexAttribArray(ColorLocation);
            GL.VertexAttribPointer(ColorLocation, 4, VertexAttribPointerType.Float, false, 8 * sizeof(float), 4 * sizeof(float));
            GUI.Use();
            RessourceManager.RessourceManager.GetGuiTexture(image).Use(TextureUnit.Texture0);
            GL.Disable(EnableCap.DepthTest);
            GL.DrawArrays(PrimitiveType.TriangleFan, 0, 4);
            GL.Enable(EnableCap.DepthTest);
            Vertices = null;

        }
        public static void RenderImage(string image,int x, int y, int w, int h, Vector4 col)
        {
            float[] Vertices = new float[]
            {
                x,y,0,0,col.X,col.Y,col.Z,col.W,
                x+w,y,1,0,col.X,col.Y,col.Z,col.W,
                x+w,y+h,1,1,col.X,col.Y, col.Z,col.W,
                x,y+h,0,1,col.X,col.Y, col.Z,col.W
            };
            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(float), Vertices, BufferUsageHint.StaticDraw);

            int vertexLocation = 0;
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            int UVLocation = 1;
            GL.EnableVertexAttribArray(UVLocation);
            GL.VertexAttribPointer(UVLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 2 * sizeof(float));
            int ColorLocation = 2;
            GL.EnableVertexAttribArray(ColorLocation);
            GL.VertexAttribPointer(ColorLocation, 4, VertexAttribPointerType.Float, false, 8 * sizeof(float), 4 * sizeof(float));
            GUI.Use();
            RessourceManager.RessourceManager.GetGuiTexture(image).Use(TextureUnit.Texture0);
            GL.Disable(EnableCap.DepthTest);
            GL.DrawArrays(PrimitiveType.TriangleFan, 0, 4);
            GL.Enable(EnableCap.DepthTest);
            Vertices = null;

        }
        public static void RenderImage(string image, float ix, float iy, float iw, float ih, int x, int y, int w, int h)
        {

            Vector4 col = new Vector4(1);
            
            float[] Vertices = new float[]
            {
                x,y,ix,iy,col.X,col.Y,col.Z,col.W,
                x+w,y,iw,iy,col.X,col.Y,col.Z,col.W,
                x+w,y+h,iw,iw,col.X,col.Y, col.Z,col.W,
                x,y+h,ix,iw,col.X,col.Y, col.Z,col.W
            };
            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(float), Vertices, BufferUsageHint.StaticDraw);

            int vertexLocation = 0;
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            int UVLocation = 1;
            GL.EnableVertexAttribArray(UVLocation);
            GL.VertexAttribPointer(UVLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 2 * sizeof(float));
            int ColorLocation = 2;
            GL.EnableVertexAttribArray(ColorLocation);
            GL.VertexAttribPointer(ColorLocation, 4, VertexAttribPointerType.Float, false, 8 * sizeof(float), 4 * sizeof(float));
            GUI.Use();
            RessourceManager.RessourceManager.GetGuiTexture(image).Use(TextureUnit.Texture0);
            GL.Disable(EnableCap.DepthTest);
            GL.DrawArrays(PrimitiveType.TriangleFan, 0, 4);
            GL.Enable(EnableCap.DepthTest);
            Vertices = null;

        }
        public static void RenderImage(string image,float ix,float iy,float iw,float ih, int x, int y, int w, int h, Vector4 col)
        {
            if(col == null)
            {
                col = new Vector4(1);
            }
            float[] Vertices = new float[]
            {
                x,y,ix,iy,col.X,col.Y,col.Z,col.W,
                x+w,y,iw,iy,col.X,col.Y,col.Z,col.W,
                x+w,y+h,iw,iw,col.X,col.Y, col.Z,col.W,
                x,y+h,ix,iw,col.X,col.Y, col.Z,col.W
            };
            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(float), Vertices, BufferUsageHint.StaticDraw);

            int vertexLocation = 0;
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            int UVLocation = 1;
            GL.EnableVertexAttribArray(UVLocation);
            GL.VertexAttribPointer(UVLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 2 * sizeof(float));
            int ColorLocation = 2;
            GL.EnableVertexAttribArray(ColorLocation);
            GL.VertexAttribPointer(ColorLocation, 4, VertexAttribPointerType.Float, false, 8 * sizeof(float), 4 * sizeof(float));
            GUI.Use();
            RessourceManager.RessourceManager.GetGuiTexture(image).Use(TextureUnit.Texture0);
            GL.Disable(EnableCap.DepthTest);
            GL.DrawArrays(PrimitiveType.TriangleFan, 0, 4);
            GL.Enable(EnableCap.DepthTest);
            Vertices = null;

        }
    }
}
