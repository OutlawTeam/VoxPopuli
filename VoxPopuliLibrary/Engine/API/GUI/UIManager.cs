namespace VoxPopuliLibrary.Engine.API.GUI
{
    public static class UIManager
    {
        static Dictionary<string ,Menu> Menus = new Dictionary<string ,Menu>();
        static bool ShowMenu = false;
        static Menu Current;

        public static void AddMenu(string menuName,Menu menu)
        {
            Menus.Add(menuName, menu);
        }
        public static void SetMenu(string MenuName)
        {
            if(MenuName == "void")
            {
                ShowMenu = false;
            }else
            {
                if (Menus.TryGetValue(MenuName, out Menu menu))
                {
                    ShowMenu = true;
                    Current = menu;
                }
            }
        }
        public static void Render()
        {
            if (ShowMenu)
            {
                Current.Render();
            }
        }
        public static void Update()
        {
            if(ShowMenu) 
            {
                Current.Update();
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
