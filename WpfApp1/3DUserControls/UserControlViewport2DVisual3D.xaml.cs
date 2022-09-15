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
    /// UserControlViewport2DVisual3D.xaml 的交互逻辑
    /// </summary>
    public partial class UserControlViewport2DVisual3D : UserControl
    {

        Trackball<Viewport2DVisual3D> mTrackball;
        public UserControlViewport2DVisual3D()
        {
            InitializeComponent();

            Loaded += UserControlViewport2DVisual3D_Loaded;
        }
        private void UserControlViewport2DVisual3D_Loaded(object sender, RoutedEventArgs e)
        {
            mTrackball = new Trackball<Viewport2DVisual3D>();
            mTrackball.Attach(this);
            mTrackball.Slaves.Add(viewport3DScan);
            mTrackball.Enabled = true;
        }


    }
}
