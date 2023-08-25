using System;
using SoftyFX.Mathematics;

namespace SoftyFX.Graphics
{
    public struct Triangle : IComparable<Triangle>
    {
        public static readonly Triangle Nullable = new Triangle
        {
            A = Vector3.Zero,
            B = Vector3.Zero,
            C = Vector3.Zero,
        };
        
        public Vector3 A;
        public Vector3 B;
        public Vector3 C;
        public Rgb Color;

        public Triangle(Vector3 a, Vector3 b, Vector3 c)
        {
            A = a;
            B = b;
            C = c;
            Color = Rgb.White;
        }

        public Vector3 this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return A;
                    case 1:
                        return B;
                    case 2:
                        return C;
                }
                throw new IndexOutOfRangeException();
            }
        }

        public int CompareTo(Triangle other)
        {
            var z1 = (A.Z + B.Z + C.Z) / 3.0f;
            var z2 = (other[0].Z + other[1].Z + other[2].Z) / 3.0f;
            return z1 > z2 ? -1 : 1;
        }
    }
}