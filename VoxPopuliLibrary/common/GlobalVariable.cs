using OpenTK.Mathematics;
using VoxPopuliLibrary.client.debug;
using VoxPopuliLibrary.client.graphic;
namespace VoxPopuliLibrary.client
{
    internal static class GlobalVariable
    {
        internal static int Distance { get { return LoadDistance + RenderDistance; } }
        internal static int LoadDistance;
        internal static int RenderDistance;
        internal static int CHUNK_SIZE;
        internal static int CHUNK_HEIGHT;
        internal static void LoadClient()
        {
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
