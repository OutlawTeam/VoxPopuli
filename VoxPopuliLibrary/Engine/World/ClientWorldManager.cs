namespace VoxPopuliLibrary.Engine.World
{
    internal static class ClientWorldManager
    {
        public static World world;
        internal static bool Initialized = false;
        public static void InitWorld()
        {
            world = new World(true, "");
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
