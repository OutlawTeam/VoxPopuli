using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxPopuliLibrary.Engine.API
{
    internal static class Utils
    {
        static StringBuilder builder = new StringBuilder();
        public static string GetName(string NameSpace,string Name)
        {
            builder.Clear();
            builder.Append(NameSpace);
            builder.Append(":");
            builder.Append(Name);
            return builder.ToString();
        }
    }
}
