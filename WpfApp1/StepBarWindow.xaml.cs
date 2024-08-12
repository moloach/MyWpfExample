using System.Collections.ObjectModel;
using System.Windows;

namespace WpfApp1
{
    /// <summary>
    /// StepBarWindow.xaml 的交互逻辑
    /// </summary>
    public partial class StepBarWindow : Window
    {
        public StepBarWindow()
        {
            InitializeComponent();


            list.Add("已完成");
            list.Add("进行中");
            list.Add("已完成");
            list.Add("进行中");

            this.stepBar1.ItemsSource = list;
            this.text.DataContext = Step;
        }


        private int _Step;

        public int Step
        {
            get
            {
                int temp = this.stepBar1.Progress;
                return ++temp;
            }
        }

        ObservableCollection<string> list = new ObservableCollection<string>();
        private void FlatButton_Click(object sender, RoutedEventArgs e)
        {
            this.stepBar1.Progress++;
            this.text.DataContext = Step;
        }

        private void FlatButton_Click1(object sender, RoutedEventArgs e)
        {
            this.stepBar1.Progress--;
            this.text.DataContext = Step;
        }

        private void btn_AddItem(object sender, RoutedEventArgs e)
        {
            list.Add("进行中");
        }

        private void btn_RemoveItem(object sender, RoutedEventArgs e)
        {
            list.RemoveAt(0);
            this.text.DataContext = Step;
        }
    }
}
