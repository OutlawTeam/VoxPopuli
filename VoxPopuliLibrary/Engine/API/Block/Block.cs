/**
 * Block definition
 * Copyrights Florian Pfeiffer
 * Author Florian Pfeiffer
 */

using OpenTK.Mathematics;

namespace VoxPopuliLibrary.Engine.API
{
    public class Block
    {

        private bool Transparency = false;
        private float Friction = 20;
        string Mesh = "Cube";
        string Collider = "Cube";
        BlockTexture Texture;
        BlockRenderType RenderType = BlockRenderType.Block;
        public Block(BlockBuilder builder)
        {
            Transparency = builder.Transparency; 
            Friction = builder.Friction;
            Mesh = builder.Mesh;
            Collider = builder.Collider;
            Texture = builder.Texture;
            RenderType = builder.RenderType;
        }
        public bool GetTransparency()
        {
            return Transparency;
        }
        public Vector3 GetFriction()
        {
            return new Vector3(Friction);
        }
        public string GetMesh()
        {
            return Mesh;
        }
        public string GetCollider()
        {
            return Collider;
        }
        public BlockTexture GetTexture()
        {
            return Texture;
        }
        public BlockRenderType GetRenderType()
        {
            return RenderType;
        }
    }
    
}
