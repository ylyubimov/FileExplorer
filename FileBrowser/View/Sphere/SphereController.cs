using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace FileBrowser.View.Sphere {
    internal class SphereController
    {
        private readonly AxisAngleRotation3D _transformX;
        private readonly AxisAngleRotation3D _transformY;
        private readonly AxisAngleRotation3D _transform;
        private DoubleAnimation _animation = new DoubleAnimation();

        public SphereController(AxisAngleRotation3D rotationTransformX, AxisAngleRotation3D rotationTransformY, AxisAngleRotation3D rotationTransform) {
            _transformX = rotationTransformX;
            _transformY = rotationTransformY;
            _transform = rotationTransform;
        }

        public void RotateSphereX(int angleInDegrees, double durationInMilliseconds) {
            lock (_animation) {
                _animation = new DoubleAnimation( 0, angleInDegrees,
                    TimeSpan.FromMilliseconds( durationInMilliseconds ) );
                _transformX.BeginAnimation( AxisAngleRotation3D.AngleProperty, _animation );
            }
        }

        public void RotateSphereY(int angleInDegrees, double durationInMilliseconds)
        {
            lock (_animation) {
                _animation = new DoubleAnimation(0, angleInDegrees,
                    TimeSpan.FromMilliseconds(durationInMilliseconds));
                _transformY.BeginAnimation( AxisAngleRotation3D.AngleProperty, _animation );
            }
        }

        public void RotateSphere(int angleInDegrees, double durationInMilliseconds, double axisX, double axisY, double axisZ) {
            lock (_animation)
            {
                _animation = new DoubleAnimation(0, angleInDegrees,
                    TimeSpan.FromMilliseconds( durationInMilliseconds ) );
//                _animation.Completed += huy;
                _transform.Axis = new Vector3D( axisX, axisY, axisZ );
                _transform.BeginAnimation( AxisAngleRotation3D.AngleProperty, _animation );
            }
        }

        private void huy(object sender, EventArgs e) {
        }
    }
}
