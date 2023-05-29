using Newtonsoft.Json;
using VoxPopuliLibrary.Engine.Physics;

namespace VoxPopuliLibrary.Engine.RessourceManager
{
    internal static partial class ServerRessourceManager
    {
        const string BasePhysicFolder = "assets/models/physics";
        public static Collider[] GetPhysicCollider(string Name)
        {
            if (Colliders.TryGetValue(Name, out Collider[] model))
            {
                return model;
            }
            else
            {
                throw new Exception("The demanded collider is missing !");
            }
        }
        private static void LoadColliders()
        {
            string[] JsonModel = Directory.GetFiles(BasePhysicFolder, "*.json");
            foreach (string filePath in JsonModel)
            {
                try
                {
                    string json = File.ReadAllText(filePath);
                    var data = JsonConvert.DeserializeObject<ColliderData>(json);
                    Colliders.Add(data.Name, data.Colliders);
                }
                catch (Exception ex)
                {
                    throw new Exception("Model can't be loaded due to :" + ex);
                }
            }
        }
    }

}
