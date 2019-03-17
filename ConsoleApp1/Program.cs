using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace ConsoleApp1
{
    static class Program
    {
        static void Main()
        {
            var a = new MainWindow();
            a.Run(60);
        }
    }

    public sealed class MainWindow : GameWindow
    {
        public MainWindow()
            : base(1280, // initial width
                720, // initial height
                GraphicsMode.Default,
                "dreamstatecoding", // initial title
                GameWindowFlags.Default,
                DisplayDevice.Default,
                4, // OpenGL major version
                0, // OpenGL minor version
                GraphicsContextFlags.ForwardCompatible)
        {
            Title += ": OpenGL Version: " + GL.GetString(StringName.Version);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.ClearColor(Color.Gold);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            float[] verticies =
            {
                -0.5f, -0.5f, 0.0f,
                0.5f, -0.5f, 0.0f,
                0.0f, 0.5f, 0.0f
            };
            int vbo;
            GL.GenBuffers(1, out vbo);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer,verticies.Length*sizeof(float),verticies,BufferUsageHint.StaticDraw);
            
            string vertexShaderCode ="#version 330 core\nlayout (location = 0) in vec3 position;\nvoid main()\n{\n\tgl_Position = vec4(position.x, position.y, position.z, 1.0);\n}";
            int vertexShader;
            vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader,vertexShaderCode);
            GL.CompileShader(vertexShader);
            Console.WriteLine(GL.GetShaderInfoLog(vertexShader));
            
            string fragmentShaderCode ="#version 330 core\nout vec4 color;\nvoid main()\n{\n\tcolor = vec4(1.0f, 0.5f, 0.2f, 1.0f);\n}";
            int fragmentShader;
            fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader,fragmentShaderCode);
            GL.CompileShader(fragmentShader);
            Console.WriteLine(GL.GetShaderInfoLog(fragmentShader));

            int shaderProgram;
            shaderProgram = GL.CreateProgram();
            GL.AttachShader(shaderProgram,vertexShader);
            GL.AttachShader(shaderProgram,fragmentShader);
            GL.LinkProgram(shaderProgram);
            Console.WriteLine(GL.GetProgramInfoLog(shaderProgram));
            
            int vao;
            GL.GenVertexArrays(1, out vao);
            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer,vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, verticies.Length * sizeof(float), verticies, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0,3,VertexAttribPointerType.Float,false,3*sizeof(float),0);
            GL.EnableVertexAttribArray(0);
            GL.BindVertexArray(0);
            GL.UseProgram(shaderProgram);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
            GL.BindVertexArray(vao);
            GL.DrawArrays(PrimitiveType.Triangles,0,3);
            GL.BindVertexArray(0);
            SwapBuffers();
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            if(e.Key == Key.Escape)
                Exit();
        }
    }
}