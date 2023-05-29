using VoxPopuliLibrary.Engine.GraphicEngine;

namespace VoxPopuliLibrary.Engine.RessourceManager
{
    internal static partial class RessourceManager
    {
        const string BaseGuiTextureFolder = "assets/textures/gui";
        private static void LoadGuiTextures()
        {
            string[] textures = Directory.GetFiles(BaseGuiTextureFolder, "*.png");
            foreach (string filePath in textures)
            {
                try
                {
                    GuiTextures.Add(Path.GetFileNameWithoutExtension(filePath), Texture.LoadFromFile(filePath));
                }
                catch
                {
                    throw new Exception("An error occur, the texture could not load !");
                }
            }
        }
        public static Texture GetGuiTexture(string Name)
        {
            if (GuiTextures.TryGetValue(Name, out var Texture))
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
