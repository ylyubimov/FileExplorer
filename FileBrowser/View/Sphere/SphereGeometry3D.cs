using System;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Diagnostics;
using System.Windows;

namespace FileBrowser.View.Sphere {
    class SphereGeometry3D
    {
        private int n;
        private int r;
        protected Point3DCollection points;
        protected Int32Collection triangleIndices;
        protected PointCollection textureCoordinates;

        public virtual int Radius
        {
            get { return r; }
            set
            {
                r = value;
                CalculateGeometry();
            }
        }

        public virtual int Separators
        {
            get { return n; }
            set
            {
                n = value;
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
            double segmentRad = Math.PI / 2 / (n + 1);
            int numberOfSeparators = 4 * n + 4;

            points = new Point3DCollection();
            triangleIndices = new Int32Collection();
            textureCoordinates = new PointCollection();

            for (e = -n; e <= n; e++) {
                double rE = r * Math.Cos( segmentRad * e );
                double yE = r * Math.Sin( segmentRad * e );

                for (int s = 0; s <= numberOfSeparators - 1; s++) {
                    double zS = rE * Math.Sin( segmentRad * s ) * (-1);
                    double xS = rE * Math.Cos( segmentRad * s );
                    points.Add( new Point3D( xS, yE, zS ) );
                    textureCoordinates.Add( new Point( (s * 50) % 100, (e * 50) % 100 ) );
                }
            }
            points.Add( new Point3D( 0, r, 0 ) );
            textureCoordinates.Add( new Point( 0, 0 ) );
            points.Add( new Point3D( 0, -r, 0 ) );
            textureCoordinates.Add( new Point( 0, 0 ) );

            for (e = 0; e < n * 2; e++) {
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
                triangleIndices.Add( numberOfSeparators * (2 * n + 1) );
                triangleIndices.Add( n * 2 * numberOfSeparators + i );
                triangleIndices.Add( n * 2 * numberOfSeparators + (i + 1) % numberOfSeparators );
            }

            for (int i = 0; i < numberOfSeparators; i++) {
                triangleIndices.Add( i );
                triangleIndices.Add( numberOfSeparators * (2 * n + 1) + 1 );
                triangleIndices.Add( (i + 1) % numberOfSeparators );
            }
        }
    }
}
