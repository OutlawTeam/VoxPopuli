using OpenTK.Mathematics;
using DotnetNoise;
using SharpFont;
using VoxPopuliLibrary.Engine.Maths;
using System.Diagnostics;

namespace VoxPopuliLibrary.Engine.World
{
    internal class WorldGenerator
    {
        FastNoise lib;
        Dictionary<Vector2i, float> NoiseData;
        internal WorldGenerator(int seed) 
        {
            lib = new FastNoise(seed);
            
            NoiseData = new Dictionary<Vector2i, float>();
        }
        internal int GetOrigin()
        {
            if (!NoiseData.ContainsKey(new Vector2i(0, 0)))
            {
                NoiseData.Add(new Vector2i(0, 0), lib.GetPerlin(0,0));
            }
            return(int)(NoiseData[new Vector2i(0, 0)] * 50);
        }
        internal void GenerateChunk(Chunk chunk)
        {
            lib.Octaves = 4;
            lib.UsedNoiseType = FastNoise.NoiseType.Perlin;
            for (int x = 0; x < 16; x++)
            {
                for (int z = 0; z < 16; z++)
                {
                    int GlobalX = x + chunk.Position.X * 16;
                    int GlobalZ = z + chunk.Position.Z * 16;
                    if(!NoiseData.ContainsKey(new Vector2i(GlobalX, GlobalZ)) )
                    {
                        NoiseData.Add(new Vector2i(GlobalX, GlobalZ), lib.GetPerlin(GlobalX, GlobalZ));
                    }
                    int Height = (int)(NoiseData[new Vector2i(GlobalX, GlobalZ)] * 50);
                    
                    for (int y = 0; y < 16; y++)
                    {
                        int GlobalY = y + chunk.Position.Y * 16;
                        if (GlobalY <= Height - 5)
                        {
                            chunk.SetBlock(x, y, z, "VoxPopuli:stone");
                        }
                        else if (GlobalY >= Height - 5 && GlobalY < Height)
                        {
                            chunk.SetBlock(x, y, z, "VoxPopuli:dirt");
                        }
                        else if(GlobalY == Height)
                        {
                            chunk.SetBlock(x, y, z, "VoxPopuli:grass");
                        }else if(GlobalY > Height && GlobalY  <= 0)
                        {
                            chunk.SetBlock(x, y, z, "VoxPopuli:water");
                        }
                        else
                        {
                            chunk.SetBlock(x, y, z, "air");
                        }
                    }
                }
            }
        }
    }
}
