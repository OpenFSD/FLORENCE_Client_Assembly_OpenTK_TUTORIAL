﻿using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using FLORENCE_Client.FrameworkSpace.ClientSpace.DataSpace.OutputSpace.GraphicsSpace;

namespace FLORENCE_Client
{
    namespace FrameworkSpace
    {
        namespace ClientSpace
        {
            namespace DataSpace
            {
                namespace OutputSpace
                {
                    public class Graphics : GameWindow
                    {
                        private int ElementBufferObject;
                        private int VertexArrayObject;
                        private int VertexBufferObject;

                        private FLORENCE_Client.FrameworkSpace.ClientSpace.DataSpace.OutputSpace.GraphicsSpace.Shader shader;
                        
                        private FLORENCE_Client.FrameworkSpace.ClientSpace.DataSpace.OutputSpace.GraphicsSpace.Texture texture0;
                        private FLORENCE_Client.FrameworkSpace.ClientSpace.DataSpace.OutputSpace.GraphicsSpace.Texture texture1;

                        private FLORENCE_Client.FrameworkSpace.ClientSpace.DataSpace.OutputSpace.GraphicsSpace.Camera camera;

                        private bool _firstMove = true;

                        private Vector2 _lastPos;

                        private Matrix4 view;
                        private Matrix4 projection;

                        private static int nrAttributes;
                        private static double time;

                        public Graphics(OpenTK.Windowing.Desktop.GameWindowSettings gws, OpenTK.Windowing.Desktop.NativeWindowSettings nws) : base(
                           gws,
                           nws
                       )
                        {
                            System.Console.WriteLine("FLORENCE: Graphics & GameWindow");
                        }

                        ~Graphics()
                        {

                        }
                        protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
                        {
                            base.OnFramebufferResize(e);
                            GL.Viewport(0, 0, e.Width, e.Height);
                        }

                        protected override void OnLoad()
                        {
                            base.OnLoad();

                            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

                            VertexArrayObject = GL.GenVertexArray();
                            GL.BindVertexArray(VertexArrayObject);

                            VertexBufferObject = GL.GenBuffer();
                            GL.BindBuffer(
                                BufferTarget.ArrayBuffer, 
                                VertexBufferObject
                            );
                            GL.BufferData(
                                BufferTarget.ArrayBuffer,
                                FLORENCE_Client.Program.Get_Framework().Get_Client().Get_Data().Get_Output().Get_Vertices().Length * sizeof(float),
                                FLORENCE_Client.Program.Get_Framework().Get_Client().Get_Data().Get_Output().Get_Vertices(), 
                                BufferUsageHint.StaticDraw
                            );

                            // draw square \/ \/ \/
                            ElementBufferObject = GL.GenBuffer();
                            GL.BindBuffer(
                                BufferTarget.ElementArrayBuffer,
                                ElementBufferObject
                            );
                            GL.BufferData(
                                BufferTarget.ElementArrayBuffer,
                                FLORENCE_Client.Program.Get_Framework().Get_Client().Get_Data().Get_Output().Get_Indices().Length * sizeof(uint),
                                FLORENCE_Client.Program.Get_Framework().Get_Client().Get_Data().Get_Output().Get_Indices(),
                                BufferUsageHint.StaticDraw
                            );
                            // draw square /\ /\ /\

                            shader = new FLORENCE_Client.FrameworkSpace.ClientSpace.DataSpace.OutputSpace.GraphicsSpace.Shader(
                                "..\\..\\..\\shader_vert.glsl",
                                "..\\..\\..\\shader_frag.glsl"
                            );
                            shader.Use();

                            GL.VertexAttribPointer(
                                0, 
                                3, 
                                VertexAttribPointerType.Float, 
                                false, 
                                5 * sizeof(float), 
                                0
                            );
                            GL.EnableVertexAttribArray(0);

                            GL.VertexAttribPointer(
                                1, 
                                3, 
                                VertexAttribPointerType.Float, 
                                false, 
                                5 * sizeof(float), 
                                3 * sizeof(float)
                            );
                            GL.EnableVertexAttribArray(1);

                            texture0 = FLORENCE_Client.FrameworkSpace.ClientSpace.DataSpace.OutputSpace.GraphicsSpace.Texture.LoadFromFile("..\\..\\..\\Textures\\container.jpg");
                            texture0.Use(TextureUnit.Texture0);

                            texture1 = FLORENCE_Client.FrameworkSpace.ClientSpace.DataSpace.OutputSpace.GraphicsSpace.Texture.LoadFromFile("..\\..\\..\\Textures\\awesomeface.png");
                            texture1.Use(TextureUnit.Texture1);

                            nrAttributes = 0;
                            GL.GetInteger(GetPName.MaxVertexAttribs, out nrAttributes);
                            Console.WriteLine("Maximum number of vertex attributes supported: " + nrAttributes);

                            shader.SetInt("texture0", 0);
                            shader.SetInt("texture1", 1);

                            camera = new Camera(Vector3.UnitZ * 3, Size.X / (float)Size.Y);
                            //CursorState = CursorState.Grabbed;
                            /*
                            view = Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);
                            projection = Matrix4.CreatePerspectiveFieldOfView(
                                MathHelper.DegreesToRadians(45f), 
                                Size.X / (float)Size.Y, 
                                0.1f, 
                                100.0f
                            );
                            */
                            GL.Enable(EnableCap.DepthTest);
                        }

                        protected override void OnRenderFrame(FrameEventArgs e)
                        {
                            base.OnRenderFrame(e);

                            time += 4.0 * e.Time;

                            GL.Clear(
                                ClearBufferMask.ColorBufferBit | 
                                ClearBufferMask.DepthBufferBit
                            );
                            GL.BindVertexArray(VertexArrayObject);

                            texture0.Use(TextureUnit.Texture0);
                            texture1.Use(TextureUnit.Texture1);
                            shader.Use();

                            var model = Matrix4.Identity * Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(time));
                            shader.SetMatrix4("model", model);
                            shader.SetMatrix4("view", camera.GetViewMatrix());
                            shader.SetMatrix4("projection", camera.GetProjectionMatrix());
                            
/*
// change colour with time \/ \/ \/
                            float greenValue = Get_New_greenValue();
                            int vertexColorLocation = GL.GetUniformLocation(shader.Get_Handle(), "ourColor");
                            GL.Uniform4(vertexColorLocation, 0.0f, greenValue, 0.0f, 1.0f);
// change colour with time /\ /\ /\
*/
                            FLORENCE_Client.Program.Get_Framework().Get_Client().Get_Data().Get_Map_Default().Draw_Triangle();
                            //FLORENCE_Client.Program.Get_Framework().Get_Client().Get_Data().Get_Map_Default().Draw_Square(FLORENCE_Client.Program.Get_Framework().Get_Client().Get_Data());
                            
                            SwapBuffers();
                        }

                        protected override void OnUnload()
                        {
                            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
                            GL.BindVertexArray(0);
                            GL.UseProgram(0);

                            // Delete all the resources.
                            GL.DeleteBuffer(VertexBufferObject);
                            GL.DeleteVertexArray(VertexArrayObject);

                            GL.DeleteProgram(shader.Get_Handle());

                            shader.Dispose();

                            base.OnUnload();
                        }

                        protected override void OnUpdateFrame(FrameEventArgs e)
                        {
                            base.OnUpdateFrame(e);

                            if (!IsFocused) // Check to see if the window is focused
                            {
                                return;
                            }

                            if (KeyboardState.IsKeyDown(Keys.Escape))
                            {
                                this.Close();
                            }

                            const float cameraSpeed = 1.5f;
                            const float sensitivity = 0.2f;

                            if (KeyboardState.IsKeyDown(Keys.W))
                            {
                                camera.Position += camera.Front * cameraSpeed * (float)e.Time; // Forward
                            }

                            if (KeyboardState.IsKeyDown(Keys.S))
                            {
                                camera.Position -= camera.Front * cameraSpeed * (float)e.Time; // Backwards
                            }
                            if (KeyboardState.IsKeyDown(Keys.A))
                            {
                                camera.Position -= camera.Right * cameraSpeed * (float)e.Time; // Left
                            }
                            if (KeyboardState.IsKeyDown(Keys.D))
                            {
                                camera.Position += camera.Right * cameraSpeed * (float)e.Time; // Right
                            }
                            if (KeyboardState.IsKeyDown(Keys.Space))
                            {
                                camera.Position += camera.Up * cameraSpeed * (float)e.Time; // Up
                            }
                            if (KeyboardState.IsKeyDown(Keys.LeftShift))
                            {
                                camera.Position -= camera.Up * cameraSpeed * (float)e.Time; // Down
                            }

                            // Get the mouse state
                            var mouse = MouseState;

                            if (_firstMove) // This bool variable is initially set to true.
                            {
                                _lastPos = new Vector2(mouse.X, mouse.Y);
                                _firstMove = false;
                            }
                            else
                            {
                                // Calculate the offset of the mouse position
                                var deltaX = mouse.X - _lastPos.X;
                                var deltaY = mouse.Y - _lastPos.Y;
                                _lastPos = new Vector2(mouse.X, mouse.Y);

                                // Apply the camera pitch and yaw (we clamp the pitch in the camera class)
                                camera.Yaw += deltaX * sensitivity;
                                camera.Pitch -= deltaY * sensitivity; // Reversed since y-coordinates range from bottom to top
                            }
                        }
                        protected override void OnMouseWheel(MouseWheelEventArgs e)
                        {
                            base.OnMouseWheel(e);

                            camera.Fov -= e.OffsetY;
                        }
                        /*
public static float Get_New_greenValue()
{
    periodOfRefresh += 0.0166666666666667;//period per frame - settings gws.UpdateFrequency = 60
    if (periodOfRefresh == 2000) periodOfRefresh = 0;
    return (float)Math.Sin(periodOfRefresh) / (2.0f + 0.5f);
}
*/

                    }
                }
            }
        }
    }
}
