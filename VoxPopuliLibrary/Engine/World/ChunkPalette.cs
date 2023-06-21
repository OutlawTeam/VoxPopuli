namespace VoxPopuliLibrary.Engine.World
{
    [Serializable]
    internal struct Palette
    {
        private Dictionary<byte, string> blocks;
        private byte IdPtr;
        private Stack<byte> freeBlockIDs;
        public Palette()
        {
            blocks = new Dictionary<byte, string>
            {
                { 0, "air" }
            };
            IdPtr = 1;
            freeBlockIDs = new Stack<byte>();
        }
        internal void RemoveBlock(byte id)
        {
            if (id != 0)
            {
                if (blocks.ContainsKey(id))
                {
                    blocks.Remove(id);
                    freeBlockIDs.Push(id);
                }
                else
                {
                    throw new Exception("The block id to remove doesn't exist!");// Gestion de l'erreur : l'identifiant de bloc n'existe pas dans la palette
                }
            }
        }
        internal byte AddBlock(string block)
        {
            byte newBlockID;
            if (freeBlockIDs.Count > 0)
            {
                newBlockID = freeBlockIDs.Pop();
            }
            else
            {
                if( IdPtr <255)
                {
                    
                    newBlockID = IdPtr;
                    IdPtr+=1;
                    
                }else
                {
                    throw new Exception("Palette is over flow");
                }
            }
            blocks.Add(newBlockID, block);
            return newBlockID;
        }
        internal string GetBlock(byte id)
        {
            if(blocks.ContainsKey(id))
            {
                return blocks[id];
            }else
            {
                throw new Exception("This block isnt in block pallete");
            }
        }
        internal bool ContainBlock(string id)
        {
            return blocks.ContainsValue(id);
        }
        internal byte GetBlockId(string block)
        {
            return blocks.FirstOrDefault(x => x.Value ==block).Key;
        }
    }
}
