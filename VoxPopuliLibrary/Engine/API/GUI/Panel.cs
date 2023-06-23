using OpenTK.Mathematics;
using VoxPopuliLibrary.Engine.API.GUI.Elements;
using VoxPopuliLibrary.Engine.GraphicEngine;

namespace VoxPopuliLibrary.Engine.API.GUI
{
    public class Panel
    {
        List<Widget> widgets;
        bool BackGroundImage = false;
        public Palette ColorPallete;
        string BackGround;
        Vector4 BackGroundColor;
        public Position PositionMin;
        public Position PositionMax;
        public int ESX = 20;
        public int ESY = 20;
        public void AddWidget(Widget wid)
        {
            wid.SetPanelParent(this);
            widgets.Add(wid);
        }
        public Panel(Position position, Position size,string backGround,Palette pal)
        {
            widgets = new List<Widget>();
            PositionMin = position;
            PositionMax = size;
            BackGroundImage = true; 
            BackGround = backGround;
            ColorPallete = pal;
        }
        public Panel(Position position, Position size, Vector4 backGround, Palette pal)
        {
            widgets = new List<Widget>();
            PositionMin = position;
            PositionMax = size;
            BackGroundColor = backGround;
            ColorPallete = pal;
        }
        internal void Render()
        {
            if(BackGroundImage)
            {
                Renderer.RenderImage(BackGround, PositionMin.GetRealX(), PositionMin.GetRealY(), PositionMax.GetRealX(), PositionMax.GetRealY());
            }else
            {
                Renderer.RenderRec(PositionMin.GetRealX(), PositionMin.GetRealY(), PositionMax.GetRealX(), PositionMax.GetRealY(), BackGroundColor);
            }
            foreach(Widget wid in widgets)
            {
                wid.Render();
            }
            ESX = 20;
            ESY = 20;
        }
    }
}
