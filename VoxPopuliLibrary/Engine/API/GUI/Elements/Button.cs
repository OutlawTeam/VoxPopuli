using Newtonsoft.Json.Linq;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using VoxPopuliLibrary.Engine.GraphicEngine;

namespace VoxPopuliLibrary.Engine.API.GUI.Elements
{
    public class Button : Widget
    {
        private Action CallBack;
        public Button(string label, Action callBack)
        {
            Label = label;
            CallBack = callBack;
        }

        public override void Render()
        {
            base.Render();
            RenderA(Panel.ESX + Panel.PositionMin.GetRealX(), Panel.ESY + Panel.PositionMin.GetRealY(),
                Panel.PositionMax.GetRealX()-Panel.ESX -(Panel.ESX + Panel.PositionMin.GetRealX()),40);
            Panel.ESY += 42;
        }
        private void RenderA(int x,int y,int w,int h)
        {
            if (UIManager.CIN(x, y, w, h))
            { // check if cursor is in the region of the checkbox
                if (Input.InputSystem.MousePressed(MouseButton.Left))
                {
                    Renderer.RenderRec(x, y,w, h,Panel.ColorPallete.Clicked);
                    CallBack();
                }else
                {
                    Renderer.RenderRec(x, y, w,  h, Panel.ColorPallete.Passed);
                }
            }
            else
            {
                Renderer.RenderRec(x, y,  w, h, Panel.ColorPallete.Normal);
            }
            //Renderer.RenderText(Label, "FreeSans", x + 10, y + 15, 20, Panel.ColorPallete.Text);
        }
    }
}
