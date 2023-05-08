/**
* Texture Atlas coordiante calculator
* Copyrights Florian Pfeiffer
* Auhtor Florian Pfeiffer
* */
namespace VoxPopuliLibrary.client.graphic
{
    public class TextureAtlas
    {
        /// <summary>
        /// Transform id to texture coordinae
        /// </summary>
        /// <param name="id">Block id</param>
        /// <param name="f">Face is juste due to a bug</param>
        /// <returns></returns>
        public static float[] IdtoCord(int id, int f = 0)
        {
            float[] textcord = new float[12];
            if (f == 0)
            {
                textcord[0] = id / GlobalVariable.TextureSize - id / GlobalVariable.TextureSize;
                textcord[1] = id / GlobalVariable.TextureSize * (1f / GlobalVariable.TextureSize);
                textcord[2] = id / GlobalVariable.TextureSize - id / GlobalVariable.TextureSize + 1f / GlobalVariable.TextureSize;
                textcord[3] = id / GlobalVariable.TextureSize * (1f / GlobalVariable.TextureSize);
                textcord[4] = id / GlobalVariable.TextureSize - id / GlobalVariable.TextureSize + 1f / GlobalVariable.TextureSize;
                textcord[5] = id / GlobalVariable.TextureSize * (1 / GlobalVariable.TextureSize) + 1f / GlobalVariable.TextureSize;
                textcord[6] = id / GlobalVariable.TextureSize - id / GlobalVariable.TextureSize + 1f / GlobalVariable.TextureSize;
                textcord[7] = id / GlobalVariable.TextureSize * (1 / GlobalVariable.TextureSize) + 1f / GlobalVariable.TextureSize;
                textcord[8] = id / GlobalVariable.TextureSize - id / GlobalVariable.TextureSize;
                textcord[9] = id / GlobalVariable.TextureSize * (1 / GlobalVariable.TextureSize) + 1f / GlobalVariable.TextureSize;
                textcord[10] = id / GlobalVariable.TextureSize - id / GlobalVariable.TextureSize;
                textcord[11] = id / GlobalVariable.TextureSize * (1 / GlobalVariable.TextureSize);
            }
            else
            {
                textcord[0] = id / GlobalVariable.TextureSize - id / GlobalVariable.TextureSize + 1f / GlobalVariable.TextureSize;
                textcord[1] = id / GlobalVariable.TextureSize * (1 / GlobalVariable.TextureSize) + 1f / GlobalVariable.TextureSize;
                textcord[2] = id / GlobalVariable.TextureSize - id / GlobalVariable.TextureSize;
                textcord[3] = id / GlobalVariable.TextureSize * (1 / GlobalVariable.TextureSize) + 1f / GlobalVariable.TextureSize;
                textcord[4] = id / GlobalVariable.TextureSize - id / GlobalVariable.TextureSize;
                textcord[5] = id / GlobalVariable.TextureSize * (1 / GlobalVariable.TextureSize);
                textcord[6] = id / GlobalVariable.TextureSize - id / GlobalVariable.TextureSize;
                textcord[7] = id / GlobalVariable.TextureSize * (1f / GlobalVariable.TextureSize);
                textcord[8] = id / GlobalVariable.TextureSize - id / GlobalVariable.TextureSize + 1f / GlobalVariable.TextureSize;
                textcord[9] = id / GlobalVariable.TextureSize * (1f / GlobalVariable.TextureSize);
                textcord[10] = id / GlobalVariable.TextureSize - id / GlobalVariable.TextureSize + 1f / GlobalVariable.TextureSize;
                textcord[11] = id / GlobalVariable.TextureSize * (1 / GlobalVariable.TextureSize) + 1f / GlobalVariable.TextureSize;
            }
            return textcord;
        }
    }
}

