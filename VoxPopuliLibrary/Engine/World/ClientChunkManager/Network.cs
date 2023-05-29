using K4os.Compression.LZ4;
using LiteNetLib;
using LiteNetLib.Utils;
using OpenTK.Mathematics;

namespace VoxPopuliLibrary.Engine.World
{
    internal partial class ClientChunkManager
    {
        internal void HandleChunk(NetDataReader data, NetPeer peer)
        {
            Vector3i cpos = new Vector3i(data.GetInt(), data.GetInt(), data.GetInt());
            byte[] blocks = data.GetRemainingBytes();

            blocks = LZ4Pickler.Unpickle(blocks);
            ushort[] block = Utils.BytesToInts(blocks);
            if (!Clist.TryGetValue(cpos, out Chunk vtff))
            {
                Clist.Add(cpos, new Chunk(block, cpos));
                if (Clist[cpos].Empty == false)
                {
                    ChunkMesh.Add(Clist[cpos]);
                }
            }
        }
        internal void HandleChunkUpdate(NetDataReader data)
        {
            ushort block = data.GetUShort();
            Vector3i cpos = new Vector3i(data.GetInt(), data.GetInt(), data.GetInt());
            Vector3i cpos2 = new Vector3i(data.GetInt(), data.GetInt(), data.GetInt());
            if (Clist.TryGetValue(cpos, out Chunk ch))
            {
                ch.SetBlock(cpos2.X, cpos2.Y, cpos2.Z, block);
                if (ch.Empty == true)
                {
                    if (ch.Blocks.All(element => element == 0))
                    {

                    }
                    else
                    {
                        ch.Empty = false;
                        ch.Changed = true;
                        ChunkMesh.Add(ch);
                    }
                }
                else
                {
                    if (ch.Blocks.All(element => element == 0))
                    {
                        ch.Empty = true;
                    }
                    else
                    {
                        ch.Empty = false;
                        ch.Changed = true;
                        ChunkMesh.Add(ch);
                    }
                }
            }
            if (Clist.TryGetValue(new Vector3i(cpos.X + 1, cpos.Y, cpos.Z), out Chunk ch1))
            {
                ch1.Changed = true;
                ChunkMesh.Add(ch1);
            }
            if (Clist.TryGetValue(new Vector3i(cpos.X - 1, cpos.Y, cpos.Z), out Chunk ch2))
            {
                ch2.Changed = true;
                ChunkMesh.Add(ch2);
            }
            if (Clist.TryGetValue(new Vector3i(cpos.X, cpos.Y - 1, cpos.Z), out Chunk ch3))
            {
                ch3.Changed = true;
                ChunkMesh.Add(ch3);
            }
            if (Clist.TryGetValue(new Vector3i(cpos.X, cpos.Y + 1, cpos.Z), out Chunk ch4))
            {
                ch4.Changed = true;
                ChunkMesh.Add(ch4);
            }
            if (Clist.TryGetValue(new Vector3i(cpos.X, cpos.Y, cpos.Z + 1), out Chunk ch5))
            {
                ch5.Changed = true;
                ChunkMesh.Add(ch5);
            }
            if (Clist.TryGetValue(new Vector3i(cpos.X, cpos.Y, cpos.Z - 1), out Chunk ch6))
            {
                ch6.Changed = true;
                ChunkMesh.Add(ch6);
            }
        }
    }
}
