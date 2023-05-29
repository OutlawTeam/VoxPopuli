using VoxPopuliLibrary.Engine.GraphicEngine;
using VoxPopuliLibrary.Engine.Physics;

namespace VoxPopuliLibrary.Engine.RessourceManager
{
    internal static partial class RessourceManager
    {
        private static Dictionary<string, Shader> Shaders = new Dictionary<string, Shader>();
        private static Dictionary<string, Texture> Textures = new Dictionary<string, Texture>();
        private static Dictionary<string, Texture> GuiTextures = new Dictionary<string, Texture>();
        private static Dictionary<string, CubeMapTexture> CubeMapTextures = new Dictionary<string, CubeMapTexture>();
        private static Dictionary<string, BlockMesh> BlockMeshs = new Dictionary<string, BlockMesh>();
        private static Dictionary<string, Model> Models = new Dictionary<string, Model>();
        private static Dictionary<string, Collider[]> Colliders = new Dictionary<string, Collider[]>();
        private static Texture VoxelAtlas;
        private static Dictionary<string, float[]> BlockAtlasTexture = new Dictionary<string, float[]>();

        internal static void LoadRessources()
        {
            LoadShaders();
            LoadTextures();
            LoadGuiTextures();
            LoadVoxelAtlas();
            LoadCubeMapTextures();
            LoadBlockMeshs();
            LoadModels();
            LoadColliders();
        }

    }
}
