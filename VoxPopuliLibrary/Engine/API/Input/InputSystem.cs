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
    }
}
