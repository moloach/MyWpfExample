using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Render.xaml 的交互逻辑
    /// </summary>
    public partial class Render : UserControl
    {

        PointCollection rightPoints, leftPoints;

        public Render(string rightLineValue, string leftLineValue)
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
            /*
             * <Path Stroke="Green" StrokeThickness="4">
            <Path.Data>
                <GeometryGroup>
                    <LineGeometry StartPoint="100,380" EndPoint="400,80" />
                    <RectangleGeometry Rect="400,80 ,9,3" />
                </GeometryGroup>
            </Path.Data>
        </Path>*/

            Path path = new Path();

            GeometryGroup group = new GeometryGroup();

            LineGeometry line = new LineGeometry
            {
                StartPoint = start,
                EndPoint = end
            };

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

            group.Children.Add(line);
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
            RenderLine(rightPoints[0], rightPoints[1], Colors.Red, true);
            RenderLine(rightPoints[1], rightPoints[2], Colors.Yellow, true);
            RenderLine(rightPoints[2], rightPoints[3], Colors.Green, true);

            RenderLine(leftPoints[0], leftPoints[1], Colors.Red, false);
            RenderLine(leftPoints[1], leftPoints[2], Colors.Yellow, false);
            RenderLine(leftPoints[2], leftPoints[3], Colors.Green, false);

        }
    }
}
