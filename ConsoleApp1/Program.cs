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
            Title += ": OpenGL Version: " + GL.GetString(StringName.Version) + "GLORY FOR UKRAINE! GLORY FOR HEROES!";
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.ClearColor(Color.Tan);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            float[] vertYellow =
            {
                0.5f, -0.0f, 0.0f,
                -0.5f, -0.0f, 0.0f,
                0.5f, 0.5f, 0.0f,
               -0.5f, 0.5f, 0.0f,
            };
            
            float[] vertBlue =
            {
                0.5f, -0.5f, 0.0f,
                -0.5f, -0.5f, 0.0f,
                0.5f, 0.0f, 0.0f,
                -0.5f, 0.0f, 0.0f,
            };
            int vbo1;
            GL.GenBuffers(1, out vbo1);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo1);
            GL.BufferData(BufferTarget.ArrayBuffer,vertYellow.Length*sizeof(float),vertYellow,BufferUsageHint.StaticDraw);
            
            string vertexShaderCode ="#version 330 core\nlayout (location = 0) in vec3 position;\nvoid main()\n{\n\tgl_Position = vec4(position.x, position.y, position.z, 1.0);\n}";
            int vertexShader;
            vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader,vertexShaderCode);
            GL.CompileShader(vertexShader);
            Console.WriteLine(GL.GetShaderInfoLog(vertexShader));
            
            string fragmentShaderYellowCode ="#version 330 core\nout vec4 color;\nvoid main()\n{\n\tcolor = vec4(0.9f, 0.4f, 0.0f, 1.0f);\n}";
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
            GL.UseProgram(shaderProgramYellow);
            //GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
            GL.BindVertexArray(vaoYellow);
            GL.DrawArrays(PrimitiveType.Triangles,0,3);
            GL.DrawArrays(PrimitiveType.Triangles,1,4);
            GL.BindVertexArray(0);
            
            int vbo2;
            GL.GenBuffers(1, out vbo2);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo2);
            GL.BufferData(BufferTarget.ArrayBuffer,vertBlue.Length*sizeof(float),vertBlue,BufferUsageHint.StaticDraw);
             
            string fragmentShaderBlueCode ="#version 330 core\nout vec4 color;\nvoid main()\n{\n\tcolor = vec4(0.0f, 0.2f, 0.5f, 1.0f);\n}";
            int fragmentShader2;
            fragmentShader2 = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader2,fragmentShaderBlueCode);
            GL.CompileShader(fragmentShader2);
            Console.WriteLine(GL.GetShaderInfoLog(fragmentShader2));

            int shaderProgramBlue;
            shaderProgramBlue = GL.CreateProgram();
            GL.AttachShader(shaderProgramBlue,vertexShader);
            GL.AttachShader(shaderProgramBlue,fragmentShader2);
            GL.LinkProgram(shaderProgramBlue);
            Console.WriteLine(GL.GetProgramInfoLog(shaderProgramBlue));
            
            int vaoBlue;
            GL.GenVertexArrays(1, out vaoBlue);
            GL.BindVertexArray(vaoBlue);
            GL.BindBuffer(BufferTarget.ArrayBuffer,vbo2);
            GL.BufferData(BufferTarget.ArrayBuffer, vertBlue.Length * sizeof(float), vertBlue, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0,3,VertexAttribPointerType.Float,false,3*sizeof(float),0);
            GL.EnableVertexAttribArray(0);
            GL.BindVertexArray(0);
            GL.UseProgram(shaderProgramBlue);
            //GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader2);
            GL.BindVertexArray(vaoBlue);
            GL.DrawArrays(PrimitiveType.Triangles,0,3);
            GL.DrawArrays(PrimitiveType.Triangles,1,4);
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