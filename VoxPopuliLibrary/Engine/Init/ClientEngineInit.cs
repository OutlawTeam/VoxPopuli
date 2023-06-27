using VoxPopuliLibrary.Engine.API.GUI;
using VoxPopuliLibrary.Engine.Font;
using VoxPopuliLibrary.Engine.GraphicEngine.FontRenderer;
using VoxPopuliLibrary.Engine.GUI;
using OpenTK.Mathematics;

namespace VoxPopuliLibrary.Engine.Init
{
    internal static class ClientEngineInit
    {
        public static void Init()
        {
            UIManager.AddUI("MainMenu",new MainUI());
            UIManager.SetUiShow("MainMenu", true);
        }
    }
}
