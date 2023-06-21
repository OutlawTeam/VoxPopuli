using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxPopuliLibrary.Engine.API
{
    internal static class Utils
    {
        public static string GetName(string NameSpace,string Name)
        {
            return NameSpace +":"+ Name;
        }
    }
}
