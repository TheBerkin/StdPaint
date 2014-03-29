using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace StdPaint
{
    /// <summary>
    /// Represents a collection of triangles in 3D space.
    /// </summary>
    public class Mesh
    {
        private Triangle3f[] _tris;

        /// <summary>
        /// Creates a new Mesh from a triangle array.
        /// </summary>
        /// <param name="triangles">The triangle array to make the mesh out of.</param>
        public Mesh(Triangle3f[] triangles)
        {
            _tris = triangles;
        }

        /// <summary>
        /// Loads vertex data from an OBJ model file and creates a Mesh object out of it. Almost like magic.
        /// </summary>
        /// <param name="file">The path to the file to load.</param>
        /// <returns></returns>
        public static Mesh FromOBJ(string file)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<Triangle3f> triangles = new List<Triangle3f>();
            using(StreamReader reader = new StreamReader(file))
            {
                Match match = null;
                while(!reader.EndOfStream)
                {
                    string line = reader.ReadLine().Trim();
                    if (line.Length == 0 || line.StartsWith("#"))
                    {
                        continue;
                    }
                    else if (CheckRegex(line, @"v\s+(?<v1>\d+(\.\d+)?)\s+(?<v2>\d+(\.\d+)?)\s+(?<v3>\d+(\.\d+)?)", out match))
                    {
                        vertices.Add(new Vector3(
                            Double.Parse(match.Groups["v1"].Value),
                            Double.Parse(match.Groups["v2"].Value),
                            Double.Parse(match.Groups["v3"].Value)
                            ));
                    }
                    else if (CheckRegex(line, @"f\s+(?<v1>\d+)\s+(?<v2>\d+)\s+(?<v3>\d+)", out match))
                    {
                        triangles.Add(new Triangle3f(
                            vertices[Int32.Parse(match.Groups["v1"].Value) - 1],
                            vertices[Int32.Parse(match.Groups["v2"].Value) - 1],
                            vertices[Int32.Parse(match.Groups["v3"].Value) - 1]
                            ));
                    }
                }
            }
            return new Mesh(triangles.ToArray());
        }

        private static bool CheckRegex(string input, string pattern, out Match match)
        {
            match = Regex.Match(input, pattern, RegexOptions.ExplicitCapture);
            return match.Success;
        }
    }
}
