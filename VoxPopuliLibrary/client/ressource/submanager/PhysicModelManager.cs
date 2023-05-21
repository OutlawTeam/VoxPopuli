using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxPopuliLibrary.client.graphic;
using VoxPopuliLibrary.common.physic;

namespace VoxPopuliLibrary.client.ressource
{
    internal static partial class RessourceManager
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
                    throw new Exception("Collider can't be loaded due to :" + ex);
                }
            }
        }
    }
    class ColliderData
    {
        public string Name { get; set; }
        public Collider[] Colliders { get; set; }
    }
}
