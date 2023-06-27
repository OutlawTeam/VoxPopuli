using VoxPopuliLibrary.Engine.Program;

namespace VoxPopuliLibrary.Engine.Program
{
    internal static class ServerThread
    {
        internal static void RunServerThread()
        {
            Window.ServerLocalThread = new Thread(() => Program.Main(false, 23482));
            Window.ServerLocalThread.Name = "MainServer";
            Window.ServerLocalThread.Start();
        }
    }
}
