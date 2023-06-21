using OpenTK.Mathematics;
using VoxPopuliLibrary.Engine.GraphicEngine;

namespace VoxPopuliLibrary.Engine.API.GUI
{
    public class Panel
    {
        bool BackGroundImage = false;
        string BackGround;
        Vector4 BackGroundColor;
        Vector2i Position;
        Vector2i Size;

        public Panel(Vector2i position, Vector2i size,string backGround)
        {
            Position = position;
            Size = size;
            BackGroundImage = true; 
            BackGround = backGround;
        }
        public Panel(Vector2i position, Vector2i size, Vector4 backGround)
        {
            Position = position;
            Size = size;
            BackGroundColor = backGround;
        }
        internal void Render()
        {
            if(BackGroundImage)
            {
                Renderer.RenderImage(BackGround,Position.X,Position.Y,Size.X,Size.Y);
            }else
            {
                Renderer.RenderRec(Position.X, Position.Y, Size.X, Size.Y, BackGroundColor);
            }
        }
    }
}
