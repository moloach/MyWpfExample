using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace WpfApp1
{
    /// <summary>
    /// ScrollViewer.xaml 的交互逻辑
    /// </summary>
    public partial class Marquee : Window
    {
        DispatcherTimer timer;
        ScrollViewer sv1;
        double offset = 0;

        public Marquee()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            offset++;
            sv1.ScrollToVerticalOffset(offset);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {

            sv1 = VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(this.dg, 0), 0) as ScrollViewer;
            sv1.ScrollChanged += Sv1_ScrollChanged;
            List<string> list = new List<string>();
            for (int i = 0; i < 20; i++)
            {
                list.Add("aa" + i.ToString());
            }
            dg.ItemsSource = list;
            timer.Start();
        }

        private void Sv1_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.VerticalOffset + e.ViewportHeight == e.ExtentHeight && e.ViewportHeight != 0)
            {
                offset = 1;
            }
        }
    }
}
