using System.Collections.Generic;
using SoftyFX.Mathematics;

namespace SoftyFX.Graphics.Geometry
{
    public class Mesh
    {
        public Mesh(Triangle[] triangles)
        {
            Triangles = triangles;
        }
        
        public Mesh(IReadOnlyList<Vector3> vertices, IReadOnlyCollection<int> indices)
        {
            Triangles = new Triangle[indices.Count / 3];
            
            var index = 0;
            for (var i = 2; i < indices.Count; i += 3)
            {
                Triangles[index] = new Triangle(vertices[i], vertices[i - 1], vertices[i - 2]);
                index++;
            }
        }

        public Triangle[] Triangles { get; }
    }
}