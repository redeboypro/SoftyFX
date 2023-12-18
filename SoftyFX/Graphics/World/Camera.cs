using SoftyFX.Mathematics;

namespace SoftyFX.Graphics.World
{
    public class Camera
    {
        private Vector3 _from;
        private Vector3 _to;
        private Vector3 _up;
        
        private float _aspectRatio;
        private float _fov;
        
        private float _near;
        private float _far;

        public Camera(float aspectRatio)
        {
            ViewMatrix = new Matrix4x4(Matrix4x4.Identity);
            ProjectionMatrix = new Matrix4x4(Matrix4x4.Identity);
            
            _from = new Vector3(0, 0, -100);
            _to = Vector3.Zero;
            _up = Vector3.Up;

            _aspectRatio = aspectRatio;
            _fov = 60f;
            _far = 100f;
            _near = 0.1f;
            
            UpdateViewMatrix();
            UpdateProjectionMatrix();
        }
        
        public Matrix4x4 ViewMatrix { get; }
        public Matrix4x4 ProjectionMatrix { get; }

        public Vector3 From
        {
            get
            {
                return _from;
            }
            set
            {
                _from = value;
                UpdateViewMatrix();
            }
        }
        
        public Vector3 To
        {
            get
            {
                return _to;
            }
            set
            {
                _to = value;
                UpdateViewMatrix();
            }
        }
        
        public Vector3 Up
        {
            get
            {
                return _up;
            }
            set
            {
                _up = value;
                UpdateViewMatrix();
            }
        }
        
        public float AspectRatio
        {
            get
            {
                return _aspectRatio;
            }
            set
            {
                _aspectRatio = value;
                UpdateProjectionMatrix();
            }
        }
        
        public float FieldOfView
        {
            get
            {
                return _fov;
            }
            set
            {
                _fov = value;
                UpdateProjectionMatrix();
            }
        }
        
        public float Near
        {
            get
            {
                return _near;
            }
            set
            {
                _near = value;
                UpdateProjectionMatrix();
            }
        }
        
        public float Far
        {
            get
            {
                return _far;
            }
            set
            {
                _far = value;
                UpdateProjectionMatrix();
            }
        }

        private void UpdateViewMatrix()
        {
            ViewMatrix.PointAt(_from, _to, _up);
            ViewMatrix.Inverse();
        }
        
        private void UpdateProjectionMatrix()
        {
            ProjectionMatrix.Project(_near, _far, _fov, _aspectRatio);
        }
    }
}