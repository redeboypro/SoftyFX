using System;

namespace SoftyFX.Mathematics
{
    public struct Vector3
    {
        public static readonly Vector3 Zero = new Vector3(0, 0, 0);
        public static readonly Vector3 One = new Vector3(1, 1, 1);
        public static readonly Vector3 Right = new Vector3(1, 0, 0);
        public static readonly Vector3 Up = new Vector3(0, 1, 0);
        public static readonly Vector3 Front = new Vector3(0, 0, 1);

        public float X;
        public float Y;
        public float Z;

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        
        public Vector3(Vector2Int xy, float z)
        {
            X = xy.X;
            Y = xy.Y;
            Z = z;
        }
        
        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return X;
                    case 1:
                        return Y;
                    case 2:
                        return Z;
                }
                throw new IndexOutOfRangeException();
            }
        }

        public Vector2Int Xy
        {
            get
            {
                return new Vector2Int((int) X, (int) Y);
            }
        }

        public float Magnitude 
        {
            get
            {
                return (float) Math.Sqrt(X * X + Y * Y + Z * Z);
            } 
        }

        public void Normalize()
        {
            var scale = 1.0f / Magnitude;
            X *= scale;
            Y *= scale;
            Z *= scale;
        }

        public static float Dot(Vector3 left, Vector3 right)
        {
            return left.X * right.X + left.Y * right.Y + left.Z * right.Z;
        }
        
        public static Vector3 Cross(Vector3 left, Vector3 right)
        {
            Vector3 result;
            result.X = left.Y * right.Z - left.Z * right.Y;
            result.Y = left.Z * right.X - left.X * right.Z;
            result.Z = left.X * right.Y - left.Y * right.X;
            return result;
        }
        
        public static Vector3 operator +(Vector3 left, Vector3 right)
        {
            left.X += right.X;
            left.Y += right.Y;
            left.Z += right.Z;
            return left;
        }
        
        public static Vector3 operator -(Vector3 left, Vector3 right)
        {
            left.X -= right.X;
            left.Y -= right.Y;
            left.Z -= right.Z;
            return left;
        }
        
        public static Vector3 operator *(Vector3 left, Vector3 right)
        {
            left.X *= right.X;
            left.Y *= right.Y;
            left.Z *= right.Z;
            return left;
        }
        
        public static Vector3 operator *(Vector3 left, Matrix4x4 right)
        {
            Vector3 result;
            result.X = left.X * right.M[0, 0] + left.Y * right.M[1, 0] + left.Z * right.M[2, 0] + right.M[3, 0];
            result.Y = left.X * right.M[0, 1] + left.Y * right.M[1, 1] + left.Z * right.M[2, 1] + right.M[3, 1];
            result.Z = left.X * right.M[0, 2] + left.Y * right.M[1, 2] + left.Z * right.M[2, 2] + right.M[3, 2];
            var w = left.X * right.M[0, 3] + left.Y * right.M[1, 3] + left.Z * right.M[2, 3] + right.M[3, 3];

            if (w == 0.0f)
            {
                return result;
            }
            
            result.X /= w;
            result.Y /= w;
            result.Z /= w;

            return result;
        }
        
        public static Vector3 operator *(Vector3 vector, float factor)
        {
            vector.X *= factor;
            vector.Y *= factor;
            vector.Z *= factor;
            return vector;
        }
        
        public static Vector3 operator /(Vector3 left, Vector3 right)
        {
            left.X /= right.X;
            left.Y /= right.Y;
            left.Z /= right.Z;
            return left;
        }
        
        public static Vector3 operator /(Vector3 vector, float factor)
        {
            vector.X /= factor;
            vector.Y /= factor;
            vector.Z /= factor;
            return vector;
        }
    }
}