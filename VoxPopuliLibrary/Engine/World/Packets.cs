using LiteNetLib;
using LiteNetLib.Utils;
using System.Runtime.Serialization.Formatters.Binary;
namespace VoxPopuliLibrary.Engine.World
{
    #region[ServerChunkData]
    internal struct ServerChunkData : INetSerializable
    {
        public int x;
        public int y;
        public int z;
        public ChunkData data;
        public NetPeer peer;
        public void Serialize(NetDataWriter writer)
        {
            writer.Put(x);
            writer.Put(y);
            writer.Put(z);
            byte[] serializedBytes;
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
#pragma warning disable SYSLIB0011 // Le type ou le membre est obsolète
                formatter.Serialize(stream, data);
#pragma warning restore SYSLIB0011 // Le type ou le membre est obsolète
                serializedBytes = stream.ToArray();
            }
            writer.Put(serializedBytes);
        }
        public void Deserialize(NetDataReader reader)
        {
            x = reader.GetInt();
            y = reader.GetInt();
            z = reader.GetInt();
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream(reader.GetRemainingBytes()))
            {
#pragma warning disable SYSLIB0011 // Le type ou le membre est obsolète
                data = (ChunkData)formatter.Deserialize(stream);
#pragma warning restore SYSLIB0011 // Le type ou le membre est obsolète
                // Utilisez la structure désérialisée selon vos besoins
            }

        }

    }
    #endregion

    #region [UnloadChunk]
    internal struct UnloadChunk : INetSerializable
    {
        public int x;
        public int y;
        public int z;
        public void Serialize(NetDataWriter writer)
        {
            writer.Put(x);
            writer.Put(y);
            writer.Put(z);
        }

        public void Deserialize(NetDataReader reader)
        {
            x = reader.GetInt();
            y = reader.GetInt();
            z = reader.GetInt();
        }

    }
    #endregion

    #region [ClientOneBlockChangeDemand]
    internal struct OneBlockChangeDemand : INetSerializable
    {
        public int cx;
        public int cy;
        public int cz;
        public int bx;
        public int by;
        public int bz;
        public string BlockID;
        public void Serialize(NetDataWriter writer)
        {
            writer.Put(cx);
            writer.Put(cy);
            writer.Put(cz);
            writer.Put(bx);
            writer.Put(by);
            writer.Put(bz);
            writer.Put(BlockID);
        }

        public void Deserialize(NetDataReader reader)
        {
            cx = reader.GetInt();
            cy = reader.GetInt();
            cz = reader.GetInt();
            bx = reader.GetInt();
            by = reader.GetInt();
            bz = reader.GetInt();
            BlockID = reader.GetString();
        }

    }
    #endregion

    #region [ServerOneBlockChange]
    internal struct OneBlockChange : INetSerializable
    {
        public int cx;
        public int cy;
        public int cz;
        public int bx;
        public int by;
        public int bz;
        public string BlockID;
        public void Serialize(NetDataWriter writer)
        {
            writer.Put(cx);
            writer.Put(cy);
            writer.Put(cz);
            writer.Put(bx);
            writer.Put(by);
            writer.Put(bz);
            writer.Put(BlockID);
        }

        public void Deserialize(NetDataReader reader)
        {
            cx = reader.GetInt();
            cy = reader.GetInt();
            cz = reader.GetInt();
            bx = reader.GetInt();
            by = reader.GetInt();
            bz = reader.GetInt();
            BlockID = reader.GetString();
        }

    }
    #endregion
}
