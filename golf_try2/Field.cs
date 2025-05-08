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
using static System.Formats.Asn1.AsnWriter;

namespace golf_try2
{
    class Field: Object
    {
        string filepath = "grass.jpg";

        private const int Width = 60; // Ширина сетки
        private const int Height = 60; // Высота сетки
        private const float Scale = 0.1f;
        public Field()
        {
            //base.vertices = new List<Vector3>()
            //{
            //    new Vector3(-2f, -0.06f, 2f), //top-left vertice
            //    new Vector3( 2f, -0.06f, 2f), //top-right vertice
            //    new Vector3( 2f, -0.06f, -2f), //bottom-right vertice
            //    new Vector3(-2f, -0.06f, -2f) //bottom-left vertice
            //};
            //base.indices = new List<uint>(){ 0, 1, 2, 2, 3, 0 };
            //base.texCoords = new List<Vector2>()
            //{
            //    new Vector2(0f, 0.25f),
            //    new Vector2(0.25f, 0.25f),
            //    new Vector2(0.25f, 0f),
            //    new Vector2(0f, 0f)
            //};

            vertices = new List<Vector3>();
            indices = new List<uint>();
            texCoords = new List<Vector2>();

            for (int x = -Width/2; x < Width/2; x++)
            {
                for (int z = -Height/2; z < Height/2; z++)
                {
                    float xPos = x * Scale;
                    float zPos = z * Scale;
                    float yPos = (float)(System.Math.Sin(xPos) * System.Math.Cos(zPos));

                    vertices.Add(new Vector3(xPos, yPos, zPos));
                    texCoords.Add(new Vector2((float)x / (Width - 1) , (float)z / (Height - 1))); // Добавляем текстурные координаты
                }
            }

            // Создание индексов для треугольников
            for (int x = 0; x < Width - 1; x++)
            {
                for (int z = 0; z < Height - 1; z++)
                {
                    uint i1 = (uint)(x * Height + z);
                    uint i2 = (uint)((x + 1) * Height + z);
                    uint i3 = (uint)(x * Height + (z + 1));
                    uint i4 = (uint)((x + 1) * Height + (z + 1));

                    // Первый треугольник
                    indices.Add(i1);
                    indices.Add(i2);
                    indices.Add(i3);

                    // Второй треугольник
                    indices.Add(i3);
                    indices.Add(i2);
                    indices.Add(i4);
                }
            }

            VAO = GL.GenVertexArray();//Create Vertex Array Object
            VBO = GL.GenBuffer();//Create Vertex Buffer Object
            EBO = GL.GenBuffer();//Create Element Buffer Object

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);//Bind the VBO
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Count * Vector3.SizeInBytes, vertices.ToArray(), BufferUsageHint.StaticDraw);//Copy vertices data to the buffer

            GL.BindVertexArray(VAO); //Bind the VAO
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);//Bind a slot number 0
            GL.EnableVertexArrayAttrib(VAO, 0);//Enable the slot

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);//Unbind the VBO

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Count * sizeof(uint), indices.ToArray(), BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);


            base.texture = new Texture(filepath, base.texCoords);
            base.texture.LoadTexture();

            base.texture.CreateBuffer();

            //Point a slot number 1
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);

            //Enable the slot
            GL.EnableVertexArrayAttrib(VAO, 1);

            GL.BindVertexArray(0);
        }
    }
}
