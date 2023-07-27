using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfApp1
{
    //reference: https://blog.csdn.net/desperaso/article/details/105616868
    public partial class TraceLine : Window
    {
        private TextBox[] input = new TextBox[2];

        private Storyboard story_circle, story_arc, story_x, story_y;
        private LineGeometry line;

        private double delay = 8;
        private double partition = 2;

        private TranslateTransform translate;
        private Polyline polyline;

        private Thread thread;
        private Button[] button = new Button[2];

        private bool threadRunFlag;

        public TraceLine()
        {
            InitializeComponent();

            polyline = new Polyline()
            {
                Stroke = Brushes.Red,
                StrokeThickness = 1
            };
            Canvas.SetLeft(polyline, 700);
            Canvas.SetTop(polyline, 350);

            Draw_Init(mainBox);

            string[] btn_text = new string[] { "开始", "清除" };
            string[] input_text = new string[] { "时间系数（毫秒）：", "绘制速率（毫秒）：" };
            for (int i = 0; i < 2; i++)
            {
                button[i] = new Button()
                {
                    Width = 80,
                    Height = 25,
                    Content = btn_text[i],
                    Margin = new Thickness(0, 0, 850, 550 - i * 60),
                    Tag = i
                };
                button[i].Click += Button_Click;
                mainGrid.Children.Add(button[i]);

                TextBlock label = new TextBlock()
                {
                    Text = input_text[i],
                    Margin = new Thickness(10, 200 + i * 45, 850, 450 - i * 45),
                };
                mainGrid.Children.Add(label);

                input[i] = new TextBox()
                {
                    Width = 80,
                    Height = 20,
                    Margin = new Thickness(10, 200 + i * 40, 850, 450 - i * 40),
                };
                input[i].Text = i == 0 ? "2" : "8";
                mainGrid.Children.Add(input[i]);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button pL = (Button)sender;

            polyline.Points.Clear();

            if ((int)pL.Tag == 0)
            {
                if (string.IsNullOrEmpty(input[0].Text))
                {
                    MessageBox.Show("时间系数 必须输入");
                    return;
                }
                if (string.IsNullOrEmpty(input[1].Text))
                {
                    MessageBox.Show("绘制速率 必须输入");
                    return;
                }
                try
                {
                    partition = int.Parse(input[0].Text);
                }
                catch
                {
                    MessageBox.Show("时间系数 输入错误");
                    return;
                }
                try
                {
                    delay = int.Parse(input[1].Text);
                }
                catch
                {
                    MessageBox.Show("绘制速率 输入错误");
                    return;
                }

                if (partition <= 0)
                {
                    MessageBox.Show("时间系数 必须大于0");
                    return;
                }
                if (delay <= 0)
                {
                    MessageBox.Show("绘制速率 必须大于0");
                    return;
                }

                mainBox.Children.Clear();
                mainBox.UnregisterName("Arc_Move");
                mainBox.UnregisterName("MyTranslateTransform");
                mainBox.UnregisterName("Line_Move");

                Draw_Init(mainBox);

                button[0].IsEnabled = false;
                button[1].IsEnabled = false;
                thread = new Thread(Modify_Draw);
                thread.IsBackground = true;
                threadRunFlag = true;
                thread.Start();

                story_circle.Begin(this, true);
                story_arc.Begin(this, true);
                story_x.Begin(this, true);
                story_y.Begin(this, true);

            }
        }

        private void Modify_Draw()
        {
            while (threadRunFlag)
            {
                this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
                {
                    polyline.Points.Add(new Point(translate.X + line.EndPoint.X - 100, translate.Y + line.EndPoint.Y - 50));
                });
                Thread.Sleep(5);
            }
        }

        public void Draw_Init(Canvas obj)
        {
            Canvas box_view = new Canvas()
            {
                Width = 800,
                Height = 600,
            };
            Canvas.SetLeft(box_view, 100);
            Canvas.SetTop(box_view, 50);
            obj.Children.Add(box_view);

            //Draw_Line(box_view, new Point(100, 300), new Point(700, 300), Colors.LightSlateGray, 1);
            //Draw_Line(box_view, new Point(400, 0), new Point(400, 600), Colors.LightSlateGray, 1);

            for (int i = 0; i < 7; i++)
            {
                //Draw_Line(obj, new Point(200 + i * 100, 350 - 5), new Point(200 + i * 100, 350 + 5), Colors.Black, 1);
                //Draw_Line(obj, new Point(495, 50 + i * 100), new Point(505, 50 + i * 100), Colors.Black, 1);

                string nx = (i - 3) != 0 ? (i - 3) + "x" : "0";
                string ny = (3 - i) != 0 ? (3 - i) + "y" : "";

                TextBlock num_x = new TextBlock()
                {
                    FontSize = 12,
                    FontWeight = FontWeights.Bold,
                    FontStyle = FontStyles.Oblique,
                    Text = nx,
                    Opacity = 0.2
                };
                Canvas.SetLeft(num_x, 195 + i * 100);
                Canvas.SetTop(num_x, 355);
                obj.Children.Add(num_x);

                TextBlock num_y = new TextBlock()
                {
                    FontSize = 12,
                    FontWeight = FontWeights.Bold,
                    FontStyle = FontStyles.Oblique,
                    Text = ny,
                    Opacity = 0.2
                };
                Canvas.SetLeft(num_y, 505);
                Canvas.SetTop(num_y, 45 + i * 100);
                obj.Children.Add(num_y);
            }
            obj.Children.Add(polyline);

            Path path_border = new Path()
            {
                Stroke = Brushes.LightSlateGray,
                StrokeThickness = 1,
                Data = Geometry.Parse("M 0,0 A 300,300 45 1 1 0,1 Z")
            };
            Canvas.SetLeft(path_border, 100);
            Canvas.SetTop(path_border, 300);
            box_view.Children.Add(path_border);
            Path path_box = new Path()
            {
                StrokeThickness = 1,
                Data = Geometry.Parse("M 0,0 A 200,200 45 1 0 0,1 Z")
            };

            Canvas.SetLeft(path_box, 600);
            Canvas.SetTop(path_box, 300);
            box_view.Children.Add(path_box);

            Canvas move_block = new Canvas()
            {
                Width = 100,
                Height = 100
            };
            Canvas.SetLeft(move_block, 150);
            Canvas.SetTop(move_block, 250);
            box_view.Children.Add(move_block);

            Path arc_centre = new Path()
            {
                Fill = Brushes.Black,
                Data = Geometry.Parse("M 0, 0 A 5, 5 45 1 1 0, 1 Z")
            };
            Canvas.SetLeft(arc_centre, 445);
            Canvas.SetTop(arc_centre, 50);
            move_block.Children.Add(arc_centre);

            Path arc_move = new Path()
            {
                Fill = Brushes.Black
            };
            EllipseGeometry arc_elips = new EllipseGeometry()
            {
                Center = new Point(200, 50),
                RadiusX = 5,
                RadiusY = 5,
            };
            arc_move.Data = arc_elips;
            Canvas.SetLeft(arc_move, 350);
            Canvas.SetTop(arc_move, 0);
            obj.RegisterName("Arc_Move", arc_elips);
            move_block.Children.Add(arc_move);

            Path path_center = new Path()
            {
                Stroke = Brushes.Blue,
                StrokeThickness = 1,
                Data = Geometry.Parse("M 200,50 A 100,100 45 1 1 200,49 Z")
            };
            Canvas.SetLeft(path_center, 350);
            Canvas.SetTop(path_center, 0);
            move_block.Children.Add(path_center);

            translate = new TranslateTransform()
            {
                X = 0,
                Y = 0
            };
            obj.RegisterName("MyTranslateTransform", translate);
            TransformGroup group = new TransformGroup();
            group.Children.Add(translate);

            move_block.RenderTransform = group;
            story_x = new Storyboard();
            story_y = new Storyboard();
            PathGeometry geometry = new PathGeometry()
            {
                Figures = PathFigureCollection.Parse("M 0,0 A 200,200 45 1 0 0,1 Z")
            };
            Set_AnimationPath(geometry, "MyTranslateTransform", "X", PathAnimationSource.X, story_x);
            Set_AnimationPath(geometry, "MyTranslateTransform", "Y", PathAnimationSource.Y, story_y);

            line = new LineGeometry()
            {
                StartPoint = new Point(100, 50),
                EndPoint = new Point(200, 50)
            };
            Path path_line = new Path()
            {
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                Data = line
            };
            Canvas.SetLeft(path_line, 350);
            Canvas.SetTop(path_line, 0);
            obj.RegisterName("Line_Move", line);
            move_block.Children.Add(path_line);

            story_circle = new Storyboard();
            story_arc = new Storyboard();
            PathGeometry pathGeometry = new PathGeometry()
            {
                Figures = PathFigureCollection.Parse("M 200,50 A 100,100 45 1 1 200,49 Z")
            };
            Set_Locus(obj, pathGeometry, "Line_Move", "EndPoint", story_circle);
            Set_Locus(obj, pathGeometry, "Arc_Move", "Center", story_arc);
        }

        private void Set_Locus(Canvas obj, PathGeometry geo, string targetname, string value, Storyboard storyboard)
        {
            PointAnimationUsingPath pointAnimationUsingPath = new PointAnimationUsingPath()
            {
                Duration = new Duration(TimeSpan.FromSeconds(delay / partition)),
                RepeatBehavior = RepeatBehavior.Forever,
            };
            PathGeometry pathGeometry = geo;
            pointAnimationUsingPath.PathGeometry = pathGeometry;

            Storyboard.SetTargetName(pointAnimationUsingPath, targetname);
            Storyboard.SetTargetProperty(pointAnimationUsingPath, new PropertyPath(value));

            storyboard.Children.Add(pointAnimationUsingPath);
            storyboard.Begin(obj, true);
            storyboard.Pause(obj);
        }

        private void Set_AnimationPath(PathGeometry geo, string targetname, string value, PathAnimationSource source, Storyboard storyboard)
        {
            DoubleAnimationUsingPath usingPath = new DoubleAnimationUsingPath()
            {
                Duration = new Duration(TimeSpan.FromSeconds(delay)),
                Source = source,
            };
            usingPath.PathGeometry = geo;

            Storyboard.SetTargetName(usingPath, targetname);
            Storyboard.SetTargetProperty(usingPath, new PropertyPath(value));

            storyboard.Children.Add(usingPath);
            storyboard.Completed += Story_Completed;
        }

        private void Story_Completed(object sender, EventArgs e)
        {
            Thread.Sleep(10);

            story_circle.Stop(this);
            story_arc.Stop(this);
            threadRunFlag = false;

            button[0].IsEnabled = true;
            button[1].IsEnabled = true;
        }


        /// <summary>
        /// 坐标线
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <param name="stroke"></param>
        /// <param name="thickness"></param>
        private void Draw_Line(Canvas obj, Point begin, Point end, Color stroke, double thickness)
        {
            LineGeometry line = new LineGeometry()
            {
                StartPoint = begin,
                EndPoint = end
            };
            Path line_path = new Path() { Stroke = new SolidColorBrush(stroke), StrokeThickness = thickness, Data = line };
            obj.Children.Add(line_path);
        }
    }
}
