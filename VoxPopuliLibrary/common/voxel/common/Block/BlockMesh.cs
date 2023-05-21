using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxPopuliLibrary.common.voxel.common
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
