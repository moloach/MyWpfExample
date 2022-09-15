using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// ShapeUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class ShapeUserControl : UserControl
    {
        public ShapeUserControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Line mydrawline = new Line();
            //mydrawline.Stroke = Brushes.Black;//外宽颜色，在直线里为线颜色
            mydrawline.Stroke = new SolidColorBrush(Color.FromArgb(0xFF, 0x5B, 0x9B, 0xD5));//自定义颜色则用这句
            mydrawline.StrokeThickness = 3;//线宽度
            mydrawline.Height = 300;
            mydrawline.Width = 400;
            mydrawline.X1 = 100;
            mydrawline.Y1 = 100;
            mydrawline.X2 = 100;
            mydrawline.Y2 = 300;

            DrawCanvas.Children.Add(mydrawline);
        }
    }
}
