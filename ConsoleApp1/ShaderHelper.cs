using System;
using System.IO;
using System.Text;
using log4net;
using OpenTK.Graphics.OpenGL;

namespace ConsoleApp1
{
    public class Shader :IDisposable
    {
        public readonly ShaderType Type;
        public readonly int Id;
        protected ILog log;
        public Shader(ShaderType type, string pathToCode)
        {
            log = LogManager.GetLogger(typeof(Shader));
            Type = type;
            Id = GL.CreateShader(type);
            var code = LoadShaderCode(pathToCode);
            GL.ShaderSource(Id,code);
            GL.CompileShader(Id);
            var compileLog = GL.GetShaderInfoLog(Id);
            if(string.IsNullOrWhiteSpace(compileLog))
                log.Info($"{type} has been succesfully compiled");
            else
                log.Fatal($"{type} {compileLog}");
            
        }

        private static string LoadShaderCode(string path)
        {
            return RemoveCyrillic(File.ReadAllText(path));
        }

        private static readonly Func<char, bool> IsCyrillic = x => 'а' <= x && x <= 'я' || 'А' <= x && x <= 'Я' || x == 'ё' || x == 'ё';
        private static string RemoveCyrillic(string sourse)
        {
            var res = new StringBuilder();
            
            foreach (var ch in sourse)
            {
                if (!IsCyrillic(ch))
                    res.Append(ch);
            }

            return res.ToString();
        }

        public void Dispose()
        {
            GL.DeleteShader(Id);
        }
    }
}