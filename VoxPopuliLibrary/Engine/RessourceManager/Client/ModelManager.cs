using Newtonsoft.Json;
using VoxPopuliLibrary.Engine.GraphicEngine;

namespace VoxPopuliLibrary.Engine.RessourceManager
{
    internal static partial class RessourceManager
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
                    Models.Add(data.Name, new Model(data.Model));
                }
                catch (Exception ex)
                {
                    throw new Exception("Model can't be loaded due to :" + ex);
                }
            }
        }
    }
    class ModelData
    {
        public string Name { get; set; }
        public float[] Model { get; set; }
    }
}
