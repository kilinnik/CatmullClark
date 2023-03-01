using System.Collections.Generic;

namespace лр1
{
    public class Point
    {
        public Point Successor { get; set; }
        public List<Edge> Edges { get; set; }
        public Vector2 Position { get; set; }

        public Point()
        {
            Successor = null;
            Edges = new List<Edge>();
        }

        public Point(Vector2 position)
        {
            Successor = null;
            Edges = new List<Edge>();
            Position = position;
        }
    }
}
