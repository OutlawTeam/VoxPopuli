namespace VoxPopuliLibrary.Engine.World
{
    [Serializable]
    internal struct Palette
    {
        internal Dictionary<byte, string> blocks;
        internal byte nextBlockID;
        private Stack<byte> freeBlockIDs; // Identifiants d'anciens blocs disponibles

        public Palette()
        {
            blocks = new Dictionary<byte, string>();
            blocks.Add(0, "air");
            nextBlockID = 1;
            freeBlockIDs = new Stack<byte>();
        }
        internal void RemoveBlock(byte id)
        {
            if (blocks.ContainsKey(id))
            {
                blocks.Remove(id);
                freeBlockIDs.Push(id);
            }
            else
            {
                // Gestion de l'erreur : l'identifiant de bloc n'existe pas dans la palette
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
                newBlockID = nextBlockID;
                nextBlockID++;
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
