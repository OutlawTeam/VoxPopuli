/**
 * Chunk implementation shared by the clien and the server
 * Copyrights Florian Pfeiffer
 * Author Florian Pfeiffer
 * Information: Séparer les shader pour les chunk et ceux des entités
 */
using OpenTK.Mathematics;

namespace VoxPopuliLibrary.Engine.World
{
    internal partial class Chunk
    {
        //Chunk state for meshing
        public bool Changed = true;
        //Chunk data
        //Chunk coordinates
        public Vector3i Position;
        //
        internal bool Used = true;
        internal bool Empty = false;
        //Chunk data
        public ushort[] Blocks = new ushort[(int)Math.Pow(16, 3)];
        /// <summary>
        /// Générate chunk for server
        /// </summary>
        /// <param name="Pos">Chunk position</param>
        internal Chunk(Vector3i Pos)
        {
            Position = Pos;
            GenerateChunk();
        }
        /// <summary>
        /// Create chunk with receved chunk data for client
        /// </summary>
        /// <param name="blocks">Blocks data</param>
        /// <param name="Pos">Position</param>
        internal Chunk(ushort[] blocks, Vector3i Pos)
        {
            Position = Pos;
            if (blocks.All(element => element == 0))
            {
                Empty = true;
            }
            else
            {
                Empty = false;
                Blocks = blocks;
            }
            InitGraphic();
        }
    };
}
