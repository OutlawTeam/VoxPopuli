using LiteNetLib;
using LiteNetLib.Utils;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;
using VoxPopuliLibrary.Engine.API;
using VoxPopuliLibrary.Engine.Debug;
using VoxPopuliLibrary.Engine.GraphicEngine;
using VoxPopuliLibrary.Engine.Network;
using VoxPopuliLibrary.Engine.Physics;
using VoxPopuliLibrary.Engine.World;

namespace VoxPopuliLibrary.Engine.Player
{
    internal class Player : PhysicEntity
    {
        internal float sensitivity = 0.2f;
        internal float NormalSpeed = 5f;
        internal float SprintSpeed = 8f;
        internal float EntityHeight = 1.80f;
        internal float EntityWidth = 0.60f;
        internal float EntityDepth = 0.60f;
        internal float EntityEYEHeight = 1.70f;
        internal bool _firstMove = true;
        internal string Name = "Test";
        internal Vector2 _lastPos;
        internal Camera _Camera;
        internal bool Local = false;
        internal ushort ClientID;
        internal Model _Model;
        internal bool forward, backward, left, right, space, shift, control = false;
        internal Vector3 Front;
        internal Vector3 Right;
        internal float Elevation;
        internal string SelectedBlock = "VoxPopuli:dirt";
        internal int Reach = 3;
        internal static DebugBox BlockBox = new DebugBox(new Vector3d(1.03125, 1.03125, 1.03125),new Vector4(0.33f,0.33f,0.33f,1));
        internal static Vector3i ViewedBlockPos;
        internal Maths.Ray ViewRay;
        internal static bool ViewBlock = false;
        const float BOUND = 89.0f;
        internal MouseState m;
        public Player(Vector3d _Position, ushort _ClientID, bool Local, bool Client)
        {
            Position = _Position;
            if (Client)
            {
                if (Local)
                {
                    _Camera = new Camera((Vector3)_Position, 16 / 9);
                    ViewRay = new Maths.Ray(Vector3d.Zero, Vector3.Zero, Reach);
                }

                //_Model = RessourceManager.RessourceManager.GetModel("Player");
                _Model = RessourceManager.RessourceManager.GetModel("Player");
            }
            ClientID = _ClientID;
        }
        internal void HitCallBack(Vector3i CurrentBlock ,Vector3i NextBlock)
        {
            ViewBlock = true;
            ViewedBlockPos = NextBlock;
            if(m.IsButtonPressed(MouseButton.Left))
            {
                ClientWorldManager.world.GetChunkManagerClient().ChangeChunk(NextBlock,"air");
            }else if(m.IsButtonPressed(MouseButton.Right))
            {
                ClientWorldManager.world.GetChunkManagerClient().ChangeChunk(CurrentBlock, SelectedBlock);
            }
            else if (m.IsButtonPressed(MouseButton.Middle))
            {
                if(ClientWorldManager.world.GetBlock(NextBlock.X,NextBlock.Y,NextBlock.Z,out string id))
                {
                    SelectedBlock = id;
                }
            }
        }
        internal void HitCallBackTest(Vector3i CurrentBlock, Vector3i Normal,string BlockID)
        {
            ViewBlock = true;
            ViewedBlockPos = CurrentBlock;
            Vector3i BlockBefore = new Vector3i(CurrentBlock.X+Normal.X,CurrentBlock.Y+Normal.Y,CurrentBlock.Z+Normal.Z);
            if (m.IsButtonPressed(MouseButton.Left))
            {
                ClientWorldManager.world.GetChunkManagerClient().ChangeChunk(CurrentBlock, "air");
            }
            else if (m.IsButtonPressed(MouseButton.Right))
            {
                ClientWorldManager.world.GetChunkManagerClient().ChangeChunk(BlockBefore, SelectedBlock);
            }
            else if (m.IsButtonPressed(MouseButton.Middle))
            {
                if (ClientWorldManager.world.GetBlock(CurrentBlock.X, CurrentBlock.Y, CurrentBlock.Z, out string id))
                {
                    SelectedBlock = id;
                }
            }
        }
        //Client
        internal void UpdateClient(float DT, KeyboardState Keyboard, MouseState Mouse, bool Grabed)
        {
            ViewBlock = false;
            m = Mouse;
            base.UpdateClient(DT);
            ViewRay.Update(new Vector3d(Position.X, Position.Y + EntityEYEHeight, Position.Z),_Camera.Front,Reach);
            ViewRay.TestWithTerrain(HitCallBackTest);
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
                    var change = Mouse.Position - _lastPos;
                    Rotation.X -= change.Y * sensitivity;
                    Rotation.Y += change.X * sensitivity;

                    if (Rotation.X > BOUND)
                        Rotation.X = BOUND;
                    else if (Rotation.X < -BOUND)
                        Rotation.X = -BOUND;
                    if (Rotation.Y > 360)
                        Rotation.Y = 0;
                    else if (Rotation.Y < 0)
                        Rotation.Y = 360;
                    _Camera.Yaw = Rotation.Y;
                    _Camera.Pitch = Rotation.X;
                    _lastPos = new Vector2(Mouse.X, Mouse.Y);
                }
            }
            Elevation = (float)Math.Abs(Math.Acos(_Camera.Front.Y));
            Front = new Vector3(_Camera.Front.X, 0, _Camera.Front.Z).Normalized();
            Right = new Vector3(_Camera.Right.X, 0, _Camera.Right.Z);
            SendControl(DT);
            CollisionTerrain(DT);
            _Camera.Position = new Vector3((float)Position.X, (float)Position.Y + EntityEYEHeight, (float)Position.Z);

        }
        internal void SendControl(float dt)
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
                Fly = Fly,
                Dt = dt
            }; 
            ClientNetwork.SendPacket(message, DeliveryMethod.ReliableUnordered);
            
        }
        internal void SendPos()
        {
            PlayerPositionTP packet = new PlayerPositionTP {Position =Position };
            ClientNetwork.SendPacket(packet,DeliveryMethod.ReliableOrdered);
        }
        internal void RenderSelectedBlock()
        {
            if (ViewBlock)
            {
                RenderSystem.RenderDebugBox(BlockBox, ViewedBlockPos-new Vector3(0.015625f));
            }
            Renderer.RenderImage("cross",API.API.WindowWidth()/2-16,API.API.WindowHeight()/2-16,32,32);
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
            RessourceManager.RessourceManager.GetFont("FreeSans").Render3DText(Name, new Vector3(0.85f, 0.78f, 0.09f),
                new Vector3((float)Position.X, (float)(Position.Y + EntityEYEHeight + 0.40), (float)Position.Z),0.0075f,new Vector3(180, Rotation.Y-90, 0));
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
            PlayerData packet = new PlayerData { ClientID = ClientID,Position = Position,Rotation = Rotation,Name = Name};
            ServerNetwork.SendPacketToAll(packet,DeliveryMethod.Unreliable);
        }
    }
}
