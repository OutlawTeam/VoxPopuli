using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoxPopuliLibrary.Engine.GraphicEngine;

namespace VoxPopuliLibrary.Engine.API.GUI
{
    public class Menu
    {
        List<Panel> Panels = new List<Panel>();
        bool BackGroundImage = false;
        string BackGround;
        Vector4 BackGroundColor;
        public Menu(string backGround)
        {
            BackGroundImage = true;
            BackGround = backGround;
        }
        public Menu(Vector4 backGround)
        {
            bool BackGroundImage = false;
            BackGroundColor = backGround;
        }
        public void AddPanel(Panel panel)
        {
            Panels.Add(panel);
        }
        public void Render()
        {
            
            if (BackGroundImage)
            {
                Renderer.RenderImage(BackGround,0, 0, API.WindowWidth(), API.WindowHeight());
            }
            else
            {
                Renderer.RenderRec(0, 0, API.WindowWidth(), API.WindowHeight(), BackGroundColor);
            }
            foreach(Panel panel in Panels)
            {
                panel.Render();
            }
        }
        public void Update()
        {

        }

    }
}
