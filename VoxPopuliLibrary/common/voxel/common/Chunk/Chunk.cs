/**
 * Chunk implementation shared by the clien and the server
 * Copyrights Florian Pfeiffer
 * Author Florian Pfeiffer
 * Information: Séparer les shader pour les chunk et ceux des entités
 */
using OpenTK.Mathematics;
using VoxPopuliLibrary.client;
namespace VoxPopuliLibrary.common.voxel.common
{
    internal partial class Chunk
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
        internal Chunk(Vector2i Pos)
        {
           Position = Pos;
           GenerateChunk();
        }
        /// <summary>
        /// Create chunk with receved chunk data for client
        /// </summary>
        /// <param name="blocks">Blocks data</param>
        /// <param name="Pos">Position</param>
        internal Chunk(ushort[] blocks, Vector2i Pos)
        {
            Blocks = blocks;
            Position = Pos;
            InitGraphic();
        }
    };
}
