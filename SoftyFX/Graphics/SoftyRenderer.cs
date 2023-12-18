using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using SoftyFX.Graphics.Geometry;
using SoftyFX.Graphics.Shaders;
using SoftyFX.Graphics.World;
using SoftyFX.Mathematics;

namespace SoftyFX.Graphics
{
    public static class SoftyRenderer
    {
        private static readonly Vector3 _offsetView = new Vector3(1, 1, 0);
        
        private static readonly bool[] RenderOptions =
        {
            //Back-face culling
            true,
            
            //Depth test
            true,
        };

        private static List<Shader> _shaderBuffer;
        private static List<RenderData> _inBuffer;
        private static List<Triangle> _outBuffer;
        private static Camera _camera;

        public static void Enable(RenderOptions option)
        {
            RenderOptions[(int) option] = true;
        }
        
        public static void Disable(RenderOptions option)
        {
            RenderOptions[(int) option] = false;
        }
        
        public static void SetCamera(Camera camera)
        {
            _camera = camera;
        }

        public static void GenBuffers()
        {
            _shaderBuffer = new List<Shader>();
            _inBuffer = new List<RenderData>();
            _outBuffer = new List<Triangle>();
        }
        
        public static void ClearBuffer()
        {
            _inBuffer.Clear();
        }
        
        public static void Prepare(Mesh mesh, int shaderId)
        {
            _inBuffer.Add(new RenderData(mesh, shaderId));
        }

        public static int BindShader(Shader shader)
        {
            _shaderBuffer.Add(shader);
            return _shaderBuffer.Count - 1;
        }

        public static void UpdateTriangleBuffer()
        {
            var memoryBuffer = new List<Triangle>();
            
            SoftyContext.GetWindowSize(out var width, out var height);

            foreach (var data in _inBuffer)
            {
                var triangles = data.Mesh.Triangles;

                foreach (var triangle in triangles)
                {
                    Triangle transformedTriangle;

                    transformedTriangle.A = _shaderBuffer[data.ShaderId].Vertex(triangle.A);
                    transformedTriangle.B = _shaderBuffer[data.ShaderId].Vertex(triangle.B);
                    transformedTriangle.C = _shaderBuffer[data.ShaderId].Vertex(triangle.C);
                    transformedTriangle.Color = triangle.Color;

                    if (RenderOptions[0] && CullFace(transformedTriangle, _camera.From))
                    {
                        transformedTriangle.A *= _camera.ViewMatrix;
                        transformedTriangle.B *= _camera.ViewMatrix;
                        transformedTriangle.C *= _camera.ViewMatrix;
                        
                        var clippedTriangles = new Triangle[2];
                        var clippedTriangleCount = Clip(Vector3.Front, Vector3.Front, transformedTriangle,
                            out clippedTriangles[0], out clippedTriangles[1]);

                        for (var i = 0; i < clippedTriangleCount; i++)
                        {
                            var projectedTriangle = transformedTriangle;
                            projectedTriangle.A *= _camera.ProjectionMatrix;
                            projectedTriangle.B *= _camera.ProjectionMatrix;
                            projectedTriangle.C *= _camera.ProjectionMatrix;

                            projectedTriangle.A.X *= -1.0f;
                            projectedTriangle.A.Y *= -1.0f;

                            projectedTriangle.B.X *= -1.0f;
                            projectedTriangle.B.Y *= -1.0f;

                            projectedTriangle.C.X *= -1.0f;
                            projectedTriangle.C.Y *= -1.0f;
                            
                            projectedTriangle.A += _offsetView;
                            projectedTriangle.B += _offsetView;
                            projectedTriangle.C += _offsetView;

                            projectedTriangle.A.X *= 0.5f * width;
                            projectedTriangle.A.Y *= 0.5f * height;

                            projectedTriangle.B.X *= 0.5f * width;
                            projectedTriangle.B.Y *= 0.5f * height;

                            projectedTriangle.C.X *= 0.5f * width;
                            projectedTriangle.C.Y *= 0.5f * height;

                            memoryBuffer.Add(projectedTriangle);
                        }
                    }
                }
            }

            _outBuffer = memoryBuffer;
        }

        private static Vector3 IntersectPlane(Vector3 lineStart, Vector3 lineEnd, Vector3 planeCenter, Vector3 planeNormal)
        {
            planeNormal.Normalize();
            var d = -Vector3.Dot(planeNormal, planeCenter);
            var ad = Vector3.Dot(lineStart, planeNormal);
            var bd = Vector3.Dot(lineEnd, planeNormal);
            var t = (-d - ad) / (bd - ad);
            var lineStartToEnd = lineEnd - lineStart;
            var lineToIntersect = lineStartToEnd * t;
            return lineStart + lineToIntersect;
        }

        private static int Clip(Vector3 planeCenter, Vector3 planeNormal, Triangle inTriangle, out Triangle outTriangle1, out Triangle outTriangle2)
        {
            outTriangle1 = default;
            outTriangle2 = default;
            
            planeNormal.Normalize();
            
            var insidePoints = new Vector3[3];
            var outsidePoints = new Vector3[3];

            var insidePointCount = 0;
            var outsidePointCount = 0;

            var distance0 = GetShortestDistance(inTriangle[0], planeCenter, planeNormal);
            var distance1 = GetShortestDistance(inTriangle[1], planeCenter, planeNormal);
            var distance2 = GetShortestDistance(inTriangle[2], planeCenter, planeNormal);

            if (distance0 >= 0)
            {
                insidePoints[insidePointCount++] = inTriangle[0];
            }
            else
            {
                outsidePoints[outsidePointCount++] = inTriangle[0];
            }
            
            if (distance1 >= 0)
            {
                insidePoints[insidePointCount++] = inTriangle[1];
            }
            else
            {
                outsidePoints[outsidePointCount++] = inTriangle[1];
            }
            
            if (distance2 >= 0)
            {
                insidePoints[insidePointCount++] = inTriangle[2];
            }
            else
            {
                outsidePoints[outsidePointCount++] = inTriangle[2];
            }

            if (insidePointCount == 0)
            {
                return 0;
            }
            
            if (insidePointCount == 3)
            {
                outTriangle1 = inTriangle;
                return 1;
            }

            if (insidePointCount == 1 && outsidePointCount == 2)
            {
                outTriangle1.A = insidePoints[0];
                outTriangle1.B = IntersectPlane(insidePoints[0], outsidePoints[0], planeCenter, planeNormal);
                outTriangle1.C = IntersectPlane(insidePoints[0], outsidePoints[1], planeCenter, planeNormal);
                return 1;
            }
            
            if (insidePointCount == 2 && outsidePointCount == 1)
            {
                outTriangle1.A = insidePoints[0];
                outTriangle1.B = insidePoints[1];
                outTriangle1.C = IntersectPlane(insidePoints[0], outsidePoints[0], planeCenter, planeNormal);
                
                outTriangle2.A = insidePoints[1];
                outTriangle2.B = outTriangle1[2];
                outTriangle2.C = IntersectPlane(insidePoints[1], outsidePoints[0], planeCenter, planeNormal);
                return 2;
            }

            return 0;
        }

        private static float GetShortestDistance(Vector3 vertex, Vector3 planeCenter, Vector3 planeNormal)
        {
            return (planeNormal.X * vertex.X + planeNormal.Y * vertex.Y + planeNormal.Z * vertex.Z -
                    Vector3.Dot(planeNormal, planeCenter));
        }
        
        public static void DrawBuffer(int width, int height, RenderMode drawMode = RenderMode.Solid)
        {
            var memoryBuffer = _outBuffer.ToList();
            
            if (RenderOptions[1])
            {
                memoryBuffer.Sort((triangle1, triangle2) => triangle1.CompareTo(triangle2));
            }

            foreach (var triangle in memoryBuffer)
            {
                var clipped = new Triangle[2];
                var resultTriangles = new List<Triangle> {triangle};

                var newTriangles = 1;

                for (var p = 0; p < 4; p++)
                {
                    var trianglesToAdd = 0;
                    while (newTriangles > 0)
                    {
                        var test = resultTriangles[0];
                        resultTriangles.Remove(test);
                        newTriangles--;

                        switch (p)
                        {
                            case 0:
                                trianglesToAdd = Clip(Vector3.Zero, Vector3.Up, test, out clipped[0], out clipped[1]);
                                break;
                            case 1:
                                trianglesToAdd = Clip(Vector3.Up * (height - 1), Vector3.Down, test, out clipped[0], out clipped[1]);
                                break;
                            case 2:
                                trianglesToAdd = Clip(Vector3.Zero, Vector3.Right, test, out clipped[0], out clipped[1]);
                                break;
                            case 3:
                                trianglesToAdd = Clip(Vector3.Right * (width - 1), Vector3.Left, test, out clipped[0], out clipped[1]);
                                break;
                        }

                        for (var w = 0; w < trianglesToAdd; w++)
                        {
                            resultTriangles.Add(clipped[w]);
                        }
                    }

                    newTriangles = resultTriangles.Count;
                }

                foreach (var triangle2 in resultTriangles)
                {
                    DrawPolygon(new[]
                    {
                        triangle2.A.XYInt,
                        triangle2.B.XYInt,
                        triangle2.C.XYInt
                    }, triangle.Color, drawMode);
                }
            }
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

        public static void DrawPolygon(Vector2Int[] vertices, Rgb color, RenderMode drawMode = RenderMode.Solid)
        {
            switch (drawMode)
            {
                case RenderMode.Points:
                    DrawPolygonPoints(vertices, color);
                    break;
                
                case RenderMode.Lines:
                    DrawPolygonLines(vertices, color);
                    break;

                case RenderMode.Solid:
                    DrawPolygonSolidColor(vertices, color);
                    break;
            }
        }

        public static void DrawPolygon(Polygon2D polygon, RenderMode drawMode = RenderMode.Solid)
        {
            DrawPolygon(polygon.Vertices, polygon.GetColor(), drawMode);
        }

        private static void DrawPolygonPoints(IEnumerable<Vector2Int> vertices, Rgb color)
        {
            foreach (var vertex in vertices)
            {
                DrawPoint(vertex, color);
            }
        }
        
        private static void DrawPolygonLines(IReadOnlyList<Vector2Int> vertices, Rgb color)
        {
            for (var i = 1; i < vertices.Count; i++)
            {
                DrawLine(vertices[i - 1], vertices[i], color);
            }

            DrawLine(vertices.First(), vertices.Last(), color);
        }

        private static void DrawPolygonSolidColor(IReadOnlyList<Vector2Int> vertices, Rgb color)
        {
            var count = vertices.Count * 2;
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
        
        public static void DrawText(string text, Vector2Int min, Vector2Int max, Rgb color)
        {
            SoftyContext.DrawTextBlock(text, min.X, min.Y, max.X, max.Y, color.R, color.G, color.B);
        }
        
        public static void DrawPolyText(string text, Rgb textColor, Polygon2D polygon2D, RenderMode polygonDrawMode = RenderMode.Solid)
        {
            DrawPolygon(polygon2D, polygonDrawMode);
            DrawText(text, polygon2D.Min, polygon2D.Max, textColor);
        }
        
        private readonly struct RenderData
        {
            public RenderData(Mesh mesh, int shaderId)
            {
                Mesh = mesh;
                ShaderId = shaderId;
            }

            public Mesh Mesh { get; }

            public int ShaderId { get; }
        }
    }
}