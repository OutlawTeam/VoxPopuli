using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxPopuliLibrary.Engine.Font
{
    public class TextMeshData
    {

        private float[] vertexPositions;
        private float[] textureCoords;

        internal TextMeshData(float[] vertexPositions, float[] textureCoords)
        {
            this.vertexPositions = vertexPositions;
            this.textureCoords = textureCoords;
        }

        public float[] getVertexPositions()
        {
            return vertexPositions;
        }

        public float[] getTextureCoords()
        {
            return textureCoords;
        }

        public int getVertexCount()
        {
            return vertexPositions.Length / 2;
        }

    }
}
