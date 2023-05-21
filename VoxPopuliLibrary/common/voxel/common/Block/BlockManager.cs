using VoxPopuliLibrary.client.graphic;
using VoxPopuliLibrary.common.physic;
using OpenTK.Mathematics;
using VoxPopuliLibrary.client.ressource;

namespace VoxPopuliLibrary.common.voxel.common
{
    internal class BlockManager
    {
        public static block dirt = new block();
        public static block grass = new block();
        public static block cobblestone = new block();
        public static block slab = new block();
        public static Dictionary<int, block> BlockList = new Dictionary<int, block>();
        public static int id = 1;
        internal static Collider CubeColl = new Collider(new Vector3(0, 0, 0), new Vector3(1, 1, 1));
        public static void InitClient()
        {
            dirt.texture0 = RessourceManager.GetAtlasTextures("dirt");/*TextureAtlas.IdtoCord(0)*///;
            dirt.texture1 = RessourceManager.GetAtlasTextures("dirt");/*TextureAtlas.IdtoCord(0)*///;
            dirt.texture2 = RessourceManager.GetAtlasTextures("dirt");/*TextureAtlas.IdtoCord(0, 1)*///;
            dirt.texture3 = RessourceManager.GetAtlasTextures("dirt");/*TextureAtlas.IdtoCord(0, 1)*///;
            dirt.texture4 = RessourceManager.GetAtlasTextures("dirt");/*TextureAtlas.IdtoCord(0)*///;
            dirt.texture5 = RessourceManager.GetAtlasTextures("dirt");/*TextureAtlas.IdtoCord(0)*///;
            dirt.IsTransparent = false;
            dirt.Mesh = RessourceManager.GetBlockMesh("Cube");
            dirt.IsSolid = true;
            dirt.Colliders = RessourceManager.GetPhysicCollider("Cube");
            BlockList.Add(id, dirt);
            id++;
            //grass
            grass.texture0 = RessourceManager.GetAtlasTextures("grass");//TextureAtlas.IdtoCord(1);
            grass.texture1 = RessourceManager.GetAtlasTextures("grass");//TextureAtlas.IdtoCord(1);
            grass.texture2 = RessourceManager.GetAtlasTextures("grass");////TextureAtlas.IdtoCord(1, 1);
            grass.texture3 = RessourceManager.GetAtlasTextures("grass");//TextureAtlas.IdtoCord(1, 1);
            grass.texture4 = RessourceManager.GetAtlasTextures("grass_top");//TextureAtlas.IdtoCord(2);
            grass.texture5 = RessourceManager.GetAtlasTextures("grass");//TextureAtlas.IdtoCord(0);
            grass.IsTransparent = false;
            grass.Mesh = RessourceManager.GetBlockMesh("Cube");
            grass.Colliders = RessourceManager.GetPhysicCollider("Cube");
            BlockList.Add(id, grass);
            id++;
            //cobblestone
            cobblestone.texture0 = RessourceManager.GetAtlasTextures("cobblestone");//TextureAtlas.IdtoCord(3);
            cobblestone.texture1 = RessourceManager.GetAtlasTextures("cobblestone");//TextureAtlas.IdtoCord(3);
            cobblestone.texture2 = RessourceManager.GetAtlasTextures("cobblestone");//TextureAtlas.IdtoCord(3, 1);
            cobblestone.texture3 = RessourceManager.GetAtlasTextures("cobblestone");//TextureAtlas.IdtoCord(3, 1);
            cobblestone.texture4 = RessourceManager.GetAtlasTextures("cobblestone");//TextureAtlas.IdtoCord(3);
            cobblestone.texture5 = RessourceManager.GetAtlasTextures("cobblestone");//TextureAtlas.IdtoCord(3);
            cobblestone.IsTransparent = false;
            cobblestone.Mesh = RessourceManager.GetBlockMesh("Cube");
            cobblestone.Colliders = RessourceManager.GetPhysicCollider("Cube");
            BlockList.Add(id, cobblestone);
            id++;
        }
        public static void InitServer()
        {
            dirt.IsTransparent = false;
            dirt.Colliders = VoxPopuliLibrary.server.ressource.RessourceManager.GetPhysicCollider("Cube");
            BlockList.Add(id, dirt);
            id++;

            grass.IsTransparent = false;
            grass.Colliders = VoxPopuliLibrary.server.ressource.RessourceManager.GetPhysicCollider("Cube");
            BlockList.Add(id, grass);
            id++;

            cobblestone.IsTransparent = false;
            cobblestone.Colliders = VoxPopuliLibrary.server.ressource.RessourceManager.GetPhysicCollider("Cube");
            BlockList.Add(id, cobblestone);
            id++;
        }
        public static float[] GetTex(ushort id, int face)
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
                return RessourceManager.GetAtlasTextures("unknow");
            }
        }
        public static bool BlockTransparent(ushort id)
        {
            if (id == 0)
            {
                return true;
            }
            else if (BlockList.TryGetValue(id, out block temp))
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
        internal static float[] BlockMesh(ushort id, int face)
        {
            if (BlockList.TryGetValue(id, out var block))
            {
                return block.Mesh.GetMesh()[face];
            }else
            {
                return Array.Empty<float>();
            }
        }
        public static bool BlockSolid(ushort id)
        {
            if (id == 0)
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
