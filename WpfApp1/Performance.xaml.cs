﻿using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Performance.xaml 的交互逻辑
    /// </summary>
    public partial class Performance : Window
    {
        public Performance()
        {
            InitializeComponent();


            Loaded += Performance_Loaded;
        }

        private void Performance_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                var rectangle = new Rectangle
                {
                    Height = 500,
                    Width = 500,
                    Opacity = ((double)i + 40) / 100d,
                    RadiusX = 108,
                    RadiusY = 98,
                    StrokeThickness = 3,
                    Stroke = new SolidColorBrush(Color.FromArgb(255, 75, 75, (byte)(i * 250d / 50d))),
                    RenderTransformOrigin = new Point(0.5, 0.5)
                };
                Root.Children.Add(rectangle);
                var angle = i * 360d / 50d;
                var transform = new RotateTransform
                {
                    Angle = angle
                };

                rectangle.RenderTransform = transform;

                var storyboard = new Storyboard();
                var animation = new DoubleAnimation { Duration = TimeSpan.FromSeconds(1), From = angle, To = angle + 360 };
                storyboard.Children.Add(animation);
                Storyboard.SetTarget(animation, rectangle);
                Storyboard.SetTargetProperty(animation, new PropertyPath("RenderTransform.Angle"));
                storyboard.RepeatBehavior = RepeatBehavior.Forever;
                storyboard.Begin();
            }
        }
    }
}
