/**
 * Chunk implementation shared by the clien and the server
 * Copyrights Florian Pfeiffer
 * Author Florian Pfeiffer
 * Information: Séparer les shader pour les chunk et ceux des entités
 */
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using VoxPopuliLibrary.client;
using VoxPopuliLibrary.common.utils;
using VoxPopuliLibrary.common.voxel.client;
namespace VoxPopuliLibrary.common.voxel.common
{
    public class Chunk
    {
        //Client chunk mesh buffer
        private int VAO, VBO;
        //Temp mesh float data
        private List<float> Vertice;
        //Chunk mesh vertices count
        public int VerticeCount = 0;
        //Chunk state for meshing
        public bool Changed = true;
        //Chunk data
        //Chunk coordinates
        public Vector2i Position;
        //
        internal bool Used = true;
        //Chunk data
        public ushort[] Blocks = new ushort[GlobalVariable.CHUNK_SIZE * GlobalVariable.CHUNK_HEIGHT * GlobalVariable.CHUNK_SIZE];
        /// <summary>
        /// Générate chunk for server
        /// </summary>
        /// <param name="Pos">Chunk position</param>
        public Chunk(Vector2i Pos)
        {
            Position = Pos;
            for (int j = 0; j < GlobalVariable.CHUNK_HEIGHT; j++)
            {
                for (int i = 0; i < GlobalVariable.CHUNK_SIZE; i++)
                {
                    for (int k = 0; k < GlobalVariable.CHUNK_SIZE; k++)
                    {

                        if (j > 300)
                        {
                            Blocks[carray.treedto1d(i, j, k)] = 0;
                        }
                        else if (j == 300)
                        {

                            Blocks[carray.treedto1d(i, j, k)] = 1;
                        }
                        else if (j < 300 && j > 275)
                        {
                            Blocks[carray.treedto1d(i, j, k)] = 1;
                        }
                        else
                        {
                            Blocks[carray.treedto1d(i, j, k)] = 3;
                        }
                        //m_block[carray.treedto1d(i, j, k)] = 1;
                    }
                }
            }
        }
        /// <summary>
        /// Create chunk with receved chunk data for client
        /// </summary>
        /// <param name="blocks">Blocks data</param>
        /// <param name="Pos">Position</param>
        public Chunk(ushort[] blocks, Vector2i Pos)
        {
            Blocks = blocks;
            Position = Pos;
            VAO = GL.GenVertexArray();
            VBO = GL.GenBuffer();
        }
        /// <summary>
        /// Generate chunk mesh for client
        /// </summary>
        public void GenerateMesh()
        {
            Vertice = new List<float>();
            for (int x = 0; x < GlobalVariable.CHUNK_SIZE; x++)
            {
                for (int y = 0; y < GlobalVariable.CHUNK_HEIGHT; y++)
                {
                    for (int z = 0; z < GlobalVariable.CHUNK_SIZE; z++)
                    {
                        if (GetBlock(x, y, z) != 0)
                        {
                            GenerateDirection(0, x, y, z);
                            GenerateDirection(1, x, y, z);
                            GenerateDirection(2, x, y, z);
                        }
                    }
                }
            }
            var Vertices = Vertice.ToArray();
            VerticeCount = Vertices.Length;
            GenerateVAO(Vertices);
            Vertice.Clear();
            Changed = false;
        }
        /// <summary>
        /// Generate mesh faces
        /// </summary>
        /// <param name="Faceid">0 top and botom ; 1 front and back ; 2 right and left</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        private void GenerateDirection(uint Faceid, int x, int y, int z)
        {
            if (Faceid == 0)
            {
                if (y == GlobalVariable.CHUNK_HEIGHT - 1)
                {
                    AddMeshFace(GetBlock(x, y, z), x, y, z, 0);
                }
                else
                {
                    if (AllBlock.BlockTransparent(GetBlock(x, y + 1, z)))
                    {
                        AddMeshFace(GetBlock(x, y, z), x, y, z, 0);
                    }
                }
                if (y != 0)
                {
                    if (AllBlock.BlockTransparent(GetBlock(x, y - 1, z)))
                    {
                        AddMeshFace(GetBlock(x, y, z), x, y, z, 1);
                    }
                }
            }
            else if (Faceid == 1)
            {
                if (z == GlobalVariable.CHUNK_SIZE - 1)
                {
                    if (AllBlock.BlockTransparent(Chunk_Manager.getchunk(Position.X, Position.Y + 1).GetBlock(x, y, 0)))
                    {
                        AddMeshFace(GetBlock(x, y, z), x, y, z, 2);
                    }
                }
                else
                {
                    if (AllBlock.BlockTransparent(GetBlock(x, y, z + 1)))
                    {
                        AddMeshFace(GetBlock(x, y, z), x, y, z, 2);
                    }
                }
                if (z == 0)
                {
                    if (AllBlock.BlockTransparent(Chunk_Manager.getchunk(Position.X, Position.Y - 1).GetBlock(x, y, GlobalVariable.CHUNK_SIZE - 1)))
                    {
                        AddMeshFace(GetBlock(x, y, z), x, y, z, 3);
                    }
                }
                else
                {
                    if (AllBlock.BlockTransparent(GetBlock(x, y, z - 1)))
                    {
                        AddMeshFace(GetBlock(x, y, z), x, y, z, 3);
                    }
                }
            }
            else
            {
                if (x == GlobalVariable.CHUNK_SIZE - 1)
                {
                    if (AllBlock.BlockTransparent(Chunk_Manager.getchunk(Position.X + 1, Position.Y).GetBlock(0, y, z)))
                    {
                        AddMeshFace(GetBlock(x, y, z), x, y, z, 4);
                    }
                }
                else
                {
                    if (AllBlock.BlockTransparent(GetBlock(x + 1, y, z)))
                    {
                        AddMeshFace(GetBlock(x, y, z), x, y, z, 4);
                    }
                }
                if (x == 0)
                {
                    if (AllBlock.BlockTransparent(Chunk_Manager.getchunk(Position.X - 1, Position.Y).GetBlock(GlobalVariable.CHUNK_SIZE - 1, y, z)))
                    {
                        AddMeshFace(GetBlock(x, y, z), x, y, z, 5);
                    }
                }
                else
                {
                    if (AllBlock.BlockTransparent(GetBlock(x - 1, y, z)))
                    {
                        AddMeshFace(GetBlock(x, y, z), x, y, z, 5);
                    }
                }
            }
        }
        /// <summary>
        /// Add mesh face to the temp mesh
        /// </summary>
        /// <param name="Block">Id of the block</param>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="z">Z coordinate</param>
        /// <param name="BF">Block face 0 top ; 1 bottom ; 2 front :3 back ; 4 right ; 5 left</param>
        private void AddMeshFace(ushort Block, int x, int y, int z, ushort BF)
        {
            var Vert = AllBlock.BlockMesh(Block, BF);
            var Tex = AllBlock.gettex(Block, BF);
            for (int i = 0; i < Vert.Length / 3; i++)
            {
                Vertice.Add(Vert[i * 3] + x);
                Vertice.Add(Vert[1 + i * 3] + y);
                Vertice.Add(Vert[2 + i * 3] + z);
                Vertice.Add(Tex[i * 2]);
                Vertice.Add(Tex[1 + i * 2]);
            }
        }
        /// <summary>
        /// Generate chunk mesh buffers
        /// </summary>
        /// <param name="Vertices"></param>
        private void GenerateVAO(float[] Vertices)
        {
            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(float), Vertices, BufferUsageHint.StaticDraw);
            var vertexLocation = GlobalVariable._shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
            var texCoordLocation = GlobalVariable._shader.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
            GL.BindVertexArray(0);
        }
        /// <summary>
        /// Return the block id of the donate coordinate
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="z">Z coordinate</param>
        /// <returns>Block id</returns>
        internal ushort GetBlock(int x, int y, int z)
        {
            try
            {
                return Blocks[carray.treedto1d(x, y, z)];
            }
            catch
            {
                Console.WriteLine(x + ":" + y + ":" + z);
                throw new Exception("Block coordinate was supperior to chunk size and height.");
            }
        }
        /// <summary>
        /// Set block id at donate coordinate
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="z">Z coordinate</param>
        /// <param name="id">Block id of the new block</param>
        internal void SetBlock(int x, int y, int z, ushort id)
        {
            Blocks[x + GlobalVariable.CHUNK_SIZE * (y + GlobalVariable.CHUNK_HEIGHT * z)] = id;
        }
        /// <summary>
        /// Render chunk 
        /// </summary>
        public void Render()
        {
            GL.BindVertexArray(VAO);
            GlobalVariable._texture.Use(TextureUnit.Texture0);
            var model = Matrix4.Identity;
            model = Matrix4.CreateTranslation(new Vector3(Position.X * 16, 0, Position.Y * 16));
            GlobalVariable._shader.SetMatrix4("model", model);
            GlobalVariable._shader.Use();
            GL.DrawArrays(PrimitiveType.Triangles, 0, VerticeCount);
            GL.BindVertexArray(0);
        }
    };
}
