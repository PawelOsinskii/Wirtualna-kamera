﻿using System;
using Tao.FreeGlut;
using OpenGL;

namespace ConsoleApp1
{
    class Program
    {
        private static int width = 1200, height = 720;
        private static VBO<Vector3> triangleColor, squareColor;

        private static ShaderProgram program;
        private static VBO<Vector3> triangle, square;
        private static VBO<int> triangleElements, squareElements;


        static void Main(string[] args)
        {
            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH);
            Glut.glutInitWindowSize(width, height);
            Glut.glutCreateWindow("Wirtualna Kamera");

            Glut.glutIdleFunc(OnRenderFrame);
            Glut.glutDisplayFunc(onDisplay);

            program = new ShaderProgram(VertexShader, FragmentShader);

            program.Use();
            program["projection_matrix"].SetValue(Matrix4.CreatePerspectiveFieldOfView(0.45f, (float)width / height, 0.1f, 1000f));
            program["view_matrix"].SetValue(Matrix4.LookAt(new Vector3(0, 0, 10), Vector3.Zero, Vector3.Up));

            triangle = new VBO<Vector3>(new Vector3[] { new Vector3(0, 1, 0), new Vector3(-1, -1, 0), new Vector3(1, -1, 0) });
            square = new VBO<Vector3>(new Vector3[] { new Vector3(-1, 1, 0), new Vector3(1, 1, 0), new Vector3(1, -1, 0), new Vector3(-1, -1, 0) });

            triangleElements = new VBO<int>(new int[] { 0, 1, 2 }, BufferTarget.ElementArrayBuffer);
            squareElements = new VBO<int>(new int[] { 0, 1, 2, 3 }, BufferTarget.ElementArrayBuffer);

            triangleColor = new VBO<Vector3>(new Vector3[] { new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1) });
            squareColor = new VBO<Vector3>(new Vector3[] { new Vector3(0.5, 0.5, 1), new Vector3(0.5, 0.5, 1), new Vector3(0.5, 0.5, 1), new Vector3(0.5, 0.5, 1) });

            Glut.glutMainLoop();
        }

        private static void OnRenderFrame()
        {
            Gl.Viewport(0, 0, width, height);
            Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

          
            program.Use();

            //draw triangle
            program["model_matrix"].SetValue(Matrix4.CreateTranslation(new Vector3(-1.5f, 0, 0)));

            uint vertexPositionIndex = (uint) Gl.GetAttribLocation(program.ProgramID, "vertexPosition");
            Gl.EnableVertexAttribArray(vertexPositionIndex);
            Gl.BindBuffer(triangle);
            Gl.VertexAttribPointer(vertexPositionIndex, triangle.Size, triangle.PointerType, true, 12, IntPtr.Zero);

            Gl.BindBufferToShaderAttribute(triangleColor,program, "vertexColor");



            Gl.DrawElements(BeginMode.Triangles, triangleElements.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);


            //draw square
            program["model_matrix"].SetValue(Matrix4.CreateTranslation(new Vector3(1.5f, 0, 0)));
            Gl.BindBufferToShaderAttribute(square, program, "vertexPosition");
            
            Gl.BindBufferToShaderAttribute(squareColor, program, "vertexColor");
            Gl.BindBuffer(squareElements);
            Gl.DrawElements(BeginMode.Quads, squareElements.Count, DrawElementsType.UnsignedInt, IntPtr.Zero);


            Glut.glutSwapBuffers();
        }

        private static void onDisplay()
        {
           
        }

        public static string VertexShader = @"
in vec3 vertexPosition;
in vec3 vertexColor;
out vec3 color;

uniform mat4 projection_matrix;
uniform mat4 view_matrix;
uniform mat4 model_matrix;

void main(void){
    color = vertexColor;
    gl_Position = projection_matrix * view_matrix * model_matrix *vec4(vertexPosition,1);
}

";
        public static string FragmentShader = @"
in vec3 color;
void main(void){
    gl_FragColor = vec4(color,1);
}
";

    }
}
