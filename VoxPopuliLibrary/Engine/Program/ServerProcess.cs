/**
 * Server main process loop
 * Copyrights Florian Pfeiffer
 * Author Florian Pfeiffer
 */
using VoxPopuliLibrary.Engine.API;
using VoxPopuliLibrary.Engine.ModdingSystem;
using VoxPopuliLibrary.Engine.Network;
using VoxPopuliLibrary.Engine.World;

namespace VoxPopuliLibrary.Engine.Program
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
        public static void Main(bool Standalone)
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
            if (Standalone)
            {
                ModManager.LoadMods();
                Console.WriteLine("Mods have been Load");
                ModManager.Init();
                Console.WriteLine("Mods have been Initialize");
                RessourceManager.RessourceManager.LoadRessourcesServer();
                Console.WriteLine("Ressources have been Initialize");
            }
            ServerWorldManager.InitWorld();
            Console.WriteLine("World have been Initialize");
            ServerNetwork.StartServer(23482);
            Console.WriteLine("Network have been Initialized");

            Console.WriteLine("The server has finished initializing, it is now ready at: " + ServerNetwork.server.LocalPort);
            Console.WriteLine("Server engine version: " + API.Version.EngineVersion);
            Console.WriteLine("Server api version: " + API.Version.APIVersion);
            while (true)
            {
                ServerNetwork.Update();
                ServerWorldManager.UpdateWorld();
            }
        }
    }
}
