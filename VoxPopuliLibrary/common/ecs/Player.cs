using LiteNetLib.Utils;
using LiteNetLib;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using VoxPopuliLibrary.client;
using VoxPopuliLibrary.client.graphic;
using OpenTK.Graphics.OpenGL4;
using VoxPopuliLibrary.common.ecs.client;
using VoxPopuliLibrary.client.graphic.renderer;
namespace VoxPopuliLibrary.common.ecs
{
    internal class Player : Entity
    {
        private static float[] vertices =
        {
             -0.5f,0.5f,0f,0f,1f,
             -0.5f,-0.5f,0f,0f,0f,
             0.5f,-0.5f,0f,1f,0f,
             0.5f,-0.5f,0f,1f,0f,
             0.5f,0.5f,0f,1f,1f,
             -0.5f,0.5f,0f,0f,1f
        };
        internal float sensitivity = 0.2f;
        internal float cameraSpeed = 5f;
        internal float speed;
        internal bool _firstMove = true;
        internal Vector2 _lastPos;
        internal Camera _Camera;
        internal bool Local = false;
        internal ushort ClientID;
        internal Model _Model;
        internal bool forward, backward, left, right, space, shift = false;
        internal Vector3 Front;
        internal Vector3 Right;
        internal float Elevation;
        internal int SelectedBlock = 3;
        internal int Reach = 3;
        internal Vector3i BlockSePos;
        internal bool BlockIsSec;

        public Player(Vector3d _Position,ushort _ClientID,bool Local,bool Client)
        {
            Position = _Position;
            if (Client)
            {
                if (Local)
                {
                    _Camera = new Camera((Vector3)_Position, 16 / 9);
                }
                _Model = new Model(vertices, GlobalVariable._playertexture, GlobalVariable._shader);
            }
            ClientID = _ClientID;
        }
        //Client
        internal new void UpdateClient(float DT,KeyboardState Keyboard,MouseState Mouse)
        {
            base.UpdateClient(DT);
            
            if (Keyboard.IsKeyDown(Keys.W))
            {
                Acceleration += Front * cameraSpeed * Elevation;
                forward = true;
            }else
            {
                forward = false;
            }

            if (Keyboard.IsKeyDown(Keys.S))
            {
                Acceleration -= Front * cameraSpeed * Elevation;
                backward = true;
            }
            else
            {
                backward = false;
            }
            if (Keyboard.IsKeyDown(Keys.A))
            {

                Acceleration -= Right * cameraSpeed * Elevation;
                left = true;
            }else
            {
                left = false;
            }
            if (Keyboard.IsKeyDown(Keys.D))
            {
                Acceleration += Right * cameraSpeed * Elevation;
                right = true;
            }
            else
            {
                right = false;
            }
            if (Keyboard.IsKeyDown(Keys.Space))
            {
                if(!Fly)
                {
                    Jump();
                }else
                {
                    Acceleration.Y = cameraSpeed; // Up
                }
                space = true;
            }
            else
            {
                space = false;
            }
            if (Keyboard.IsKeyDown(Keys.LeftShift))
            {
                if (!Fly)
                {
                }
                else
                {
                    Acceleration.Y = -cameraSpeed; // Down
                }
                shift = true;
            }else
            {
                shift = false;
            }
            if (_firstMove) // This bool variable is initially set to true.
            {
                _lastPos = new Vector2(Mouse.X, Mouse.Y);
                _firstMove = false;
            }
            else
            {
                // Calculate the offset of the mouse position
                var deltaX = Mouse.X - _lastPos.X;
                var deltaY = Mouse.Y - _lastPos.Y;

                
                _lastPos = new Vector2(Mouse.X, Mouse.Y);
                // Apply the camera pitch and yaw (we clamp the pitch in the camera class)
                _Camera.Yaw += deltaX * sensitivity;
                _Camera.Pitch -= deltaY * sensitivity; // Reversed since y-coordinates range from bottom to top
                Rotation.X = _Camera.Yaw;
                Rotation.Y = _Camera.Pitch;
            }
            Elevation = (float)Math.Abs(Math.Acos(_Camera.Front.Y));
            Front = new Vector3(_Camera.Front.X, 0, _Camera.Front.Z);
            Right = new Vector3(_Camera.Right.X, 0, _Camera.Right.Z);
            SendControl();
            CollisionTerrain(DT);
            _Camera.Position = new Vector3((float)Position.X, (float)Position.Y+EntityEYEHeight, (float)Position.Z);
        }
       
        internal void SendControl()
        {
            NetDataWriter message = new NetDataWriter();
            message.Put(Convert.ToUInt16(network.NetworkProtocol.PlayerSendControl));
            message.Put(forward);
            message.Put(backward);
            message.Put(right);
            message.Put(left);
            message.Put(space);
            message.Put(shift);
            message.Put(Rotation.X);
            message.Put(Rotation.Y);
            message.Put(Rotation.Z);
            message.Put(_Camera.Front.X);
            message.Put(0f);
            message.Put(_Camera.Front.Z);
            message.Put(_Camera.Right.X);
            message.Put(0f);
            message.Put(_Camera.Right.Z);
            message.Put(Elevation);
            
            message.Put(Fly);
            if (VoxPopuliLibrary.client.network.Network.Server != null)
            {
                VoxPopuliLibrary.client.network.Network.Server.Send(message, DeliveryMethod.ReliableUnordered);
            }
        }
        internal void SendPos()
        {
            NetDataWriter message = new NetDataWriter();
            message.Put(Convert.ToUInt16(network.NetworkProtocol.PlayerClientSendPos));
            message.Put(Position.X);
            message.Put(Position.Y);
            message.Put(Position.Z);
            if (VoxPopuliLibrary.client.network.Network.Server != null)
            {
                VoxPopuliLibrary.client.network.Network.Server.Send(message, DeliveryMethod.ReliableUnordered);
            }
        }
        internal void RenderPlayerUtils()
        {
            if(BlockIsSec)
            {
                RenderSystem.RenderDebugBox(GlobalVariable.BlockSelectionBox, new Vector3(BlockSePos.X-0.125f, BlockSePos.Y - 0.125f, BlockSePos.Z -0.125f));
            }
            
        }
        internal void Render()
        {
            GL.BindVertexArray(_Model.Vao);
            _Model.Texture.Use(TextureUnit.Texture0);
            _Model._Shader.Use();
            var model = Matrix4.Identity * Matrix4.CreateTranslation((Vector3)Position) * Matrix4.CreateRotationY(Rotation.Y);
            _Model._Shader.SetMatrix4("model", model);
            _Model._Shader.SetMatrix4("view", PlayerFactory.LocalPlayer._Camera.GetViewMatrix());
            _Model._Shader.SetMatrix4("projection", PlayerFactory.LocalPlayer._Camera.GetProjectionMatrix());
            GL.DrawArrays(PrimitiveType.Triangles, 0, _Model._Vertices.Count());
            GL.BindVertexArray(0);
        }
        //Server
        internal override void UpdateServer(float DT)
        {
            base.UpdateServer(DT);
            if (forward)
            {
                Acceleration += Front * cameraSpeed;
                forward = false;
            }

            if (backward)
            {
                Acceleration -= Front * cameraSpeed;
                backward = false;
            }
            if (left)
            {
                Acceleration -= Right * cameraSpeed;
                left = false;
            }
            if (right)
            {
                Acceleration += Right * cameraSpeed;
                right = false;
            }
            if (space)
            {
                if (!Fly)
                {
                    Jump();
                }
                else
                {
                    Acceleration.Y = cameraSpeed; // Up
                }
                space = false;
            }
            if (shift)
            {
                if (!Fly)
                {
                }
                else
                {
                    Acceleration.Y = -cameraSpeed; // Down
                }
                shift = false;
            }
            CollisionTerrainServer(DT);
        }
        internal void SendData()
        {
            NetDataWriter message = new NetDataWriter();
            message.Put(Convert.ToUInt16(network.NetworkProtocol.PlayerPosition));
            message.Put(ClientID);
            message.Put(Position.X);
            message.Put(Position.Y);
            message.Put(Position.Z);
            message.Put(Rotation.X);
            message.Put(Rotation.Y);
            message.Put(Rotation.Z);
            VoxPopuliLibrary.server.network.Network.server.SendToAll(message, DeliveryMethod.ReliableUnordered);
        }
        
    }
}
