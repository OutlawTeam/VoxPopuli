using LiteNetLib.Utils;

namespace VoxPopuliLibrary.Engine.API
{
    internal struct ServerInitialPacket : INetSerializable
    {
        public string EngineVersion;
        public string APIVersion;
        public void Serialize(NetDataWriter writer)
        {
            writer.Put(EngineVersion); writer.Put(APIVersion);
        }
        public void Deserialize(NetDataReader reader)
        {
            EngineVersion = reader.GetString();
            APIVersion = reader.GetString();
        }

        
    }

    internal struct ClientInitialPacket : INetSerializable
    {
        public string Name;
        public void Serialize(NetDataWriter writer)
        {
            writer.Put(Name);
        }
        public void Deserialize(NetDataReader reader)
        {
            Name = reader.GetString();
        }
    }
}
