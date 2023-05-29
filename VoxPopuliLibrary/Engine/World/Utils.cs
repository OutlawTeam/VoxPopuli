namespace VoxPopuliLibrary.Engine.World
{
    public static class Utils
    {
        public static byte[] IntsToBytes(ushort[] intArray)
        {
            byte[] result = new byte[intArray.Length * sizeof(ushort)];
            Buffer.BlockCopy(intArray, 0, result, 0, result.Length);
            return result;
        }
        internal static ushort[] BytesToInts(byte[] input)
        {
            var size = input.Length / sizeof(short);
            var ints = new ushort[size];
            for (var index = 0; index < size; index++)
            {
                ints[index] = BitConverter.ToUInt16(input, index * sizeof(ushort));
            }
            return ints;
        }
    }
}
