using Newtonsoft.Json;
using VoxPopuliLibrary.Engine.API;
using VoxPopuliLibrary.Engine.GraphicEngine;
using VoxPopuliLibrary.Engine.ModdingSystem;

namespace VoxPopuliLibrary.Engine.RessourceManager
{
    public static partial class RessourceManager
    {
        const string BaseBlockModelFolder = "assets/models/blocks";

        public static BlockMesh GetBlockMesh(string Name)
        {
            if (BlockMeshs.TryGetValue(Name, out var blockMesh))
            {
                return blockMesh;
            }
            else
            {
                throw new Exception("The demended model is missing !");
            }
        }
        private static void LoadBlockMeshs()
        {
            string[] JsonModel = Directory.GetFiles(BaseBlockModelFolder, "*.json");
            foreach (string filePath in JsonModel)
            {
                try
                {
                    string json = File.ReadAllText(filePath);
                    var data = JsonConvert.DeserializeObject<BlockModelData>(json);
                    BlockMeshs.Add(data.Name, new BlockMesh(data.Model));
                }
                catch (Exception ex)
                {
                    throw new Exception("Model can't be loaded due to :" + ex);
                }
            }
            foreach (string mod in ModManager.ModAssetFolder)
            {
                try
                {
                    string[] JsonsModel = Directory.GetFiles(mod + "/" + BaseBlockModelFolder, "*.json");
                    foreach (string filePath in JsonsModel)
                    {
                        try
                        {
                            string json = File.ReadAllText(filePath);
                            var data = JsonConvert.DeserializeObject<BlockModelData>(json);
                            BlockMeshs.Add(data.Name, new BlockMesh(data.Model));
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Model can't be loaded due to :" + ex);
                        }
                    }
                }
                catch { }

            }
        
        }
    }
    class BlockModelData
    {
        public string Name { get; set; }
        public float[][] Model { get; set; }
    }
}
