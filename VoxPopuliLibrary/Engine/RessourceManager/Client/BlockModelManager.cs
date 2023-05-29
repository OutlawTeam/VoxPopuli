using Newtonsoft.Json;
using VoxPopuliLibrary.Engine.GraphicEngine;

namespace VoxPopuliLibrary.Engine.RessourceManager
{
    internal static partial class RessourceManager
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
        }
    }
    class BlockModelData
    {
        public string Name { get; set; }
        public float[][] Model { get; set; }
    }
}
