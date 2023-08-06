using System.Collections.Generic;
using SoftyFX.Mathematics;

namespace SoftyFX.Graphics
{
    public static class Software3D
    {
        public static void DrawTriangles(IEnumerable<Triangle> triangles, Matrix4x4 transformation, Rgb color, DrawMode drawMode = DrawMode.Lines)
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

                Software2D.DrawPolygon(new []
                {
                    transformedTriangle.A.Xy,
                    transformedTriangle.B.Xy,
                    transformedTriangle.C.Xy
                }, color);
            }
        }
    }
}