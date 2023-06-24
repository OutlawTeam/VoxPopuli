using VoxPopuliLibrary.Engine.API.GUI.Elements;

namespace VoxPopuliLibrary.Engine.API.GUI
{
    public class UI
    {
        public List<Widget> widgets = new List<Widget>();
        public bool Show = false;
        public virtual void Render()
        {
            if(widgets.Count >0)
            {
                foreach (Widget widget in widgets)
                {
                    widget.Render();
                }
            }
        }
        public virtual void Update()
        {
            if (widgets.Count > 0)
            {
                foreach (Widget widget in widgets)
                {
                    widget.Update();
                }
            }
        }
    }
}
