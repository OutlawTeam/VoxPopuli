using VoxPopuliLibrary.common.physic;

namespace VoxPopuliLibrary.server.ressource
{
    internal static partial class RessourceManager
    {
        private static Dictionary<string, Collider[]> Colliders = new Dictionary<string, Collider[]>();
        internal static void LoadRessources()
        {
            LoadColliders();
        }
        
    }
}
