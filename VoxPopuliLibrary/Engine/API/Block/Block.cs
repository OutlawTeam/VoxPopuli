/**
 * Block definition
 * Copyrights Florian Pfeiffer
 * Author Florian Pfeiffer
 */

using VoxPopuliLibrary.Engine.GraphicEngine;
using VoxPopuliLibrary.Engine.Physics;

namespace VoxPopuliLibrary.Engine.API
{
    internal class Block
    {
        internal float[] texture0;
        internal float[] texture1;
        internal float[] texture2;
        internal float[] texture3;
        internal float[] texture4;
        internal float[] texture5;
        internal bool IsTransparent;
        internal bool IsSolid = true;
        internal bool Cube = true;
        internal BlockMesh Mesh;
        internal Collider[] Colliders;
    }
}
