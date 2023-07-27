using System;
using System.Collections.Generic;
using System.Data;
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


//reference: https://zhuanlan.zhihu.com/p/428371408
namespace WpfApp1
{
    /// <summary>
    /// SimpleDataGridSample.xaml 的交互逻辑
    /// </summary>
    public partial class SimpleDataGridSample : Window
    {
        public SimpleDataGridSample()
        {
            InitializeComponent();

            Random random = new Random();

            //List<List<int>> users = new List<List<int>>();

            //for (int i = 0; i < 10; i++)
            //{
            //    List<int> item = new List<int>();
            //    for (int j = 0; j < 10; j++)
            //    {
            //        item.Add(random.Next(0, 101));
            //    }
            //    users.Add(item);
            //}

            List<User> users = new List<User>();
            users.Add(new User() { Id = 1, Name = "John Doe", Birthday = new DateTime(1971, 7, 23) });
            users.Add(new User() { Id = 2, Name = "Jane Doe", Birthday = new DateTime(1974, 1, 17) });
            users.Add(new User() { Id = 3, Name = "Sammy Doe", Birthday = new DateTime(1991, 9, 2) });

            dgSimple.ItemsSource = users;
        }



        //protected override void OnAutoGeneratingColumn(DataGridAutoGeneratingColumnEventArgs e)
        //{
        //    if (this.ItemsSource.GetType() == typeof(DataView))
        //    {
        //        e.Column = CreateColumn(e.PropertyType, e.PropertyName)
        //    }

        //    base.OnAutoGeneratingColumn(e);

        //}
    }
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Birthday { get; set; }

        public string Details
        {
            get
            {
                return string.Format("{0} was born on {1} and this is a long description of the person.", Name, Birthday.ToLongDateString());
            }
        }
    }
}
