using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// CarLineBack.xaml 的交互逻辑
    /// </summary>
    public partial class CarLineBack : Window
    {

        RenderBack render = null;

        Configuration config;


        public CarLineBack()
        {
            InitializeComponent();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            RenderCarLine();
        }

        private void RenderCarLine()
        {

            if (render != null)
            {
                canvas1.Children.Remove(render);
            }

            var rightlinedata = config.AppSettings.Settings["rightline"].Value;
            var leftlinedata = config.AppSettings.Settings["leftline"].Value;



            render = new RenderBack(rightlinedata, leftlinedata);

            canvas1.Children.Add(render);
        }


        private void TurningAnimation()
        {
            

        }

    }
}
