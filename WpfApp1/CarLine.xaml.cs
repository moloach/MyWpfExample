using System;
using System.Configuration;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfApp1
{
    /// <summary>
    /// CarLine.xaml 的交互逻辑
    /// </summary>
    public partial class CarLine : Window
    {
        MarkLine rightline = null;
        MarkLine leftline = null;
        Render render = null;

        Configuration config;

        public CarLine()
        {
            InitializeComponent();
            //Left = -1920;
            Left = 0;
            Top = 0;

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.H)
            {
                var isEdit = MessageBox.Show("是否需要编辑车道线？", "询问", MessageBoxButton.OKCancel);
                if (isEdit == MessageBoxResult.OK)
                {
                    if (render != null)
                    {
                        canvas1.Children.Remove(render);
                    }

                    ModifyCarLine();
                    save.Visibility = Visibility.Visible;
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var isEdit = bool.Parse(config.AppSettings.Settings["isEdit"].Value);
            if (isEdit)
            {
                ModifyCarLine();
            }
            else
            {
                RenderCarLine();
            }
        }

        private void RenderCarLine()
        {

            if(render != null)
            {
                canvas1.Children.Remove(render);
            }

            var rightlinedata = config.AppSettings.Settings["rightline"].Value;
            var leftlinedata = config.AppSettings.Settings["leftline"].Value;



            render = new Render(rightlinedata, leftlinedata);

            canvas1.Children.Add(render);
        }

        private void ModifyCarLine()
        {
            var rightlinedata = config.AppSettings.Settings["rightline"].Value;
            var leftlinedata = config.AppSettings.Settings["leftline"].Value;


            rightline = new MarkLine("right", rightlinedata);
            leftline = new MarkLine("left", leftlinedata);

            canvas1.Children.Add(rightline);
            canvas1.Children.Add(leftline);
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {

            var right = rightline.Save();
            var left = leftline.Save();
            SaveToConfig("rightline", right);
            SaveToConfig("leftline", left);

        }

        private void SaveToConfig(string lineType, string value)
        {
            if (config.AppSettings.Settings[lineType] != null)
            {

                config.AppSettings.Settings[lineType].Value = value;
            }
            else
            {
                config.AppSettings.Settings.Add(lineType, value);
            }

            config.Save(ConfigurationSaveMode.Modified);

            canvas1.Children.Remove(rightline);
            canvas1.Children.Remove(leftline);
            save.Visibility = Visibility.Hidden;


            RenderCarLine();
        }
    }
}
