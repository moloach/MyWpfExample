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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// UserControlModelVisual3D.xaml 的交互逻辑
    /// </summary>
    public partial class UserControlModelVisual3D : UserControl
    {

        Trackball<ModelVisual3D> mTrackball;
        public UserControlModelVisual3D()
        {
            InitializeComponent();
            Loaded += UserControlModelVisual3D_Loaded; 
        }

        private void UserControlModelVisual3D_Loaded(object sender, RoutedEventArgs e)
        {
            mTrackball = new Trackball<ModelVisual3D>();
            mTrackball.Attach(this);
            mTrackball.Slaves.Add(viewport3DScan);
            mTrackball.Enabled = true;
        }
    }
}
