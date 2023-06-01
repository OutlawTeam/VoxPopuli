/**
* Utils file where different utils function and class was store
* Copyrights Florian Pfeiffer
* Author Florian Pfeiffer
*/
namespace VoxPopuliLibrary.Engine.Maths
{
    internal class Carray
    {
        internal static int TreetoOne(int x, int y, int z)
        {
            return x + 16 * (y + 16 * z);
        }
    }
}
