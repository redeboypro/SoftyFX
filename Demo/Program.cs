using System;
using System.Diagnostics;
using System.Globalization;
using SoftyFX;
using SoftyFX.Common;
using SoftyFX.Graphics;
using SoftyFX.Mathematics;
using SoftyFX.Time;

namespace Demo
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var time = new Time();
            var frameTime = 0.0f;
            var fps = 0;

            Wavefront.Import("bunny.obj", out var mesh);

            SoftyContext.InitDevice();

            var translation = new Matrix4x4(Matrix4x4.Identity);
            var rotation = new Matrix4x4(Matrix4x4.Identity);
            var projection = new Matrix4x4(Matrix4x4.Identity);
            projection.Project(0.01f, 100.0f, 60, 1);

            var angle = 0.0f;
            while (!SoftyContext.ReadyToQuit)
            {
                time.Begin();
                frameTime += time.GetDeltaTime();
                fps++;

                if (frameTime >= 1.0f)
                {
                    Console.SetCursorPosition(0, 0);
                    Console.Title = $"FPS: {fps}";
                    frameTime = 0.0f;
                    fps = 0;
                }

                SoftyContext.UnlockDoubleBuffer();
                SoftyContext.Clear();

                angle += time.GetDeltaTime();
                rotation.CreateRotationY(angle);

                translation.CreatePosition(new Vector3(0, 0, 5));

                SoftyRenderer.DrawTriangles(mesh, Vector3.Zero, rotation * translation, projection, Rgb.Red, RenderMode.Solid);
                SoftyContext.LockDoubleBuffer();

                SoftyContext.WaitForFrame(10000);
                time.End();
            }

            SoftyContext.ReleaseDevice();
        }
    }
}