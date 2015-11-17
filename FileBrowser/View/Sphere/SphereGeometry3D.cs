using System;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Diagnostics;
using System.Windows;

namespace FileBrowser.View.Sphere {
    class SphereGeometry3D
    {
        private int _n;
        private int _r;
        protected Point3DCollection points;
        protected Int32Collection triangleIndices;
        protected PointCollection textureCoordinates;

        public virtual int Radius
        {
            get { return _r; }
            set
            {
                _r = value;
                CalculateGeometry();
            }
        }

        public virtual int Separators
        {
            get { return _n; }
            set
            {
                _n = value;
                CalculateGeometry();
            }
        }

        public Point3DCollection Points
        {
            get { return points; }
        }

        public Int32Collection TriangleIndices
        {
            get { return triangleIndices; }
        }

        public PointCollection TextureCoordinates
        {
            get { return textureCoordinates; }
        }

        public SphereGeometry3D() {
            Radius = 75;
            Separators = 15;
        }

        protected void CalculateGeometry() {
            int e;
            double segmentRad = Math.PI / 2 / (_n + 1);
            int numberOfSeparators = 4 * _n + 4;

            points = new Point3DCollection();
            triangleIndices = new Int32Collection();
            textureCoordinates = new PointCollection();

            for (e = -_n; e <= _n; e++) {
                double rE = _r * Math.Cos( segmentRad * e );
                double yE = _r * Math.Sin( segmentRad * e );

                for (int s = 0; s <= numberOfSeparators - 1; s++) {
                    double zS = rE * Math.Sin( segmentRad * (s + 1) - Math.PI / 2) * (-1);
                    double xS = rE * Math.Cos( segmentRad * (s + 1) - Math.PI / 2 );
                    points.Add( new Point3D( xS, yE, zS ) );
                    textureCoordinates.Add( new Point( ((double)s / (2 * _n)), ((1 - (double)e / ((double)_n / 2)) / 2)) );
                }
            }
            points.Add( new Point3D( 0, _r, 0 ) );
            textureCoordinates.Add( new Point( 0, 0 ) );
            points.Add( new Point3D( 0, -_r, 0 ) );
            textureCoordinates.Add( new Point( 0, 0 ) );

            for (e = 0; e < _n * 2; e++) {
                for (int i = 0; i < numberOfSeparators; i++) {
                    triangleIndices.Add( e * numberOfSeparators + i );
                    triangleIndices.Add( e * numberOfSeparators + (i + 1) % numberOfSeparators + numberOfSeparators );
                    triangleIndices.Add( e * numberOfSeparators + i + numberOfSeparators );
                    triangleIndices.Add( e * numberOfSeparators + (i + 1) % numberOfSeparators );
                    triangleIndices.Add( e * numberOfSeparators + (i + 1) % numberOfSeparators + numberOfSeparators );
                    triangleIndices.Add( e * numberOfSeparators + i );
                }
            }

            for (int i = 0; i < numberOfSeparators; i++) {
                triangleIndices.Add( numberOfSeparators * (2 * _n + 1) );
                triangleIndices.Add( _n * 2 * numberOfSeparators + i );
                triangleIndices.Add( _n * 2 * numberOfSeparators + (i + 1) % numberOfSeparators );
            }

            for (int i = 0; i < numberOfSeparators; i++) {
                triangleIndices.Add( i );
                triangleIndices.Add( numberOfSeparators * (2 * _n + 1) + 1 );
                triangleIndices.Add( (i + 1) % numberOfSeparators );
            }
        }
    }
}
