namespace VoxPopuliLibrary.Engine.GraphicEngine
{
    internal class BlockMesh
    {
        private float[][] Mesh;
        internal BlockMesh(float[][] Model)
        {
            Mesh = Model;
        }
        public float[][] GetMesh()
        {
            return Mesh;
        }
    }
}
