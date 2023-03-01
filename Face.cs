using System.Collections.Generic;
using System.Drawing;

namespace лр1
{
    public class Face
    {
        public List<Point> AllPoints { get; set; }
        public List<Edge> Edges { get; set; }
        public Point FacePoint { get; set; }

        public Face()
        {
            AllPoints = new List<Point>();
            Edges = new List<Edge>();
        }
        public Face(List<Point> points, List<Edge> edges)
        {
            AllPoints = points;
            Edges = edges;
        }
        public Face(List<Point> points)
        {
            AllPoints = points;
        }
    }
}
