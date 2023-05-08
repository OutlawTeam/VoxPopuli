using OpenTK.Graphics.OpenGL4;
namespace VoxPopuliLibrary.client.graphic
{
    internal struct Model
    {
        internal int Vao;
        internal Texture Texture;
        internal Shader _Shader;
        internal float[] _Vertices;
        public Model(float[] Vertices, Texture texture, Shader shader)
        {
            _Vertices = Vertices;
            Texture = texture;
            _Shader = shader;
            Vao = GL.GenVertexArray();
            int Vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, Vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(float), Vertices, BufferUsageHint.StaticDraw);
            GL.BindVertexArray(Vao);
            var vertexLocation = _Shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            var texCoordLocation = _Shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            GL.BindVertexArray(0);
        }
    }
}
