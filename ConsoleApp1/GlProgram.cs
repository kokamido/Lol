using System;
using OpenTK.Graphics.OpenGL;

namespace ConsoleApp1
{
    public class GlProgram
    {
        public readonly int Id;
        public readonly Guid GlobalId;

        public GlProgram(params Shader[] shaders)
        {
            GlobalId = Guid.NewGuid();
            Id = GL.CreateProgram();
            foreach (var shader in shaders)
                GL.AttachShader(Id,shader.Id);
            GL.LinkProgram(Id);
            var compileLog = GL.GetShaderInfoLog(Id);
            if(string.IsNullOrWhiteSpace(compileLog))
                MyLog.Log.Info($"Program {Id} has been succesfully linked");
            else
                MyLog.Log.Fatal($"Program {Id} {compileLog}");
        }

        public int GetUniformLocation(string name)
        {
            return GL.GetUniformLocation(Id,name);
        }
    }
}