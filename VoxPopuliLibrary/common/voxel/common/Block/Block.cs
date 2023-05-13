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
        internal float[] texture0 = new float[12];
        internal float[] texture1 = new float[12];
        internal float[] texture2 = new float[12];
        internal float[] texture3 = new float[12];
        internal float[] texture4 = new float[12];
        internal float[] texture5 = new float[12];
        internal bool IsTransparent;
        internal bool IsSolid = true;
        internal bool Cube = true;
        internal BlockMesh Mesh;
        internal List<Collider> Colliders = new List<Collider>();
    }
    internal class BlockMesh
    {
        internal float[][] Mesh;
    }
    
}
