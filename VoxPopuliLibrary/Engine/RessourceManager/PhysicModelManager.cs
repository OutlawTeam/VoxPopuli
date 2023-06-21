using Newtonsoft.Json;
using VoxPopuliLibrary.Engine.ModdingSystem;
using VoxPopuliLibrary.Engine.Physics;

namespace VoxPopuliLibrary.Engine.RessourceManager
{
    public static partial class RessourceManager
    {
        const string BasePhysicFolder = "assets/models/physics";
        public static Collider[] GetPhysicCollider(string Name)
        {
            if(Name == null)
            {
                return new Collider[0];
            }
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
                    throw new Exception("Collider can't be loaded due to :" + ex);
                }
            }
            foreach (string mod in ModManager.ModAssetFolder)
            {
                try
                {
                    string[] JsonsModel = Directory.GetFiles(mod + "/" + BasePhysicFolder, "*.json");
                    foreach (string filePath in JsonsModel)
                    {
                        try
                        {
                            string json = File.ReadAllText(filePath);
                            var data = JsonConvert.DeserializeObject<ColliderData>(json);
                            Colliders.Add(data.Name, data.Colliders);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Collider can't be loaded due to :" + ex);
                        }
                    }
                }
                catch { }
            }
        }
    }
}
