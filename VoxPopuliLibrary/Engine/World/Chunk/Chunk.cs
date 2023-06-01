/**
 * Chunk implementation shared by the clien and the server
 * Copyrights Florian Pfeiffer
 * Author Florian Pfeiffer
 * Information: Séparer les shader pour les chunk et ceux des entités
 */
using OpenTK.Mathematics;
using VoxPopuliLibrary.Engine.Player;

namespace VoxPopuliLibrary.Engine.World
{
    internal partial class Chunk
    {
        //Chunk state for meshing
        public bool Changed = true;

        internal List<Player.Player> PlayerInChunk;
        //Chunk data
        //Chunk coordinates
        public Vector3i Position;
        //
        internal bool Used = true;
        internal bool Empty = false;
        internal Palette ChunkPalette;
        //Chunk data
        public byte[] Blocks = new byte[(int)Math.Pow(16, 3)];
        /// <summary>
        /// Générate chunk for server
        /// </summary>
        /// <param name="Pos">Chunk position</param>
        internal Chunk(Vector3i Pos)
        {
            PlayerInChunk = new List<Player.Player>();
            Position = Pos;
            ChunkPalette = new Palette();
            GenerateChunk();
        }
        /// <summary>
        /// Create chunk with receved chunk data for client
        /// </summary>
        /// <param name="blocks">Blocks data</param>
        /// <param name="Pos">Position</param>
        internal Chunk(byte[] blocks,Palette chunkp, Vector3i Pos)
        {
            ChunkPalette = chunkp;
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
