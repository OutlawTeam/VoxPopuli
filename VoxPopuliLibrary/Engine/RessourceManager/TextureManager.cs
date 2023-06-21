using VoxPopuliLibrary.Engine.GraphicEngine;
using VoxPopuliLibrary.Engine.ModdingSystem;

namespace VoxPopuliLibrary.Engine.RessourceManager
{
    public static partial class RessourceManager
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
                    throw new Exception("An error occur, the texture can't not load !");
                }
            }
            foreach (string mod in ModManager.ModAssetFolder)
            {
                try
                {
                    string[] texturess = Directory.GetFiles(mod + "/" + BaseTextureFolder, "*.png");
                    foreach (string filePath in texturess)
                    {
                        try
                        {
                            Textures.Add(Path.GetFileNameWithoutExtension(filePath), Texture.LoadFromFile(filePath));
                        }
                        catch
                        {
                            throw new Exception("An error occur, the texture can't not load !");
                        }
                    }
                }catch {}
            }
        }
        public static Texture GetTexture(string Name)
        {
            if (Textures.TryGetValue(Name, out var Texture))
            {
                return Texture;
            }
            else
            {
                throw new Exception("The demended texture is missing !");
            }
        }
    }
}
