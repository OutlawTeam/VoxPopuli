using OpenTK.Mathematics;
using System.Reflection;
using OpenTK.Graphics.OpenGL4;
using SharpFont;
using VoxPopuliLibrary.Engine.World;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace VoxPopuliLibrary.Engine.GraphicEngine
{
 
    public struct Character
    {
        public int TextureID { get; set; }
        public Vector2 Size { get; set; }
        public Vector2 Bearing { get; set; }
        public int Advance { get; set; }
    }
    internal static class FontManager
    {
        static internal Matrix4 projectionM;
        internal static void Update(int X ,int Y)
        {
            projectionM = Matrix4.CreateScale(new Vector3(1f / X, 1f / Y, 1.0f));
            projectionM = Matrix4.CreateOrthographicOffCenter(0.0f, X, Y, 0.0f, -1.0f, 1.0f);
        }
        internal static Library lib = new Library();
    }
    public class FreeTypeFont
    {
        Dictionary<uint, Character> _characters = new Dictionary<uint, Character>();
        int _vao;
        int _vbo;

        public FreeTypeFont(string path)
        {
            // initialize library
           
            Face face = new Face(FontManager.lib,path);
            face.SetPixelSizes(0, 48);

            // set 1 byte pixel alignment 
            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);

            // set texture unit
            GL.ActiveTexture(TextureUnit.Texture0);

            // Load first 128 characters of ASCII set
            for (uint c = 0; c < 128; c++)
            {
                try
                {
                    // load glyph
                    //face.LoadGlyph(c, LoadFlags.Render, LoadTarget.Normal);
                    face.LoadChar(c, LoadFlags.Render, LoadTarget.Normal);
                    GlyphSlot glyph = face.Glyph;
                    FTBitmap bitmap = glyph.Bitmap;

                    // create glyph texture
                    int texObj = GL.GenTexture();
                    GL.BindTexture(TextureTarget.Texture2D, texObj);
                    GL.TexImage2D(TextureTarget.Texture2D, 0,
                                  PixelInternalFormat.R8, bitmap.Width, bitmap.Rows, 0,
                                  PixelFormat.Red, PixelType.UnsignedByte, bitmap.Buffer);

                    // set texture parameters
                    GL.TextureParameter(texObj, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                    GL.TextureParameter(texObj, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                    GL.TextureParameter(texObj, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
                    GL.TextureParameter(texObj, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

                    // add character
                    Character ch = new Character();
                    ch.TextureID = texObj;
                    ch.Size = new Vector2(bitmap.Width, bitmap.Rows);
                    ch.Bearing = new Vector2(glyph.BitmapLeft, glyph.BitmapTop);
                    ch.Advance = (int)glyph.Advance.X;
                    _characters.Add(c, ch);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            // bind default texture
            GL.BindTexture(TextureTarget.Texture2D, 0);

            // set default (4 byte) pixel alignment 
            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 4);

            float[] vquad =
            {
            // x      y      u     v    
                0.0f, -1.0f,   0.0f, 0.0f,
                0.0f,  0.0f,   0.0f, 1.0f,
                1.0f,  0.0f,   1.0f, 1.0f,
                0.0f, -1.0f,   0.0f, 0.0f,
                1.0f,  0.0f,   1.0f, 1.0f,
                1.0f, -1.0f,   1.0f, 0.0f
            };

            // Create [Vertex Buffer Object](https://www.khronos.org/opengl/wiki/Vertex_Specification#Vertex_Buffer_Object)
            _vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, 4 * 6 * 4, vquad, BufferUsageHint.StaticDraw);

            // [Vertex Array Object](https://www.khronos.org/opengl/wiki/Vertex_Specification#Vertex_Array_Object)
            _vao = GL.GenVertexArray();
            GL.BindVertexArray(_vao);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 4 * 4, 0);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 4 * 4, 2 * 4);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
        }

        public void RenderText(string text, Vector3 color,float x, float y, float scale, Vector2 dir)
        {
            GL.Disable(EnableCap.DepthTest);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindVertexArray(_vao);
            RessourceManager.RessourceManager.GetShader("Font").Use();
            RessourceManager.RessourceManager.GetShader("Font").SetVector3("textColor", color);
            RessourceManager.RessourceManager.GetShader("Font")
                .SetMatrix4NoTranspose("projection", FontManager.projectionM);
            float angle_rad = (float)Math.Atan2(dir.Y, dir.X);
            Matrix4 rotateM = Matrix4.CreateRotationZ(angle_rad);
            Matrix4 transOriginM = Matrix4.CreateTranslation(new Vector3(x, y, 0f));

            // Iterate through all characters
            float char_x = 0.0f;
            foreach (var c in text)
            {
                if (_characters.ContainsKey(c) == false)
                    continue;
                Character ch = _characters[c];

                float w = ch.Size.X * scale;
                float h = ch.Size.Y * scale;
                float xrel = char_x + ch.Bearing.X * scale;
                float yrel = (ch.Size.Y - ch.Bearing.Y) * scale;

                // Now advance cursors for next glyph (note that advance is number of 1/64 pixels)
                char_x += (ch.Advance >> 6) * scale; // Bitshift by 6 to get value in pixels (2^6 = 64 (divide amount of 1/64th pixels by 64 to get amount of pixels))

                Matrix4 scaleM = Matrix4.CreateScale(new Vector3(w, h, 1.0f));
                Matrix4 transRelM = Matrix4.CreateTranslation(new Vector3(xrel, yrel, 0.0f));

                Matrix4 modelM = scaleM * transRelM * rotateM * transOriginM; // OpenTK `*`-operator is reversed
                RessourceManager.RessourceManager.GetShader("Font").SetMatrix4NoTranspose("model", modelM);

                // Render glyph texture over quad
                GL.BindTexture(TextureTarget.Texture2D, ch.TextureID);

                // Render quad
                GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
            }

            GL.BindVertexArray(0);
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.Enable(EnableCap.DepthTest);
        }
        public void Render3DText(string text, Vector3 color, Vector3 position, float scale, Vector3 dir)
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindVertexArray(_vao);
            RessourceManager.RessourceManager.GetShader("Font3D").Use();
            RessourceManager.RessourceManager.GetShader("Font3D").SetVector3("textColor", color);
            RessourceManager.RessourceManager.GetShader("Font3D")
                .SetMatrix4("projection", ClientWorldManager.world.LocalPlayerCamera().GetProjectionMatrix());
            RessourceManager.RessourceManager.GetShader("Font3D")
                .SetMatrix4("view", ClientWorldManager.world.LocalPlayerCamera().GetViewMatrix());
            
            Matrix4 transOriginM = Matrix4.CreateTranslation(position);
            Matrix4 rotateM = Matrix4.CreateFromQuaternion(Quaternion.FromEulerAngles(MathHelper.DegreesToRadians(dir.X), MathHelper.DegreesToRadians(dir.Y), MathHelper.DegreesToRadians(dir.Z)));

            // Iterate through all characters
            float char_x = 0.0f;
            foreach (var c in text)
            {
                if (_characters.ContainsKey(c) == false)
                    continue;
                Character ch = _characters[c];

                float w = ch.Size.X * scale;
                float h = ch.Size.Y * scale;
                float xrel = char_x + ch.Bearing.X * scale;
                float yrel = (ch.Size.Y - ch.Bearing.Y) * scale;

                // Now advance cursors for next glyph (note that advance is number of 1/64 pixels)
                char_x += (ch.Advance >> 6) * scale; // Bitshift by 6 to get value in pixels (2^6 = 64 (divide amount of 1/64th pixels by 64 to get amount of pixels))

                Matrix4 scaleM = Matrix4.CreateScale(new Vector3(w, h, 1.0f));
                Matrix4 transRelM = Matrix4.CreateTranslation(new Vector3(xrel, yrel, 0.0f));

                Matrix4 modelM = scaleM * transRelM * rotateM *transOriginM; // OpenTK `*`-operator is reversed
                RessourceManager.RessourceManager.GetShader("Font3D").SetMatrix4("model", modelM);

                // Render glyph texture over quad
                GL.BindTexture(TextureTarget.Texture2D, ch.TextureID);

                // Render quad
                GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
            }

            GL.BindVertexArray(0);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
    }
}
