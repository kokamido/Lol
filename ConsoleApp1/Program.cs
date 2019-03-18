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
            float[] vertYellow =
            {
                -0.5f, -0.5f, 0.0f,
                0.5f, -0.5f, 0.0f,
                0.0f,  0.5f, 0.0f
            };
            
            int vbo1;
            GL.GenBuffers(1, out vbo1);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo1);
            GL.BufferData(BufferTarget.ArrayBuffer,vertYellow.Length*sizeof(float),vertYellow,BufferUsageHint.StaticDraw);

            string vertexShaderCode = ShaderHelper.LoadShader("VertexShader");
            int vertexShader;
            vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader,vertexShaderCode);
            GL.CompileShader(vertexShader);
            Console.WriteLine(GL.GetShaderInfoLog(vertexShader));
            
            string fragmentShaderYellowCode = ShaderHelper.LoadShader("FragmentShader");;
            int fragmentShader;
            fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader,fragmentShaderYellowCode);
            GL.CompileShader(fragmentShader);
            Console.WriteLine(GL.GetShaderInfoLog(fragmentShader));

            int shaderProgramYellow;
            shaderProgramYellow = GL.CreateProgram();
            GL.AttachShader(shaderProgramYellow,vertexShader);
            GL.AttachShader(shaderProgramYellow,fragmentShader);
            GL.LinkProgram(shaderProgramYellow);
            Console.WriteLine(GL.GetProgramInfoLog(shaderProgramYellow));
            
            int vaoYellow;
            GL.GenVertexArrays(1, out vaoYellow);
            GL.BindVertexArray(vaoYellow);
            GL.BindBuffer(BufferTarget.ArrayBuffer,vbo1);
            GL.BufferData(BufferTarget.ArrayBuffer, vertYellow.Length * sizeof(float), vertYellow, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0,3,VertexAttribPointerType.Float,false,3*sizeof(float),0);
            GL.EnableVertexAttribArray(0);
            GL.BindVertexArray(0);
            float timeValue = DateTime.UtcNow.Second+DateTime.UtcNow.Millisecond/1000.0f;
            var greenValue = (float)((Math.Sin(timeValue) / 2) + 0.5);
            int vertexColorLocation = GL.GetUniformLocation(shaderProgramYellow, "ourColor");

            GL.UseProgram(shaderProgramYellow);
            GL.Uniform4(vertexColorLocation, 0.0f, greenValue, 0.0f, 1.0f);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
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