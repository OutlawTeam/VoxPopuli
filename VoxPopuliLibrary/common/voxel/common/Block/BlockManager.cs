using VoxPopuliLibrary.client.graphic;
using VoxPopuliLibrary.common.physic;
using OpenTK.Mathematics;
namespace VoxPopuliLibrary.common.voxel.common
{
    internal class BlockManager
    {
        public static block dirt = new block();
        public static block grass = new block();
        public static block cobblestone = new block();
        public static Dictionary<int, block> BlockList = new Dictionary<int, block>();
        public static int id = 1;
        public static BlockMesh Cube;
        internal static Collider CubeColl = new Collider(new Vector3(0, 0, 0), new Vector3(1, 1, 1));
        public static void init()
        {
            Cube = new BlockMesh();
            Cube.Mesh = new float[][]{
                new float[]{0f, 1f, 0f, 1f, 1f, 0f, 1f, 1f, 1f, 1f, 1f, 1f, 0f, 1f, 1f, 0f, 1f, 0f},//top
                new float[]{0f, 0f, 0f, 1f, 0f, 0f, 1f, 0f, 1f, 1f, 0f, 1f, 0f, 0f, 1f, 0f, 0f, 0f},//bottom
                new float[]{0f, 0f, 1f, 1f, 0f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 0f, 1f, 1f, 0f, 0f, 1f}, //front
                new float[]{0f, 0f, 0f, 1f, 0f, 0f, 1f, 1f, 0f, 1f, 1f, 0f, 0f, 1f, 0f, 0f, 0f, 0f},//back
                new float[]{1f, 1f, 1f, 1f, 1f, 0f, 1f, 0f, 0f, 1f, 0f, 0f, 1f, 0f, 1f, 1f, 1f, 1f},// right
                new float[]{0f, 1f, 1f, 0f, 1f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 1f, 0f, 1f, 1f}//left
                };
            //dirt
            dirt.texture0 = TextureAtlas.IdtoCord(0);
            dirt.texture1 = TextureAtlas.IdtoCord(0);
            dirt.texture2 = TextureAtlas.IdtoCord(0, 1);
            dirt.texture3 = TextureAtlas.IdtoCord(0, 1);
            dirt.texture4 = TextureAtlas.IdtoCord(0);
            dirt.texture5 = TextureAtlas.IdtoCord(0);
            dirt.IsTransparent = false;
            dirt.Mesh = Cube;
            dirt.IsSolid = true;
            dirt.Colliders.Add(CubeColl);
            BlockList.Add(id, dirt);
            id++;
            //grass
            grass.texture0 = TextureAtlas.IdtoCord(1);
            grass.texture1 = TextureAtlas.IdtoCord(1);
            grass.texture2 = TextureAtlas.IdtoCord(1, 1);
            grass.texture3 = TextureAtlas.IdtoCord(1, 1);
            grass.texture4 = TextureAtlas.IdtoCord(2);
            grass.texture5 = TextureAtlas.IdtoCord(0);
            grass.IsTransparent = false;
            grass.Mesh = Cube;
            grass.Colliders.Add(CubeColl);
            BlockList.Add(id, grass);
            id++;
            //cobblestone
            cobblestone.texture0 = TextureAtlas.IdtoCord(3);
            cobblestone.texture1 = TextureAtlas.IdtoCord(3);
            cobblestone.texture2 = TextureAtlas.IdtoCord(3, 1);
            cobblestone.texture3 = TextureAtlas.IdtoCord(3, 1);
            cobblestone.texture4 = TextureAtlas.IdtoCord(3);
            cobblestone.texture5 = TextureAtlas.IdtoCord(3);
            cobblestone.IsTransparent = false;
            cobblestone.Mesh = Cube;
            cobblestone.Colliders.Add(CubeColl);
            BlockList.Add(id, cobblestone);
            id++;
        }
        public static float[] gettex(ushort id, int face)
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
                return TextureAtlas.IdtoCord(1022);
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
                return block.Mesh.Mesh[face];
            }else
            {
                return new float[0];
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
