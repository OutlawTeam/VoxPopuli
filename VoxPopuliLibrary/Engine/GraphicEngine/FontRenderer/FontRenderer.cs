using OpenTK.Graphics.OpenGL4;
using VoxPopuliLibrary.Engine.Font;
using OpenTK.Mathematics;

namespace VoxPopuliLibrary.Engine.GraphicEngine.FontRenderer
{
    public class FontRenderer
    {

        private Shader Fontshader;

        public FontRenderer()
        {
            Fontshader = new Shader("assets/engine/shaders/font.vert", "assets/engine/shaders/font.frag");
        }

        public void Render(Dictionary<FontType, List<GUIText>> texts)
        {
            prepare();
            foreach (FontType font in texts.Keys)
            {
                font.getTextureAtlas().Use(TextureUnit.Texture0);
                foreach (GUIText text in texts[font])
                {
                    if(text.Show == true)
                    {
                        renderText(text);
                        text.show(false);
                    }
                }
            }
            endRendering();
        }

        private void prepare()
        {
            GL.Enable(EnableCap.Blend);
            //GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.BlendFunc(0, BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            
            GL.Disable(EnableCap.DepthTest);
            Fontshader.Use();
        }

        private void renderText(GUIText text)
        {
            GL.BindVertexArray(text.getMesh());
            Fontshader.SetVector3("color" ,text.getColour());
            Fontshader.SetVector2("translation", text.getPosition());
            GL.DrawArrays(PrimitiveType.Triangles, 0, text.getVertexCount());
            GL.BindVertexArray(0);
        }

        private void endRendering()
        {
            GL.Disable(EnableCap.Blend);
            GL.Enable(EnableCap.DepthTest);
        }

    }
}
