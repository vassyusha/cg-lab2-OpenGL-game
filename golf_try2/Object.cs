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
    class Object
    {
        protected Texture texture;

        protected List<Vector3> vertices;
        protected List<uint> indices;
        protected List<Vector2> texCoords;

        protected int VAO;
        protected int VBO;
        protected int EBO;

        protected Object() { }

        public Object(List<Vector3> vert, List<uint> ind, List<Vector2> tex, string filepath)
        {
            vertices = vert;
            indices = ind;
            texCoords = tex;

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


            texture = new Texture(filepath, texCoords);
            texture.LoadTexture();

            texture.CreateBuffer();

            //Point a slot number 1
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);

            //Enable the slot
            GL.EnableVertexArrayAttrib(VAO, 1);

            GL.BindVertexArray(0);
        }

        public void Delete()
        {
            GL.DeleteBuffer(VAO);
            GL.DeleteBuffer(VBO);
            GL.DeleteBuffer(EBO);

            texture.DeleteTexture();
        }

        public void Draw()
        {
            texture.Bind();

            GL.BindVertexArray(VAO);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.DrawElements(PrimitiveType.Triangles, indices.Count, DrawElementsType.UnsignedInt, 0);
        }

    }
}
