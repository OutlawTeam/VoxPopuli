using OpenTK.Mathematics;

namespace VoxPopuliLibrary.common.math
{
    internal static class Min
    {
        internal static float MinAbs(float a, float b)
        {
            return new[] { a, b }.Aggregate((x, y) => Math.Abs(x) < Math.Abs(y) ? x : y);
        }
        internal static Vector3 MinAbs(Vector3 a, Vector3 b)
        {
            return new Vector3(
                Math.Abs(a.X) < Math.Abs(b.X) ? a.X : b.X,
                Math.Abs(a.Y) < Math.Abs(b.Y) ? a.Y : b.Y,
                Math.Abs(a.Z) < Math.Abs(b.Z) ? a.Z : b.Z
            );
        }
    }
}
