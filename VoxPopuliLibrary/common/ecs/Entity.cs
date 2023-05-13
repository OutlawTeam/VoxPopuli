using OpenTK.Mathematics;
using VoxPopuliLibrary.common.physic;
using VoxPopuliLibrary.common.voxel.client;
using VoxPopuliLibrary.common.voxel.common;

namespace VoxPopuliLibrary.common.ecs
{
    internal class Entity
    {
        internal float EntityEYEHeight = 1.70f;
        internal float EntityHeight = 1.80f;
        internal float EntityWidth = 0.6f;
        internal float EntityZWidth = 0.40f;
        internal float JumpHeight = 1.25f;

        internal Vector3d Position;
        internal Vector3 Rotation = new Vector3(0, 0, 0);
        internal Vector3 Velocity = new Vector3(0);
        internal Vector3 Acceleration = new Vector3(0);

        internal string ID = Guid.NewGuid().ToString();
        internal Collider Coll = new Collider();

        internal Vector3 Gravity = new Vector3(0, -32, 0);
        internal Vector3 Zero = new Vector3(0, 0, 0);

        internal Vector3 DefFriction = new Vector3(20, 20, 20);
        internal Vector3 DragFly = new Vector3(5f, 5f, 5f);
        internal Vector3 DragJump = new Vector3(1.8f, 0, 1.8f);
        internal Vector3 DragFall = new Vector3(1.8f, 0.4f, 1.8f);


        internal bool Fly = false;

        internal bool Grounded = false;
        public Vector3 Friction
        {
            get
            {
                if (Fly)
                {
                    return DragFly;
                }
                else if (Grounded)
                {
                    return DefFriction;
                }
                else if (Velocity.Y > 0)
                {
                    return DragJump;
                }
                else
                {
                    return DragFall;
                }
            }
        }
        private void UpdateCollider()
        {
            float x = (float)Position.X;
            float y = (float)Position.Y;
            float z = (float)Position.Z;

            Coll.x1 = x - EntityWidth / 2;
            Coll.x2 = x + EntityWidth / 2;

            Coll.y1 = y;
            Coll.y2 = y + EntityHeight;

            Coll.z1 = z - EntityWidth / 2;
            Coll.z2 = z + EntityWidth / 2;
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
            Velocity.Y = (float)Math.Sqrt(-2 * Gravity.Y * height);
        }
        internal void CollisionTerrain(float DT)
        {
            Grounded = false;
            for (int _ = 0; _ < 3; _++)
            {
                Vector3 AVel = Velocity * DT;
                float vx = AVel.X;
                float vy = AVel.Y;
                float vz = AVel.Z;
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
                List<Tuple<float?, Vector3>> PossibleCollision = new List<Tuple<float?, Vector3>>();
                for (int i = x - step_x * (steps_xz + 1); vx > 0 ? i < cx + step_x * (steps_xz + 2) : i > cx + step_x * (steps_xz + 2); i += step_x)
                {
                    for (int j = y - step_y * (steps_y + 2); vy > 0 ? j < cy + step_y * (steps_y + 3) : j > cy + step_y * (steps_y + 3); j += step_y)
                    {
                        for (int k = z - step_z * (steps_xz + 1); vz > 0 ? k < cz + step_z * (steps_xz + 2) : k > cz + step_z * (steps_xz + 2); k += step_z)
                        {
                            if (ChunkManager.GetBlock(i, j, k, out ushort id))
                            {
                                if (id != 0)
                                {
                                    foreach (Collider collider in AllBlock.BlockList[id].Colliders)
                                    {
                                        (float? entry_time, Vector3 normal) = Coll.Collide(collider.Move(new Vector3i(i, j, k)), AVel);
                                        if (entry_time == null)
                                        {
                                            continue;
                                        }
                                        //Console.WriteLine("Entryt: " + entry_time + " Normal" + normal.ToString());
                                        PossibleCollision.Add(new Tuple<float?, Vector3>(entry_time, normal));
                                    }
                                }
                            }
                        }
                    }
                }
                if (PossibleCollision.Count() != 0)
                {
                    (float? entry_time, Vector3 normal) = PossibleCollision.OrderBy(x => x.Item1).First();
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
                    }
                }
            }
            Position += Velocity * DT;
            Vector3 gravity;
            if (Fly)
            {
                gravity = Zero;
            }
            else
            {
                gravity = Gravity;
            }
            Velocity += gravity * DT;
            Velocity -= math.Min.MinAbs(Velocity * Friction * DT, Velocity);

        }
        internal void CollisionTerrainServer(float DT)
        {
            Grounded = false;
            for (int _ = 0; _ < 3; _++)
            {
                Vector3 AVel = Velocity * DT;

                float vx = AVel.X;
                float vy = AVel.Y;
                float vz = AVel.Z;

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
                List<Tuple<float?, Vector3>> PossibleCollision = new List<Tuple<float?, Vector3>>();
                for (int i = x - step_x * (steps_xz + 1); vx > 0 ? i < cx + step_x * (steps_xz + 2) : i > cx + step_x * (steps_xz + 2); i += step_x)
                {
                    for (int j = y - step_y * (steps_y + 2); vy > 0 ? j < cy + step_y * (steps_y + 3) : j > cy + step_y * (steps_y + 3); j += step_y)
                    {
                        for (int k = z - step_z * (steps_xz + 1); vz > 0 ? k < cz + step_z * (steps_xz + 2) : k > cz + step_z * (steps_xz + 2); k += step_z)
                        {
                            if (voxel.server.ChunkManager.GetBlock(i, j, k, out ushort id))
                            {
                                if (id != 0)
                                {
                                    foreach (Collider collider in AllBlock.BlockList[id].Colliders)
                                    {
                                        //Console.WriteLine("Block " + i + " : " + j + " : " + k + " id " + id);
                                        (float? entry_time, Vector3 normal) = Coll.Collide(collider.Move(new Vector3i(i, j, k)), AVel);

                                        if (entry_time == null)
                                        {
                                            continue;
                                        }
                                        //Console.WriteLine("Entryt: " + entry_time + " Normal" + normal.ToString());
                                        PossibleCollision.Add(new Tuple<float?, Vector3>(entry_time, normal));
                                    }
                                }
                            }
                        }
                    }
                }
                if (PossibleCollision.Count() != 0)
                {
                    (float? entry_time, Vector3 normal) = PossibleCollision.OrderBy(x => x.Item1).First();

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
                    }
                }
            }
            Position += Velocity * DT;
            Vector3 gravity;
            if (Fly)
            {
                gravity = Zero;
            }
            else
            {
                gravity = Gravity;
            }

            Velocity += gravity * DT;

            Velocity -= math.Min.MinAbs(Velocity * Friction * DT, Velocity);


        }
        internal virtual void UpdateClient(float DT)
        {
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
