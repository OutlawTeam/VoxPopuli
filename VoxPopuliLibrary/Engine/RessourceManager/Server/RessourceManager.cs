using VoxPopuliLibrary.Engine.Physics;

namespace VoxPopuliLibrary.Engine.RessourceManager
{
    internal static partial class ServerRessourceManager
    {
        private static Dictionary<string, Collider[]> Colliders = new Dictionary<string, Collider[]>();
        internal static void LoadRessources()
        {
            LoadColliders();
        }

    }
}
