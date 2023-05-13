/**
 * Server main process loop
 * Copyrights Florian Pfeiffer
 * Author Florian Pfeiffer
 */
using System.Diagnostics;
using VoxPopuliLibrary.client;
using VoxPopuliLibrary.common.ecs.server;
using VoxPopuliLibrary.common.voxel.common;
using VoxPopuliLibrary.common.voxel.server;
using VoxPopuliLibrary.server.network;
namespace VoxPopuliLibrary.server.program
{
    public static class Program
    {

        internal static string Title = @"
         __      __       _____                  _ _    _____                          
         \ \    / /      |  __ \                | (_)  / ____|                         
          \ \  / /____  _| |__) |__  _ __  _   _| |_  | (___   ___ _ ____   _____ _ __ 
           \ \/ / _ \ \/ /  ___/ _ \| '_ \| | | | | |  \___ \ / _ \ '__\ \ / / _ \ '__|
            \  / (_) >  <| |  | (_) | |_) | |_| | | |  ____) |  __/ |   \ V /  __/ |   
             \/ \___/_/\_\_|   \___/| .__/ \__,_|_|_| |_____/ \___|_|    \_/ \___|_|   
                                    | |                                                
                                    |_|                                                
        ";
        internal static string Author = @"
           ___           ____       __  __                ______             
          / _ )__ __    / __ \__ __/ /_/ /__ __    __    /_  __/__ ___ ___ _ 
         / _  / // /   / /_/ / // / __/ / _ `/ |/|/ /     / / / -_) _ `/  ' \
        /____/\_, /    \____/\_,_/\__/_/\_,_/|__,__/     /_/  \__/\_,_/_/_/_/
             /___/                                                           
        ";
        public static void Main()
        {
            Console.SetWindowSize(100, 25);
            Console.WriteLine(Title);
            Console.WriteLine(Author);
            Console.WriteLine("The standalone server starts is initialization");
            /*
             _____      _ _   _       _ _           _____                                      
            |_   _|    (_| | (_)     | (_)         /  ___|                                     
              | | _ __  _| |_ _  __ _| |_ _______  \ `--.  ___  __ _ _   _  ___ _ __   ___ ___ 
              | || '_ \| | __| |/ _` | | |_  / _ \  `--. \/ _ \/ _` | | | |/ _ | '_ \ / __/ _ \
             _| || | | | | |_| | (_| | | |/ |  __/ /\__/ |  __| (_| | |_| |  __| | | | (_|  __/
             \___|_| |_|_|\__|_|\__,_|_|_/___\___| \____/ \___|\__, |\__,_|\___|_| |_|\___\___|
                                                                  | |                          
                                                                  |_|              
            */

            AllBlock.init();
            Console.WriteLine("Blocks has been Initialize");
            GlobalVariable.LoadServer();
            Console.WriteLine("Settings has been Initialize");
            Network.StartServer(23482);
            Console.WriteLine("Network has been Initialized");

            Console.WriteLine("The server has finished initializing, it is now ready at: " + Network.server.LocalPort);
            Console.WriteLine("Server game version: " + common.Version.VersionNumber);

            // end

            var frameWatch = Stopwatch.StartNew();
            var LowUpdate = Stopwatch.StartNew();
            float dt = 1f / 60f;
            while (true)
            {
                Network.Update();

                PlayerFactory.Update(dt);

                ChunkManager.Update();

                if (LowUpdate.ElapsedMilliseconds > 3)
                {
                    PlayerFactory.SendData();
                    LowUpdate.Restart();
                }
                while (frameWatch.Elapsed.TotalSeconds < 0.016)
                {
                    Thread.Sleep(1);
                }
                dt = (float)frameWatch.Elapsed.TotalSeconds;
                frameWatch.Restart();
            }
        }
    }
}
