using System;
using System.Collections.Generic;
using System.Linq;
using SoftyFX.Mathematics;

namespace SoftyFX.Graphics
{
    public static class SoftyRenderer
    {
        public static void DrawTriangles(IEnumerable<Triangle> triangles, Vector3 eye, Matrix4x4 transformation, Matrix4x4 projection, Rgb color, RenderMode drawMode = RenderMode.Lines)
        {
            var depthSortedTriangles = new List<Triangle>();
            
            foreach (var triangle in triangles)
            {
                Triangle transformedTriangle;

                transformedTriangle.A = triangle.A * transformation;
                transformedTriangle.B = triangle.B * transformation;
                transformedTriangle.C = triangle.C * transformation;
                
                var edge1 = transformedTriangle[1] - transformedTriangle[0];
                var edge2 = transformedTriangle[2] - transformedTriangle[0];
                
                Vector3 normal;
                normal.X = edge1.Y * edge2.Z - edge1.Z * edge2.Y;
                normal.Y = edge1.Z * edge2.X - edge1.X * edge2.Z;
                normal.Z = edge1.X * edge2.Y - edge1.Y * edge2.X;

                var length = (float) Math.Sqrt(normal.X * normal.X + normal.Y * normal.Y + normal.Z * normal.Z);
                normal /= length;

                if (normal.X * (transformedTriangle.A.X - eye.X) +
                    normal.Y * (transformedTriangle.A.Y - eye.Y) +
                    normal.Z * (transformedTriangle.A.Z - eye.Z) < 0)
                {
                    transformedTriangle.A *= projection;
                    transformedTriangle.B *= projection;
                    transformedTriangle.C *= projection;
                    
                    transformedTriangle.A.X += 1.0f;
                    transformedTriangle.A.Y += 1.0f;

                    transformedTriangle.B.X += 1.0f;
                    transformedTriangle.B.Y += 1.0f;

                    transformedTriangle.C.X += 1.0f;
                    transformedTriangle.C.Y += 1.0f;

                    SoftyContext.GetWindowSize(out var width, out var height);

                    transformedTriangle.A.X *= 0.5f * width;
                    transformedTriangle.A.Y *= 0.5f * height;

                    transformedTriangle.B.X *= 0.5f * width;
                    transformedTriangle.B.Y *= 0.5f * height;

                    transformedTriangle.C.X *= 0.5f * width;
                    transformedTriangle.C.Y *= 0.5f * height;
                    
                    depthSortedTriangles.Add(transformedTriangle);
                }
            }
            
            depthSortedTriangles.Sort((triangle1, triangle2) => triangle1.CompareTo(triangle2));

            foreach (var triangle in depthSortedTriangles)
            {
                DrawPolygon(new[]
                {
                    triangle.A.Xy,
                    triangle.B.Xy,
                    triangle.C.Xy
                }, color, drawMode);
            }
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