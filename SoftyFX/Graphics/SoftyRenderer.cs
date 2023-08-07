using System.Collections.Generic;
using System.Linq;
using SoftyFX.Mathematics;

namespace SoftyFX.Graphics
{
    public static class SoftyRenderer
    {
        public static void DrawTriangles(IEnumerable<Triangle> triangles, Matrix4x4 transformation, Rgb color, RenderMode drawMode = RenderMode.Lines)
        {
            foreach (var triangle in triangles)
            {
                Triangle transformedTriangle;

                transformedTriangle.A = triangle.A * transformation;
                transformedTriangle.B = triangle.B * transformation;
                transformedTriangle.C = triangle.C * transformation;

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

                DrawPolygon(new []
                {
                    transformedTriangle.A.Xy,
                    transformedTriangle.B.Xy,
                    transformedTriangle.C.Xy
                }, color);
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
            }
        }

        public static void DrawPoint(Vector2Int point, Rgb color)
        {
            SoftyContext.GetWindowSize(out _, out var height);
            SoftyContext.DrawPoint(point.X, height - point.Y, color.R, color.G, color.B);
        }

        public static void DrawLine(Vector2Int a, Vector2Int b, Rgb color)
        {
            SoftyContext.GetWindowSize(out _, out var height);
            SoftyContext.DrawLine(a.X, height - a.Y, b.X, height - b.Y, color.R, color.G, color.B);
        }

        public static void DrawBresenhamLine(Vector2Int a, Vector2Int b, Rgb color)
        {
            SoftyContext.GetWindowSize(out _, out var height);
            SoftyContext.DrawBresenhamLine(a.X, height - a.Y, b.X, height - b.Y, color.R, color.G, color.B);
        }
    }
}