using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace VoxPopuliLibrary.Engine.API.Input
{
    internal static class InputSystem
    {
        public static KeyboardState Keyboard;
        public static MouseState Mouse;
        public static FrameEventArgs FrameEvent;
        public static bool Grab = false;
        internal static void Update(KeyboardState K, MouseState M, FrameEventArgs F)
        {
            Keyboard = K;
            Mouse = M;
            FrameEvent = F;
        }
        internal static bool KeyPressed(Keys key)
        {
            if (Keyboard.IsKeyPressed(key)) { return true; } return false;
        }
        internal static bool MousePressed(MouseButton but)
        {
            if (Mouse.IsButtonPressed(but)) { return true; } return false;
        }
        internal static bool MouseDown(MouseButton but)
        {
            if (Mouse.IsButtonDown(but)) { return true; }
            return false;
        }
        internal static bool KeyDown(Keys key)
        {
            if (Keyboard.IsKeyDown(key)) { return true; }
            return false;
        }
    }
}
