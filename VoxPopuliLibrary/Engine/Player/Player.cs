using LiteNetLib;
using LiteNetLib.Utils;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using VoxPopuliLibrary.Engine.API;
using VoxPopuliLibrary.Engine.GraphicEngine;
using VoxPopuliLibrary.Engine.Network;
using VoxPopuliLibrary.Engine.World;

namespace VoxPopuliLibrary.Engine.Player
{
    internal class Player : Entity
    {
        internal float sensitivity = 0.2f;
        internal float NormalSpeed = 5f;
        internal float SprintSpeed = 8f;
        internal bool _firstMove = true;
        internal Vector2 _lastPos;
        internal Camera _Camera;
        internal bool Local = false;
        internal ushort ClientID;
        internal Model _Model;
        internal bool forward, backward, left, right, space, shift, control = false;
        internal Vector3 Front;
        internal Vector3 Right;
        internal float Elevation;
        internal int SelectedBlock = 3;
        internal int Reach = 3;
        internal Vector3i BlockSePos;
        internal bool BlockIsSec;

        public Player(Vector3d _Position, ushort _ClientID, bool Local, bool Client)
        {
            Position = _Position;
            if (Client)
            {
                if (Local)
                {
                    _Camera = new Camera((Vector3)_Position, 16 / 9);
                }
                _Model = RessourceManager.RessourceManager.GetModel("Player");
            }
            ClientID = _ClientID;
        }
        //Client
        internal void UpdateClient(float DT, KeyboardState Keyboard, MouseState Mouse, bool Grabed)
        {
            base.UpdateClient(DT);
            float Speed = NormalSpeed;
            if (Keyboard.IsKeyDown(Keys.LeftControl))
            {
                Speed = SprintSpeed;
                control = true;
            }
            else
            {
                control = false;
            }
            if (Keyboard.IsKeyDown(Keys.W))
            {
                Acceleration += Front * Speed * Elevation;
                forward = true;
            }
            else
            {
                forward = false;
            }

            if (Keyboard.IsKeyDown(Keys.S))
            {
                Acceleration -= Front * Speed * Elevation;
                backward = true;
            }
            else
            {
                backward = false;
            }
            if (Keyboard.IsKeyDown(Keys.A))
            {

                Acceleration -= Right * Speed * Elevation;
                left = true;
            }
            else
            {
                left = false;
            }
            if (Keyboard.IsKeyDown(Keys.D))
            {
                Acceleration += Right * Speed * Elevation;
                right = true;
            }
            else
            {
                right = false;
            }
            if (Keyboard.IsKeyDown(Keys.Space))
            {
                if (!Fly)
                {
                    Jump();
                }
                else
                {
                    Acceleration.Y = Speed; // Up
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
                    Acceleration.Y = -Speed; // Down
                }
                shift = true;
            }
            else
            {
                shift = false;
            }

            if (Grabed)
            {
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
            }
            Elevation = (float)Math.Abs(Math.Acos(_Camera.Front.Y));
            Front = new Vector3(_Camera.Front.X, 0, _Camera.Front.Z).Normalized();
            Right = new Vector3(_Camera.Right.X, 0, _Camera.Right.Z);
            SendControl();
            CollisionTerrain(DT);
            _Camera.Position = new Vector3((float)Position.X, (float)Position.Y + EntityEYEHeight, (float)Position.Z);
        }
        internal void SendControl()
        {
            PlayerControl message = new PlayerControl
            {
                Forward = forward,
                Backward = backward,
                Right = right,
                Left = left,
                Space = space,
                Shift = shift,
                Control = control,
                Rotation = Rotation,
                Front = Front,
                CRight = _Camera.Right,
                Elevation = Elevation,
                Fly = Fly
            }; 
            ClientNetwork.SendPacket(message, DeliveryMethod.ReliableUnordered);
            
        }
        internal void SendPos()
        {
            PlayerPositionTP packet = new PlayerPositionTP {Position =Position };
            ClientNetwork.SendPacket(packet,DeliveryMethod.ReliableOrdered);
        }
        internal void Render()
        {
            GL.BindVertexArray(_Model.Vao);
            RessourceManager.RessourceManager.GetTexture("Player").Use(TextureUnit.Texture0);
            RessourceManager.RessourceManager.GetShader("Entity").Use();
            var model = Matrix4.Identity * Matrix4.CreateTranslation(new Vector3((float)Position.X, (float)(Position.Y + EntityEYEHeight), (float)Position.Z)) /** Matrix4.CreateRotationY(Rotation.Y)*/;
            RessourceManager.RessourceManager.GetShader("Entity").SetMatrix4("model", model);
            RessourceManager.RessourceManager.GetShader("Entity").SetMatrix4("view", ClientWorldManager.world.LocalPlayerCamera().GetViewMatrix());
            RessourceManager.RessourceManager.GetShader("Entity").SetMatrix4("projection", ClientWorldManager.world.LocalPlayerCamera().GetProjectionMatrix());
            GL.DrawArrays(PrimitiveType.Triangles, 0, _Model._Vertices.Count());
            GL.BindVertexArray(0);
        }
        //Server
        internal override void UpdateServer(float DT)
        {
            base.UpdateServer(DT);
            float Speed = NormalSpeed;
            if (control)
            {
                Speed = SprintSpeed;
            }
            if (forward)
            {
                Acceleration += Front * Speed;
                forward = false;
            }

            if (backward)
            {
                Acceleration -= Front * Speed;
                backward = false;
            }
            if (left)
            {
                Acceleration -= Right * Speed;
                left = false;
            }
            if (right)
            {
                Acceleration += Right * Speed;
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
                    Acceleration.Y = Speed; // Up
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
                    Acceleration.Y = -Speed; // Down
                }
                shift = false;
            }
            CollisionTerrainServer(DT);
        }
        internal void SendData()
        {
            PlayerData packet = new PlayerData { ClientID = ClientID,Position = Position,Rotation = Rotation};
            ServerNetwork.SendPacketToAll(packet,DeliveryMethod.Unreliable);
        }
    }
}
