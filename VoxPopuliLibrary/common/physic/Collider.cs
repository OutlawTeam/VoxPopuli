using OpenTK.Mathematics;
namespace VoxPopuliLibrary.common.physic
{
    internal class Collider 
    {
        internal float x1, y1, z1;
        internal float x2, y2, z2;
        internal Collider(Vector3 pos1= default,Vector3 pos2 =default)
        {
            x1 = pos1.X;
            y1 = pos1.Y;
            z1 = pos1.Z;
            x2 = pos2.X;
            y2 = pos2.Y;
            z2 = pos2.Z;
        }
        internal bool Intersect(Collider other)
        {
            return (Math.Min(x2, other.x2) - Math.Max(x1, other.x1)) > 0
            && (Math.Min(y2, other.y2) - Math.Max(y1, other.y1)) > 0
            && (Math.Min(z2, other.z2) - Math.Max(z1, other.z1) > 0);

        }
  
        internal  float Time( float x, float y)
        {
            if (y == 0f)
            {
                return x > 0 ? float.NegativeInfinity:  float.PositiveInfinity;
            }
            else
            {
                return x / y;
            }
        }

        public (float?, Vector3) Collide(Collider collider, Vector3 velocity)
        {
            float vx = velocity.X, vy = velocity.Y, vz = velocity.Z;
            float x_entry = Time(vx > 0 ? collider.x1 - x2 : collider.x2 - x1, vx);
            float x_exit = Time(vx > 0 ? collider.x2 - x1 : collider.x1 - x2, vx);

            float y_entry = Time(vy > 0 ? collider.y1 - y2 : collider.y2 - y1, vy);
            float y_exit = Time(vy > 0 ? collider.y2 - y1 : collider.y1 - y2, vy);

            float z_entry = Time(vz > 0 ? collider.z1 - z2 : collider.z2 - z1, vz);
            float z_exit = Time(vz > 0 ? collider.z2 - z1 : collider.z1 - z2, vz);

            if (x_entry < 0 && y_entry < 0 && z_entry < 0)
            {
                return (null,Vector3.Zero);
            }

            if (x_entry > 1 || y_entry > 1 || z_entry > 1)
            {
                return (null,Vector3.Zero);
            }
            
            float entry = Math.Max(Math.Max(x_entry, y_entry), z_entry);
            float exit_ = Math.Min(Math.Min(x_exit, y_exit), z_exit);
            //Console.WriteLine(entry + " " + exit_);
            if (entry > exit_)
            {
                return (null, Vector3.Zero);
            }
            Vector3 normal = Vector3.Zero;
            if (entry == x_entry)
            {
                normal = new Vector3(vx > 0 ? -1:1, 0, 0);
            }
            else if (entry == y_entry)
            {
                normal = new Vector3(0, vy > 0 ? -1 : 1, 0);
            }
            else if (entry == z_entry)
            {
                normal = new Vector3(0, 0, vz > 0 ? -1 : 1);
            }
            return (entry, normal);
        }

        public Collider Move(Vector3 pos)
        {
            float x = pos.X;
            float y = pos.Y;
            float z = pos.Z;
            return new Collider(new Vector3(this.x1 + x, this.y1 + y, this.z1 + z),new Vector3(this.x2 + x, this.y2 + y, this.z2 + z));
        }
    }
}
