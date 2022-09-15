using System.Collections.Generic;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;

namespace WpfApp1
{
    /// <summary>
    /// ListRender.xaml 的交互逻辑
    /// </summary>
    public partial class ListRender : Window
    {
        public ListRender()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var cars = new List<Car>(){
                new Car{ Name = "矿卡1（卡特）", IsOnline = true, Status = "Busy"},
                new Car{ Name = "矿卡2（中环）", IsOnline = true, Status = "Off"},
                new Car{ Name = "矿卡3（潍柴）", IsOnline = false,Status = "Connect"},
                new Car{ Name = "矿卡4（卡特）", IsOnline = true, Status = "Busy"},
                new Car{ Name = "矿卡5（卡特）", IsOnline = true, Status = "Busy"},
                new Car{ Name = "矿卡6（卡特）", IsOnline = true, Status = "Connect"},
                new Car{ Name = "矿卡7（卡特）", IsOnline = true, Status = "Connect"},
                new Car{ Name = "矿卡8（卡特）", IsOnline = true, Status = "Off"},
                new Car{ Name = "矿卡9（卡特）", IsOnline = true, Status = "Busy"},
                new Car{ Name = "矿卡10（卡特）", IsOnline = true, Status = "Off"},
                new Car{ Name = "矿卡11（卡特）", IsOnline = true, Status = "Off"},
                new Car{ Name = "矿卡12（卡特）", IsOnline = true, Status = "Connect"},
                new Car{ Name = "矿卡13（卡特）", IsOnline = true, Status = "Off"},

            };

            CarListView.ItemsSource = cars;

            
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ListViewAutomationPeer view = new ListViewAutomationPeer(CarListView);
            var swap = view.GetPattern(PatternInterface.Scroll) as ScrollViewerAutomationPeer;
            ScrollViewer scroll = swap.Owner as ScrollViewer;
            if (scroll.ContentVerticalOffset + scroll.ViewportHeight == scroll.ExtentHeight)
            {
                scroll.ScrollToEnd();
            }
            //scroll.ScrollToEnd();
            //scroll.ScrollToVerticalOffset();
            scroll.ScrollToVerticalOffset(scroll.ContentVerticalOffset + 1);
        }
    }

    public class Car
    {
        public string Name { get; set; }
        public bool IsOnline { get; set; }
        public string Status { get; set; }
    }
}
