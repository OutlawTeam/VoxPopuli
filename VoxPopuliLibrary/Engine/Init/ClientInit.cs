using VoxPopuliLibrary.Engine.API.GUI;
using VoxPopuliLibrary.Engine.API.GUI.Elements;
using static System.Net.Mime.MediaTypeNames;

namespace VoxPopuliLibrary.Engine.Init
{
    internal static class ClientInit
    {
        public static void Init()
        {

            Menu mun = new Menu("MainMenuBackGround");
            //CheckBox check = new CheckBox("Test1", (v) => { Debug.DebugSystem.CheckTest = v; });
            //Button but = new Button("TestButton", () => { Console.WriteLine("TestButton"); });
           
            Panel pan = new Panel(new Position(PositionType.Pixel, 0, 0),
                new Position(PositionType.Proportion, 0.2f, 1),
                new OpenTK.Mathematics.Vector4(0.117f, 0.117f, 0.117f, 0.5f),new Palette(
                    new OpenTK.Mathematics.Vector4(0.3f,0.3f,0.3f,1),
                    new OpenTK.Mathematics.Vector4(0.2f, 0.2f, 0.2f, 1),
                    new OpenTK.Mathematics.Vector4(0.175f, 0.175f, 0.175f, 1),
                    new OpenTK.Mathematics.Vector4(1f, 1f, 1f, 1)));
            //pan.AddWidget(check);
            //pan.AddWidget(but);
            mun.AddPanel(pan);
            UIManager.AddMenu("MainMenu",mun);
        }
    }
}
