

using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using VoxPopuliLibrary.Engine.GraphicEngine;
using System.Reflection.Emit;

namespace VoxPopuliLibrary.Engine.API.GUI.Elements
{/*
    internal class CheckBox : Widget
    {
        public bool Value = false;
        private Action<bool> CallBack;
        public CheckBox(string label,Action<bool> callback) 
        { 
            Label = label;
            CallBack = callback;
        }

        public override void Render()
        {
            base.Render();
            RenderA(Panel.ESX+Panel.PositionMin.GetRealX(), Panel.ESY + Panel.PositionMin.GetRealY());
            Panel.ESY += 32;
        }
        private void RenderA(int x,int y)
        {
            Renderer.RenderRec(x - 2, y - 2, 30, 30, Panel.ColorPallete.Passed); // background/outline
            if (Value)
            {
                Renderer.RenderImage("check", x - 1, y - 1, 28, 28);
                //s.RenderRec(x - 1, y - 1, 28, 28, Panel.ColorPallete.Clicked); // if the option is enabled, draw a green square to let us know
            }
            if (UIManager.CIN(x - 1, y - 1, 28, 28))
            { // check if cursor is in the region of the checkbox
                if (!Value)
                    Renderer.RenderRec(x, y, 26, 26, Panel.ColorPallete.Passed); // hover over animation if the option is not enabled
                if (Input.InputSystem.MousePressed(MouseButton.Left))
                {
                    Value = !Value; // if we click while in the region, change the option
                    CallBack(Value);
                }
            }
            if (!Value)
            { // draw this if the checkbox is not enabled
                
                Renderer.RenderRec(x, y, 26, 26, Panel.ColorPallete.Normal);
                
            }
            //Renderer.RenderText(Label,"FreeSans", x + 48, y+13,20,Panel.ColorPallete.Text); // drawing the message

        }
    }*/
}
