using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

namespace лр1
{
    public class Edge
    {
        public List<Point> Points { get; set; }
        public List<Face> Faces { get; set; }
        public Point EdgePoint { get; set; }
        public Vector2 Middle { get; set; }

        public Edge()
        {
            Points = new List<Point>();
            Faces = new List<Face>();
        }
        public Edge(List<Point> points, Vector2 middle)
        {
            Points = points;
            Faces = new List<Face>();
            Middle = middle;
        }
    }
}
