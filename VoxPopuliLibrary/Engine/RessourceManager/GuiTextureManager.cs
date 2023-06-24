using VoxPopuliLibrary.Engine.GraphicEngine;
using VoxPopuliLibrary.Engine.ModdingSystem;

namespace VoxPopuliLibrary.Engine.RessourceManager
{
    public static partial class RessourceManager
    {
        const string BaseGuiTextureFolder = "assets/textures/gui";
        private static void LoadGuiTextures()
        {
            string[] textures = Directory.GetFiles(BaseGuiTextureFolder, "*.png");
            foreach (string filePath in textures)
            {
                try
                {
                    GuiTextures.Add(Path.GetFileNameWithoutExtension(filePath), Texture.LoadFromFileGUI(filePath));
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
                    string[] texturess = Directory.GetFiles(mod + "/" + BaseGuiTextureFolder, "*.png");
                    foreach (string filePath in texturess)
                    {
                        try
                        {
                            GuiTextures.Add(Path.GetFileNameWithoutExtension(filePath), Texture.LoadFromFileGUI(filePath));
                        }
                        catch
                        {
                            throw new Exception("An error occur, the texture can't not load !");
                        }
                    }
                }
                catch { }
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
