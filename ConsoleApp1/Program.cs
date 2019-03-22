using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Xml;
using log4net.Util;
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
            ConfigureLogging();
            var a = new MainWindow();
            a.Run(30);
        }

        private static void ConfigureLogging()
        {
            XmlDocument log4netConfig = new XmlDocument();
            log4netConfig.Load(File.OpenRead(Path.Combine("Data","Settings","log4net.config")));
            var repo = log4net.LogManager.CreateRepository(Assembly.GetEntryAssembly(),
                typeof(log4net.Repository.Hierarchy.Hierarchy));
            log4net.Config.XmlConfigurator.Configure(repo, log4netConfig["log4net"]);
        }
    }

    public sealed class MainWindow : GameWindow
    {
        private int loc;
        private int ass;
        private float coeff = 0.8f;
        public MainWindow()
            : base(800, // initial width
                600, // initial height
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
            GL.ClearColor(Color.Teal);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            float[] vertices = {
                // Positions          // Colors           // Texture Coords
                0.5f, -0.5f, 0.0f,   0.0f, 1.0f, 0.0f,   1.0f, 0.0f, // Bottom Right
                -0.5f, -0.5f, 0.0f,   0.0f, 0.0f, 1.0f,   0.0f, 0.0f, // Bottom Left
                0.5f,  0.5f, 0.0f,   1.0f, 0.0f, 0.0f,   1.0f, 1.0f, // Top Right
                -0.5f,  0.5f, 0.0f,   1.0f, 1.0f, 0.0f,   0.0f, 1.0f,  // Top Left
                
            };

            GlProgram program;

            using (var vertexShader = new Shader(ShaderType.VertexShader,Path.Combine("Data","Shaders","VertexShader")))
            using (var fragmentShader = new Shader(ShaderType.FragmentShader,Path.Combine("Data","Shaders","FragmentShader")))
            {
                program = new GlProgram(vertexShader,fragmentShader);
            }
                        
            int vbo1 = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo1);
            GL.BufferData(BufferTarget.ArrayBuffer,vertices.Length*sizeof(float),vertices,BufferUsageHint.StaticDraw);

            int vaoYellow = GL.GenVertexArray();
            GL.BindVertexArray(vaoYellow);
            GL.BindBuffer(BufferTarget.ArrayBuffer,vbo1);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(0,3,VertexAttribPointerType.Float,false,8*sizeof(float),0);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(1,3,VertexAttribPointerType.Float,false,8*sizeof(float),3*sizeof(float));
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(2,2,VertexAttribPointerType.Float,false,8*sizeof(float),6*sizeof(float));
            GL.EnableVertexAttribArray(2);
            GL.BindVertexArray(0);
            
            GL.UseProgram(program.Id);

            var textureSettings = new Action[]
            {
                () => Texture2D.SetFilter(TextureMinFilter.LinearMipmapLinear),
                () => Texture2D.SetFilter(TextureMagFilter.Linear),
                () => Texture2D.SetWrapModeX(TextureWrapMode.Repeat),
                () => Texture2D.SetWrapModeY(TextureWrapMode.Repeat),
            };
            Texture2D.BindToUniform(program,"ourTexture1",Path.Combine("Data", "Images", "container.jpg"),textureSettings);
            Texture2D.BindToUniform(program,"ourTexture2",Path.Combine("Data", "Images", "awesomeface1.png"),textureSettings);
            loc = program.GetUniformLocation("mixCoeff");
            GL.Uniform1(loc,0.8f);
            ass = vaoYellow;
            GL.BindVertexArray(vaoYellow);
            GL.DrawArrays(PrimitiveType.Triangles,0,3);
            GL.DrawArrays(PrimitiveType.Triangles,1,3);
            GL.BindVertexArray(0);

             
            SwapBuffers();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Uniform1(loc,coeff);
            GL.BindVertexArray(ass);
            GL.DrawArrays(PrimitiveType.Triangles,0,3);
            GL.DrawArrays(PrimitiveType.Triangles,1,3);
            GL.BindVertexArray(0);
            SwapBuffers();
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            if(e.Key == Key.Escape)
                Exit();
            if (e.Key == Key.Down)
                coeff -= 0.1f;
            if (e.Key == Key.Up)
                coeff += 0.1f;
        }
    }
}