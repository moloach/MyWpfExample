using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// VariableCarLine.xaml 的交互逻辑
    /// </summary>
    public partial class VariableCarLine : Window
    {
        List<double> C_x;
        List<double> C_y;
        List<double> C_max_y;
        List<double> C_min_y;

        private Polyline polyline;
        private int multi = 1;

        public VariableCarLine()
        {
            InitializeComponent();

            C_x = new List<double>();
            C_y = new List<double>();
            C_max_y = new List<double>();
            C_min_y = new List<double>();

            polyline = new Polyline()
            {
                Stroke = Brushes.IndianRed,
                StrokeThickness = 4
            };

            Canvas.SetLeft(polyline, 400);
            Canvas.SetTop(polyline, 350);
        }


       

        private void RenderLineB()
        {
            if (multi > 10) multi = 0;
            multi++;

            canvas.Children.Remove(polyline);

            polyline.Points.Clear();
            for (int i = 0; i < 20; i++)
            {
                var point = new Point(i * multi, i * multi);
                polyline.Points.Add(point);
            }

            canvas.Children.Add(polyline);
        }


        private void RenderLineC()
        {
            double angle = direction.Value;

            canvas.Children.Remove(polyline);

            polyline.Points.Clear();
            for (int i = 0; i < 20; i++)
            {
                var point = new Point(i * angle, i * angle);
                polyline.Points.Add(point);
            }

            canvas.Children.Add(polyline);
        }

        private void RenderLine(List<double> origin, List<double> right, List<double> left)
        {

            canvas.Children.Remove(polyline);
            //var rightPath = new Path()
            //{
            //    Stroke = Brushes.LightSlateGray,
            //    StrokeThickness = 1,
            //    Data = Geometry.Parse("M 0,0 A 300,500 45 1 1 0,1 Z")
            //};

            //var leftPath = new Path()
            //{
            //    Stroke = Brushes.LightSlateGray,
            //    StrokeThickness = 1,
            //    Data = Geometry.Parse("M 0,0 A 300,300 45 1 1 0,1 Z")
            //};

            //right

            polyline.Points.Clear();
            for (int i = 0; i < origin.Count; i++)
            {
                var point = new Point(origin[i], right[i]);
                polyline.Points.Add(point);
            }

            //;
            //polyline.Points.Add()

            canvas.Children.Add(polyline);
            //canvas.Children.Add(leftPath);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            Thread calcThread = new Thread(new ThreadStart(() => Render()));
            calcThread.IsBackground = true;
            calcThread.Start();
        }


        private void Render()
        {
            while (true)
            {


                Dispatcher.BeginInvoke((Action)(() =>
                {
                    CalcLineCar();
                    //multi++;
                    //RenderLineA();
                    //RenderLineB();
                    RenderLineC();
                    //RenderLine(C_x, C_min_y, C_max_y);
                }));
                Thread.Sleep(50);
            }
        }



        private void CalcLineCar()
        {


            double angle = direction.Value;


            //double R = ()


            double width = double.Parse(carWidth.Text);
            var length = double.Parse(wheelbase.Text);


            var rad = angle / 180.0 * Math.PI;

            TurnLine(rad, length, width);
        }


        private void TurnLine(double theta, double width, double length)
        {

            double R = (length / Math.Tan(theta) + width / 2) / Math.Cos(theta);
            // 计算车后轮两点
            double R_min = Math.Cos(theta) * R - width;
            //转弯圆心
            double[] center = { 50, 0 };

            for (double i = 0; i < length * 100; i++)
            {
                double x = -i / 100;
                //std::cout << x << std::endl;
                C_x.Add(x);
                C_min_y.Add(Math.Sqrt(R_min * R_min - x * x));
                C_max_y.Add(Math.Sqrt(R_min * R_min - x * x) + width);


            }
        }




        /// <summary>
        /// 作出圆圈
        /// </summary>
        private void DrawCircle(double x, double y, int r, Brush PenColor)
        {
            Path x_Arrow = new Path();//x轴箭头

            x_Arrow.Fill = PenColor;

            PathFigure x_Figure = new PathFigure();
            x_Figure.IsClosed = true;
            x_Figure.StartPoint = new Point(x, y);//路径的起点
            x_Figure.Segments.Add(new ArcSegment(new Point(x, y + r / 2), new Size(2 * r, 2 * r), 1, true, SweepDirection.Counterclockwise, true));


            PathGeometry x_Geometry = new PathGeometry();

            x_Geometry.Figures.Add(x_Figure);

            x_Arrow.Data = x_Geometry;


            Canvas chartCanvas = new Canvas();
            chartCanvas.Children.Add(x_Arrow);
            canvas.Children.Add(chartCanvas);
            //canvas.Children.Add(x_Arrow);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DrawCircle(400, 350, 20, Brushes.Black);
        }
    }
}
