using System.Collections.Generic;
using System.Globalization;
using System.IO;
using SoftyFX.Graphics;
using SoftyFX.Mathematics;

namespace SoftyFX.Common
{
    public static class Wavefront
    {
        private const string VertexPrefix = "v ";
        private const string FacePrefix = "f ";
        
        public static void Import(string fileName, out Triangle[] triangles)
        {
            var vertexList = new List<Vector3>();
            var triangleList = new List<Triangle>();

            using (var reader = new StreamReader(fileName))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    if (line == null)
                    {
                        continue;
                    }

                    var data = line.Split(' ');

                    if (line.StartsWith(VertexPrefix))
                    {
                        var x = float.Parse(data[1], NumberStyles.Any, CultureInfo.InvariantCulture);
                        var y = float.Parse(data[2], NumberStyles.Any, CultureInfo.InvariantCulture);
                        var z = float.Parse(data[3], NumberStyles.Any, CultureInfo.InvariantCulture);
                        
                        vertexList.Add(new Vector3(x, y, z));
                    }

                    if (!line.StartsWith(FacePrefix))
                    {
                        continue;
                    }
                    
                    var a = int.Parse(data[1]) - 1;
                    var b = int.Parse(data[2]) - 1;
                    var c = int.Parse(data[3]) - 1;
                    
                    triangleList.Add(new Triangle(vertexList[a], vertexList[b], vertexList[c]));
                }
            }

            triangles = triangleList.ToArray();
        }
    }
}