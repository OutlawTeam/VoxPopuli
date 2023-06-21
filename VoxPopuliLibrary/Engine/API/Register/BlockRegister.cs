using OpenTK.Mathematics;
using VoxPopuliLibrary.Engine.Physics;
using VoxPopuliLibrary.Engine.RessourceManager;

namespace VoxPopuliLibrary.Engine.API
{
    public static class BlockRegister
    {
        public static Dictionary<string, Block> BlockList = new Dictionary<string, Block>();

        public static Block RegisterBlock(string BlockID,Block block)
        {
            Console.WriteLine(BlockID + " is loaded !");
            try
            {
                BlockList.Add(BlockID, block);
                return block;
            }catch
            {
                return null;
            }
            
        }
        public static Vector3 GetFriction(string id)
        {
            if(id != "air")
            {
                return BlockList[id].GetFriction();
            }else
            {
                return Vector3.Zero;
            }
        }
        public static float[] GetTexture(string id, int face)
        {
            BlockList.TryGetValue(id, out var block);
            if (block != null)
            {
                if (face == 2)
                {
                    return RessourceManager.RessourceManager.GetAtlasTextures(block.GetTexture().RightTexture);
                }
                else if (face == 3)
                {
                    return RessourceManager.RessourceManager.GetAtlasTextures(block.GetTexture().LeftTexture);
                }
                else if (face == 5)
                {
                    return RessourceManager.RessourceManager.GetAtlasTextures(block.GetTexture().BackTexture);
                }
                else if (face == 4)
                {
                    return RessourceManager.RessourceManager.GetAtlasTextures(block.GetTexture().FrontTexture);
                }
                else if (face == 0)
                {
                    return RessourceManager.RessourceManager.GetAtlasTextures(block.GetTexture().TopTexture);
                }
                else
                {
                    return RessourceManager.RessourceManager.GetAtlasTextures(block.GetTexture().BottomTexture);
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
                if (temp.GetTransparency())
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
                var modelData = RessourceManager.RessourceManager.GetBlockMesh(block.GetMesh());
                return modelData.GetMesh()[face];
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
