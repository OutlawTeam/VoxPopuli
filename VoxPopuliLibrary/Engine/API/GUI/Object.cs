using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxPopuliLibrary.Engine.API.GUI
{
    enum ObjectType
    {
        TEXT,
        RECTANGLE,
        IMAGE
    };
    struct Object
    {

        public int x;
        public int y;
        public int w;
        public int h;
        public uint col;
        public string label;
        public string text;
        public int r;
        public ImFontPtr font;
        public ObjectType type;
        public ImDrawFlags flags;
        public string image;
        public uint topr;
        public uint topl;
        public uint botl;
        public uint botr;
    };
    struct DropDown
    {
        public DropDown() { }
        public bool isClicked = false;
    };
}
