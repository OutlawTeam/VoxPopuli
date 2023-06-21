using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;
using Positron.component;
using OpenTK.Mathematics;
using VoxPopuliLibrary.Engine.GraphicEngine;

namespace Positron
{
    public class Positron
    {
        BackendType backendType;
        GameWindow Window;
        internal Shader Base2D = new Shader(ShaderSources.Base2DVert,ShaderSources.Base2DFrag,true);
        internal Shader BaseFont = new Shader(ShaderSources.BaseFontVert,ShaderSources.BaseFontFrag, true);
        Texture Base2DTex = Texture.Base();
        Matrix4 Pro = Matrix4.Identity;
        Matrix4 View = Matrix4.Identity;
        public Positron(BackendType BType,GameWindow window)
        {
            backendType = BType;
            Window = window;
            if(backendType == BackendType.OpenGL)
            {
                InitOpenGl();
            }else
            {
                throw new Exception("Other backend than OpenGL don't implemented!!");
            }
        }
        private void InitOpenGl()
        {
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            //GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.BlendFunc(0, BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Enable(EnableCap.Multisample);
            // Configurer le framebuffer avec 4 samples
            int colorBuffer;
            GL.GenRenderbuffers(1, out colorBuffer);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, colorBuffer);
            GL.RenderbufferStorageMultisample(RenderbufferTarget.Renderbuffer, 4, RenderbufferStorage.Rgba32f, Window.Size.X, Window.Size.Y);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, RenderbufferTarget.Renderbuffer, colorBuffer);

            int depthBuffer;
            GL.GenRenderbuffers(1, out depthBuffer);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, depthBuffer);
            GL.RenderbufferStorageMultisample(RenderbufferTarget.Renderbuffer, 4, RenderbufferStorage.DepthComponent24, Window.Size.X, Window.Size.Y);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, depthBuffer);
        }
        private  Matrix4 MVP = Matrix4.Identity;
        public  void UpdateResize(int w, int h)
        {
            MVP = Matrix4.CreateOrthographicOffCenter(
                0.0f,
                w,
                h,
                0.0f,
                -1.0f,
                1.0f);
            Base2D.SetMatrix4NoTranspose("projection", MVP);
        }
        public  void RenderRec(int x, int y, int w, int h, Vector4 col)
        {
            float[] Vertices = new float[]
            {
                x,y,0,0,col.X,col.Y,col.Z,col.W,
                x+w,y,1,0,col.X,col.Y,col.Z,col.W,
                x+w,y+h,1,1,col.X,col.Y, col.Z,col.W,
                x,y+h,0,1,col.X,col.Y, col.Z,col.W
            };
            int Vao = GL.GenVertexArray();
            int Vbo = GL.GenBuffer();
            GL.BindVertexArray(Vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, Vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(float), Vertices, BufferUsageHint.StaticDraw);

            var vertexLocation = 0;
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            var UVLocation = 1;
            GL.EnableVertexAttribArray(UVLocation);
            GL.VertexAttribPointer(UVLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 2 * sizeof(float));
            var ColorLocation = 2;
            GL.EnableVertexAttribArray(ColorLocation);
            GL.VertexAttribPointer(ColorLocation, 4, VertexAttribPointerType.Float, false, 8 * sizeof(float), 4 * sizeof(float));
            Base2D.Use();
            Base2DTex.Use(TextureUnit.Texture0);
            GL.Disable(EnableCap.DepthTest);
            GL.DrawArrays(PrimitiveType.TriangleFan, 0, 4);
            GL.Enable(EnableCap.DepthTest);

        }
        public  void RenderImage(string image, int x, int y, int w, int h)
        {

            Vector4 col = new Vector4(1);


            float[] Vertices = new float[]
            {
                x,y,0,0,col.X,col.Y,col.Z,col.W,
                x+w,y,1,0,col.X,col.Y,col.Z,col.W,
                x+w,y+h,1,1,col.X,col.Y, col.Z,col.W,
                x,y+h,0,1,col.X,col.Y, col.Z,col.W
            };
            int Vao = GL.GenVertexArray();
            int Vbo = GL.GenBuffer();
            GL.BindVertexArray(Vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, Vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(float), Vertices, BufferUsageHint.StaticDraw);

            var vertexLocation = 0;
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            var UVLocation = 1;
            GL.EnableVertexAttribArray(UVLocation);
            GL.VertexAttribPointer(UVLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 2 * sizeof(float));
            var ColorLocation = 2;
            GL.EnableVertexAttribArray(ColorLocation);
            GL.VertexAttribPointer(ColorLocation, 4, VertexAttribPointerType.Float, false, 8 * sizeof(float), 4 * sizeof(float));
            Base2D.Use();
            Base2DTex.Use(TextureUnit.Texture0);
            GL.Disable(EnableCap.DepthTest);
            GL.DrawArrays(PrimitiveType.TriangleFan, 0, 4);
            GL.Enable(EnableCap.DepthTest);

        }
        public  void RenderImage(string image, int x, int y, int w, int h, Vector4 col)
        {
            float[] Vertices = new float[]
            {
                x,y,0,0,col.X,col.Y,col.Z,col.W,
                x+w,y,1,0,col.X,col.Y,col.Z,col.W,
                x+w,y+h,1,1,col.X,col.Y, col.Z,col.W,
                x,y+h,0,1,col.X,col.Y, col.Z,col.W
            };
            int Vao = GL.GenVertexArray();
            int Vbo = GL.GenBuffer();
            GL.BindVertexArray(Vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, Vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(float), Vertices, BufferUsageHint.StaticDraw);

            var vertexLocation = 0;
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            var UVLocation = 1;
            GL.EnableVertexAttribArray(UVLocation);
            GL.VertexAttribPointer(UVLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 2 * sizeof(float));
            var ColorLocation = 2;
            GL.EnableVertexAttribArray(ColorLocation);
            GL.VertexAttribPointer(ColorLocation, 4, VertexAttribPointerType.Float, false, 8 * sizeof(float), 4 * sizeof(float));
            Base2D.Use();
            Base2DTex.Use(TextureUnit.Texture0);
            GL.Disable(EnableCap.DepthTest);
            GL.DrawArrays(PrimitiveType.TriangleFan, 0, 4);
            GL.Enable(EnableCap.DepthTest);

        }
        public  void RenderImage(string image, float ix, float iy, float iw, float ih, int x, int y, int w, int h)
        {

            Vector4 col = new Vector4(1);

            float[] Vertices = new float[]
            {
                x,y,ix,iy,col.X,col.Y,col.Z,col.W,
                x+w,y,iw,iy,col.X,col.Y,col.Z,col.W,
                x+w,y+h,iw,iw,col.X,col.Y, col.Z,col.W,
                x,y+h,ix,iw,col.X,col.Y, col.Z,col.W
            };
            int Vao = GL.GenVertexArray();
            int Vbo = GL.GenBuffer();
            GL.BindVertexArray(Vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, Vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(float), Vertices, BufferUsageHint.StaticDraw);

            var vertexLocation = 0;
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            var UVLocation = 1;
            GL.EnableVertexAttribArray(UVLocation);
            GL.VertexAttribPointer(UVLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 2 * sizeof(float));
            var ColorLocation = 2;
            GL.EnableVertexAttribArray(ColorLocation);
            GL.VertexAttribPointer(ColorLocation, 4, VertexAttribPointerType.Float, false, 8 * sizeof(float), 4 * sizeof(float));
            Base2D.Use();
            Base2DTex.Use(TextureUnit.Texture0);
            GL.Disable(EnableCap.DepthTest);
            GL.DrawArrays(PrimitiveType.TriangleFan, 0, 4);
            GL.Enable(EnableCap.DepthTest);

        }
        public  void RenderImage(string image, float ix, float iy, float iw, float ih, int x, int y, int w, int h, Vector4 col)
        {
            if (col == null)
            {
                col = new Vector4(1);
            }
            float[] Vertices = new float[]
            {
                x,y,ix,iy,col.X,col.Y,col.Z,col.W,
                x+w,y,iw,iy,col.X,col.Y,col.Z,col.W,
                x+w,y+h,iw,iw,col.X,col.Y, col.Z,col.W,
                x,y+h,ix,iw,col.X,col.Y, col.Z,col.W
            };
            int Vao = GL.GenVertexArray();
            int Vbo = GL.GenBuffer();
            GL.BindVertexArray(Vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, Vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(float), Vertices, BufferUsageHint.StaticDraw);

            var vertexLocation = 0;
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            var UVLocation = 1;
            GL.EnableVertexAttribArray(UVLocation);
            GL.VertexAttribPointer(UVLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 2 * sizeof(float));
            var ColorLocation = 2;
            GL.EnableVertexAttribArray(ColorLocation);
            GL.VertexAttribPointer(ColorLocation, 4, VertexAttribPointerType.Float, false, 8 * sizeof(float), 4 * sizeof(float));
            Base2D.Use();
            Base2DTex.Use(TextureUnit.Texture0);
            GL.Disable(EnableCap.DepthTest);
            GL.DrawArrays(PrimitiveType.TriangleFan, 0, 4);
            GL.Enable(EnableCap.DepthTest);

        }
        public void RenderText(string text, FreeTypeFont font, int x, int y, int size, Vector4 color)
        {
            font.RenderText(text,this, color.Xyz, x - 1, y + size - 13, (float)size / 48, new Vector2(1, 0));
        }
        public PositronModel GeneratePositronModel(Shader shader, float[] vertices, Texture[] textures)
        {
            PositronModel model = new PositronModel();
            model.EBO = false;
            model.shader = shader;
            model.textures = textures;
            model.VertCount = vertices.Count();
            model.VAO = GL.GenVertexArray();
            int VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            int Total = 0;
            for (int i = 0; i < model.shader.GetAttribs(); i++)
            {
                GL.GetActiveAttrib(model.shader.Handle, i, out int size, out var type);
                if (type == ActiveAttribType.FloatVec2)
                {
                    Total += 2;
                }
                else if (type == ActiveAttribType.FloatVec3)
                {
                    Total += 3;
                }
                else if (type == ActiveAttribType.FloatVec4)
                {
                    Total += 4;
                }
                else if (type == ActiveAttribType.Float)
                {
                    Total += 1;
                }
                else
                {
                    throw new Exception("Current attribute is not supported by the graphic engine ,please contact Bambou72 to add this fonctionnality if is needed.");
                }
            }

            int lasttype = 0;

            for(int i = 0;i < model.shader.GetAttribs(); i++)
            {
                GL.GetActiveAttrib(model.shader.Handle, i, out int size, out var type);
                GL.EnableVertexAttribArray(i);
                int typen;
                if (type== ActiveAttribType.FloatVec2)
                {
                    typen = 2;
                }else if (type== ActiveAttribType.FloatVec3)
                {
                    typen = 3;
                }else if (type== ActiveAttribType.FloatVec4)
                {
                    typen = 4;
                }else if(type == ActiveAttribType.Float)
                {
                    typen = 1;
                }else
                {
                    throw new Exception("Current attribute is not supported by the graphic engine ,please contact Bambou72 to add this fonctionnality if is needed.");
                }

                GL.VertexAttribPointer(i, typen, VertexAttribPointerType.Float, false, Total * sizeof(float), lasttype* sizeof(float));
                lasttype += typen;
            }
            GL.BindVertexArray(0);
            return model;
        }
        public PositronModel GeneratePositronModelEBO(Shader shader, float[] vertices, Texture[] textures, uint[] indices)
        {
            PositronModel model = new PositronModel();
            model.EBO = true;
            model.shader = shader;
            model.textures = textures;
            model.VertCount = vertices.Count();
            model.EBoCount = indices.Count();
            model.VAO = GL.GenVertexArray();
            int VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            int Total = 0;
            for (int i = 0; i < model.shader.GetAttribs(); i++)
            {
                GL.GetActiveAttrib(model.shader.Handle, i, out int size, out var type);
                if (type == ActiveAttribType.FloatVec2)
                {
                    Total += 2;
                }
                else if (type == ActiveAttribType.FloatVec3)
                {
                    Total += 3;
                }
                else if (type == ActiveAttribType.FloatVec4)
                {
                    Total += 4;
                }
                else if (type == ActiveAttribType.Float)
                {
                    Total += 1;
                }
                else
                {
                    throw new Exception("Current attribute is not supported by the graphic engine ,please contact Bambou72 to add this fonctionnality if is needed.");
                }
            }

            int lasttype = 0;

            for (int i = 0; i < model.shader.GetAttribs(); i++)
            {
                GL.GetActiveAttrib(model.shader.Handle, i, out int size, out var type);
                GL.EnableVertexAttribArray(i);
                int typen;
                if (type == ActiveAttribType.FloatVec2)
                {
                    typen = 2;
                }
                else if (type == ActiveAttribType.FloatVec3)
                {
                    typen = 3;
                }
                else if (type == ActiveAttribType.FloatVec4)
                {
                    typen = 4;
                }
                else if (type == ActiveAttribType.Float)
                {
                    typen = 1;
                }
                else
                {
                    throw new Exception("Current attribute is not supported by the graphic engine ,please contact Bambou72 to add this fonctionnality if is needed.");
                }

                GL.VertexAttribPointer(i, typen, VertexAttribPointerType.Float, false, Total * sizeof(float), lasttype * sizeof(float));
                lasttype += typen;
            }
            int EBO = GL.GenBuffer();
            //EBO
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Count() * sizeof(uint), indices, BufferUsageHint.StaticDraw);
            GL.BindVertexArray(0);
            return model;
        }
        public void UpdateMatrices(Matrix4 pro ,Matrix4 vie)
        {
            Pro = pro;
            View = vie;
        }
        public void Render3DModel(PositronModel model,Vector3 Position ,Vector3 rotation,Vector3 scale)
        {
            model.shader.Use();
            model.shader.SetMatrix4("projection", Pro);
            model.shader.SetMatrix4("view", View);
            Matrix4 Translation = Matrix4.CreateTranslation(Position);
            Matrix4 Rotation = Matrix4.CreateRotationX(rotation.X) * Matrix4.CreateRotationY(rotation.Y) * Matrix4.CreateRotationZ(rotation.Z);
            Matrix4 Scale = Matrix4.CreateScale(scale);
            Matrix4 Model = Translation * Rotation * Scale;
            model.shader.SetMatrix4("model", Model);
            GL.BindVertexArray(model.VAO);
            if (model.EBO == true)
            {
                GL.DrawElements(BeginMode.Triangles,model.EBoCount,DrawElementsType.UnsignedInt,0);
            }else
            {
                GL.DrawArrays(PrimitiveType.Triangles,0,model.VertCount);
            }
            GL.BindVertexArray(0);
        }
    }
}