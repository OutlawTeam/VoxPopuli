using OpenTK.Mathematics;
using System.Text.Json.Serialization;
using VoxPopuliLibrary.Engine.Physics;
using VoxPopuliLibrary.Engine.World;
namespace VoxPopuliLibrary.Engine.API
{
    public class PhysicEntity : Entity
    {
        internal Collider Coll = new Collider();
        internal float EntityHeight = 1.80f;
        internal float EntityWidth= 0.60f;
        internal float EntityDepth = 0.60f;
        internal float EntityEYEHeight;
        internal float JumpHeight = 1.25f;
        internal bool Grounded;

        internal string GroundedBlock;

        internal Vector3 Velocity;
        internal Vector3 Acceleration;

        internal bool Fly;
        internal Vector3 Friction
        {
            get
            {
                if (Fly)
                {
                    return PhysicConst.DragFly;
                }
                else if (Grounded)
                {
                    return BlockRegister.GetFriction(GroundedBlock);
                }
                else if (Velocity.Y > 0)
                {
                    return PhysicConst.DragJump;
                }
                else
                {
                    return PhysicConst.DragFall;
                }
            }
        }
        internal void UpdateCollider()
        {
            double x = Position.X;
            double y = Position.Y;
            double z = Position.Z;

            Coll.x1 = x - EntityWidth / 2;
            Coll.x2 = x + EntityWidth / 2;

            Coll.y1 = y;
            Coll.y2 = y + EntityHeight;

            Coll.z1 = z - EntityDepth / 2;
            Coll.z2 = z + EntityDepth / 2;
        }
        internal void Jump(float height = default)
        {
            if (!Grounded)
            {
                return;
            }
            if (height == default)
            {
                height = JumpHeight;
            }
            Velocity.Y = (float)Math.Sqrt(-2 * PhysicConst.Gravity.Y * height);
        }
        internal void CollisionTerrain(float DT)
        {
            Grounded = false;
            for (int _ = 0; _ < 3; _++)
            {
                Vector3 AVel = Velocity * DT;
                double vx = AVel.X;
                double vy = AVel.Y;
                double vz = AVel.Z;
                int step_x = vx > 0 ? 1 : -1;
                int step_y = vy > 0 ? 1 : -1;
                int step_z = vz > 0 ? 1 : -1;
                int steps_xz = (int)Math.Ceiling(EntityWidth / 2);
                int steps_y = (int)EntityHeight;
                int x = (int)Math.Floor(Position.X);
                int y = (int)Math.Floor(Position.Y);
                int z = (int)Math.Floor(Position.Z);
                int cx = (int)Math.Floor(Position.X + Velocity.X);
                int cy = (int)Math.Floor(Position.Y + Velocity.Y);
                int cz = (int)Math.Floor(Position.Z + Velocity.Z);
                List<Tuple<double?, Vector3d,string>> PossibleCollision = new List<Tuple<double?, Vector3d,string>>();
                for (int i = x - step_x * (steps_xz + 1); vx > 0 ? i < cx + step_x * (steps_xz + 2) : i > cx + step_x * (steps_xz + 2); i += step_x)
                {
                    for (int j = y - step_y * (steps_y + 2); vy > 0 ? j < cy + step_y * (steps_y + 3) : j > cy + step_y * (steps_y + 3); j += step_y)
                    {
                        for (int k = z - step_z * (steps_xz + 1); vz > 0 ? k < cz + step_z * (steps_xz + 2) : k > cz + step_z * (steps_xz + 2); k += step_z)
                        {
                            if (ClientWorldManager.world.GetChunkManagerClient().GetBlock(i, j, k, out string id))
                            {
                                if (id != "air")
                                {
                                    foreach (Collider collider in RessourceManager.RessourceManager.GetPhysicCollider(BlockRegister.BlockList[id].GetCollider()))
                                    {
                                        (double? entry_time, Vector3d normal) = Coll.Collide(collider.Move(new Vector3i(i, j, k)), AVel);
                                        if (entry_time == null)
                                        {
                                            continue;
                                        }
                                        PossibleCollision.Add(new Tuple<double?, Vector3d,string>(entry_time, normal,id));
                                    }
                                }
                            }
                        }
                    }
                }
                if (PossibleCollision.Count != 0)
                {
                    (double? entry_time, Vector3d normal,string id) = PossibleCollision.OrderBy(x => x.Item1).First();
                    entry_time -= 0.001f;
                    if (normal.X != 0)
                    {
                        Velocity.X = 0;
                        Position.X += (double)(vx * entry_time);
                    }
                    if (normal.Y != 0)
                    {
                        Velocity.Y = 0;

                        Position.Y += (double)(vy * entry_time);
                    }
                    if (normal.Z != 0)
                    {
                        Velocity.Z = 0;

                        Position.Z += (double)(vz * entry_time);
                    }
                    if (normal.Y == 1)
                    {
                        Grounded = true;
                        GroundedBlock = id;
                    }
                }
            }
            Position += Velocity * DT;
            Vector3 gravity;
            if (Fly)
            {
                gravity = PhysicConst.Zero;
            }
            else
            {
                gravity = PhysicConst.Gravity;
            }
            Velocity += gravity * DT;
            Velocity -= Maths.Min.MinAbs(Velocity * Friction * DT, Velocity);

        }
        internal void CollisionTerrainServer(float DT)
        {
            Grounded = false;
            for (int _ = 0; _ < 3; _++)
            {
                Vector3 AVel = Velocity * DT;

                double vx = AVel.X;
                double vy = AVel.Y;
                double vz = AVel.Z;

                int step_x = vx > 0 ? 1 : -1;

                int step_y = vy > 0 ? 1 : -1;

                int step_z = vz > 0 ? 1 : -1;

                int steps_xz = (int)Math.Ceiling(EntityWidth / 2);

                int steps_y = (int)EntityHeight;

                int x = (int)Math.Floor(Position.X);
                int y = (int)Math.Floor(Position.Y);
                int z = (int)Math.Floor(Position.Z);

                int cx = (int)Math.Floor(Position.X + Velocity.X);
                int cy = (int)Math.Floor(Position.Y + Velocity.Y);
                int cz = (int)Math.Floor(Position.Z + Velocity.Z);
                List<Tuple<double?, Vector3d,string>> PossibleCollision = new List<Tuple<double?, Vector3d,string>>();
                for (int i = x - step_x * (steps_xz + 1); vx > 0 ? i < cx + step_x * (steps_xz + 2) : i > cx + step_x * (steps_xz + 2); i += step_x)
                {
                    for (int j = y - step_y * (steps_y + 2); vy > 0 ? j < cy + step_y * (steps_y + 3) : j > cy + step_y * (steps_y + 3); j += step_y)
                    {
                        for (int k = z - step_z * (steps_xz + 1); vz > 0 ? k < cz + step_z * (steps_xz + 2) : k > cz + step_z * (steps_xz + 2); k += step_z)
                        {
                            if (ServerWorldManager.world.GetChunkManagerServer().GetBlock(i, j, k, out string id))
                            {
                                if (id != "air")
                                {
                                    foreach (Collider collider in RessourceManager.RessourceManager.GetPhysicCollider(BlockRegister.BlockList[id].GetCollider()))
                                    {
                                        (double? entry_time, Vector3d normal) = Coll.Collide(collider.Move(new Vector3i(i, j, k)), AVel);

                                        if (entry_time == null)
                                        {
                                            continue;
                                        }
                                        PossibleCollision.Add(new Tuple<double?, Vector3d,string>(entry_time, normal,id));
                                    }
                                }
                            }
                        }
                    }
                }
                if (PossibleCollision.Count != 0)
                {
                    (double? entry_time, Vector3d normal,string id) = PossibleCollision.OrderBy(x => x.Item1).First();

                    entry_time -= 0.001f;
                    if (normal.X != 0)
                    {
                        Velocity.X = 0;
                        Position.X += (double)(vx * entry_time);
                    }
                    if (normal.Y != 0)
                    {
                        Velocity.Y = 0;

                        Position.Y += (double)(vy * entry_time);
                    }
                    if (normal.Z != 0)
                    {
                        Velocity.Z = 0;

                        Position.Z += (double)(vz * entry_time);
                    }
                    if (normal.Y == 1)
                    {
                        Grounded = true;
                        GroundedBlock = id;
                    }
                }
            }
            Position += Velocity * DT;
            Vector3 gravity;
            if (Fly)
            {
                gravity = PhysicConst.Zero;
            }
            else
            {
                gravity = PhysicConst.Gravity;
            }

            Velocity += gravity * DT;

            Velocity -= Maths.Min.MinAbs(Velocity * Friction * DT, Velocity);


        }
        internal virtual void UpdateClient(float DT)
        {
            base.UpdateClient(DT);
            Velocity += Acceleration * DT * Friction;
            Acceleration = new Vector3(0);
            UpdateCollider();
        }
        internal virtual void UpdateServer(float DT)
        {
            Velocity += Acceleration * DT * Friction;
            Acceleration = new Vector3(0);
            UpdateCollider();
        }
    }
}
