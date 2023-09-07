using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    /// <summary>
    /// InitWindow.xaml 的交互逻辑
    /// </summary>
    public partial class InitWindow : Window
    {
        public InitWindow()
        {
            InitializeComponent();
        }

        private void Show_Click(object sender, RoutedEventArgs e)
        {
            string name = ((Button)sender).Name;
            NewWindow(name);
        }

        private void NewWindow(string className)
        {
            if (CheckExist(className))
            {
                MessageBox.Show("已经打开相同窗口", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Window window;
            if (className == "AllInOne")
            {
                window = new AllInOne();
            }
            else if (className == "ButtonWindow")
            {
                window = new ButtonWindow();
            }
            else if (className == "CarLine")
            {
                window = new CarLine();
            }
            else if (className == "CarLineBack")
            {
                window = new CarLineBack();
            }
            else if (className == "ClinderProgress")
            {
                window = new ClinderProgress();
            }
            else if (className == "GraphicsRendering")
            {
                window = new GraphicsRendering();
            }
            else if (className == "LearningWPF")
            {
                window = new LearningWPF();
            }
            else if (className == "ListRender")
            {
                window = new ListRender();
            }
            else if (className == "MainWindow")
            {
                window = new MainWindow();
            }
            else if (className == "Marquee")
            {
                window = new Marquee();
            }
            else if (className == "Performance")
            {
                window = new Performance();
            }
            else if (className == "_3DWindow")
            {
                window = new _3DWindow();
            }
            else if (className == "VariableCarLine")
            {
                window = new VariableCarLine();
            }
            else if (className == "TraceLine")
            {
                window = new TraceLine();
            }
            else if (className == "SimpleDataGridSample")
            {
                window = new SimpleDataGridSample();
            }
            else if (className == "TestScrollViewer")
            {
                window = new TestScrollViewer();
            }

            else if (className == "ItemsControls")
            {
                window = new ItemsControl();
            }
            else

            {
                window = null;
            }
            window?.Show();
        }

        private bool CheckExist(string windowName)
        {
            var allWindows = Application.Current.Windows;
            foreach (Window window in allWindows)
            {
                string name = window.GetType().ToString();
                if (name.Contains(windowName))
                {
                    return true;
                }
            }

            return false;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Utils.KillSelfProcess();
        }
    }
}
