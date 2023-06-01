using LiteNetLib.Utils;

namespace VoxPopuliLibrary.Engine.API
{
    internal struct InitialPacket : INetSerializable
    {
        public string EngineVersion;
        public string GameVersion;
        public void Serialize(NetDataWriter writer)
        {
            writer.Put(EngineVersion); writer.Put(GameVersion);
        }
        public void Deserialize(NetDataReader reader)
        {
            EngineVersion = reader.GetString();
            GameVersion = reader.GetString();
        }

        
    }
}
