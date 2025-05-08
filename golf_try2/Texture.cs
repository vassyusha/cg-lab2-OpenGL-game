using StbImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Audio.OpenAL;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;


namespace golf_try2
{
    class Texture
    {
        public int textureID;
        public int textureVBO;

        public List<Vector2> texCoords;
        string filepath;

        public Texture(string name, List<Vector2> coords)
        {
            filepath = name;
            texCoords = coords;
        }

        public void LoadTexture()
        {
            textureID = GL.GenTexture(); //Generate empty texture
            GL.ActiveTexture(TextureUnit.Texture0); //Activate the texture in the unit
            GL.BindTexture(TextureTarget.Texture2D, textureID); //Bind texture

            //Texture parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            LoadImage(filepath);

            //Unbind the texture
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void CreateBuffer()
        {
            //Create, bind texture
            textureVBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, textureVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, texCoords.Count * Vector3.SizeInBytes, texCoords.ToArray(), BufferUsageHint.StaticDraw);
            
        }

        public static void LoadImage(string filepath) //передаем путь до файла, из которого грузить шейдеры
        {
            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult texture = ImageResult.FromStream(File.OpenRead("../../../Textures/" + filepath), ColorComponents.RedGreenBlueAlpha);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, texture.Width, texture.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, texture.Data);
        }

        public void Bind()
        {
            GL.BindTexture(TextureTarget.Texture2D, textureID);
        }

        public void DeleteTexture()
        {
            GL.DeleteTexture(textureID);
            GL.DeleteBuffer(textureVBO);
        }
    }
}
