using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxPopuliLibrary.Engine.API
{
    public struct BlockTexture
    {
        public BlockTexture() { }
        public string AllFace
        {
            set
            {
                TopTexture = value; BottomTexture = value; BackTexture = value;
                FrontTexture = value; RightTexture = value; LeftTexture = value;
            }
        }
        public string TopTexture = "unknow";
        public string BottomTexture = "unknow";
        public string BackTexture = "unknow";
        public string FrontTexture = "unknow";
        public string RightTexture = "unknow";
        public string LeftTexture = "unknow";
    }
}
