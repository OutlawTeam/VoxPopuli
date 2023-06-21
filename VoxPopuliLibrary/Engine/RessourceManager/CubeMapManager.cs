using Tomlyn;
using Tomlyn.Model;
using VoxPopuliLibrary.Engine.GraphicEngine;
using VoxPopuliLibrary.Engine.ModdingSystem;

namespace VoxPopuliLibrary.Engine.RessourceManager
{
    public static partial class RessourceManager
    {
        const string BaseCubeMapTextureFolder = "assets/textures/cubemaps";

        public static CubeMapTexture GetCubeMapTexture(string Name)
        {
            if (CubeMapTextures.TryGetValue(Name, out CubeMapTexture texture))
            {
                return texture;
            }
            else
            {
                throw new Exception("The demanded cubemap texture is missing !");
            }
        }
        private static void LoadCubeMapTextures()
        {
            string[] TomlShader = Directory.GetFiles(BaseCubeMapTextureFolder, "*.toml");
            foreach (string filePath in TomlShader)
            {
                TomlTable tomlData = Toml.ToModel(File.ReadAllText(filePath));
                try
                {
                    List<string> faces = new List<string>
                    {
                    BaseCubeMapTextureFolder+"/"+(string)tomlData["Right"],
                    BaseCubeMapTextureFolder+"/"+(string)tomlData["Left"],
                    BaseCubeMapTextureFolder+"/"+(string)tomlData["Bottom"],
                    BaseCubeMapTextureFolder+"/"+(string)tomlData["Top"],
                    BaseCubeMapTextureFolder+"/"+(string)tomlData["Front"],
                    BaseCubeMapTextureFolder+"/"+(string)tomlData["Back"]
                    };
                    CubeMapTextures.Add((string)tomlData["Name"], CubeMapTexture.LoadCubeMap(faces));
                }
                catch
                {
                    throw new Exception("Failed to load cubemap texture " + (string)tomlData["Name"]);
                }

            }
            foreach(string mod in ModManager.ModAssetFolder)
            {
                try
                {
                    string[] TomlsShader = Directory.GetFiles(mod + "/" + BaseCubeMapTextureFolder, "*.toml");
                    foreach (string filePath in TomlsShader)
                    {
                        TomlTable tomlData = Toml.ToModel(File.ReadAllText(filePath));
                        try
                        {
                            List<string> faces = new List<string>
                    {
                    mod+"/"+BaseCubeMapTextureFolder+"/"+(string)tomlData["Right"],
                    mod+"/"+BaseCubeMapTextureFolder+"/"+(string)tomlData["Left"],
                    mod+"/"+BaseCubeMapTextureFolder+"/"+(string)tomlData["Bottom"],
                    mod+"/"+BaseCubeMapTextureFolder+"/"+(string)tomlData["Top"],
                    mod+"/"+BaseCubeMapTextureFolder+"/"+(string)tomlData["Front"],
                    mod+"/"+BaseCubeMapTextureFolder+"/"+(string)tomlData["Back"]
                    };
                            CubeMapTextures.Add((string)tomlData["Name"], CubeMapTexture.LoadCubeMap(faces));
                        }
                        catch
                        {
                            throw new Exception("Failed to load cubemap texture " + (string)tomlData["Name"]);
                        }

                    }
                }
                catch { }
            }
        }
    }
}
