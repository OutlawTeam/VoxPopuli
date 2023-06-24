using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace VoxPopuliLibrary.Engine.Debug
{
    internal struct DebugBox
    {
        public int VAO, VBO;
        int EBO;
        public Vector4 Color;
        public DebugBox(Vector3d BoxSize, Vector4 color)
        {
            Color = color;
            float xmax = (float)BoxSize.X;
            float ymax = (float)BoxSize.Y;
            float zmax = (float)BoxSize.Z;
            float x = 0;
            float y = 0;
            float z = 0;
            float[] vertices = new float[]
            {
                        x,y, z,
                        xmax,y,z,
                        xmax,ymax,z,
                        x,ymax,z,
                        x,y,zmax,
                        xmax,y,zmax,
                        xmax,ymax,zmax,
                        x,ymax,zmax,
            };
            uint[] indices = new uint[] {
                        0, 1, 1, 2, 2, 3, 3, 0, // Lignes du plan z = aabb.Min.Z
                        4, 5, 5, 6, 6, 7, 7, 4, // Lignes du plan z = aabb.Max.Z
                        0, 4, 1, 5, 2, 6, 3, 7  // Lignes entre les plans
                    };
            VAO = GL.GenVertexArray();
            VBO = GL.GenBuffer();
            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            var vertexLocation = 0;
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, sizeof(uint) * 24, indices, BufferUsageHint.StaticDraw);
            GL.BindVertexArray(0);
        }
    }
}
