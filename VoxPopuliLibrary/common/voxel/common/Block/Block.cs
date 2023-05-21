/**
 * Block definition
 * Copyrights Florian Pfeiffer
 * Author Florian Pfeiffer
 */

using VoxPopuliLibrary.common.physic;

namespace VoxPopuliLibrary.common.voxel.common
{
    public enum BlockFace
    {
        Top,
        Bottom,
        Left,
        Right,
        Front,
        Back
    }

    internal class block
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
