using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Render.xaml 的交互逻辑
    /// </summary>
    public partial class RenderBack : UserControl
    {

        PointCollection rightPoints, leftPoints;

        public RenderBack(string rightLineValue, string leftLineValue)
        {
            InitializeComponent();


            rightPoints = ReadPointsFromValue(rightLineValue);
            leftPoints = ReadPointsFromValue(leftLineValue);
        }


        private PointCollection ReadPointsFromValue(string pointsValue)
        {
            var result = new PointCollection();

            var points = pointsValue.Split(';');
            foreach (var pointstring in points)
            {
                if (pointstring != "")
                {
                    var thisPoint = pointstring.Split(',');
                    var point = new Point(double.Parse(thisPoint[0]), double.Parse(thisPoint[1]));
                    result.Add(point);
                }
            }

            return result;
        }

        private void RenderLine(Point start, Point end, Color color, bool isRight)
        {
            Path path = new Path();
            GeometryGroup group = new GeometryGroup();

            LineGeometry line = new LineGeometry
            {
                StartPoint = start,
                EndPoint = end
            };


            PathFigureCollection figures = new PathFigureCollection();
            PathFigure figure = new PathFigure();
            figure.StartPoint = start;


            PathSegmentCollection pathSegments = new PathSegmentCollection();

            //ArcSegment arc = new ArcSegment()
            //{
            //    Point = end,
            //    Size = new Size { Height = 200, Width = 50 },
            //    SweepDirection = SweepDirection.Counterclockwise,
            //    IsLargeArc = true
            //};
            //pathSegments.Add(arc);

            QuadraticBezierSegment bezier = new QuadraticBezierSegment()
            {
                Point1 = new Point((start.X + end.X), (start.Y + end.Y)),
                Point2 = end,
            };
            pathSegments.Add(bezier);

            figure.Segments = pathSegments;
            figures.Add(figure);

            PathGeometry pathGeometry = new PathGeometry();
            pathGeometry.Figures = figures;




            RectangleGeometry rect = new RectangleGeometry();
            Rect rectg;
            if (isRight)
            {
                rectg = new Rect(end.X - 9, end.Y + 2, 9, 3);
            }
            else
            {

                rectg = new Rect(end.X, end.Y + 2, 9, 3);
            }
            rect.Rect = rectg;

            group.Children.Add(pathGeometry);
            group.Children.Add(rect);

            path.Data = group;
            path.StrokeThickness = 4;

            SolidColorBrush brush = new SolidColorBrush
            {
                Color = color
            };
            path.Stroke = brush;


            cvs.Children.Add(path);
        }


        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            //var rightPointstart = new Point(1370, 786);
            //var rightPointend = new Point(1370, 95);
            //var leftPointstart = new Point(733, 786);
            //var leftPointend = new Point(733, 95);
            //RenderLine(rightPointstart, rightPointend, Colors.Red, true);

            RenderLine(rightPoints[0], rightPoints[3], Colors.Yellow, true);
            //RenderLine(rightPoints[1], rightPoints[2], Colors.Yellow, true);
            //RenderLine(rightPoints[2], rightPoints[3], Colors.Green, true);

            RenderLine(leftPoints[0], leftPoints[3], Colors.Yellow, false);
            //RenderLine(leftPointstart, leftPointend, Colors.Red, false);
            //RenderLine(leftPoints[1], leftPoints[2], Colors.Yellow, false);
            //RenderLine(leftPoints[2], leftPoints[3], Colors.Green, false);

        }
    }
}
