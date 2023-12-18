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

        public float this[int row, int col] => M[row, col];

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
        
        public void CreateFromQuaternion(Quaternion quaternion)
        {
            var sqx = quaternion.X * quaternion.X;
            var sqy = quaternion.Y * quaternion.Y;
            var sqz = quaternion.Z * quaternion.Z;
            var sqw = quaternion.W * quaternion.W;

            var xy = quaternion.X * quaternion.Y;
            var xz = quaternion.X * quaternion.Z;
            var xw = quaternion.X * quaternion.W;

            var yz = quaternion.Y * quaternion.Z;
            var yw = quaternion.Y * quaternion.W;

            var zw = quaternion.Z * quaternion.W;

            var s2 = 2f / (sqx + sqy + sqz + sqw);

            M[0, 0] = 1f - (s2 * (sqy + sqz));
            M[1, 1] = 1f - (s2 * (sqx + sqz));
            M[2, 2] = 1f - (s2 * (sqx + sqy));

            M[0, 1] = s2 * (xy + zw);
            M[1, 0] = s2 * (xy - zw);

            M[2, 0] = s2 * (xz + yw);
            M[0, 2] = s2 * (xz - yw);

            M[2, 1] = s2 * (yz - xw);
            M[1, 2] = s2 * (yz + xw);

            M[0, 3] = 0;
            M[1, 3] = 0;
            M[2, 3] = 0;
            
            M[3, 0] = 0;
            M[3, 1] = 0;
            M[3, 2] = 0;
            M[3, 3] = 1;
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
        
        public void CreateFromQuaternion(float angle)
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

        public void LookAt(Vector3 eye, Vector3 target, Vector3 up)
        {
            var front = eye - target;
            front.Normalize();

            var right = Vector3.Cross(up, front);
            right.Normalize();

            var top = Vector3.Cross(front, right);
            top.Normalize();

            M = new[,]
            {
                {right.X, top.X, front.X, 0.0f},

                {right.Y, top.Y, front.Y, 0.0f},

                {right.Z, top.Z, front.Z, 0.0f},

                {
                    -((right.X * eye.X) + (right.Y * eye.Y) + (right.Z * eye.Z)), 
                    -((top.X * eye.X) + (top.Y * eye.Y) + (top.Z * eye.Z)),
                    -((front.X * eye.X) + (front.Y * eye.Y) + (front.Z * eye.Z)),
                    1.0f
                }
            };
        }
        
        public void PointAt(Vector3 eye, Vector3 target, Vector3 up)
        {
            var front = target - eye;
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

                {eye.X, eye.Y, eye.Z, 1.0f}
            };
        }

        public Vector3 ExtractPosition()
        {
            return new Vector3(M[3, 0], M[3, 1], M[3, 2]);
        }

        public Quaternion ExtractOrientation()
        {
            var row0 = new Vector3(M[0, 0],M[0, 1], M[0, 2]);
            var row1 = new Vector3(M[1, 0],M[1, 1], M[1, 2]);
            var row2 = new Vector3(M[2, 0],M[2, 1], M[2, 2]);
            
            row0.Normalize();
            row1.Normalize();
            row2.Normalize();
            
            var quaternion = new Quaternion(Vector3.Zero);
            var trace = 0.25 * (row0[0] + row1[1] + row2[2] + 1.0);

            if (trace > 0)
            {
                var sq = Math.Sqrt(trace);

                quaternion.W = (float)sq;
                sq = 1.0 / (4.0 * sq);
                quaternion.X = (float)((row1[2] - row2[1]) * sq);
                quaternion.Y = (float)((row2[0] - row0[2]) * sq);
                quaternion.Z = (float)((row0[1] - row1[0]) * sq);
            }
            else if (row0[0] > row1[1] && row0[0] > row2[2])
            {
                var sq = 2.0 * Math.Sqrt(1.0 + row0[0] - row1[1] - row2[2]);

                quaternion.X = (float)(0.25 * sq);
                sq = 1.0 / sq;
                quaternion.W = (float)((row2[1] - row1[2]) * sq);
                quaternion.Y = (float)((row1[0] + row0[1]) * sq);
                quaternion.Z = (float)((row2[0] + row0[2]) * sq);
            }
            else if (row1[1] > row2[2])
            {
                var sq = 2.0 * Math.Sqrt(1.0 + row1[1] - row0[0] - row2[2]);

                quaternion.Y = (float)(0.25 * sq);
                sq = 1.0 / sq;
                quaternion.W = (float)((row2[0] - row0[2]) * sq);
                quaternion.X = (float)((row1[0] + row0[1]) * sq);
                quaternion.Z = (float)((row2[1] + row1[2]) * sq);
            }
            else
            {
                var sq = 2.0 * Math.Sqrt(1.0 + row2[2] - row0[0] - row1[1]);

                quaternion.Z = (float)(0.25 * sq);
                sq = 1.0 / sq;
                quaternion.W = (float)((row1[0] - row0[1]) * sq);
                quaternion.X = (float)((row2[0] + row0[2]) * sq);
                quaternion.Y = (float)((row2[1] + row1[2]) * sq);
            }

            quaternion.Normalize();
            return quaternion;
        }
        
        public Vector3 ExtractScale()
        {
            return new Vector3(GetRowLength(0), GetRowLength(1), GetRowLength(2));
        }

        public void Inverse()
        {
            var result = new float[4, 4];
            result[0, 0] = M[0, 0]; result[0, 1] = M[1, 0]; result[0, 2] = M[2, 0]; result[0, 3] = 0.0f;
            result[1, 0] = M[0, 1]; result[1, 1] = M[1, 1]; result[1, 2] = M[2, 1]; result[1, 3] = 0.0f;
            result[2, 0] = M[0, 2]; result[2, 1] = M[1, 2]; result[2, 2] = M[2, 2]; result[2, 3] = 0.0f;
            result[3, 0] = -(M[3, 0] * result[0, 0] + M[3, 1] * result[1, 0] + M[3, 2] * result[2, 0]);
            result[3, 1] = -(M[3, 0] * result[0, 1] + M[3, 1] * result[1, 1] + M[3, 2] * result[2, 1]);
            result[3, 2] = -(M[3, 0] * result[0, 2] + M[3, 1] * result[1, 2] + M[3, 2] * result[2, 2]);
            result[3, 3] = 1.0f;
            M = result;
        }

        private float GetRowLength(int row)
        {
            var X = M[row, 0];
            var Y = M[row, 1];
            var Z = M[row, 2];
            
            return (float) Math.Sqrt(X * X + Y * Y + Z * Z);
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