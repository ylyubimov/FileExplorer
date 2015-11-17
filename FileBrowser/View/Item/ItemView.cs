using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace FileBrowser.View.Item
{
    class ItemView {
        private int _r = 0;
        private string _name;
        private Image _image;

        public ItemView(int r, string name, Image image) {
            _r = r;
            _name = name;
            _image = image;
        }

        public Geometry3D Calculate(float angleZ, float angleY) {
            angleZ = (float)(angleZ * Math.PI / 180);
            angleY = (float)(angleY * Math.PI / 180);
            var geometry = new MeshGeometry3D();
//            geometry.Positions.Add(new Point3D(_r + 5, 5, +5));
//            geometry.Positions.Add(new Point3D(_r + 5, 5, -5));
//            geometry.Positions.Add(new Point3D(_r + 5, -5, +5));
//            geometry.Positions.Add(new Point3D(_r + 5, -5, -5));
////            geometry.Positions.Add(new Point3D((_r + 5) * Math.Cos(angleZ), 5, 5 * Math.Sin(angleZ)));
////            geometry.Positions.Add(new Point3D((_r + 5) * Math.Cos(angleZ), 5, -5 * Math.Sin(angleZ)));
////            geometry.Positions.Add(new Point3D((_r + 5) * Math.Cos(angleZ), -5, 5 * Math.Sin(angleZ)));
////            geometry.Positions.Add(new Point3D((_r + 5) * Math.Cos(angleZ), -5, -5 * Math.Sin(angleZ)));
//            geometry.TriangleIndices.Add(0);
//            geometry.TriangleIndices.Add(2);
//            geometry.TriangleIndices.Add(1);
//            geometry.TriangleIndices.Add(1);
//            geometry.TriangleIndices.Add(2);
//            geometry.TriangleIndices.Add(3);
            return geometry;
        }
    }
}
