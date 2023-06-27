using VoxPopuliLibrary.Engine.API;
using VoxPopuliLibrary.Engine.API.GUI;
using VoxPopuliLibrary.Engine.GraphicEngine;
using OpenTK.Mathematics;
using VoxPopuliLibrary.Engine.API.GUI.Elements;
using VoxPopuliLibrary.Engine.Network;
using VoxPopuliLibrary.Engine.Program;

namespace VoxPopuliLibrary.Engine.GUI
{
    internal class MainUI : UI
    {
        public MainUI()
        {
            widgets.Add(new Button("Singleplayer", LaunchSinglePlayer,new Vector2(0.0125f,0.2f), new Vector2(0.175f,0.075f)));
            widgets.Add(new Button("Multiplayer", ()=>{ Console.WriteLine("Multiplayer");},new Vector2(0.0125f,0.3f), new Vector2(0.175f,0.075f)));
            widgets.Add(new Button("Options", ()=>{ Console.WriteLine("Options");},new Vector2(0.0125f,0.4f), new Vector2(0.175f,0.075f)));
            widgets.Add(new Button("Code Editor for later", ()=>{ Console.WriteLine("Code Editor for later");},new Vector2(0.0125f,0.5f), new Vector2(0.175f,0.075f)));
            widgets.Add(new Button("Model Editor for later", ()=>{ Console.WriteLine("Model Editor for later");},new Vector2(0.0125f,0.6f), new Vector2(0.175f,0.075f)));
        }
        public override void Render()
        {
            Renderer.RenderImage("MainMenuBackGround", 0, 0,15f, 1080f/256f*2f, 0,0,API.API.WindowWidth(), API.API.WindowHeight());
            Renderer.RenderRec(0,0, (int)(0.2f * API.API.WindowWidth()),  API.API.WindowHeight(),new Vector4(0.177f, 0.177f, 0.177f,0.5f));
            Renderer.RenderImage("Logo", 20, 20, (int)(0.2f * API.API.WindowWidth())-40, (int)(0.1f * API.API.WindowHeight()));

            base.Render();
        }
        public override void Update()
        {
            base.Update();
        }
        public static void LaunchSinglePlayer()
        {
            ServerThread.RunServerThread();
            ClientNetwork.Connect("localhost",23482);
            UIManager.SetUiShow("MainMenu", false);
        }
    }
}
