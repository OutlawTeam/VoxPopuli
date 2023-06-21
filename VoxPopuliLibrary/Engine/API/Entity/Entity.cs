using OpenTK.Mathematics;
namespace VoxPopuliLibrary.Engine.API
{
    public class Entity
    {
        
        internal Vector3d Position;
        internal Vector3 Rotation = new Vector3(0, 0, 0);
        internal string ID = Guid.NewGuid().ToString();
        internal virtual void UpdateClient(float DT) { }
        internal virtual void UpdateServer(float DT) { }
    }
}
