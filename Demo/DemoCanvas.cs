using System;
using System.IO;
using Demo.Common;
using SoftyFX;
using SoftyFX.Display;
using SoftyFX.Graphics;
using SoftyFX.Graphics.Geometry;
using SoftyFX.Graphics.Shaders;
using SoftyFX.Graphics.World;
using SoftyFX.Mathematics;
using SoftyFX.Time;

namespace Demo
{
    public class DemoCanvas : Display
    {
        private static Matrix4x4 _translationMatrix;
        private static Matrix4x4 _rotationMatrix;
        
        private Camera _camera;

        private float _rotationAngle;

        private Polygon2D _circle;

        private float z;

        public DemoCanvas(string title, int width, int height) : base(title, width, height, true) { }

        protected override void OnLoad()
        {
            _camera = new Camera(1);

            _rotationMatrix = new Matrix4x4(Matrix4x4.Identity);
            _rotationAngle = 0.0f;
            z = 10;

            _translationMatrix = new Matrix4x4(Matrix4x4.Identity);
            _translationMatrix.CreatePosition(new Vector3(0, 0, 10f));
            
            Wavefront.Import("C:\\Users\\redeb\\RiderProjects\\SoftyFX\\Demo\\bin\\Debug\\teapot.obj", out var _demoMeshTris);
            var _demoMesh = new Mesh(_demoMeshTris);
            
            _circle = Polygon2D.Rectangle(0, 50, 300, 100, Rgb.Red);
            
            SoftyRenderer.SetCamera(_camera);
            var shader = SoftyRenderer.BindShader(new BasicShader());
            SoftyRenderer.Prepare(_demoMesh, shader);
        }

        protected override void OnUpdateFrame(Time time)
        {
            _rotationAngle += time.DeltaTime;
            _rotationMatrix.CreateRotationY(_rotationAngle);

            if (Input.IsKeyDown('W'))
            {
                z += time.DeltaTime * 2;
                _translationMatrix.CreatePosition(new Vector3(0, 0, z));
            }
            
            if (Input.IsKeyDown('S'))
            {
                z -= time.DeltaTime * 2;
                _translationMatrix.CreatePosition(new Vector3(0, 0, z));
            }
        }

        protected override void OnRenderFrame(Time time)
        {
            SoftyContext.Clear();
            SoftyRenderer.DrawBuffer(600, 600);
            Console.Title = time.FramesPerSecond.ToString();
            //SoftyRenderer.DrawPolyText($"FPS: {time.FramesPerSecond}", Rgb.Black, _circle);
        }

        private class BasicShader : Shader
        {
            public override Vector3 Vertex(in Vector3 pos)
            {
                var vertex = pos * (_rotationMatrix * _translationMatrix);
                return vertex;
            }
        }
    }
}