using OpenTK.Mathematics;
using VoxPopuliLibrary.client.debug;
using VoxPopuliLibrary.client.graphic;
namespace VoxPopuliLibrary.client
{
    internal static class GlobalVariable
    {
        internal static Shader _shader;
        internal static Shader DebugShader;
        internal static Texture _texture;
        internal static Texture _playertexture;
        internal static int Distance { get { return LoadDistance + RenderDistance; } }
        internal static int LoadDistance;
        internal static int RenderDistance;
        internal static int CHUNK_SIZE;
        internal static int CHUNK_HEIGHT;
        internal static DebugBox BlockSelectionBox;
        internal static int maxThreads = 2; // nombre maximal de threads de génération de mesh à utiliser

        internal static float TextureSize = 32f;

        internal static void LoadClient()
        {
            _shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
            DebugShader = new Shader("Shaders/debug.vert", "Shaders/debug.frag");
            _texture = Texture.LoadFromFile("Textures/Voxel.png");
            _playertexture = Texture.LoadFromFile("Textures/player.png");
            BlockSelectionBox = new DebugBox(new Vector3d(1.125f), new Vector4(0, 1, 0, 1));
            RenderDistance = 7;
            LoadDistance = 3;
            CHUNK_SIZE = 16;
            CHUNK_HEIGHT = 512;
        }
        internal static void LoadServer()
        {
            RenderDistance = 7;
            LoadDistance = 3;
            CHUNK_SIZE = 16;
            CHUNK_HEIGHT = 512;
        }
    }
}
