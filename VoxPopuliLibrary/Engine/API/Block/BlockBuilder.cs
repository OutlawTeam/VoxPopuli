using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxPopuliLibrary.Engine.API
{
    public class BlockBuilder
    {
        internal bool Transparency = false;
        internal float Friction = 20;
        internal string Mesh = "Cube";
        internal string Collider = "Cube";
        internal BlockTexture Texture = new BlockTexture() { AllFace = "Unknow" };
        internal BlockRenderType RenderType = BlockRenderType.Block;
        public BlockBuilder() { }
        public BlockBuilder SetTransparency(bool transparency)
        {this.Transparency = transparency; return this;}
        public BlockBuilder SetFriction(float friction)
        { this.Friction = friction; return this; }
        public BlockBuilder SetMesh(string mesh)
        { this.Mesh = mesh; return this;}
        public BlockBuilder SetCollider(string collider)
        { this.Collider = collider; return this;}
        public BlockBuilder SetTexture(BlockTexture blockTexture)
        { this.Texture = blockTexture; return this;}
        public BlockBuilder SetRenderType(BlockRenderType renderType)
        { this.RenderType = renderType; return this;}
    }

    public enum BlockRenderType
    {
        Block,
        Other
    }
}
