using VoxPopuliLibrary.Engine.API;

namespace VoxPopuliLibrary.Game
{
    internal  class VoxPopuliMod : IMod
    {
        public string Name => "VoxPopuli";
        public static string NameSpace => "VoxPopuli";

        public string Description => "Official Game";

        public string Version => "0.0.0.1";

        public void Init()
        {
            BlockMod.RegistersBlocks();
        }
        public void DeInit()
        {
            throw new NotImplementedException();
        }
    }
}
