using Newtonsoft.Json.Linq;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using VoxPopuliLibrary.Engine.GraphicEngine;
using VoxPopuliLibrary.Engine.Font;
using VoxPopuliLibrary.Engine.GraphicEngine.FontRenderer;

namespace VoxPopuliLibrary.Engine.API.GUI.Elements
{
    public class Button : Widget
    {
        private Action CallBack;
        private Vector2 position;
        private Vector2 size;
        private GUIText text;
        public Button(string label, Action callBack, Vector2 position, Vector2 size)
        {
            text = new GUIText(label,8f / label.Length,TextMaster.Base,position+new Vector2(0,(size.Y/2)-(8f / label.Length * 0.015625f)),position.X+size.X,true);
            text.setColour(0f, 0f, 0f);
            text.show(false);
            CallBack = callBack;
            this.position = position;
            this.size = size;
        }

        public override void Render()
        {
            base.Render();
            text.show(true);
            RenderA((int)(position.X * API.WindowWidth()), (int)(position.Y * API.WindowHeight()), (int)(size.X * API.WindowWidth()), (int)(size.Y * API.WindowHeight()));
        }
        private void RenderA(int x,int y,int w,int h)
        {
            if (UIManager.CIN(x, y, w, h))
            { // check if cursor is in the region of the checkbox
                if (Input.InputSystem.MousePressed(MouseButton.Left))
                {
                    Renderer.RenderImage("button_pressed",x, y,w, h);
                    CallBack();
                }else
                {
                    Renderer.RenderImage("button_over",x, y, w,  h);
                }
            }
            else
            {
                Renderer.RenderImage("button",x, y,  w, h);
            }
            //Renderer.RenderText(Label, "FreeSans", x + 10, y + 15, 20, Panel.ColorPallete.Text);
        }
    }
}
