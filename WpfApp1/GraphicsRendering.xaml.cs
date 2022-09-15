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
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// GraphicsRendering.xaml 的交互逻辑
    /// </summary>
    public partial class GraphicsRendering : Window
    {
        public GraphicsRendering()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RetrieveDrawing(this);
        }

        public void RetrieveDrawing(Visual v)
        {
            DrawingGroup drawingGroup = VisualTreeHelper.GetDrawing(v);
            EnumDrawingGroup(drawingGroup);
        }

        private void EnumDrawingGroup(DrawingGroup group)
        {
            if (group == null) return;

            DrawingCollection drawings = group.Children;

            foreach (Drawing item in drawings)
            {
                if (item is DrawingGroup subgroup)
                {
                    EnumDrawingGroup(subgroup);
                }
                else if (item is GeometryDrawing)
                {
                    Console.WriteLine("DrawingGroup");
                }
                else if (item is ImageDrawing)
                {
                    Console.WriteLine("ImageDrawing");
                }
                else if (item is GlyphRunDrawing)
                {
                    Console.WriteLine("GlyphRunDrawing");
                }
                else if (item is VideoDrawing)
                {
                    Console.WriteLine("VideoDrawing");

                }
            }
        }
    }
}
