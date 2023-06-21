using Newtonsoft.Json;
using VoxPopuliLibrary.Engine.GraphicEngine;
using VoxPopuliLibrary.Engine.ModdingSystem;

namespace VoxPopuliLibrary.Engine.RessourceManager
{
    public static partial class RessourceManager
    {
        const string BaseModelFolder = "assets/models/entities";
        public static Model GetModel(string Name)
        {
            if (Models.TryGetValue(Name, out Model model))
            {
                return model;
            }
            else
            {
                throw new Exception("The demanded model is missing !");
            }
        }
        private static void LoadModels()
        {
            string[] JsonModel = Directory.GetFiles(BaseModelFolder, "*.json");
            foreach (string filePath in JsonModel)
            {
                try
                {
                    string json = File.ReadAllText(filePath);
                    var data = JsonConvert.DeserializeObject<ModelData>(json);

                    Models.Add(data.Name,new Model(data.Model));
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
                    string[] JsonsModel = Directory.GetFiles(mod + "/" + BaseModelFolder, "*.json");
                    foreach (string filePath in JsonsModel)
                    {
                        try
                        {
                            string json = File.ReadAllText(filePath);
                            var data = JsonConvert.DeserializeObject<ModelData>(json);
                            Models.Add(data.Name, new Model(data.Model));
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
    class ModelData
    {
        public string Name { get; set; }
        public float[] Model { get; set; }
    }
}
