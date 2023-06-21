using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using Window = VoxPopuliLibrary.Engine.Program.Window;
namespace VoxPopuliLibrary.Engine.API
{
    internal static class API
    {
        public static Window window;
        public static void SetFullscreen(bool fullscreen)
        {
            if (fullscreen)
            {
                window.WindowState = WindowState.Fullscreen;
            }
            else
            {
                window.WindowState = WindowState.Normal;
            }

        }
        public static Vector2 GetCursorPos()
        {
            return window.MousePosition;
        }
        public static void Init(Window wind)
        {
            window = wind;
        }
        public static void ResizeWindow(int x, int y)
        {
            window.Size = new Vector2i(x, y);
        }
        public static int WindowWidth()
        {
            return window.Size.X;
        }
        public static int WindowHeight()
        {
            return window.Size.Y;
        }
    }
}
