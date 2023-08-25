using System;
using System.Collections.Generic;
using System.Linq;
using SoftyFX.Mathematics;

namespace SoftyFX.Graphics
{
    public static class SoftyRenderer
    {
        private static readonly bool[] RenderOptions =
        {
            //Back-face culling
            true,
            
            //Depth test
            true,
        };

        private static List<Triangle> _trianglesBuffer;

        public static void InitBuffer()
        {
            _trianglesBuffer = new List<Triangle>();
        }

        public static void Enable(RenderOptions option)
        {
            RenderOptions[(int) option] = true;
        }
        
        public static void Disable(RenderOptions option)
        {
            RenderOptions[(int) option] = false;
        }
        
        public static void DrawTriangles(IEnumerable<Triangle> triangles, Matrix4x4 transformation, Camera camera, RenderMode drawMode = RenderMode.Solid)
        {
            SoftyContext.GetWindowSize(out var width, out var height);
            var isSolid = drawMode is RenderMode.Solid;

            foreach (var triangle in triangles)
            {
                Triangle transformedTriangle;

                transformedTriangle.A = triangle.A * transformation;
                transformedTriangle.B = triangle.B * transformation;
                transformedTriangle.C = triangle.C * transformation;
                transformedTriangle.Color = triangle.Color;

                if (RenderOptions[0] && !isSolid || CullFace(transformedTriangle, camera.From))
                {
                    transformedTriangle.A *= camera.ViewMatrix;
                    transformedTriangle.B *= camera.ViewMatrix;
                    transformedTriangle.C *= camera.ViewMatrix;

                    transformedTriangle.A *= camera.ProjectionMatrix;
                    transformedTriangle.B *= camera.ProjectionMatrix;
                    transformedTriangle.C *= camera.ProjectionMatrix;

                    transformedTriangle.A.X += 1.0f;
                    transformedTriangle.A.Y += 1.0f;

                    transformedTriangle.B.X += 1.0f;
                    transformedTriangle.B.Y += 1.0f;

                    transformedTriangle.C.X += 1.0f;
                    transformedTriangle.C.Y += 1.0f;

                    transformedTriangle.A.X *= 0.5f * width;
                    transformedTriangle.A.Y *= 0.5f * height;

                    transformedTriangle.B.X *= 0.5f * width;
                    transformedTriangle.B.Y *= 0.5f * height;

                    transformedTriangle.C.X *= 0.5f * width;
                    transformedTriangle.C.Y *= 0.5f * height;

                    _trianglesBuffer.Add(transformedTriangle);
                }
            }
            
            if (RenderOptions[1] && isSolid)
            {
                _trianglesBuffer.Sort((triangle1, triangle2) => triangle1.CompareTo(triangle2));
            }
            
            foreach (var triangle in _trianglesBuffer)
            {
                DrawPolygon(new[]
                {
                    triangle.A.XYInt,
                    triangle.B.XYInt,
                    triangle.C.XYInt
                }, triangle.Color, drawMode);
            }
            _trianglesBuffer.Clear();
        }

        private static bool CullFace(Triangle triangle, Vector3 eye)
        {
            var edge1 = triangle[1] - triangle[0];
            var edge2 = triangle[2] - triangle[0];

            var normal = Vector3.Cross(edge1, edge2);
            normal.Normalize();

            var cameraRay = triangle[0] - eye;

            return Vector3.Dot(normal, cameraRay) < 0;
        }

        public static void DrawPolygon(Vector2Int[] vertices, Rgb color, RenderMode drawMode = RenderMode.Lines)
        {
            switch (drawMode)
            {
                case RenderMode.Points:
                    foreach (var vertex in vertices)
                        DrawPoint(vertex, color);
                    break;
                case RenderMode.Lines:
                    for (var i = 1; i < vertices.Length; i++)
                        DrawLine(vertices[i - 1], vertices[i], color);
                    DrawLine(vertices.First(), vertices.Last(), color);
                    break;
                case RenderMode.BresenhamLines:
                    for (var i = 1; i < vertices.Length; i++)
                        DrawBresenhamLine(vertices[i - 1], vertices[i], color);
                    DrawBresenhamLine(vertices.First(), vertices.Last(), color);
                    break;
                case RenderMode.Solid:
                    var count = vertices.Length * 2;
                    var resultVertices = new int[count];
                    var vertexIndex = 0;
                    
                    for (var i = 1; i < count; i += 2)
                    {
                        var vertex = vertices[vertexIndex];
                        resultVertices[i - 1] = vertex.X;
                        resultVertices[i] = vertex.Y;
                        vertexIndex++;
                    }
                    SoftyContext.FillPolygon(resultVertices, resultVertices.Length, color.R, color.G, color.B);
                    break;
            }
        }

        public static void DrawPoint(Vector2Int point, Rgb color)
        {
            SoftyContext.DrawPoint(point.X, point.Y, color.R, color.G, color.B);
        }

        public static void DrawLine(Vector2Int a, Vector2Int b, Rgb color)
        {
            SoftyContext.DrawLine(a.X, a.Y, b.X, b.Y, color.R, color.G, color.B);
        }

        public static void DrawBresenhamLine(Vector2Int a, Vector2Int b, Rgb color)
        {
            SoftyContext.DrawBresenhamLine(a.X, a.Y, b.X, b.Y, color.R, color.G, color.B);
        }
    }
}