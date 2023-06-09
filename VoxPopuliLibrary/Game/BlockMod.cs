﻿using VoxPopuliLibrary.Engine.API;

namespace VoxPopuliLibrary.Game
{
    internal class BlockMod
    {
        public static Block dirt;
        public static Block grass;
        public static Block stone;
        public static Block cobblestone;
        public static Block water;
        public static Block stoneslab;
        static BlockMod()
        {
            dirt = RegisterBlock(Utils.GetName(VoxPopuliMod.NameSpace,"dirt"), new Block(new BlockBuilder().SetTexture(new BlockTexture { AllFace = "dirt" })));
            grass = RegisterBlock(Utils.GetName(VoxPopuliMod.NameSpace, "grass"), new Block(new BlockBuilder().SetTexture(new BlockTexture
            { AllFace = "grass", TopTexture = "grass_top", BottomTexture = "dirt" })));
            stone = RegisterBlock(Utils.GetName(VoxPopuliMod.NameSpace, "stone"), new Block(new BlockBuilder().SetTexture(new BlockTexture
            { AllFace = "stone" })));
            cobblestone = RegisterBlock(Utils.GetName(VoxPopuliMod.NameSpace, "cobblestone"), new Block(new BlockBuilder().SetTexture(new BlockTexture
            { AllFace = "cobblestone" })));
            water = RegisterBlock(Utils.GetName(VoxPopuliMod.NameSpace, "water"), new Block(new BlockBuilder().SetTexture(new BlockTexture
            { AllFace = "water" }).SetTransparency(true).SetCollider("Nothing")));
            stoneslab = RegisterBlock(Utils.GetName(VoxPopuliMod.NameSpace, "stone_slab"),new Block(new BlockBuilder().SetTexture(new BlockTexture { AllFace ="stone"})
                .SetCollider("Slab").SetMesh("Slab").SetRenderType(BlockRenderType.Other).SetTransparency(true)));
        }
        private static Block RegisterBlock(string Name, Block block)
        {
            return BlockRegister.RegisterBlock(Name,block);
        }
        public static void RegistersBlocks()
        {
            Console.WriteLine("Register all block of the mod!");
        }
    }
}
