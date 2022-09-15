using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace WpfApp1
{
    /// <summary>
    /// LineUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class MarkLine : UserControl
    {
        private Point point1, point2;
        // static double left_gradient;//斜线斜率
        //private static double right_gradient;

        public MarkLine(string typename, string pointsData)
        {
            InitializeComponent();

            ReadPointsFromValue(pointsData);
        }

        private void ReadPointsFromValue(string pointsValue)
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

            point1 = result[1];
            point2 = result[2];

            DataPoints = new PointCollection()
            {
                result[0],
                result[3],
            };
        }


        #region 端点数据集

        public PointCollection DataPoints
        {
            get => (PointCollection)GetValue(DataPointsProperty);
            set => SetValue(DataPointsProperty, value);
        }

        // Using a DependencyProperty as the backing store for DataPoints.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataPointsProperty =
            DependencyProperty.Register("DataPoints", typeof(PointCollection), typeof(MarkLine), new PropertyMetadata(new PointCollection() { new Point(4, 4), new Point(8, 8) }, DataPointsChanged));

        private static void DataPointsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MarkLine line = (MarkLine)d;
            if (e.NewValue != e.OldValue)
            {
                //数据集改变时重新绘制线段和设置端点圆形位置
                PointCollection points = (PointCollection)e.NewValue;


                Canvas.SetTop(line.thmStart, points[0].Y - 4);
                Canvas.SetLeft(line.thmStart, points[0].X - 4);
                Canvas.SetTop(line.thmEnd, points[1].Y - 4);
                Canvas.SetLeft(line.thmEnd, points[1].X - 8);

                //set point1 and point2
                Point startPoint = points[0];
                Point endPoint = points[1];
                var gradient = (double)(endPoint.Y - startPoint.Y) / (endPoint.X - startPoint.X);


                var point1x = startPoint.X + (endPoint.X - startPoint.X) / 3;
                double point1y = gradient * (point1x - startPoint.X) + startPoint.Y;
                Canvas.SetLeft(line.thmPoint1, point1x - 8);
                Canvas.SetTop(line.thmPoint1, point1y - 4);

                var point2x = startPoint.X + ((endPoint.X - startPoint.X) / 3 * 2);
                double point2y = gradient * (point2x - startPoint.X) + startPoint.Y;
                Canvas.SetLeft(line.thmPoint2, point2x - 8);
                Canvas.SetTop(line.thmPoint2, point2y - 4);

            }
        }

        #endregion

        private PointCollection _points;// = new PointCollection();

        private void thmStart_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Thumb thm = (Thumb)sender;
            double y = Canvas.GetTop(thm) + e.VerticalChange;
            double x = Canvas.GetLeft(thm) + e.HorizontalChange;

            ////if (x > DataPoints[1].X) x = DataPoints[1].X;
            ////if (y < DataPoints[1].Y) y = DataPoints[1].Y;

            Canvas.SetTop(thm, Canvas.GetTop(thm) + e.VerticalChange);
            Canvas.SetLeft(thm, Canvas.GetLeft(thm) + e.HorizontalChange);
            _points = DataPoints;
            DataPoints = new PointCollection()
            {
                new Point(x+thm.ActualWidth/2,y+thm.ActualHeight/2),
                _points[1],
            };
        }

        private void thmEnd_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Thumb thm = (Thumb)sender;
            double y = Canvas.GetTop(thm) + e.VerticalChange + 8;
            double x = Canvas.GetLeft(thm) + e.HorizontalChange + 8;

            //if (x < DataPoints[0].X) x = DataPoints[0].X;
            //if (y > DataPoints[0].Y) y = DataPoints[0].Y;


            Canvas.SetTop(thm, Canvas.GetTop(thm) + e.VerticalChange);
            Canvas.SetLeft(thm, Canvas.GetLeft(thm) + e.HorizontalChange);
            _points = DataPoints;
            DataPoints = new PointCollection()
            {
                _points[0],
                new Point(x+ (thm.ActualWidth/2),y + (thm.ActualHeight  / 2))
            };
        }

        private void thmPoint1_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Thumb thm = (Thumb)sender;
            Point startPoint = DataPoints[0];
            Point endPoint = DataPoints[1];

            //Point endPoint = point2 == null ? DataPoints[1] : point2;

            double x = Canvas.GetLeft(thm) + e.HorizontalChange;
            if (endPoint.X > startPoint.X)
            {
                if (x > endPoint.X) x = endPoint.X;
                if (x < startPoint.X) x = startPoint.X;
            }
            else
            {
                if (x < endPoint.X) x = endPoint.X;
                if (x > startPoint.X) x = startPoint.X;
            }
            var gradient = (double)(endPoint.Y - startPoint.Y) / (endPoint.X - startPoint.X);

            double y = gradient * (x - startPoint.X) + startPoint.Y;

            Canvas.SetTop(thm, y - 4);
            Canvas.SetLeft(thm, x - 8);
            _points = DataPoints;

            point1 = new Point { X = x, Y = y };
        }

        private void thmPoint2_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Thumb thm = (Thumb)sender;
            //Point startPoint = point1 == null ? DataPoints[0] : point1;
            Point startPoint = DataPoints[0];
            Point endPoint = DataPoints[1];

            double x = Canvas.GetLeft(thm) + e.HorizontalChange;
            if (endPoint.X > startPoint.X)
            {
                if (x > endPoint.X) x = endPoint.X;
                if (x < startPoint.X) x = startPoint.X;
            }
            else
            {
                if (x < endPoint.X) x = endPoint.X;
                if (x > startPoint.X) x = startPoint.X;
            }
            var gradient = (double)(endPoint.Y - startPoint.Y) / (endPoint.X - startPoint.X);

            double y = (gradient * (x - startPoint.X)) + startPoint.Y;

            Canvas.SetTop(thm, y - 4);
            Canvas.SetLeft(thm, x - 8);


            point2 = new Point { X = x, Y = y };

        }

        public string Save()
        {
            var result = new List<Point>();

            result.Add(DataPoints[0]);
            if (point1.X == 0 && point1.Y == 0)
            {
                var startPoint = DataPoints[0];
                var endPoint = DataPoints[1];
                var point1x = startPoint.X + (endPoint.X - startPoint.X) / 3;

                var gradient = (double)(endPoint.Y - startPoint.Y) / (endPoint.X - startPoint.X);
                double point1y = gradient * (point1x - startPoint.X) + startPoint.Y;

                result.Add(new Point { Y = point1y, X = point1x });
            }
            else
            {
                result.Add(point1);
            }
            if (point2.X == 0 && point2.Y == 0)
            {
                var startPoint = DataPoints[0];
                var endPoint = DataPoints[1];
                var point1x = startPoint.X + (endPoint.X - startPoint.X) / 3 * 2;

                var gradient = (double)(endPoint.Y - startPoint.Y) / (endPoint.X - startPoint.X);
                double point1y = gradient * (point1x - startPoint.X) + startPoint.Y;

                result.Add(new Point { Y = point1y, X = point1x });
            }
            else
            {
                result.Add(point2);
            }
            result.Add(DataPoints[1]);

            StringBuilder sb = new StringBuilder();
            foreach (var item in result)
            {
                sb.Append($"{item.X},{item.Y};");
            }

            return sb.ToString();

        }
    }
}
