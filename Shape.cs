using System.Collections.Generic;
namespace лр1
{
    public class Shape
    {
        public List<Face> Faces { get; set; }
        public List<Edge> AllEdges { get; set; }
        public List<Point> AllPoints { get; set; }

        public Shape()
        {
            Faces = new List<Face>();
            AllEdges = new List<Edge>();
            AllPoints = new List<Point>();
        }
    }
}
