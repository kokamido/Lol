using System;
using System.IO;
using System.Text;

namespace ConsoleApp1
{
    public static class ShaderHelper
    {
        public static string ShadersFolderPath;

        public static string LoadShader(string name)
        {
            return RemoveCyrillic(File.ReadAllText(ShadersFolderPath ?? Path.Combine(Directory.GetCurrentDirectory(), "Shaders", name)));

        }

        private static readonly Func<char, bool> isCyrillic = x => 'а' <= x && x <= 'я' || 'А' <= x && x <= 'Я' || x == 'ё' || x == 'ё';
        private static string RemoveCyrillic(string sourse)
        {
            var res = new StringBuilder();
            
            foreach (var ch in sourse)
            {
                if (!isCyrillic(ch))
                    res.Append(ch);
            }

            return res.ToString();
        }
    }
}