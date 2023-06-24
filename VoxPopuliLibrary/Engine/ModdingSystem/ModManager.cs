using System.IO.Compression;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json.Nodes;
using VoxPopuliLibrary.Engine.API;

namespace VoxPopuliLibrary.Engine.ModdingSystem
{
    internal static class ModManager
    {
        static Dictionary<string,IMod> ModList = new Dictionary<string, IMod>();
        internal static List<string> ModAssetFolder = new List<string>();
        static string BaseModFolder = "mods";
        static string BaseTempModFolder = "temp/mods";
        internal static void LoadMods()
        {
            DirectoryInfo di = new DirectoryInfo(BaseTempModFolder);
            FileInfo[] files = di.GetFiles();
            foreach (FileInfo file in files)
            {
                file.Delete();
            }
            DirectoryInfo[] subDirectories = di.GetDirectories();
            foreach (DirectoryInfo subDirectory in subDirectories)
            {
                subDirectory.Delete(true);
            }
            Console.WriteLine("The temp folder has been clean.");
            string[] Mods = Directory.GetFiles(BaseModFolder, "*.zip");
            foreach (string filePath in Mods)
            {
                using (ZipArchive zip = ZipFile.Open(filePath, ZipArchiveMode.Read))
                {
                    zip.ExtractToDirectory(BaseTempModFolder + "/" + Path.GetFileNameWithoutExtension(filePath));
                    ModAssetFolder.Add(BaseTempModFolder + "/" + Path.GetFileNameWithoutExtension(filePath));
                    string[] Modss = Directory.GetFiles(BaseTempModFolder + "/"+Path.GetFileNameWithoutExtension(filePath), "*.dll");
                    foreach (string filesPath in Modss)
                    {
                        try
                        {
                            // load it
                            Assembly ass = null;
                            ass = Assembly.LoadFrom(filesPath);
                            if (ass != null)
                            {
                                foreach(Type t in ass.GetTypes())
                                {
                                    if (t.GetInterface("VoxPopuliLibrary.Engine.API.IMod") != null)
                                    {
                                        ModList.Add(t.Namespace, (IMod)Activator.CreateInstance(t));

                                        Console.WriteLine("Load a new mod: " + t.Name);
                                    }
                                    else
                                    {
                                        RuntimeHelpers.RunClassConstructor(t.TypeHandle);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("A mod failed to load: "+ex.Message);
                        }
                    }
                }

            }
        }
        internal static void Init()
        {
            ModList.Add("VoxPopuli",new VoxPopuliLibrary.Game.VoxPopuliMod());
            foreach (IMod mod in ModList.Values)
            {
                try
                {
                    mod.Init();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(mod.Name+" failed to init: " + ex);
                }

            }
            Console.WriteLine("All mods are initialized");
        }
    }
}
