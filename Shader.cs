﻿using OpenTK.Graphics.OpenGL4;

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
                    namespace GraphicsSpace
                    {
                        public class Shader
                        {
                            int Handle, VertexShader, FragmentShader;
                            private bool disposedValue = false;

                            public Shader(string vertexPath, string fragmentPath)
                            {
                                string VertexShaderSource = File.ReadAllText(vertexPath);

                                string FragmentShaderSource = File.ReadAllText(fragmentPath);

                                VertexShader = GL.CreateShader(ShaderType.VertexShader);
                                GL.ShaderSource(VertexShader, VertexShaderSource);

                                FragmentShader = GL.CreateShader(ShaderType.FragmentShader);
                                GL.ShaderSource(FragmentShader, FragmentShaderSource);

                                GL.CompileShader(VertexShader);

                                GL.GetShader(VertexShader, ShaderParameter.CompileStatus, out int success_a);
                                if (success_a == 0)
                                {
                                    string infoLog = GL.GetShaderInfoLog(VertexShader);
                                    Console.WriteLine(infoLog);
                                }

                                GL.CompileShader(FragmentShader);

                                GL.GetShader(FragmentShader, ShaderParameter.CompileStatus, out int success_b);
                                if (success_b == 0)
                                {
                                    string infoLog = GL.GetShaderInfoLog(FragmentShader);
                                    Console.WriteLine(infoLog);
                                }

                                Handle = GL.CreateProgram();

                                GL.AttachShader(Handle, VertexShader);
                                GL.AttachShader(Handle, FragmentShader);

                                GL.LinkProgram(Handle);

                                GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out int success);
                                if (success == 0)
                                {
                                    string infoLog = GL.GetProgramInfoLog(Handle);
                                    Console.WriteLine(infoLog);
                                }

                                GL.DetachShader(Handle, VertexShader);
                                GL.DetachShader(Handle, FragmentShader);
                                GL.DeleteShader(FragmentShader);
                                GL.DeleteShader(VertexShader);
                            }

                            ~Shader()
                            {
                                if (disposedValue == false)
                                {
                                    Console.WriteLine("GPU Resource leak! Did you forget to call Dispose()?");
                                }
                            }

                            public void Dispose()
                            {
                                Dispose(true);
                                GC.SuppressFinalize(this);
                            }

                            protected virtual void Dispose(bool disposing)
                            {
                                if (!disposedValue)
                                {
                                    GL.DeleteProgram(Handle);

                                    disposedValue = true;
                                }
                            }

                            public void SetInt(string name, int value)
                            {
                                int location = GL.GetUniformLocation(Handle, name);

                                GL.Uniform1(location, value);
                            }

                            public void Use()
                            {
                                GL.UseProgram(Handle);
                            }

                            public int Get_Handle()
                            {
                                return Handle;
                            }
                        }
                    }
                }
            }
        }
    }
}
