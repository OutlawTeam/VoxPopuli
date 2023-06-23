using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxPopuliLibrary.Engine.API.GUI.Elements
{
    public class Widget
    {
        public Panel Panel;
        public string Label;
        public void SetPanelParent(Panel panel)
        {
            Panel = panel;
        }
        public virtual void Render()
        {

        }
    }
}
