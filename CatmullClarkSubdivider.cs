using System.Collections;
using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;
using System.Windows.Documents;

namespace лр1
{
    public class CatmullClarkSubdivider
    {
        public static Shape Subdivide(Shape shape)
        {
            //фигура
            Shape subdivided = new();

            //для каждой грани создается точка грани, которая является средним значением всех точек грани.
            CreateFacePoints(shape);

            //для каждого ребра создается точка ребра, которая является средним между центром ребра и центром сегмента, образованного точками двух соседних граней.
            CreateEdgePoints(shape);

            // для каждой точки вершины ее координаты обновляются из (new_coords): старых координат (old_coords), среднего значения точек граней, 
            //к которым принадлежит точка (avg_face_points), среднее значение центров рёбер, к которым принадлежит точка (avg_mid_edges), скольким граням принадлежит точка (n)
            CreateVertexPoints(shape);

            //создание граней
            CreateFaces(shape, subdivided);

            return subdivided;
        }
        //для каждой грани создается точка грани, которая является средним значением всех точек грани.
        private static void CreateFacePoints(Shape shape)
        {
            foreach (Face face in shape.Faces)
            {
                List<Point> points = face.AllPoints;
                face.FacePoint = new Point(Average(points));
            }
        }
        //для каждого ребра создается точка ребра, которая является средним между центром ребра и центром сегмента, образованного точками двух соседних граней.
        private static void CreateEdgePoints(Shape shape)
        {
            List<Edge> edges = shape.AllEdges;
            foreach (Edge edge in edges)
            {
                var pos = Average(edge.Points[0], edge.Points[1], edge.Faces[0].FacePoint, edge.Faces[1].FacePoint);
                edge.EdgePoint = new Point(pos);
            }
        }
        // для каждой точки вершины ее координаты обновляются из (new_coords): старых координат (old_coords), среднего значения точек граней, 
        //к которым принадлежит точка (avg_face_points), среднее значение центров рёбер, к которым принадлежит точка (avg_mid_edges), скольким граням принадлежит точка (n)
        private static void CreateVertexPoints(Shape shape)
        {
            List<Point> allPoints = shape.AllPoints;

            foreach (Point oldPoint in allPoints)
            {
                oldPoint.Successor = CreateVertexPoint(oldPoint);
            }
        }
        //создание точки вершины
        private static Point CreateVertexPoint(Point oldPoint)
        {
            // среднее значение центров рёбер, к которым принадлежит точка 
            Vector2 avgMidEdges = Vector2.Average(oldPoint.Edges.Select(e => e.Middle).ToList());

            // среднее значение точек граней, к которым принадлежит точка 
            List<Face> pointFaces = oldPoint.Edges.SelectMany(e => e.Faces).Distinct().ToList();
            Vector2 avgFacePoints = Average(pointFaces.Select(pf => pf.FacePoint).ToList());

            int faceCount = pointFaces.Count;

            double m1 = (faceCount - 3) / faceCount;
            double m2 = 1 / faceCount;
            double m3 = 2 / faceCount;

            Point newPoint = new(m1 * oldPoint.Position + m2 * avgFacePoints + m3 * avgMidEdges);
            return newPoint;
        }
        //создание граней
        private static void CreateFaces(Shape shape, Shape subdivided)
        {
            List<Face> faces = shape.Faces;
            List<Edge> existingEdges = new();
            foreach (Face face in faces)
            {
                if (face.AllPoints.Count == 3)
                {
                    CreateTriangleFace(existingEdges, subdivided, face);
                }
                else if (face.AllPoints.Count == 4)
                {
                    CreateQuadFace(existingEdges, subdivided, face);
                }
                else
                {
                    throw new InvalidOperationException(string.Format("Unhandled facetype (point count={0})!", face.AllPoints.Count));
                }
            }
        }

        private static void CreateTriangleFace(List<Edge> existingEdges, Shape subdivided, Face face)
        {
            List<Point> points = face.AllPoints;
            Point a = points[0].Successor;
            Point b = points[1].Successor;
            Point c = points[2].Successor;

            Point facePoint = face.FacePoint;

            subdivided.Faces.Add(CreateFace(existingEdges, new List<Point> { a, face.Edges[0].EdgePoint, facePoint, face.Edges[2].EdgePoint }));
            subdivided.Faces.Add(CreateFace(existingEdges, new List<Point> { b, face.Edges[1].EdgePoint, facePoint, face.Edges[0].EdgePoint }));
            subdivided.Faces.Add(CreateFace(existingEdges, new List<Point> { c, face.Edges[2].EdgePoint, facePoint, face.Edges[1].EdgePoint }));
        }

        private static void CreateQuadFace(List<Edge> existingEdges, Shape subdivided, Face face)
        {
            List<Point> points = face.AllPoints;
            Point a = points[0].Successor;
            Point b = points[1].Successor;
            Point c = points[2].Successor;
            Point d = points[3].Successor;

            Point facePoint = face.FacePoint;

            subdivided.Faces.Add(CreateFace(existingEdges, new List<Point> { a, face.Edges[0].EdgePoint, facePoint, face.Edges[3].EdgePoint }));
            subdivided.Faces.Add(CreateFace(existingEdges, new List<Point> { b, face.Edges[1].EdgePoint, facePoint, face.Edges[0].EdgePoint }));
            subdivided.Faces.Add(CreateFace(existingEdges, new List<Point> { c, face.Edges[2].EdgePoint, facePoint, face.Edges[1].EdgePoint }));
            subdivided.Faces.Add(CreateFace(existingEdges, new List<Point> { d, face.Edges[3].EdgePoint, facePoint, face.Edges[2].EdgePoint }));
        }

        private static Vector2 Average(List<Point> points)
        {
            return Vector2.Average(points.Select(p => p.Position).ToList());
        }

        private static Vector2 Average(params Point[] points)
        {
            return Vector2.Average(points.Select(p => p.Position).ToList());
        }

        public static Face CreateFace(List<Edge> existingEdges, List<Point> allPoints)
        {
            List<Edge> allEdges = new();
            Edge edge;
            for (int i = 0; i < 3; i++)
            {
                edge = existingEdges.FirstOrDefault(edge => edge.Points[0] == allPoints[3] && edge.Points[1] == allPoints[0]);
                if (edge == null) allEdges.Add(new Edge() { Points = new List<Point> { allPoints[i], allPoints[i + 1] }, Middle = Vector2.Average(new List<Vector2> { allPoints[i].Position, allPoints[i + 1].Position }) });
                else allEdges.Add(edge);
            }
            edge = existingEdges.FirstOrDefault(edge => edge.Points[0] == allPoints[3] && edge.Points[1] == allPoints[0]);
            if (edge == null) allEdges.Add(new Edge() { Points = new List<Point> { allPoints[3], allPoints[0] }, Middle = Vector2.Average(new List<Vector2> { allPoints[3].Position, allPoints[0].Position }) });
            else allEdges.Add(edge);

            Face newFace = new()
            {
                AllPoints = allPoints,
                Edges = allEdges
            };
            return newFace;
        }
    }
}
