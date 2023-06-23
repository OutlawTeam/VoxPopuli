using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using VoxPopuliLibrary.Engine.API;
using VoxPopuliLibrary.Engine.GraphicEngine;

namespace VoxPopuliLibrary.Engine.World
{
    internal partial class Chunk
    {
        //Client chunk mesh buffer
        private int VAO, VBO, EBO;
        //Temp mesh float data
        private List<float> Vertice;
        //Chunk mesh vertices count
        public int VerticeCount = 0;
        uint IndexCounter = 0;
        int IndexCount = 0;
        List<uint> indices;
        internal void Render()
        {
            GL.BindVertexArray(VAO);
            RessourceManager.RessourceManager.GetAtlas().Use(TextureUnit.Texture0);
            Matrix4 model;

            model = Matrix4.CreateTranslation(new Vector3(Position.X * 16, Position.Y * 16, Position.Z * 16));

            RessourceManager.RessourceManager.GetShader("Chunk").SetMatrix4("model", model);
            RessourceManager.RessourceManager.GetShader("Chunk").Use();
            GL.DrawElements(PrimitiveType.Triangles, IndexCount, DrawElementsType.UnsignedInt, 0);
            GL.BindVertexArray(0);
        }
        internal void InitGraphic()
        {
            VAO = GL.GenVertexArray();
            VBO = GL.GenBuffer();
            EBO = GL.GenBuffer();
        }
        public void GenerateMesh()
        {
            IndexCounter = 0;
            Vertice = new List<float>();
            indices = new List<uint>();
            for (int x = 0; x < ClientWorldManager.world.CHUNK_SIZE; x++)
            {
                for (int y = 0; y < ClientWorldManager.world.CHUNK_SIZE; y++)
                {
                    for (int z = 0; z < ClientWorldManager.world.CHUNK_SIZE; z++)
                    {
                        if (GetBlock(x, y, z) != "air")
                        {
                            if (BlockRegister.BlockList[GetBlock(x, y, z)].GetRenderType()==BlockRenderType.Block)
                            {
                                GenerateDirection(0, x, y, z);
                                GenerateDirection(1, x, y, z);
                                GenerateDirection(2, x, y, z);
                            }
                            else
                            {
                                for (int i = 0; i < RessourceManager.RessourceManager.GetBlockMesh(BlockRegister.BlockList[GetBlock(x, y, z)].GetMesh()).GetMesh().Length; i++)
                                {
                                    AddMeshFace(GetBlock(x, y, z), x, y, z, i);
                                }
                            }

                        }
                    }
                }
            }
            var Vertices = Vertice.ToArray();
            VerticeCount = Vertices.Length;
            IndexCount = indices.Count;
            GenerateVAO(Vertices);
            Vertice.Clear();
            Vertice = null;
            indices.Clear();
            indices = null;
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
                if (y == ClientWorldManager.world.CHUNK_SIZE - 1)
                {
                    if (BlockRegister.BlockTransparent(ClientWorldManager.world.GetChunkManagerClient().GetChunk(Position.X, Position.Y + 1, Position.Z).GetBlock(x, 0, z)))
                    {
                        AddMeshFace(GetBlock(x, y, z), x, y, z, 0);
                    }
                }
                else
                {
                    if (BlockRegister.BlockTransparent(GetBlock(x, y + 1, z)))
                    {
                        AddMeshFace(GetBlock(x, y, z), x, y, z, 0);
                    }
                }
                if (y == 0)
                {
                    if (BlockRegister.BlockTransparent(ClientWorldManager.world.GetChunkManagerClient().GetChunk(Position.X, Position.Y - 1, Position.Z).GetBlock(x, ClientWorldManager.world.CHUNK_SIZE - 1, z)))
                    {
                        AddMeshFace(GetBlock(x, y, z), x, y, z, 1);
                    }
                }
                else
                {
                    if (BlockRegister.BlockTransparent(GetBlock(x, y - 1, z)))
                    {
                        AddMeshFace(GetBlock(x, y, z), x, y, z, 1);
                    }
                }
            }
            else if (Faceid == 1)
            {
                if (z == ClientWorldManager.world.CHUNK_SIZE - 1)
                {
                    if (BlockRegister.BlockTransparent(ClientWorldManager.world.GetChunkManagerClient().GetChunk(Position.X, Position.Y, Position.Z + 1).GetBlock(x, y, 0)))
                    {
                        AddMeshFace(GetBlock(x, y, z), x, y, z, 2);
                    }
                }
                else
                {
                    if (BlockRegister.BlockTransparent(GetBlock(x, y, z + 1)))
                    {
                        AddMeshFace(GetBlock(x, y, z), x, y, z, 2);
                    }
                }
                if (z == 0)
                {
                    if (BlockRegister.BlockTransparent(ClientWorldManager.world.GetChunkManagerClient().GetChunk(Position.X, Position.Y, Position.Z - 1).GetBlock(x, y, ClientWorldManager.world.CHUNK_SIZE - 1)))
                    {
                        AddMeshFace(GetBlock(x, y, z), x, y, z, 3);
                    }
                }
                else
                {
                    if (BlockRegister.BlockTransparent(GetBlock(x, y, z - 1)))
                    {
                        AddMeshFace(GetBlock(x, y, z), x, y, z, 3);
                    }
                }
            }
            else
            {
                if (x == ClientWorldManager.world.CHUNK_SIZE - 1)
                {
                    if (BlockRegister.BlockTransparent(ClientWorldManager.world.GetChunkManagerClient().GetChunk(Position.X + 1, Position.Y, Position.Z).GetBlock(0, y, z)))
                    {
                        AddMeshFace(GetBlock(x, y, z), x, y, z, 4);
                    }
                }
                else
                {
                    if (BlockRegister.BlockTransparent(GetBlock(x + 1, y, z)))
                    {
                        AddMeshFace(GetBlock(x, y, z), x, y, z, 4);
                    }
                }
                if (x == 0)
                {
                    if (BlockRegister.BlockTransparent(ClientWorldManager.world.GetChunkManagerClient().GetChunk(Position.X - 1, Position.Y, Position.Z).GetBlock(ClientWorldManager.world.CHUNK_SIZE - 1, y, z)))
                    {

                        AddMeshFace(GetBlock(x, y, z), x, y, z, 5);
                    }
                }
                else
                {
                    if (BlockRegister.BlockTransparent(GetBlock(x - 1, y, z)))
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

        private void AddMeshFace(string Block, int x, int y, int z, int BF)
        {
            var Vert = BlockRegister.BlockMesh(Block, BF);
            var Tex = BlockRegister.GetTexture(Block, BF);
            for (int i = 0; i < 4; i++)
            {
                Vertice.Add(Vert[i * 3] + x);
                Vertice.Add(Vert[i * 3 + 1] + y);
                Vertice.Add(Vert[i * 3 + 2] + z);
                Vertice.Add(Tex[i * 2]);
                Vertice.Add(Tex[i * 2 + 1]);
                Vertice.Add(CalculateAO(new Vector3(Vert[i * 3], Vert[i * 3 + 1], Vert[i * 3 + 2]),new Vector3i(x,y,z)));

            }
            uint[] indice = { 0, 1, 2, 0, 2, 3 };
            for (uint i = 0; i < 6; i++)
            {
                indice[i] += IndexCounter;
            }
            indices.AddRange(indice);
            IndexCounter += 4;
        }
        private int CalculateAO(Vector3 vertpos,Vector3i bpos)
        {
            int StepY = vertpos.Y  <= 0.5 ? -1: 1;
            int StepX = vertpos.X  <= 0.5 ? -1: 1;
            int StepZ = vertpos.Z  <= 0.5 ? -1: 1;
            string Side1 = ClientWorldManager.world.GetChunkManagerClient().
                GetBlockForMesh(new Vector3i(bpos.X +StepX,bpos.Y+StepY,bpos.Z),Position);
            string Side2 = ClientWorldManager.world.GetChunkManagerClient().
                GetBlockForMesh(new Vector3i(bpos.X, bpos.Y + StepY, bpos.Z + StepZ), Position);
            string Corner = ClientWorldManager.world.GetChunkManagerClient().
                GetBlockForMesh(new Vector3i(bpos.X+ StepX, bpos.Y + StepY, bpos.Z + StepZ), Position);
            int side1 = BlockRegister.BlockTransparent(Side1) == false ?  1:0;
            int side2 = BlockRegister.BlockTransparent(Side2) == false ? 1 : 0;
            int corner = BlockRegister.BlockTransparent(Corner) == false ? 1 : 0;
            if (side1 == 1 && side2 == 1)
            {
                return 0;
            }
            return 3 - (side1 + side2 + corner);
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
            var vertexLocation = RessourceManager.RessourceManager.GetShader("Chunk").GetAttribLocation("Vertex");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            var texCoordLocation = RessourceManager.RessourceManager.GetShader("Chunk").GetAttribLocation("Texture");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));
            var AOLocation = RessourceManager.RessourceManager.GetShader("Chunk").GetAttribLocation("AO");
            GL.EnableVertexAttribArray(AOLocation);
            GL.VertexAttribPointer(AOLocation, 1, VertexAttribPointerType.Float, false, 6 * sizeof(float), 5 * sizeof(float));
            //EBO
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Count * sizeof(uint), indices.ToArray(), BufferUsageHint.StaticDraw);
            GL.BindVertexArray(0);
        }
    }
}
