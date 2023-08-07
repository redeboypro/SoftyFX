using System;

namespace SoftyFX.Mathematics
{
    public struct Matrix4x4
    {
        public const int Empty = 0;
        public const int Identity = 1;
        
        public float[,] M;

        public Matrix4x4(int matrixMode)
        {
            M = new float[,]
            {
                {matrixMode, 0, 0, 0},
                {0, matrixMode, 0, 0},
                {0, 0, matrixMode, 0},
                {0, 0, 0, matrixMode}
            };
        }

        public void Project(float near, float far, float fov, float aspectRatio)
        {
            var fovRad = (float) (1.0 / Math.Tan(fov * 0.5 / 180.0 * Math.PI));
            M[0, 0] = aspectRatio * fovRad;
            M[1, 1] = fovRad;
            M[2, 2] = far / (far - near);
            M[3, 2] = -far * near / (far - near);
            M[2, 3] = 1.0f;
            M[3, 3] = 0.0f;
        }
        
        public void CreatePosition(Vector3 position)
        {
            M[3, 0] = position.X;
            M[3, 1] = position.Y;
            M[3, 2] = position.Z;
        }

        public void CreateRotationX(float angle)
        {
            M[1, 1] = (float) Math.Cos(angle * 0.5f);
            M[1, 2] = (float) Math.Sin(angle * 0.5f);
            M[2, 1] = (float) -Math.Sin(angle * 0.5f);
            M[2, 2] = (float) Math.Cos(angle * 0.5f);
        }
        
        public void CreateRotationY(float angle)
        {
            M[0, 0] = (float) Math.Cos(angle);
            M[0, 2] = (float) -Math.Sin(angle);
            M[2, 0] = (float) Math.Sin(angle);
            M[2, 2] = (float) Math.Cos(angle);
        }

        public void CreateRotationZ(float angle)
        {
            M[0, 0] = (float) Math.Cos(angle);
            M[0, 1] = (float) Math.Sin(angle);
            M[1, 0] = (float) -Math.Sin(angle);
            M[1, 1] = (float) Math.Cos(angle);
        }
        
        public void CreateScale(Vector3 scale)
        {
            M[0, 0] = scale.X;
            M[1, 1] = scale.Y;
            M[2, 2] = scale.Z;
        }

        public void LookAt(Vector3 position, Vector3 target, Vector3 up)
        {
            var front = target - position;
            front.Normalize();

            var offset = front * Vector3.Dot(up, front);
            up -= offset;
            up.Normalize();

            var right = Vector3.Cross(up, front);
            M = new[,]
            {
                {right.X, right.Y, right.Z, 0.0f},

                {up.X, up.Y, up.Z, 0.0f},

                {front.X, front.Y, front.Z, 0.0f},

                {position.X, position.Y, position.Z, 1.0f}
            };
        }

        public static Matrix4x4 operator *(Matrix4x4 left, Matrix4x4 right)
        {
            var matrix = new Matrix4x4(Identity);
            for (var col = 0; col < 4; col++)
            {
                for (var raw = 0; raw < 4; raw++)
                {
                    matrix.M[raw, col] = left.M[raw, 0] * right.M[0, col] + left.M[raw, 1] * right.M[1, col] +
                                         left.M[raw, 2] * right.M[2, col] + left.M[raw, 3] * right.M[3, col];
                }
            }

            return matrix;
        }
    }
}