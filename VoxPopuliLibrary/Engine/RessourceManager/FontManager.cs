using VoxPopuliLibrary.Engine.GraphicEngine;
using VoxPopuliLibrary.Engine.ModdingSystem;

namespace VoxPopuliLibrary.Engine.RessourceManager
{
    public static partial class RessourceManager
    {
        const string BaseFontFolder = "assets/fonts";
        private static void LoadFonts()
        {
            string[] font = Directory.GetFiles(BaseFontFolder, "*.ttf");
            foreach (string filePath in font)
            {
                try
                {
                    Fonts.Add(Path.GetFileNameWithoutExtension(filePath), new FreeTypeFont(filePath));
                }
                catch(Exception ex )
                {
                    throw new Exception("An error occur, the font can't not load !: "+ex);
                }
            }
            foreach (string mod in ModManager.ModAssetFolder)
            {
                try
                {
                    string[] fonts = Directory.GetFiles(mod + "/" + BaseFontFolder, "*.ttf");
                    foreach (string filePath in fonts)
                    {
                        try
                        {
                           
                            Fonts.Add(Path.GetFileNameWithoutExtension(filePath), new FreeTypeFont(filePath));
                        }
                        catch
                        {
                            throw new Exception("An error occur, the font can't not load !");
                        }
                    }
                }
                catch { }
            }
        }
        public static FreeTypeFont GetFont(string Name)
        {
            if (Fonts.TryGetValue(Name, out var Font))
            {
                return Font;
            }
            else
            {
                throw new Exception("The demended font is missing !");
            }
        }
    }
}
