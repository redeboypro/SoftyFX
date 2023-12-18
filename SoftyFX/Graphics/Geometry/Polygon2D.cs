using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using SoftyFX.Mathematics;

namespace SoftyFX.Graphics.Geometry
{
    public struct Polygon2D
    {
        private Rgb _color;
        private Vector2Int[] _vertices;
        private Vector2Int _min;
        private Vector2Int _max;

        public Polygon2D(Vector2Int[] vertices, Rgb color)
        {
            _color = color;
            _vertices = vertices;
            _min = Vector2Int.Zero;
            _max = Vector2Int.Zero;
            RecalculateBounds();
        }

        public Vector2Int Min
        {
            get
            {
                return _min;
            }
        }
        
        public Vector2Int Max
        {
            get
            {
                return _max;
            }
        }

        public Vector2Int[] Vertices
        {
            get
            {
                return _vertices;
            }
            set
            {
                _vertices = value;
                RecalculateBounds();
            }
        }

        public Rgb GetColor()
        {
            return _color;
        }
        
        public void GetColor(out byte r, out byte g, out byte b)
        {
            r = _color.R;
            g = _color.G;
            b = _color.B;
        }
        
        public void SetColor(Rgb color)
        {
            _color = color;
        }
        
        public void SetColor(byte r, byte g, byte b)
        {
            _color.R = r;
            _color.G = g;
            _color.B = b;
        }

        private void RecalculateBounds()
        {
            var minX = int.MaxValue;
            var minY = int.MaxValue;
            
            var maxX = int.MinValue;
            var maxY = int.MinValue;
            
            foreach (var vertex in _vertices)
            {
                var x = vertex.X;
                var y = vertex.Y;

                if (x < minX)
                {
                    minX = x;
                }
                
                if (y < minY)
                {
                    minY = y;
                }
                
                if (x > maxX)
                {
                    maxX = x;
                }
                
                if (y > maxY)
                {
                    maxY = y;
                }
            }

            _min.X = minX;
            _min.Y = minY;
            _max.X = maxX;
            _max.Y = maxY;
        }

        public static Polygon2D Rectangle(int left, int bottom, int right, int top, Rgb color)
        {
            return new Polygon2D(new[]
            {
                new Vector2Int(left, bottom),
                new Vector2Int(left, top),
                new Vector2Int(right, top),
                new Vector2Int(right, bottom)
            }, color);
        }
        
        public static Polygon2D Circle(Vector2Int center, int radius, int segmentCount, Rgb color)
        {
            var vertices = new List<Vector2Int>();

            for (var i = 0; i < segmentCount; i++)
            {
                var theta = 2.0 * Math.PI * i / segmentCount;
                Vector2Int step;
                step.X = (int) (radius * Math.Cos(theta));
                step.Y = (int) (radius * Math.Sin(theta));
                vertices.Add(center + step);
            }
            
            return new Polygon2D(vertices.ToArray(), color);
        }
    }
}