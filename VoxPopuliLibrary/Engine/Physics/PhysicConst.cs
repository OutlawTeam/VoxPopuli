using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxPopuliLibrary.Engine.Physics
{
    internal static class PhysicConst
    {
        public static Vector3 Gravity = new Vector3(0, -32, 0);
        public static Vector3 Zero = new Vector3(0, 0, 0);
        public static Vector3 DragFly = new Vector3(5f, 5f, 5f);
        public static Vector3 DragJump = new Vector3(1.8f, 0, 1.8f);
        public static Vector3 DragFall = new Vector3(1.8f, 0.4f, 1.8f);
    }
}
