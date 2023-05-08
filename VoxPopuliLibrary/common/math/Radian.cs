using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxPopuliLibrary.common.math
{
    internal static class Radian
    {

        public static double ConvertDegreesToRadians(double degrees)
        {
                double radians = (Math.PI / 180) * degrees;
                return (radians);
        }
    }
}
