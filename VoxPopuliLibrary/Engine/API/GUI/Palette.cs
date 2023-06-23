using OpenTK.Mathematics;

namespace VoxPopuliLibrary.Engine.API.GUI
{
    public struct Palette
    {
        public Vector4 Normal;
        public Vector4 Passed;
        public Vector4 Clicked;
        public Vector4 Text;
        public Palette(Vector4 normal,Vector4 passed,Vector4 clicked,Vector4 text)
        {
            Normal = normal;
            Passed = passed;
            Clicked = clicked;
            Text = text;
        }
    }
}
