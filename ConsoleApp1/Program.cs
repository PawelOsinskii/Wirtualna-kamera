using System;
using Tao.FreeGlut;
using OpenGL;

namespace ConsoleApp1
{
    class Program
    {
        private static int width = 1200, height = 720;
        static void Main(string[] args)
        {
            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH);
            Glut.glutInitWindowSize(width, height);
            Glut.glutCreateWindow("Wirtualna Kamera");

            Glut.glutIdleFunc(OnRenderFrame);
            Glut.glutDisplayFunc(onDisplay);

            Glut.glutMainLoop();
        }

        private static void OnRenderFrame()
        {

            
        }

        private static void onDisplay()
        {
            Gl.Viewport(0, 0, width, height);
            Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Glut.glutSwapBuffers();
        }
    }
}
