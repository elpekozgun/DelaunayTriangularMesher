using MathUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathEntities
{
    public class Circle2D
    {
        private XYPT _center;
        private float _radius;

        public XYPT Center
        {
            get
            {
                return _center;
            }

            set
            {
                _center = value;
            }
        }
        public float Radius
        {
            get
            {
                return _radius;
            }

            set
            {
                _radius = value;
            }
        }

        public Circle2D(XYPT p, float r)
        {
            _center = p;
            _radius = r;
        }


        public bool ContainsPoint(XYPT p)
        {
            return p.DistTo(_center) < _radius;
        }
        /// <summary>
        /// Gets the 2D circle that circumscribes the triangle given.
        /// vince - Geometry for Computer Graphics - page 83
        /// </summary>
        /// <param name="triangle"></param>
        /// <returns></returns>
        public static Circle2D GetCircumscribedCircle(Triangle2D triangle)
        {
            float x1 = (float)triangle.A.Coordinate.x;
            float y1 = (float)triangle.A.Coordinate.y;
            float x2 = (float)triangle.B.Coordinate.x;
            float y2 = (float)triangle.B.Coordinate.y;
            float x3 = (float)triangle.C.Coordinate.x;
            float y3 = (float)triangle.C.Coordinate.y;

            float x1_2 = x1 * x1;
            float x2_2 = x2 * x2;
            float x3_2 = x3 * x3;
            float y1_2 = y1 * y1;
            float y2_2 = y2 * y2;
            float y3_2 = y3 * y3;

            float c = 2f * ((x2 - x1) * (y3 - y1) - (y2 - y1) * (x3 - x1));
            float x = ((y3 - y1) * (x2_2 - x1_2 + y2_2 - y1_2) + (y1 - y2) * (x3_2 - x1_2 + y3_2 - y1_2)) / c;
            float y = ((x1 - x3) * (x2_2 - x1_2 + y2_2 - y1_2) + (x2 - x1) * (x3_2 - x1_2 + y3_2 - y1_2)) / c;
            float _x = (x1 - x);
            float _y = (y1 - y);

            float r = (float)Math.Sqrt((_x * _x) + (_y * _y));
            return new Circle2D(new XYPT(x, y), r);


            //var lengthProduct = triangle.S0.Length * triangle.S1.Length * triangle.S2.Length;
            //var R = lengthProduct / 4 * triangle.GetArea();

            //var xp = x1 + R / lengthProduct * (triangle.S2.ProjectedLengthOverY * triangle.S0.Length * triangle.S0.Length - 
            //        triangle.S2.Length * triangle.S2.Length * triangle.S0.ProjectedLengthOverY);

            //var yp = y1 + R / lengthProduct * (triangle.S2.Length * triangle.S2.Length * triangle.S0.ProjectedLengthOverX -
            //        triangle.S0.Length * triangle.S0.Length * triangle.S2.ProjectedLengthOverX);

            //return new Circle2D(new XYPT(xp,yp),R);

        }

    }
}
