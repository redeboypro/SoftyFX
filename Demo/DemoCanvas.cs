using System.IO;
using SoftyFX;
using SoftyFX.Common;
using SoftyFX.Display;
using SoftyFX.Graphics;
using SoftyFX.Mathematics;
using SoftyFX.Time;

namespace Demo
{
    public class DemoCanvas : Display
    {
        private Camera _camera;
        private Triangle[] _demoMesh;

        private Matrix4x4 _translationMatrix;
        private Matrix4x4 _rotationMatrix;

        private float _rotationAngle;

        protected override void OnLoad()
        {
            _camera = new Camera(1);
            
            _rotationMatrix = new Matrix4x4(Matrix4x4.Identity);
            _rotationAngle = 0.0f;

            _translationMatrix = new Matrix4x4(Matrix4x4.Identity);
            _translationMatrix.CreatePosition(new Vector3(0, 0, 5));
            
            Wavefront.Import("player.obj", out _demoMesh);
        }

        protected override void OnUpdateFrame(Time time)
        {
            _rotationAngle += time.DeltaTime;
            _rotationMatrix.CreateRotationY(_rotationAngle);
            SoftyContext.Clear();
            SoftyRenderer.DrawTriangles(_demoMesh, _rotationMatrix * _translationMatrix, _camera, RenderMode.Lines);
        }
    }
}