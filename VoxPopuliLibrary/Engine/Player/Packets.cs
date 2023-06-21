using LiteNetLib.Utils;
using OpenTK.Mathematics;

namespace VoxPopuliLibrary.Engine.Player
{
    #region ClientPlayerControl
    internal struct PlayerControl : INetSerializable
    {
        public bool Forward;
        public bool Backward;
        public bool Right;
        public bool Left;
        public bool Space;
        public bool Shift;
        public bool Control;
        public Vector3 Rotation;
        public Vector3 Front;
        public Vector3 CRight;
        public float Elevation;
        public bool Fly;
        public float Dt;

        public void Deserialize(NetDataReader reader)
        {
            Forward = reader.GetBool();
            Backward = reader.GetBool();
            Right = reader.GetBool();
            Left = reader.GetBool();
            Space = reader.GetBool();
            Shift = reader.GetBool();
            Control = reader.GetBool();
            Rotation.X = reader.GetFloat();
            Rotation.Y = reader.GetFloat();
            Rotation.Z = reader.GetFloat();
            Front.X = reader.GetFloat();
            Front.Y = reader.GetFloat();
            Front.Z = reader.GetFloat();
            CRight.X = reader.GetFloat();
            CRight.Y = reader.GetFloat();
            CRight.Z = reader.GetFloat();
            Elevation = reader.GetFloat();
            Fly = reader.GetBool();
            Dt = reader.GetFloat();
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(Forward);
            writer.Put(Backward);
            writer.Put(Right);
            writer.Put(Left);
            writer.Put(Space);
            writer.Put(Shift);
            writer.Put(Control);
            writer.Put(Rotation.X);
            writer.Put(Rotation.Y);
            writer.Put(Rotation.Z);
            writer.Put(Front.X);
            writer.Put(0);
            writer.Put(Front.Z);
            writer.Put(CRight.X);
            writer.Put(0);
            writer.Put(CRight.Z);
            writer.Put(Elevation);
            writer.Put(Fly);
            writer.Put(Dt);
        }
    }
    #endregion

    #region PlayerData
    internal struct PlayerData : INetSerializable
    {
        public ushort ClientID;
        public string Name;
        public Vector3d Position;
        public Vector3 Rotation;
        public void Serialize(NetDataWriter writer)
        {
            writer.Put(ClientID);
            writer.Put(Name);
            writer.Put(Position.X);
            writer.Put(Position.Y);
            writer.Put(Position.Z);
            writer.Put(Rotation.X);
            writer.Put(Rotation.Y);
            writer.Put(Rotation.Z);
        }
        public void Deserialize(NetDataReader reader)
        {
            ClientID = reader.GetUShort();
            Name = reader.GetString();
            Position.X = reader.GetDouble();
            Position.Y = reader.GetDouble();
            Position.Z = reader.GetDouble();
            Rotation.X = reader.GetFloat();
            Rotation.Y = reader.GetFloat();
            Rotation.Z = reader.GetFloat();
        }

    }
    #endregion

    #region PlayerSpawn
    internal struct PlayerSpawn : INetSerializable
    {
        public ushort ClientID;
        public Vector3d Position;
        public void Serialize(NetDataWriter writer)
        {
            writer.Put(ClientID);
            writer.Put(Position.X);
            writer.Put(Position.Y);
            writer.Put(Position.Z);
        }
        public void Deserialize(NetDataReader reader)
        {
            ClientID = reader.GetUShort();
            Position.X = reader.GetDouble();
            Position.Y = reader.GetDouble();
            Position.Z = reader.GetDouble();
        }

    }
    #endregion

    #region PlayerSpawnLocal
    internal struct PlayerSpawnLocal : INetSerializable
    {
        public ushort ClientID;
        public Vector3d Position;
        public void Serialize(NetDataWriter writer)
        {
            writer.Put(ClientID);
            writer.Put(Position.X);
            writer.Put(Position.Y);
            writer.Put(Position.Z);
        }
        public void Deserialize(NetDataReader reader)
        {
            ClientID = reader.GetUShort();
            Position.X = reader.GetDouble();
            Position.Y = reader.GetDouble();
            Position.Z = reader.GetDouble();
        }
    }
    #endregion

    #region PlayerDeco
    internal struct PlayerDeco: INetSerializable
    {
        public ushort ClientID;
        public void Serialize(NetDataWriter writer)
        {
            writer.Put(ClientID);
        }
        public void Deserialize(NetDataReader reader)
        {
            ClientID = reader.GetUShort();
        }
    }
    #endregion
    //
    //Warning to remove for server domination
    //
    #region PlayerPositionTP
    internal struct PlayerPositionTP : INetSerializable
    {
        public Vector3d Position;
        public void Serialize(NetDataWriter writer)
        {
            writer.Put(Position.X);
            writer.Put(Position.Y);
            writer.Put(Position.Z);
        }
        public void Deserialize(NetDataReader reader)
        {
            Position.X = reader.GetDouble();
            Position.Y = reader.GetDouble();
            Position.Z = reader.GetDouble();
        }
    }
    #endregion
}
