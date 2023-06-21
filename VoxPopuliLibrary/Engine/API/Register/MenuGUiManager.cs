using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using VoxPopuliLibrary.Engine.Program;


namespace VoxPopuliLibrary.Engine.API
{
    internal static class MenuGUiManager
    {
        public static float Scale=1;
        public static int Width = 1000;
        public static int Height = 800;
        public static Matrix4 projectionMatrix;
        
        
        internal static void UpdateScale(int width,int height)
        {
            float referenceWidth = 1920f; // Largeur de référence
            float referenceHeight = 1080f; // Hauteur de référence
            float fwidth = (float)width;
            float fheight = (float)height;
            if(fheight == 1055)
            {
                fheight += 25;
            }
            if (fwidth/fheight != 16f/9f)
            {
                 Scale = Math.Min(width / referenceWidth, height / referenceHeight);
            }else
            {
                Scale = 1;
            }
            Width = width;
            Height = height;
            projectionMatrix = Matrix4.Identity;
            projectionMatrix = Matrix4.CreateOrthographic(width,height, -1, 1);
        }
    }
}
