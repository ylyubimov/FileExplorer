using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace FileBrowser.View {
    /// <summary>
    /// Class for calculating geometry on sphere
    /// </summary>
    sealed class SphereGeometry3D {
        public SphereGeometry3D()
        {
            Radius = 75;
            Separators = 15;
        }

        /// <summary>
        /// Specifies the size of sphere
        /// </summary>
        public int Radius
        {
            get {
                return _radius;
            }
            set {
                _radius = value;
                calculateGeometry();
            }
        }

        /// <summary>
        /// Specifies the 
        /// </summary>
        public int Separators
        {
            get {
                return _n;
            }
            set {
                _n = value;
                calculateGeometry();
            }
        }

        public Point3DCollection Points
        {
            get {
                return _points;
            }
        }

        public Int32Collection TriangleIndices
        {
            get {
                return _triangleIndices;
            }
        }

        public PointCollection TextureCoordinates
        {
            get {
                return _textureCoordinates;
            }
        }

		private void calculateGeometry() {
            int e;
            double segmentAngle = Math.PI / ( 2 * ( _n + 1 ) );
            int numberOfSeparators = 4 * _n + 4;

            _points = new Point3DCollection();
            _triangleIndices = new Int32Collection();
            _textureCoordinates = new PointCollection();

            for( e = -_n; e <= _n; e++ ) {
                double rE = _radius * Math.Cos( segmentAngle * e );
                double yE = _radius * Math.Sin( segmentAngle * e );

                for( int s = 0; s <= numberOfSeparators - 1; s++ ) {
                    double zS = rE * Math.Sin( segmentAngle * ( s + 1 ) - Math.PI / 2 ) * ( -1 );
                    double xS = rE * Math.Cos( segmentAngle * ( s + 1 ) - Math.PI / 2 );
                    _points.Add( new Point3D( xS, yE, zS ) );
                    _textureCoordinates.Add( new Point( ( ( double) s / ( 2 * _n ) ),
                        ( ( 1 - e / ( ( double )_n / 2 ) ) / 2 ) ) );
                }
            } 
            _points.Add( new Point3D( 0, _radius, 0 ) );
            _textureCoordinates.Add( new Point( 0, 0 ) );
            _points.Add( new Point3D( 0, -_radius, 0 ) );
            _textureCoordinates.Add( new Point( 0, 0 ) );

            for( e = 0; e < _n * 2; e++ ) {
                for( int i = 0; i < numberOfSeparators; i++ ) {
                    _triangleIndices.Add( e * numberOfSeparators + i );
                    _triangleIndices.Add( e * numberOfSeparators + ( i + 1 ) % numberOfSeparators + numberOfSeparators );
                    _triangleIndices.Add( e * numberOfSeparators + i + numberOfSeparators );
                    _triangleIndices.Add( e * numberOfSeparators + ( i + 1 ) % numberOfSeparators );
                    _triangleIndices.Add( e * numberOfSeparators + ( i + 1 ) % numberOfSeparators + numberOfSeparators );
                    _triangleIndices.Add( e * numberOfSeparators + i );
                }
            }

            for( int i = 0; i < numberOfSeparators; i++ ) {
                _triangleIndices.Add( numberOfSeparators * ( 2 * _n + 1 ) );
                _triangleIndices.Add( _n * 2 * numberOfSeparators + i );
                _triangleIndices.Add( _n * 2 * numberOfSeparators + ( i + 1 ) % numberOfSeparators );
            }

            for( int i = 0; i < numberOfSeparators; i++ ) {
                _triangleIndices.Add( i );
                _triangleIndices.Add( numberOfSeparators * ( 2 * _n + 1 ) + 1 );
                _triangleIndices.Add( ( i + 1 ) % numberOfSeparators );
            }
        }

        private int _n;
        private int _radius;
        private Point3DCollection _points;
        private Int32Collection _triangleIndices;
        private PointCollection _textureCoordinates;
    }
}