using System.Collections.Generic;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        private static int zindex = 1;
        private static bool isFore = true;
        public MainWindow()
        {
            InitializeComponent();
        }

        public List<AllCarList> ReadAllCarsInfo()
        {
            var results = new List<AllCarList>();

            var theKeys = new List<string>();

            foreach (string key in ConfigurationManager.AppSettings.Keys)
            {
                if (key.StartsWith("car"))
                    theKeys.Add(key);
            }


            for (int i = 1; i < 10; i++)
            {
                var carName = $"car{i}";
                var carSN = $"car{i}SN";
                if (theKeys.Contains(carSN))
                {
                    foreach (string theValue in ConfigurationManager.AppSettings.GetValues(carSN))
                    {
                        results.Add(new AllCarList()
                        {
                            name = carName,
                            sn = theValue
                        });
                    }
                }
            }


            return results;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // text.ItemsSource = ReadAllCarsInfo();
        }

        private void foreSight_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            //if (isFore)
            //{
            //    backSight.SetValue(Grid.ColumnProperty, 0);
            //    backSight.SetValue(Grid.RowProperty, 0);
            //    backSight.SetValue(Grid.ColumnSpanProperty, 2);
            //    backSight.SetValue(Grid.RowSpanProperty, 2);
            //    backSight.Stretch = Stretch.Fill;

            //    foreSight.SetValue(Grid.ColumnProperty, 1);
            //    foreSight.SetValue(Grid.RowSpanProperty, 1);
            //    foreSight.SetValue(Panel.ZIndexProperty, zindex);
            //}
            //else
            //{
            //    foreSight.SetValue(Grid.ColumnProperty, 0);
            //    foreSight.SetValue(Grid.RowProperty, 0);
            //    foreSight.SetValue(Grid.ColumnSpanProperty, 2);
            //    foreSight.SetValue(Grid.RowSpanProperty, 2);
            //    foreSight.Stretch = Stretch.Fill;

            //    backSight.SetValue(Grid.ColumnProperty, 1);
            //    backSight.SetValue(Grid.RowSpanProperty, 1);
            //    backSight.SetValue(Panel.ZIndexProperty, zindex);
            //}
            isFore = !isFore;
            zindex++;
        }
    }

    public class AllCarList
    {
        /// <summary>
        /// RD0000000011
        /// </summary>
        public string sn { get; set; }
        /// <summary>
        /// 中车矿卡1
        /// </summary>
        public string name { get; set; }
    }
}
