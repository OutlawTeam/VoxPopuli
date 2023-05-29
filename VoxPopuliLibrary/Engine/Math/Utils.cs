/**
* Utils file where different utils function and class was store
* Copyrights Florian Pfeiffer
* Author Florian Pfeiffer
*/
namespace VoxPopuliLibrary.Engine.Maths
{
    public class Carray
    {
        public static int TreetoOne(int x, int y, int z)
        {
            return x + 16 * (y + 16 * z);
        }
    }
}
