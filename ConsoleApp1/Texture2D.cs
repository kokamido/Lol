using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;    
using OpenTK.Graphics.OpenGL;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace ConsoleApp1
{
    public static class Texture2D
    {
        public static void BindToUniform(GlProgram program, string nameUniform, string pathToFile)
        {
            
            GL.Uniform1(program.GetUniformLocation(nameUniform), LoadAndBindTexture(program, pathToFile));
        }

        public static void SetFilter(TextureMinFilter minFilter)
        {
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) minFilter);
        }

        public static void SetFilter(TextureMagFilter magFilter)
        {
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) magFilter);
        }

        public static void SetWrapModeX(TextureWrapMode mode)
        {
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) mode);
        }

        public static void SetWrapModeY(TextureWrapMode mode)
        {
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int) mode);
        }

        private static int LoadAndBindTexture(GlProgram program,string filename)
        {
            (PixelInternalFormat, OpenTK.Graphics.OpenGL.PixelFormat) formats;

            var img = Image.FromFile(filename, true);
            img.RotateFlip(RotateFlipType.Rotate180FlipX);

            if (!formatMapping.TryGetValue(img.PixelFormat, out formats))
                MyLog.Log.Fatal($"Unexpected texture format: '{img.PixelFormat}'");
            else
                MyLog.Log.Info($"Texture from '{filename}' has been successfully loaded as '{img.PixelFormat}'");
            
            int texturesActive = _texturesInProgram.GetOrAdd(program.GlobalId, 0);
            _texturesInProgram[program.GlobalId] += 1;
            
            Bitmap bmp = new Bitmap(img);
            var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width,
                bmp.Height), ImageLockMode.ReadOnly, img.PixelFormat);
            
            GL.GenTextures(1, out int id);
            id -= 1;
            SetFilter(TextureMinFilter.LinearMipmapLinear); //TODO Fix this shit
            SetFilter(TextureMagFilter.Linear);
            SetWrapModeX(TextureWrapMode.Repeat);
            SetWrapModeY(TextureWrapMode.Repeat);

            GL.BindTexture(TextureTarget.Texture2D, id);

            GL.TexImage2D(TextureTarget.Texture2D, 0,
                formats.Item1, bmpData.Width, bmpData.Height, 0,
                formats.Item2, PixelType.UnsignedByte, bmpData.Scan0);
            
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            bmp.UnlockBits(bmpData);
            Console.WriteLine(id);
            return id;
        }

        private static Dictionary<PixelFormat, (PixelInternalFormat, OpenTK.Graphics.OpenGL.PixelFormat)> formatMapping
            = new Dictionary<PixelFormat, (PixelInternalFormat, OpenTK.Graphics.OpenGL.PixelFormat)>
            {
                {PixelFormat.Format24bppRgb, (PixelInternalFormat.Rgb, OpenTK.Graphics.OpenGL.PixelFormat.Bgr)},
                {PixelFormat.Format32bppArgb, (PixelInternalFormat.Rgba, OpenTK.Graphics.OpenGL.PixelFormat.Bgra)},
            };
        
        private static ConcurrentDictionary<Guid,int> _texturesInProgram = new ConcurrentDictionary<Guid, int>();
    }
}