using System;
using System.Drawing;
using System.IO;
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
            Title += ": OpenGL Version: " + GL.GetString(StringName.Version) + "GLORY FOR UKRAINE! GLORY FOR HEROES!";
        }

        protected override void OnLoad(EventArgs e)
        {    
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.ClearColor(Color.Tan);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            float[] vertices = {
                // Позиции         // Цвета
                0.5f, -0.5f, 0.0f,  1.0f, 0.0f, 0.0f,   // Нижний правый угол
                -0.5f, -0.5f, 0.0f,  0.0f, 1.0f, 0.0f,   // Нижний левый угол
                0.0f,  0.5f, 0.0f,  0.0f, 0.0f, 1.0f    // Верхний угол
            };

            GlProgram program = null;

            using (var vertexShader = new Shader(ShaderType.VertexShader,Path.Combine("Shaders","VertexShader")))
            using (var fragmentShader = new Shader(ShaderType.FragmentShader,Path.Combine("Shaders","FragmentShader")))
            {
                program = new GlProgram(vertexShader,fragmentShader);
            }
            
            int vbo1;
            GL.GenBuffers(1, out vbo1);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo1);
            GL.BufferData(BufferTarget.ArrayBuffer,vertices.Length*sizeof(float),vertices,BufferUsageHint.StaticDraw);
            
            int vaoYellow;
            GL.GenVertexArrays(1, out vaoYellow);
            GL.BindVertexArray(vaoYellow);
            GL.BindBuffer(BufferTarget.ArrayBuffer,vbo1);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0,3,VertexAttribPointerType.Float,false,6*sizeof(float),0);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(1,3,VertexAttribPointerType.Float,false,6*sizeof(float),3*sizeof(float));
            GL.EnableVertexAttribArray(1);
            GL.BindVertexArray(0);
            
            GL.UseProgram(program.Id);
            GL.BindVertexArray(vaoYellow);
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