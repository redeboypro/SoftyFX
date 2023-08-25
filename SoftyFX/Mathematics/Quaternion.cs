using System;

namespace SoftyFX.Mathematics
{
    public struct Quaternion
    {
        public static readonly Quaternion Identity = new Quaternion(Vector3.Zero);
        
        private Vector3 _xyz;
        private float _w;

        public Quaternion(Vector3 xyz, float w = 1)
        {
            _xyz = xyz;
            _w = w;
        }
        
        public Quaternion(float x, float y, float z, float w) : this(new Vector3(x, y, z), w) { }

        public Vector3 XYZ
        {
            get
            {
                return XYZ;
            }
            set
            {
                XYZ = value;
            }
        }
        
        public float X
        {
            get
            {
                return _xyz.X;
            }
            set
            {
                _xyz.X = value;
            }
        }
        
        public float Y
        {
            get
            {
                return _xyz.Y;
            }
            set
            {
                _xyz.Y = value;
            }
        }
        
        public float Z
        {
            get
            {
                return _xyz.Z;
            }
            set
            {
                _xyz.Z = value;
            }
        }

        public float W
        {
            get
            {
                return _w;
            }
            set
            {
                _w = value;
            }
        }
        
        public float Magnitude
        {
            get
            {
                return (float) Math.Sqrt(W * W + _xyz.MagnitudeSquared);
            }
        }
        
        public void Normalize()
        {
            var scale = 1.0f / Magnitude;
            _xyz *= scale;
            W *= scale;
        }
        
        public void Conjugate()
        {
            _xyz = -_xyz;
        }

        public Vector3 ToEulerAngles()
        {
            const float edge = 0.4995f;
            
            var squaredW = W * W;
            var squaredX = X * X;
            var squaredY = Y * Y;
            var squaredZ = Z * Z;
            
            var unit = squaredX + squaredY + squaredZ + squaredW;
            var test = X * W - Y * Z;
            
            Vector3 resultAngles;

            if (test > edge * unit)
            {
                resultAngles.Y = 2.0f * (float) Math.Atan2(Y, X);
                resultAngles.X = (float) Math.PI / 2.0f;
                resultAngles.Z = 0.0f;
                return resultAngles;
            }

            if (test < -edge * unit)
            {
                resultAngles.Y = -2.0f * (float) Math.Atan2(Y, X);
                resultAngles.X = (float) -Math.PI / 2.0f;
                resultAngles.Z = 0.0f;
                return resultAngles;
            }

            resultAngles.Y = (float) Math.Atan2(
                2.0f * X * W + 2.0f * Y * Z,
                1.0f - 2.0f * (Z * Z + W * W));

            resultAngles.X =
                (float) Math.Asin(2.0f * (X * Z - W * Y));

            resultAngles.Z = (float) Math.Atan2(
                2.0f * X * Y + 2.0f * Z * W,
                1.0f - 2.0f * (Y * Y + Z * Z));

            return resultAngles;
        }
        
        public static Quaternion FromEulerAngles(Vector3 eulerAngles)
        {
            var cos1 = (float) Math.Cos(eulerAngles.X * 0.5f);
            var cos2 = (float) Math.Cos(eulerAngles.Y * 0.5f);
            var cos3 = (float) Math.Cos(eulerAngles.Z * 0.5f);
            
            var sin1 = (float) Math.Sin(eulerAngles.X * 0.5f);
            var sin2 = (float) Math.Sin(eulerAngles.Y * 0.5f);
            var sin3 = (float) Math.Sin(eulerAngles.Z * 0.5f);

            var quaternion = new Quaternion
            {
                W = cos1 * cos2 * cos3 - sin1 * sin2 * sin3,
                XYZ = new Vector3
                {
                    X = sin1 * cos2 * cos3 + cos1 * sin2 * sin3,
                    Y = cos1 * sin2 * cos3 - sin1 * cos2 * sin3,
                    Z = cos1 * cos2 * sin3 + sin1 * sin2 * cos3
                }
            };
            return quaternion;
        }

        public static Quaternion operator +(Quaternion left, Quaternion right)
        {
            left.XYZ += right.XYZ;
            left.W += right.W;
            return left;
        }
        
        public static Quaternion operator -(Quaternion left, Quaternion right)
        {
            left.XYZ -= right.XYZ;
            left.W -= right.W;
            return left;
        }
        
        public static Quaternion operator *(Quaternion left, Quaternion right)
        {
            return new Quaternion(left.XYZ * right.W + left.XYZ * left.W + Vector3.Cross(left.XYZ, right.XYZ),
                left.W * right.W - Vector3.Dot(left.XYZ, right.XYZ));
        }
    }
}