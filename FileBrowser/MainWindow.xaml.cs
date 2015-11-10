using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Navigation;
using System.Xml;
using FileBrowser.View.Sphere;

namespace FileBrowser {
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();

            SphereController controller = new SphereController( SphereTransformX, SphereTransformY, SphereTransform, Models );
            controller.RotateSphereX( 148, 3000 );
            SphereGeometry3D sphere = (SphereGeometry3D) Resources["SphereGeometrySource"];
            
            controller.AddItem( sphere, "" );
        }
    }
}
