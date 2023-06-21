using VoxPopuliLibrary.Engine.API;

namespace ModExemple
{
    public class ExampleMod : IMod
    {
        public string Name => "Example Mod";

        public string NameSpace => "ExampleMod";

        public string Description => "An Example mod to show modding features and learn.";

        public string Version => "0.0.0.1";

        public void DeInit()
        {
            throw new NotImplementedException();
        }

        public void Init()
        {
            Block ExempleBlock = new Block();
            ExempleBlock.AllFace = "test";
            BlockRegister.RegisterBlock(NameSpace, ExempleBlock);
        }
    }
}