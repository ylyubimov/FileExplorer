using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace FileBrowser.View.Sphere {
    internal class SphereController {
        private readonly AxisAngleRotation3D _transform;

        public SphereController(AxisAngleRotation3D rotationTransform) {
            _transform = rotationTransform;
        }

        public void RotateSphere(int angleInDegrees, double durationInMilliseconds) {
            DoubleAnimation animation = new DoubleAnimation( 0, angleInDegrees,
                    TimeSpan.FromMilliseconds( durationInMilliseconds ) );
            _transform.BeginAnimation( AxisAngleRotation3D.AngleProperty, animation );
        }
    }
}
