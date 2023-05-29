using OpenTK.Graphics.OpenGL4;
namespace VoxPopuliLibrary.Engine.GraphicEngine
{
    internal struct Model
    {
        internal int Vao;
        internal float[] _Vertices;
        public Model(float[] Vertices)
        {
            _Vertices = Vertices;
            Vao = GL.GenVertexArray();
            int Vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, Vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(float), Vertices, BufferUsageHint.StaticDraw);
            GL.BindVertexArray(Vao);
            var vertexLocation = RessourceManager.RessourceManager.GetShader("Entity").GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            var texCoordLocation = RessourceManager.RessourceManager.GetShader("Entity").GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            GL.BindVertexArray(0);
        }
    }
}
