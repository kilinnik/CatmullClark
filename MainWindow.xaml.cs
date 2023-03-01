using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;


namespace лр1
{
    public partial class MainWindow : Window
    {
        Canvas canvas;

        public MainWindow()
        {
            InitializeComponent();
            canvas = new()
            {
                Width = ActualWidth,
                Height = ActualHeight
            };
            Content = canvas;
            List<Point> allPoints = new()
            {
                new Point(new Vector2(0, 0)),
                new Point(new Vector2(100, -50)),
                new Point(new Vector2(200, 0)),
                new Point(new Vector2(0, 100)),
                new Point(new Vector2(200, 100)),
                new Point(new Vector2(100, 150)),
                new Point(new Vector2(100, 50))
            };

            List<Edge> allEdges = new()
            {
                new Edge(new List<Point>(){ allPoints[0], allPoints[1] }, new Vector2(50, -25)),
                new Edge(new List<Point>(){ allPoints[1], allPoints[2] }, new Vector2(150, -25)),
                new Edge(new List<Point>(){ allPoints[0], allPoints[3] }, new Vector2(0, 50)),
                new Edge(new List<Point>(){ allPoints[2], allPoints[4] }, new Vector2(200, 50)),
                new Edge(new List<Point>(){ allPoints[3], allPoints[5] }, new Vector2(50, 125)),
                new Edge(new List<Point>(){ allPoints[4], allPoints[5] }, new Vector2(150, 125)),
                new Edge(new List<Point>(){ allPoints[1], allPoints[6] }, new Vector2(100, 0)),
                new Edge(new List<Point>(){ allPoints[3], allPoints[6] }, new Vector2(50, 75)),
                new Edge(new List<Point>(){ allPoints[4], allPoints[6] }, new Vector2(150, 75)),
                new Edge(new List<Point>(){ allPoints[0], allPoints[6] }, new Vector2(50, 25)),
                new Edge(new List<Point>(){ allPoints[2], allPoints[6] }, new Vector2(150, 25)),
                new Edge(new List<Point>(){ allPoints[6], allPoints[5] }, new Vector2(100, 100))
            };
            List<Face> faces = new()
            {
                new Face(new List<Point>(){ allPoints[3], allPoints[5], allPoints[4], allPoints[6] },
                         new List<Edge>(){ allEdges[4], allEdges[5], allEdges[7], allEdges[8], }),
                new Face(new List<Point>(){ allPoints[0], allPoints[1], allPoints[6], allPoints[3] },
                         new List<Edge>(){ allEdges[0], allEdges[2], allEdges[6], allEdges[7], }),
                new Face(new List<Point>(){ allPoints[1], allPoints[2], allPoints[4], allPoints[6] },
                         new List<Edge>(){ allEdges[1], allEdges[3], allEdges[6], allEdges[8], }),

                new Face(new List<Point>(){ allPoints[0], allPoints[3], allPoints[5], allPoints[6] },
                         new List<Edge>(){ allEdges[2], allEdges[4], allEdges[9], allEdges[11], }),
                new Face(new List<Point>(){ allPoints[2], allPoints[4], allPoints[5], allPoints[6] },
                         new List<Edge>(){ allEdges[3], allEdges[5], allEdges[10], allEdges[11], }),
                new Face(new List<Point>(){ allPoints[0], allPoints[1], allPoints[2], allPoints[6] },
                         new List<Edge>(){ allEdges[0], allEdges[1], allEdges[9], allEdges[10], })
            };
            for (int i = 0; i < allPoints.Count; i++)
            {
                allPoints[i].Edges = allEdges.Where(x => x.Points.Contains(allPoints[i])).ToList();
            }
            for (int i = 0; i < allEdges.Count; i++)
            {
                allEdges[i].Faces = faces.Where(x => x.Edges.Contains(allEdges[i])).ToList();
            }
            Shape shape = new()
            {
                AllPoints = allPoints,
                AllEdges = allEdges,
                Faces = faces
            };
            foreach (Face face in shape.Faces)
            {
                DrawLine(face.AllPoints[0].Position.X, face.AllPoints[0].Position.Y, face.AllPoints[1].Position.X, face.AllPoints[1].Position.Y, Brushes.Green);
                DrawLine(face.AllPoints[1].Position.X, face.AllPoints[1].Position.Y, face.AllPoints[2].Position.X, face.AllPoints[2].Position.Y, Brushes.Green);
                DrawLine(face.AllPoints[2].Position.X, face.AllPoints[2].Position.Y, face.AllPoints[3].Position.X, face.AllPoints[3].Position.Y, Brushes.Green);
                DrawLine(face.AllPoints[3].Position.X, face.AllPoints[3].Position.Y, face.AllPoints[0].Position.X, face.AllPoints[0].Position.Y, Brushes.Green);
            }
            shape = CatmullClarkSubdivider.Subdivide(shape);
            foreach (Face face in shape.Faces)
            {
                DrawLine(face.AllPoints[0].Position.X, face.AllPoints[0].Position.Y, face.AllPoints[1].Position.X, face.AllPoints[1].Position.Y, Brushes.Green);
                DrawLine(face.AllPoints[1].Position.X, face.AllPoints[1].Position.Y, face.AllPoints[2].Position.X, face.AllPoints[2].Position.Y, Brushes.Green);
                DrawLine(face.AllPoints[2].Position.X, face.AllPoints[2].Position.Y, face.AllPoints[3].Position.X, face.AllPoints[3].Position.Y, Brushes.Green);
                DrawLine(face.AllPoints[3].Position.X, face.AllPoints[3].Position.Y, face.AllPoints[0].Position.X, face.AllPoints[0].Position.Y, Brushes.Green);
            }
        }

        private void DrawLine(double x1, double y1, double x2, double y2, SolidColorBrush color)
        {
            int ix1 = (int)Math.Round(x1);
            int ix2 = (int)Math.Round(x2);
            int iy1 = (int)Math.Round(y1);
            int iy2 = (int)Math.Round(y2);

            int deltaX = Math.Abs(ix1 - ix2);
            int deltaY = Math.Abs(iy1 - iy2);

            int length = Math.Max(deltaX, deltaY);

            if (length == 0)
            {
                Ellipse ellipse = new()
                {
                    Width = 2,
                    Height = 2,
                    Fill = color
                };

                Canvas.SetLeft(ellipse, ix1);
                Canvas.SetTop(ellipse, iy1);
                canvas.Children.Add(ellipse);
                return;
            }

            double dX = (x2 - x1) / length;
            double dY = (y2 - y1) / length;

            double x = x1;
            double y = y1;


            for (int i = 0; i <= length; i++)
            {
                Ellipse ellipse = new()
                {
                    Width = 2,
                    Height = 2,
                    Fill = color
                };

                Canvas.SetLeft(ellipse, Math.Round(x));
                Canvas.SetBottom(ellipse, Math.Round(y));
                canvas.Children.Add(ellipse);

                x += dX;
                y += dY;
            }
        }

    }
}
