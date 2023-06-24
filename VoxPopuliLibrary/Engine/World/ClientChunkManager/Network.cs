using K4os.Compression.LZ4;
using LiteNetLib;
using LiteNetLib.Utils;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace VoxPopuliLibrary.Engine.World
{
    internal partial class ClientChunkManager
    {
        internal void HandleChunk(ServerChunkData data, NetPeer peer)
        {
            Vector3i pos = new Vector3i(data.x, data.y, data.z);
            if (!Clist.TryGetValue(pos, out Chunk _))
            {
                Clist.Add(pos, new Chunk(data.data.data, data.data.pal, pos));
                if (Clist[pos].Empty == false)
                {
                    ChunkMesh.Add(Clist[pos]);
                }
            }
        }
        internal void HandleChunkUnload(UnloadChunk data, NetPeer peer)
        {
            Vector3i pos = new Vector3i(data.x, data.y, data.z);
            if (Clist.TryGetValue(pos, out Chunk ch))
            {
                ch.DeleteBuffer();
                ch = null;
                Clist.Remove(pos);
            }
        }
        internal void HandleChunkUpdate(OneBlockChange data, NetPeer peer)
        {
            Vector3i cpos = new Vector3i(data.cx, data.cy, data.cz);
            Vector3i bpos = new Vector3i(data.bx, data.by, data.bz);
            if (Clist.TryGetValue(cpos, out Chunk ch))
            {
                ch.SetBlock(bpos.X, bpos.Y, bpos.Z, data.BlockID);
                if (ch.Empty == true)
                {
                    if (!ch.Blocks.All(element => element == 0))
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
                        ch.Changed = true;
                        ChunkMesh.Add(ch);
                    }
                }
            }
            if (Clist.TryGetValue(new Vector3i(cpos.X + 1, cpos.Y, cpos.Z), out Chunk ch1))
            {
                if (ch1.Empty == false)
                {
                    ch1.Changed = true;
                    ChunkMesh.Add(ch1);
                }
            }
            if (Clist.TryGetValue(new Vector3i(cpos.X - 1, cpos.Y, cpos.Z), out Chunk ch2))
            {
                if (ch2.Empty == false)
                {
                    ch2.Changed = true;
                    ChunkMesh.Add(ch2);
                }
            }
            if (Clist.TryGetValue(new Vector3i(cpos.X, cpos.Y - 1, cpos.Z), out Chunk ch3))
            {
                if (ch3.Empty == false)
                {
                    ch3.Changed = true;
                    ChunkMesh.Add(ch3);
                }
            }
            if (Clist.TryGetValue(new Vector3i(cpos.X, cpos.Y + 1, cpos.Z), out Chunk ch4))
            {
                if (ch4.Empty == false)
                {
                    ch4.Changed = true;
                    ChunkMesh.Add(ch4);
                }
            }
            if (Clist.TryGetValue(new Vector3i(cpos.X, cpos.Y, cpos.Z + 1), out Chunk ch5))
            {
                if (ch5.Empty == false)
                {
                    ch5.Changed = true;
                    ChunkMesh.Add(ch5);
                }
            }
            if (Clist.TryGetValue(new Vector3i(cpos.X, cpos.Y, cpos.Z - 1), out Chunk ch6))
            {
                if (ch6.Empty == false)
                {
                    ch6.Changed = true;
                    ChunkMesh.Add(ch6);
                }
            }
        }
    }
}
