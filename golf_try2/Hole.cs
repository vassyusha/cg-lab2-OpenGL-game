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
    class Hole : Object
    {
        public Vector3 position = Vector3.Zero;

        Random random = new Random();
        float min = -2.8f, max = 2.8f;

        public Hole(float radius, uint segments)
        {
            base.vertices = new List<Vector3>();
            base.indices = new List<uint>();
            base.texCoords = new List<Vector2>();
            string filepath = "black.jpg";

            for (uint i = 0; i <= segments; i++)
            {
                double angle = 2.0 * Math.PI * i / segments;
                float x = (float)(radius * Math.Cos(angle));
                float y = (float)(radius * Math.Sin(angle));
                base.vertices.Add(new Vector3(x, -0.05f, y));
                
                base.texCoords.Add(new Vector2((MathF.Cos((float)angle) + 1.0f) / 2.0f, (MathF.Sin((float)angle) + 1.0f) / 2.0f));
                if (i > 0)
                {
                    indices.Add(0);
                    indices.Add(i);
                    indices.Add(i + 1 >= segments ? 1 : i + 1); 
                }

                if (segments > 2)
                {
                    indices.Add(0);
                    indices.Add(segments);
                    indices.Add(1);
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
        public void NewPos()
        {
            position[0] = (float)(min + (random.NextDouble() * (max - min)));
            position[2] = (float)(min + (random.NextDouble() * (max - min)));
            position[1] = (float)(System.Math.Sin(position[0]) * System.Math.Cos(position[2])) + 0.12f;
        }
    }

    
}
