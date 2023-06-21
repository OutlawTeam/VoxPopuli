using ImGuiNET;
using Newtonsoft.Json.Linq;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SharpFont;
using System.Data;
using System.Numerics;
using System.Reflection.Emit;
using VoxPopuliLibrary.Engine.API;
using VoxPopuliLibrary.Engine.Maths;

namespace VoxPopuliLibrary.Engine.API.GUI
{
    public class GUI
    {
        int ESX = 10;
        int ESY = 10;
        bool wait;
        bool go;
        Vector2 Positon = new Vector2(20,20);
        List<Object> objects = new List<Object>();
        static bool test = false;
        static int testint = 1;
        DropDown[] DDList = new DropDown[50];
        int DDIDX;
        static int man;
        static List<string> list = new List<string>() {"Debug1","Debug2","Debug3" };
        public void Render()
        {
            DrawRect(0, 0, 600, 400, 4, ColorToUInt(0.117f, 0.117f, 0.117f, 1));
            DrawCheckBox(ref test,"Test");
            DrawSliderInt(ref testint,0,100,"Test2");
            

            End();
        }
        void  DrawRect(int x, int y, int w, int h, int r, uint col, ImDrawFlags flags = 0)
        {
            Object o = new Object();
            o.x = x;
            o.y = y;
            o.w = w;
            o.h = h;
            o.r = r;
            o.col = col;
            o.flags = flags;
            o.type = ObjectType.RECTANGLE;
            objects.Add(o); // now put our object on the object list, so we can reverse the order later
        }
        void DrawMessage(/*ImFont* font,*/ string text, int x, int y, uint col = 4294967295) {
            Object o = new Object();
            /*o.font = font;*/
            o.text = text;
            o.x = x;
            o.y = y;
            o.col = col;
            o.type = ObjectType.TEXT;
            objects.Add(o);
        }
        void DrawImage(string image, int x, int y, int w, int h, uint col = 4294967295)
        {
            Object o = new Object();
            o.image = image;
            o.x = x;
            o.y = y;
            o.w = w;
            o.h = h;
            o.col = col;
            o.type = ObjectType.IMAGE;
            objects.Add(o);
        }
        void End()
        {
            for (int i = 0; i <= objects.Count - 1; i++)
            {
                Object obj = objects[i];
                switch (obj.type)
                {
                    case ObjectType.RECTANGLE:
                        {
                            DrawRectangleA(obj.x, obj.y, obj.w, obj.h, obj.r, obj.col, obj.flags);
                            break;
                        }
                    case ObjectType.IMAGE:
                        {
                            DrawImageA(obj.image, obj.x, obj.y, obj.w, obj.h, obj.col);
                            break;
                        }
                    case ObjectType.TEXT:
                        {
                            DrawMessageA(/*obj.font,*/ obj.text, obj.x, obj.y, obj.col);
                            break;
                        }

                }
            }
            objects.Clear();
            ESX =(int)Positon.X + 10; // resetting the element spacing to default (read below on automatic spacing)
            ESY= (int)Positon.Y + 10; // resetting the element spacing to default (read below on automatic spacing)
        }
        public void DrawRectangleA(int x,int y,int width, int height,int r,uint col,ImDrawFlags flags =0)
        {
            ImDrawListPtr DrawList =  ImGui.GetBackgroundDrawList();
            x += (int)Positon.X;
            y += (int)Positon.Y;
            DrawList.AddRectFilled(new Vector2(x,y), new Vector2(x+width, y+height),col,r,flags);
        }
        public void DrawMessageA(string text,int x,int y,uint col = 4294967295)
        {
            ImDrawListPtr DrawList = ImGui.GetBackgroundDrawList();
            x += (int)Positon.X;
            y += (int)Positon.Y;
            DrawList.AddText( new Vector2(x, y),col,text);
        }
        void DrawCheckBoxA(int x, int y,ref bool change, string label) 
        {

            if (change) {
                DrawRect(x - 1, y - 1, 14, 14, 3, ColorToUInt(0,1,0,1)); // if the option is enabled, draw a green square to let us know
            }
            if (CIN(x - 1, y - 1, 14, 14)) { // check if cursor is in the region of the checkbox
                if (!change)
                DrawRect(x, y, 12, 12, 3, ColorToUInt(0.196f, 0.196f, 0.196f, 1)); // hover over animation if the option is not enabled
                if (Input.InputSystem.MousePressed(MouseButton.Left)) {
                    change = !change; // if we click while in the region, change the option
                }
            }
            if (!change)
            { // draw this if the checkbox is not enabled
                DrawRect(x, y, 12, 12, 3, ColorToUInt(0.137f, 0.137f, 0.137f, 1));
                DrawRect(x - 2, y - 2, 16, 16, 3, ColorToUInt(0.235f, 0.235f, 0.235f, 255)); // background/outline
            }
            DrawMessage(label, x + 24, y - 2); // drawing the message
        }
        void DrawCheckBox( ref bool change, string label)
        {
            DrawCheckBoxA(ESX, ESY, ref change, label); // note how the spacing isn't done here, otherwise it would be really aids to combine elements
            ESY += 25; // automatic spacing is that simple
        }
        void DrawSliderIntA(int x, int y,  ref int manage, int min, int max, string label) 
        {
            int SliderWidth = 220;
            int valueX = ((manage - min) * SliderWidth / (max - min)); // this calculates the width of the slider (the highlighted part)

            DrawMessage( label, x, y); // draws the label

            DrawMessage(manage.ToString(), x + 200 , y); // draw the value

            

            DrawRect(x, y + 22, SliderWidth, 10, 3, ColorToUInt(0.117f, 0.117f, 0.117f, 255)); // the background of the slider

            DrawRect(x, y + 22, valueX, 10, 3, ColorToUInt(0, 1, 0, 1)); // draw the green part of the slider, notice how valueX is the width

            if (CIN(x, y + 15, SliderWidth + 1, 24)) { // checking if the cursor is in region of the slider


                if (Input.InputSystem.MouseDown(MouseButton.Left))
                {
                    // funky math which makes it work
                    float value_unmapped = Math.Clamp(API.GetCursorPos().X - Positon.X - ESX, 0.0f, (float)(SliderWidth)); // yoinked from zgui
                    int value_mapped = (int)(value_unmapped / SliderWidth * (max - min) + min);
                    manage = value_mapped;
                }
            }
            
        }
        void DrawSliderInt(ref int manage, int min, int max, string label)
        {
            DrawSliderIntA(ESX, ESY, ref manage, min, max, label);
            ESY += 35;

        }
        public  void DrawImageA(string Texture,int x, int y, int width, int height, uint col = 4294967295)
        {
            ImDrawListPtr DrawList = ImGui.GetBackgroundDrawList();
            x += (int)Positon.X;
            y += (int)Positon.Y;
            DrawList.AddImage(RessourceManager.RessourceManager.GetGuiTexture(Texture).Handle,
                new Vector2(x, y), new Vector2(x + width, y + height));
        }
        public bool CIN(int x, int y, int w, int h)
        { // cursor in region

            var CP = API.GetCursorPos();
            if (CP.X > Positon.X + x && CP.X < Positon.X + x + w && CP.Y > Positon.Y + y && CP.Y < Positon.Y + y + h)
                return true;
            return false;
        }
        public static uint ColorToUInt(float r, float g, float b, float a)
        {
            return ImGui.ColorConvertFloat4ToU32(new Vector4(r,g,b,a));
        }
    }

}
