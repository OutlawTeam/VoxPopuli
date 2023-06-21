namespace VoxPopuliLibrary.Engine.GraphicEngine
{
    public class BlockMesh
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
