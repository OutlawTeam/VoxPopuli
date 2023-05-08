/**
 * Utils file where different utils function and class was store
 * Copyrights Florian Pfeiffer
 * Author Florian Pfeiffer
 */
using VoxPopuliLibrary.client;
namespace VoxPopuliLibrary.common.utils
{
    public class carray
    {
        public static int treedto1d(int x, int y, int z)
        {
            return x + GlobalVariable.CHUNK_SIZE * (y + GlobalVariable.CHUNK_HEIGHT * z);
        }
        public static int twodto1d(int x, int y, int width)
        {
            return x + y * width;
        }
    }
}
