using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Audio.OpenAL;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

using StbImageSharp;

namespace golf_try2
{
    class Game : GameWindow
    {
        int width, height;
        float yRot = 0f;

        //List<float> vertices;

        //List<int> indices;
        //List<Vector2> texCoords;

        ////front face
        //new Vector3(-0.5f, 0.5f, 0.5f), //top-left vertice
        //new Vector3( 0.5f, 0.5f, 0.5f), //top-right vertice
        //new Vector3( 0.5f, -0.5f, 0.5f), //bottom-right vertice
        //new Vector3(-0.5f, -0.5f, 0.5f) //bottom-left vertice

        List<Vector3> vertices = new List<Vector3>()
        {
            //1st
            new Vector3(0.0f, 0.25f, 0.0f),
            new Vector3(0.25f, 0.0f, 0.0f),
            new Vector3(0f, 0.0f, 0.25f),
            //2nd
            new Vector3(0f, 0.0f, 0.25f),
            new Vector3(0.25f, 0.0f, 0.0f),
            new Vector3(0.0f, -0.25f, 0.0f),
            //3rd
            new Vector3(0.0f, -0.25f, 0.0f),
            new Vector3(-0.25f, 0.0f, 0.0f),
            new Vector3(0f, 0.0f, 0.25f),
            //4th
            new Vector3(0f, 0.0f, 0.25f),
            new Vector3(0.0f, 0.25f, 0.0f),
            new Vector3(-0.25f, 0.0f, 0.0f),
            //5th
            new Vector3(-0.25f, 0.0f, 0.0f),
            new Vector3(0.0f, 0.25f, 0.0f),
            new Vector3(0f, 0.0f, -0.25f),
            //6th
            new Vector3(0f, 0.0f, -0.25f),
            new Vector3(-0.25f, 0.0f, 0.0f),
            new Vector3(0.0f, -0.25f, 0.0f),
            //7th
            new Vector3(0.0f, -0.25f, 0.0f),
            new Vector3(0f, 0.0f, -0.25f),
            new Vector3(0.25f, 0.0f, 0.0f),
            //8th
            new Vector3(0.25f, 0.0f, 0.0f),
            new Vector3(0f, 0.0f, -0.25f),
            new Vector3(0f, 0.25f, 0.0f),
            
        };

        List<Vector3> pictVertices = new List<Vector3>()
        {
            new Vector3(-2f, -0.05f, 2f), //top-left vertice
            new Vector3( 2f, -0.05f, 2f), //top-right vertice
            new Vector3( 2f, -0.05f, -2f), //bottom-right vertice
            new Vector3(-2f, -0.05f, -2f) //bottom-left vertice
        };

        //float[] vertices =
        //{
        //    -0.5f, 0.5f, 0.0f,
        //    0.5f, 0.5f, 0.0f,
        //    0.5f, -0.5f, 0.0f,
        //    -0.5f, -0.5f, 0.0f
        //};


        List<uint> indices =new List<uint>()
        {
            0, 1, 2, 
            3, 4, 5,
            6,7,8,
            9,10,11,
            12,13,14,
            15,16,17,
            18,19,20,
            21,22,23
        };

        List<uint> pictIndices = new List<uint>()
        {
            0, 1, 2, 2, 3, 0
        };

        List<Vector2> nepoiduCoords = new List<Vector2>()
        {
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f),
            new Vector2(0f, 0f)
        };

        List<Vector2> glassCoords = new List<Vector2>()
        {
            new Vector2(0f, 0f),
            new Vector2(0.5f, 1f),
            new Vector2(1f, 0f),

            new Vector2(0f, 0f),
            new Vector2(0.5f, 1f),
            new Vector2(1f, 0f),

            new Vector2(0f, 0f),
            new Vector2(0.5f, 1f),
            new Vector2(1f, 0f),

            new Vector2(0f, 0f),
            new Vector2(0.5f, 1f),
            new Vector2(1f, 0f),

            new Vector2(0f, 0f),
            new Vector2(0.5f, 1f),
            new Vector2(1f, 0f),

            new Vector2(0f, 0f),
            new Vector2(0.5f, 1f),
            new Vector2(1f, 0f),

            new Vector2(0f, 0f),
            new Vector2(0.5f, 1f),
            new Vector2(1f, 0f),

            new Vector2(0f, 0f),
            new Vector2(0.5f, 1f),
            new Vector2(1f, 0f)
        };

        //float[] nepoiduCoords =
        //{
        //    0f, 1f,
        //    1f, 1f,
        //    1f, 0f,
        //    0f, 0f
        //};


        int VAO;
        int VBO;
        int EBO;

        Vector3 pos = new Vector3(0.0f, 0.0f, 0.0f);

        Shader shaderProgram;
        Texture texture;
        Camera camera;

        Object obj, obj1;
        Ball sph;
        Field field;
        Hole hole;

        public Game(int width, int height) : base
        (GameWindowSettings.Default, NativeWindowSettings.Default)
        {
            this.CenterWindow(new Vector2i(width, height));
            this.height = height;
            this.width = width;
        }
        protected override void OnLoad()
        {

            //obj = new Object(vertices, indices, glassCoords, "glass.jpg");
            //obj1 = new Object(pictVertices, pictIndices, nepoiduCoords, "grass.jpg");
            field = new Field();
            sph = new Ball(0.1f, 10, 10);
            hole = new Hole(0.15f, 100);

            shaderProgram = new Shader();
            shaderProgram.LoadShader();

            camera = new Camera(width, height, Vector3.Zero);

            GL.Enable(EnableCap.DepthTest);

            base.OnLoad();

        }
        protected override void OnUnload()
        {
            sph.Delete();
            field.Delete();

            shaderProgram.DeleteShader();

            base.OnUnload();
        }
        protected override void OnRenderFrame(FrameEventArgs args)
        {

            yRot += 0.1f;

            pos += new Vector3(0.1f, 0.0f, 0.0f);

            GL.ClearColor(0.2f, 0.3f, 0.6f, 1f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            shaderProgram.UseShader();

            Matrix4 view = camera.GetViewMatrix();
            Matrix4 projection = camera.GetProjection();

            int viewLocation = GL.GetUniformLocation(shaderProgram.shaderHandle, "view");
            int projectionLocation = GL.GetUniformLocation(shaderProgram.shaderHandle, "projection");

            GL.UniformMatrix4(viewLocation, true, ref view);
            GL.UniformMatrix4(projectionLocation, true, ref projection);

            Matrix4 model1 = Matrix4.CreateRotationX(yRot);
            Matrix4 translation = Matrix4.CreateTranslation(sph.position);
            model1 *= translation;

            int modelLocation = GL.GetUniformLocation(shaderProgram.shaderHandle, "model");

            GL.UniformMatrix4(modelLocation, true, ref model1);

            sph.Draw();

            Matrix4 model2 = Matrix4.Identity;
            //model2 = Matrix4.CreateTranslation(-3f, 0f, -3f);

            modelLocation = GL.GetUniformLocation(shaderProgram.shaderHandle, "model");

            GL.UniformMatrix4(modelLocation, true, ref model2);
            field.Draw();

            //Matrix4 model3 = Matrix4.Identity;
            Matrix4 model3 = Matrix4.CreateTranslation(hole.position);
            modelLocation = GL.GetUniformLocation(shaderProgram.shaderHandle, "model");
            GL.UniformMatrix4(modelLocation, true, ref model3);
            hole.Draw();

            Context.SwapBuffers();
            base.OnRenderFrame(args);
            Thread.Sleep(100);


        }
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Close();
            }
            if (Vector3.Distance(hole.position, sph.position) < 0.1f || KeyboardState.IsKeyDown(Keys.Space))
            {
                hole.NewPos();
                sph.direction = Vector3.Zero;
                sph.velocity = 0.0f;
            }
            sph.Move(MouseState, width, height);
            MouseState mouse = MouseState;
            KeyboardState input = KeyboardState;
            base.OnUpdateFrame(args);
            camera.Update(input, mouse, args);

        }
        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
            this.width = e.Width;
            this.height = e.Height;
        }

        

    }
}
