using System.Windows;
using System.Windows.Media.Effects;

namespace WpfApp1
{
    /// <summary>
    /// InnerShadowWindow.xaml 的交互逻辑
    /// </summary>
    public partial class InnerShadowWindow : Window
    {
        public InnerShadowWindow()
        {
            InitializeComponent();
        }

        //refer:https://www.cnblogs.com/dino623/p/inner-shadows-in-wpf.html
        // https://github.com/DinoChan/wpf_design_and_animation_lab/blob/ec0154fdb9f5735e9f6a9528eb9417ab99fbc367/WpfDesignAndAnimationLab/Demos/InnerShadows/VariableSizeInnerShadowDemo.xaml.cs#L13

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ShadowElement.Margin = new Thickness(-e.NewValue);
            ShadowElement.BorderThickness = new Thickness(e.NewValue);
            (ShadowElement.Effect as DropShadowEffect).BlurRadius = e.NewValue * 2;
        }
    }
}
