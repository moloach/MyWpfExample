using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// ItemsControl.xaml 的交互逻辑
    /// </summary>
    public partial class ItemsControl : Window
    {

        Random random = new Random();
        //public ObservableCollection<TodoItem> TodoItems { get; set; } = new ObservableCollection<TodoItem>();
        public ItemsControl()
        {
            InitializeComponent();

            DataContext = GlobalVars.TodoItems;
            icTodoList.ItemsSource = GlobalVars.TodoItems;

        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            List<TodoItem> items = new List<TodoItem>();
            items.Add(new TodoItem() { Title = "Complete this WPF tutorial", Completion = 45 });
            items.Add(new TodoItem() { Title = "Learn C#", Completion = 80 });
            items.Add(new TodoItem() { Title = "Wash the car", Completion = 0 });

            icTodoList.ItemsSource = GlobalVars.TodoItems;


            //AddWindow add = new AddWindow();
            //add.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int num = random.Next(100);


            var mode = new TodoItem
            {
                Title = $"write {num} documents",
                Completion = num
            };
            //TodoItems.Add(mode);
            GlobalVars.TodoItems.Add(mode);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //    if (TodoItems.Count > 0)
            //    {
            //        int index = random.Next(TodoItems.Count);
            //        TodoItems.RemoveAt(index);
            //    }


            if (GlobalVars.TodoItems.Count > 0)
            {
                int index = random.Next(GlobalVars.TodoItems.Count);
                GlobalVars.TodoItems.RemoveAt(index);
            }
        }
    }

    public class TodoItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string title;
        private int completion;

        public string Title
        {
            get => title;
            set
            {
                title = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title)));
            }
        }
        public int Completion
        {
            get => completion; set
            {
                completion = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Completion)));

            }
        }

    }


    static class GlobalVars
    {
        public static ObservableCollection<TodoItem> TodoItems { get; set; } = new ObservableCollection<TodoItem>();

    }
}
