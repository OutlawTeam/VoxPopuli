
using VoxPopuliLibrary.Engine.API;
namespace ModExemple
{
    public class ExampleMod : IMod
    {
        public string Name => "Example Mod";

        public  static string NameSpace => "ExampleMod";

        public string Description => "An Example mod to show modding features and learn.";

        public string Version => "0.0.0.1";

        public void DeInit()
        {
            throw new NotImplementedException();
        }

        public void Init()
        {
            var test = BlockRegister.RegisterBlock(Utils.GetName(NameSpace, "test_block"), new Block(new BlockBuilder().SetTexture(new BlockTexture() { AllFace = "Test" })));
        }
    }
}