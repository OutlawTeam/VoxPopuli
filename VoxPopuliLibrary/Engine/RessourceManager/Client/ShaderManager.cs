using Tomlyn;
using Tomlyn.Model;
using VoxPopuliLibrary.Engine.GraphicEngine;

namespace VoxPopuliLibrary.Engine.RessourceManager
{
    internal static partial class RessourceManager
    {
        const string BaseShaderFolder = "assets/shaders";
        private static void LoadShaders()
        {
            string[] TomlShader = Directory.GetFiles(BaseShaderFolder, "*.toml");
            foreach (string filePath in TomlShader)
            {
                TomlTable tomlData = Toml.ToModel(File.ReadAllText(filePath));
                try
                {
                    Shaders.Add((string)tomlData["Name"], new Shader(BaseShaderFolder + "/" + (string)tomlData["Vertex"], BaseShaderFolder + "/" + (string)tomlData["Fragment"]));
                }
                catch
                {
                    throw new Exception("There is problem when loading" + filePath + " shader toml ,vertex or fragment file are possibly missing otherwise toml file is not conform!");
                }
            }
        }
        public static Shader GetShader(string Name)
        {
            if (Shaders.TryGetValue(Name, out Shader Shady))
            {
                return Shady;
            }
            else
            {
                throw new Exception("The shader named" + Name + "is missing !");
            }
        }
    }
}
