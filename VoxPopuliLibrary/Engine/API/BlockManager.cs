using OpenTK.Mathematics;
using VoxPopuliLibrary.Engine.Physics;

namespace VoxPopuliLibrary.Engine.API
{
    internal static class BlockManager
    {
        public static Block dirt = new Block();
        public static Block grass = new Block();
        public static Block cobblestone = new Block();
        public static Block slab = new Block();
        public static Dictionary<string, Block> BlockList = new Dictionary<string, Block>();
        public static void InitClient()
        {
            dirt.texture0 = RessourceManager.RessourceManager.GetAtlasTextures("dirt");/*TextureAtlas.IdtoCord(0)*///;
            dirt.texture1 = RessourceManager.RessourceManager.GetAtlasTextures("dirt");/*TextureAtlas.IdtoCord(0)*///;
            dirt.texture2 = RessourceManager.RessourceManager.GetAtlasTextures("dirt");/*TextureAtlas.IdtoCord(0, 1)*///;
            dirt.texture3 = RessourceManager.RessourceManager.GetAtlasTextures("dirt");/*TextureAtlas.IdtoCord(0, 1)*///;
            dirt.texture4 = RessourceManager.RessourceManager.GetAtlasTextures("dirt");/*TextureAtlas.IdtoCord(0)*///;
            dirt.texture5 = RessourceManager.RessourceManager.GetAtlasTextures("dirt");/*TextureAtlas.IdtoCord(0)*///;
            dirt.IsTransparent = false;
            dirt.Mesh = RessourceManager.RessourceManager.GetBlockMesh("Cube");
            dirt.IsSolid = true;
            dirt.Colliders = RessourceManager.RessourceManager.GetPhysicCollider("Cube");
            BlockList.Add("dirt", dirt);
            //grass
            grass.texture0 = RessourceManager.RessourceManager.GetAtlasTextures("grass");//TextureAtlas.IdtoCord(1);
            grass.texture1 = RessourceManager.RessourceManager.GetAtlasTextures("grass");//TextureAtlas.IdtoCord(1);
            grass.texture2 = RessourceManager.RessourceManager.GetAtlasTextures("grass");////TextureAtlas.IdtoCord(1, 1);
            grass.texture3 = RessourceManager.RessourceManager.GetAtlasTextures("grass");//TextureAtlas.IdtoCord(1, 1);
            grass.texture4 = RessourceManager.RessourceManager.GetAtlasTextures("grass_top");//TextureAtlas.IdtoCord(2);
            grass.texture5 = RessourceManager.RessourceManager.GetAtlasTextures("grass");//TextureAtlas.IdtoCord(0);
            grass.IsTransparent = false;
            grass.Mesh = RessourceManager.RessourceManager.GetBlockMesh("Cube");
            grass.Colliders = RessourceManager.RessourceManager.GetPhysicCollider("Cube");
            BlockList.Add("gras", grass);
            //cobblestone
            cobblestone.texture0 = RessourceManager.RessourceManager.GetAtlasTextures("cobblestone");//TextureAtlas.IdtoCord(3);
            cobblestone.texture1 = RessourceManager.RessourceManager.GetAtlasTextures("cobblestone");//TextureAtlas.IdtoCord(3);
            cobblestone.texture2 = RessourceManager.RessourceManager.GetAtlasTextures("cobblestone");//TextureAtlas.IdtoCord(3, 1);
            cobblestone.texture3 = RessourceManager.RessourceManager.GetAtlasTextures("cobblestone");//TextureAtlas.IdtoCord(3, 1);
            cobblestone.texture4 = RessourceManager.RessourceManager.GetAtlasTextures("cobblestone");//TextureAtlas.IdtoCord(3);
            cobblestone.texture5 = RessourceManager.RessourceManager.GetAtlasTextures("cobblestone");//TextureAtlas.IdtoCord(3);
            cobblestone.IsTransparent = false;
            cobblestone.Mesh = RessourceManager.RessourceManager.GetBlockMesh("Cube");
            cobblestone.Colliders = RessourceManager.RessourceManager.GetPhysicCollider("Cube");
            BlockList.Add("cobblestone", cobblestone);
        }
        public static void InitServer()
        {
            dirt.IsTransparent = false;
            dirt.Colliders = RessourceManager.ServerRessourceManager.GetPhysicCollider("Cube");
            if(!BlockList.ContainsKey("dirt"))
            {
                BlockList.Add("dirt", dirt);
            }
            

            grass.IsTransparent = false;
            grass.Colliders = RessourceManager.ServerRessourceManager.GetPhysicCollider("Cube");
            if (!BlockList.ContainsKey("grass"))
            {
                BlockList.Add("grass", grass);
            }

            cobblestone.IsTransparent = false;
            cobblestone.Colliders = RessourceManager.ServerRessourceManager.GetPhysicCollider("Cube");
            if (!BlockList.ContainsKey("cobblestone"))
            {
                BlockList.Add("cobblestone", cobblestone);
            }
        }
        public static float[] GetTex(string id, int face)
        {
            BlockList.TryGetValue(id, out var block);
            if (block != null)
            {
                if (face == 2)
                {
                    return block.texture0;
                }
                else if (face == 3)
                {
                    return block.texture1;
                }
                else if (face == 5)
                {
                    return block.texture2;
                }
                else if (face == 4)
                {
                    return block.texture3;
                }
                else if (face == 0)
                {
                    return block.texture4;
                }
                else
                {
                    return block.texture5;
                }
            }
            else
            {
                return RessourceManager.RessourceManager.GetAtlasTextures("unknow");
            }
        }
        public static bool BlockTransparent(string id)
        {
            if (id == "air")
            {
                return true;
            }
            else if (BlockList.TryGetValue(id, out Block temp))
            {
                if (temp.IsTransparent)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
        internal static float[] BlockMesh(string id, int face)
        {
            if (BlockList.TryGetValue(id, out var block))
            {
                return block.Mesh.GetMesh()[face];
            }
            else
            {
                return Array.Empty<float>();
            }
        }
        public static bool BlockSolid(string id)
        {
            if (id == "air")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
