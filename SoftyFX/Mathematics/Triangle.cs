﻿using System;

namespace SoftyFX.Mathematics
{
    public struct Triangle
    {
        public Vector3 A;
        public Vector3 B;
        public Vector3 C;

        public Triangle(Vector3 a, Vector3 b, Vector3 c)
        {
            A = a;
            B = b;
            C = c;
        }
    }
}