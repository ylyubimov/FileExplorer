using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using FileBrowser.View.Item;

namespace FileBrowser.View.Sphere {
    internal class SphereController {
        private readonly AxisAngleRotation3D _transformX;
        private readonly AxisAngleRotation3D _transformY;
        private readonly AxisAngleRotation3D _transform;
        private DoubleAnimation _animation = new DoubleAnimation();
        private Model3DGroup _models;

        public SphereController(AxisAngleRotation3D rotationTransformX, AxisAngleRotation3D rotationTransformY,
                AxisAngleRotation3D rotationTransform, Model3DGroup models) {
            _transformX = rotationTransformX;
            _transformY = rotationTransformY;
            _transform = rotationTransform;
            _models = models;
        }

        public void RotateSphereX(int angleInDegrees, double durationInMilliseconds) {
            lock (_animation) {
                _animation = new DoubleAnimation( 0, angleInDegrees, TimeSpan.FromMilliseconds( durationInMilliseconds ) );
                _transformX.BeginAnimation( AxisAngleRotation3D.AngleProperty, _animation );
            }
        }

        public void RotateSphereY(int angleInDegrees, double durationInMilliseconds) {
            lock (_animation) {
                _animation = new DoubleAnimation( 0, angleInDegrees, TimeSpan.FromMilliseconds( durationInMilliseconds ) );
                _transformY.BeginAnimation( AxisAngleRotation3D.AngleProperty, _animation );
            }
        }

        public void RotateSphere(int angleInDegrees, double durationInMilliseconds, double axisX, double axisY,
                double axisZ) {
            lock (_animation) {
                _animation = new DoubleAnimation( 0, angleInDegrees, TimeSpan.FromMilliseconds( durationInMilliseconds ) );
                _transform.Axis = new Vector3D( axisX, axisY, axisZ );
                _transform.BeginAnimation( AxisAngleRotation3D.AngleProperty, _animation );
            }
        }

        public void AddItem(SphereGeometry3D sphere, string name) {
            _models.Children.Add( new GeometryModel3D( new ItemView( sphere.Radius, name, new Image() ).Calculate(60, 0),
                    new DiffuseMaterial( new SolidColorBrush( new Color {R = 50, G = 200, B = 75, A = 230} ) ) ) );
        }
    }
}