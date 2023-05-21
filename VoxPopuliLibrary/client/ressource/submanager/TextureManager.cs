using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxPopuliLibrary.client.graphic;

namespace VoxPopuliLibrary.client.ressource
{
    internal static partial class RessourceManager
    {
        const string BaseTextureFolder = "assets/textures/other";
        private static void LoadTextures()
        {
            string[] textures = Directory.GetFiles(BaseTextureFolder, "*.png");
            foreach (string filePath in textures)
            {
                try
                {
                    Textures.Add(Path.GetFileNameWithoutExtension(filePath), Texture.LoadFromFile(filePath));
                }
                catch
                {
                    throw new Exception("An error occur, the texture could not load !");
                }
            }
        }
        public static Texture GetTexture(string Name)
        {
            if (Textures.TryGetValue(Name, out var Texture))
            {
                return Texture;
            }else
            {
                throw new Exception("The demended texture is missing !");
            }
        }
    }
}
