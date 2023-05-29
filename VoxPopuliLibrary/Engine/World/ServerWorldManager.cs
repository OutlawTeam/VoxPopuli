namespace VoxPopuliLibrary.Engine.World
{
    internal static class ServerWorldManager
    {
        public static World world;
        internal static bool Initialized = false;
        public static void InitWorld()
        {
            world = new World(false, "");
            Initialized = true;
        }
        public static void UpdateWorld()
        {
            world.Update();
        }
        public static void RenderWorld()
        {
            world.Render();
        }
    }
}
