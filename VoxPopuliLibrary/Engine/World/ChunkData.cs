using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxPopuliLibrary.Engine.World
{
    [Serializable]
    internal struct ChunkData
    {
        internal byte[] data;
        internal Palette pal;
    }
}
