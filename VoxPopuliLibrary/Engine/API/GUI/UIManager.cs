namespace VoxPopuliLibrary.Engine.API.GUI
{
    public static class UIManager
    {
        static Dictionary<string ,UI> uis = new Dictionary<string ,UI>();
        static bool ShowMenu = false;
        static int frame = 0;

        public static void AddUI(string menuName,UI menu)
        {
            uis.Add(menuName, menu);
        }
        public static void SetUiShow(string MenuName,bool show)
        {
            if (uis.TryGetValue(MenuName, out UI ui))
            {
                ui.Show = show;
            }
        }
        public static void Render()
        {
            foreach(UI ui in uis.Values)
            {
                if(ui.Show)
                {
                    ui.Render();
                }
            }
            frame++;

            if(frame >= 240)
            {
                GC.Collect(GC.MaxGeneration,GCCollectionMode.Aggressive,true,true);
                frame = 0;
            }
        }
        public static void Update()
        {
            foreach (UI ui in uis.Values)
            {
                if (ui.Show)
                {
                    ui.Update();
                }
            }
        }
        public static bool CIN(int x, int y, int w, int h)
        { // cursor in region

            var CP = API.GetCursorPos();
            if (CP.X >  x && CP.X < x + w && CP.Y >  y && CP.Y <  y + h)
                return true;
            return false;
        }
    }
}
