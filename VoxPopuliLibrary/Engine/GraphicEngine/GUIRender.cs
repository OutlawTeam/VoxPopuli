using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
namespace VoxPopuliLibrary.Engine.GraphicEngine
{
    internal static partial class Renderer
    {
        private static Matrix4 MVP = Matrix4.Identity;
        public static void UpdateResize(int w,int h)
        {
            MVP = Matrix4.CreateOrthographicOffCenter(
                0.0f,
                w,
                h,
                0.0f,
                -1.0f,
                1.0f);
            RessourceManager.RessourceManager.GetShader("GUI").SetMatrix4NoTranspose("projection",MVP);
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
            int Vao = GL.GenVertexArray();
            int Vbo = GL.GenBuffer();
            GL.BindVertexArray(Vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, Vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(float), Vertices, BufferUsageHint.StaticDraw);
           
            var vertexLocation = 0;
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            var UVLocation = 1;
            GL.EnableVertexAttribArray(UVLocation);
            GL.VertexAttribPointer(UVLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 2 * sizeof(float));
            var ColorLocation = 2;
            GL.EnableVertexAttribArray(ColorLocation);
            GL.VertexAttribPointer(ColorLocation, 4, VertexAttribPointerType.Float, false, 8 * sizeof(float), 4 * sizeof(float));
            RessourceManager.RessourceManager.GetShader("GUI").Use();
            RessourceManager.RessourceManager.GetGuiTexture("base").Use(TextureUnit.Texture0);
            GL.Disable(EnableCap.DepthTest);
            GL.DrawArrays(PrimitiveType.TriangleFan, 0, 4);
            GL.Enable(EnableCap.DepthTest);

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
            int Vao = GL.GenVertexArray();
            int Vbo = GL.GenBuffer();
            GL.BindVertexArray(Vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, Vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(float), Vertices, BufferUsageHint.StaticDraw);

            var vertexLocation = 0;
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            var UVLocation = 1;
            GL.EnableVertexAttribArray(UVLocation);
            GL.VertexAttribPointer(UVLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 2 * sizeof(float));
            var ColorLocation = 2;
            GL.EnableVertexAttribArray(ColorLocation);
            GL.VertexAttribPointer(ColorLocation, 4, VertexAttribPointerType.Float, false, 8 * sizeof(float), 4 * sizeof(float));
            RessourceManager.RessourceManager.GetShader("GUI").Use();
            RessourceManager.RessourceManager.GetGuiTexture(image).Use(TextureUnit.Texture0);
            GL.Disable(EnableCap.DepthTest);
            GL.DrawArrays(PrimitiveType.TriangleFan, 0, 4);
            GL.Enable(EnableCap.DepthTest);

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
            int Vao = GL.GenVertexArray();
            int Vbo = GL.GenBuffer();
            GL.BindVertexArray(Vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, Vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(float), Vertices, BufferUsageHint.StaticDraw);

            var vertexLocation = 0;
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            var UVLocation = 1;
            GL.EnableVertexAttribArray(UVLocation);
            GL.VertexAttribPointer(UVLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 2 * sizeof(float));
            var ColorLocation = 2;
            GL.EnableVertexAttribArray(ColorLocation);
            GL.VertexAttribPointer(ColorLocation, 4, VertexAttribPointerType.Float, false, 8 * sizeof(float), 4 * sizeof(float));
            RessourceManager.RessourceManager.GetShader("GUI").Use();
            RessourceManager.RessourceManager.GetGuiTexture(image).Use(TextureUnit.Texture0);
            GL.Disable(EnableCap.DepthTest);
            GL.DrawArrays(PrimitiveType.TriangleFan, 0, 4);
            GL.Enable(EnableCap.DepthTest);

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
            int Vao = GL.GenVertexArray();
            int Vbo = GL.GenBuffer();
            GL.BindVertexArray(Vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, Vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(float), Vertices, BufferUsageHint.StaticDraw);

            var vertexLocation = 0;
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            var UVLocation = 1;
            GL.EnableVertexAttribArray(UVLocation);
            GL.VertexAttribPointer(UVLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 2 * sizeof(float));
            var ColorLocation = 2;
            GL.EnableVertexAttribArray(ColorLocation);
            GL.VertexAttribPointer(ColorLocation, 4, VertexAttribPointerType.Float, false, 8 * sizeof(float), 4 * sizeof(float));
            RessourceManager.RessourceManager.GetShader("GUI").Use();
            RessourceManager.RessourceManager.GetGuiTexture(image).Use(TextureUnit.Texture0);
            GL.Disable(EnableCap.DepthTest);
            GL.DrawArrays(PrimitiveType.TriangleFan, 0, 4);
            GL.Enable(EnableCap.DepthTest);

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
            int Vao = GL.GenVertexArray();
            int Vbo = GL.GenBuffer();
            GL.BindVertexArray(Vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, Vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(float), Vertices, BufferUsageHint.StaticDraw);

            var vertexLocation = 0;
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            var UVLocation = 1;
            GL.EnableVertexAttribArray(UVLocation);
            GL.VertexAttribPointer(UVLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 2 * sizeof(float));
            var ColorLocation = 2;
            GL.EnableVertexAttribArray(ColorLocation);
            GL.VertexAttribPointer(ColorLocation, 4, VertexAttribPointerType.Float, false, 8 * sizeof(float), 4 * sizeof(float));
            RessourceManager.RessourceManager.GetShader("GUI").Use();
            RessourceManager.RessourceManager.GetGuiTexture(image).Use(TextureUnit.Texture0);
            GL.Disable(EnableCap.DepthTest);
            GL.DrawArrays(PrimitiveType.TriangleFan, 0, 4);
            GL.Enable(EnableCap.DepthTest);

        }
        /*
        public static void RenderText(string text,string font,int x,int y,int size,Vector4 color)
        {
            RessourceManager.RessourceManager.GetFont(font).RenderText(text,color.Xyz,x-1,y+size-13,(float)size/48,new Vector2(1,0));
        }*/
    }
}
