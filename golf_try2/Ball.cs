using System;
using System.Collections.Generic;
using System.Dynamic;
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
    class Ball: Object
    {
        public Vector3 direction = new Vector3(0.0f, 0.0f, 0.0f);

        public Vector3 position = new Vector3(0.0f, 0.0f, 0.0f);
        public float velocity = 0.0f, acceleration = 0.01f;

        bool isFirstMove = true;
        public Vector2 mousePos;

        public Ball(float radius, uint longitudeBands, uint latitudeBands)
        {
            base.vertices = new List<Vector3>();
            base.indices = new List<uint>();
            base.texCoords = new List<Vector2>();
            string filepath = "golf.jpg";
            int i = 0;

            for (int latNumber = 0; latNumber <= latitudeBands; latNumber++)
            {
                double theta = latNumber * Math.PI / latitudeBands;
                double sinTheta = Math.Sin(theta);
                double cosTheta = Math.Cos(theta);

                for (int longNumber = 0; longNumber <= longitudeBands; longNumber++)
                {
                    double phi = longNumber * 2 * Math.PI / longitudeBands;
                    double sinPhi = Math.Sin(phi);
                    double cosPhi = Math.Cos(phi);

                    float x = (float)(cosPhi * sinTheta);
                    float y = (float)(cosTheta);
                    float z = (float)(sinPhi * sinTheta);

                    vertices.Add(new Vector3(radius * x, radius * y, radius * z));

                    switch (i % 4)
                    {
                        case 0:
                            texCoords.Add(new Vector2(0f, 0.1f));
                            break;
                        case 1:
                            texCoords.Add(new Vector2(0.1f, 0.1f));
                            break;
                        case 2:
                            texCoords.Add(new Vector2(0.1f, 0f));
                            break;
                        case 3:
                            texCoords.Add(new Vector2(0f, 0f));
                            break;
                    }
                    i++;
                }
            }

            for (uint latNumber = 0; latNumber < latitudeBands; latNumber++)
            {
                for (uint longNumber = 0; longNumber < longitudeBands; longNumber++)
                {
                    uint first = (latNumber * (longitudeBands + 1)) + longNumber;
                    uint second = first + longitudeBands + 1;

                    indices.Add(second);
                    indices.Add(first);
                    indices.Add(first + 1);
                    indices.Add(first + 1);
                    indices.Add(second + 1);
                    indices.Add(second);
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

        public void Move(MouseState mouse, int width, int height)
        {
            
            if (mouse.IsButtonDown(MouseButton.Left))
            {
                if (isFirstMove)
                {
                    isFirstMove = false;
                    mousePos = mouse.Position;
                }
                else
                {
                    direction = new Vector3(-(mousePos - mouse.Position)[1], 0.0f, (mousePos - mouse.Position)[0]);
                    direction.Normalize();
                    Matrix3 matrix = Matrix3.CreateRotationY(-45f);
                    direction *= matrix;
                    velocity += 0.01f;
                }   
            }
            else if (velocity > 0)
            {
                position += velocity * direction;
                position[1] = (float)(System.Math.Sin(position[0]) * System.Math.Cos(position[2])) + 0.1f;
                velocity -= acceleration;

                if (position[0] >3.0f || position[0] < -3.0f || position[2] > 3.0f || position[2] < -3.0f) direction *= -1;
                //if (position[1] > 2.0f || position[1] < -2.0f) direction *= -1;
                isFirstMove = true;
            }
            
        }
    }
}
