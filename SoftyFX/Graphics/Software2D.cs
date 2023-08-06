using System.Linq;
using SoftyFX.Mathematics;

namespace SoftyFX.Graphics
{
    public static class Software2D
    {
        public static void DrawPolygon(Vector2Int[] vertices, Rgb color, DrawMode drawMode = DrawMode.Lines)
        {
            switch (drawMode)
            {
                case DrawMode.Points:
                    foreach (var vertex in vertices)
                        DrawPoint(vertex, color);
                    break;
                case DrawMode.Lines:
                    for (var i = 1; i < vertices.Length; i++)
                        DrawLine(vertices[i - 1], vertices[i], color);
                    DrawLine(vertices.First(), vertices.Last(), color);
                    break;
                case DrawMode.BresenhamLines:
                    for (var i = 1; i < vertices.Length; i++)
                        DrawBresenhamLine(vertices[i - 1], vertices[i], color);
                    DrawBresenhamLine(vertices.First(), vertices.Last(), color);
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