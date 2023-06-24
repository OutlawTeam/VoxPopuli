using VoxPopuliLibrary.Engine.Font;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;

namespace VoxPopuliLibrary.Engine.GraphicEngine.FontRenderer
{
    public static class TextMaster
    {
        private static Dictionary<FontType,List<GUIText>> texts = new();
        private static FontRenderer renderer;
        public static FontType Base = new FontType("assets/engine/fonts/arial.png",
            "assets/engine/fonts/arial.fnt");
        public static void Init()
        {
            renderer = new FontRenderer();
        }
        public static void render()
        {
            renderer.Render(texts);
        }
        public static void LoadText(GUIText text)
        {
            FontType font = text.getFont();
            TextMeshData data = font.loadText(text);
            int VAO = GL.GenVertexArray();
            int VBO = GL.GenBuffer();
            List<float> Vertices = new();
            for (int i =0; i< data.getVertexCount(); i++)
            {
                Vertices.Add(data.getVertexPositions()[i *2]);
                Vertices.Add(data.getVertexPositions()[i * 2 + 1]);
                Vertices.Add(data.getTextureCoords()[i *2]);
                Vertices.Add(data.getTextureCoords()[i *2 + 1]);
            }
            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Count * sizeof(float), Vertices.ToArray(), BufferUsageHint.StaticDraw);

            int vertexLocation = 0;
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
            int UVLocation = 1;
            GL.EnableVertexAttribArray(UVLocation);
            GL.VertexAttribPointer(UVLocation, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 2 * sizeof(float));
            text.setMeshInfo(VAO, data.getVertexCount());
            if (texts.TryGetValue(font,out List<GUIText> textBatch))
            {
            }
            else
            {
                textBatch = new List<GUIText>();
                texts.Add(font, textBatch);
            }
            textBatch.Add(text);
        }
        public static void RemoveText(GUIText text)
        {
            List<GUIText> textBatch = texts[text.getFont()];
            textBatch.Remove(text);
            if (textBatch.Count !=0)
            {
                texts.Remove(text.getFont());
            }
        }
    }
}
