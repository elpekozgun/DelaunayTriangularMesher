using MathUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathEntities
{
    public class Triangle2D
    {
        private Vertex2D _a, _b, _c;
        private Segment2D _s0, _s1, _s2;
        private Circle2D _circum;
           

        public Vertex2D A
        {
            get
            {
                return _a;
            }
        }
        public Vertex2D B
        {
            get
            {
                return _b;
            }
        }
        public Vertex2D C
        {
            get
            {
                return _c;
            }

        }
        public Segment2D S0
        {
            get
            {
                return _s0;
            }
        }
        public Segment2D S1
        {
            get
            {
                return _s1;
            }
        }
        public Segment2D S2
        {
            get
            {
                return _s2;
            }
        }
        public Circle2D Circum
        {
            get
            {
                return _circum;
            }

            set
            {
                _circum = value;
            }
        }

        public Triangle2D(Segment2D s0, Segment2D s1, Segment2D s2)
        {
            _s0 = s0;
            _s1 = s1;
            _s2 = s2;
            _a = s0.StartPt;
            _b = s0.EndPt;
            _c = (s2.EndPt == _a || s2.EndPt == _b) ? s2.StartPt : s2.EndPt;
        }
        public Triangle2D(Vertex2D a, Vertex2D b, Vertex2D c)
        {
            _a = a;
            _b = b;
            _c = c;
            _s0 = new Segment2D(_a, _b);
            _s1 = new Segment2D(_b, _c);
            _s2 = new Segment2D(_c, _a);
        }


        public bool HasPoint(Vertex2D v)
        {
            return _a == v || _b == v || _c == v;
        }
        public bool HasCommonPoint(Triangle2D t)
        {
            return HasPoint(t.A) || HasPoint(t.B) || HasPoint(t.C);
        }
        public bool HasSegment(Segment2D s)
        {
            return s == _s0 || s == _s1 || s == _s2;
        }
        public bool HasSegment(XYPT a, XYPT b)
        {
            return _s0.HasPoint(a) && _s0.HasPoint(b) || _s1.HasPoint(a) && _s1.HasPoint(b) || _s2.HasPoint(a) && _s2.HasPoint(b);
        }
        public Vertex2D ExcludePoint(Segment2D s)
        {
            if (!s.HasPoint(_a))
                return _a;
            else if (!s.HasPoint(_b))
                return _b;
            else
                return _c;
        }
        public Vertex2D ExcludePoint(XYPT p1, XYPT p2)
        {
            if (p1 != _a.Coordinate && p2 != _a.Coordinate)
                return _a;
            else if (p1 != _b.Coordinate && p2 != _b.Coordinate)
                return _b;
            else
                return _c;
        }
        public Segment2D[] ExcludeSegment(Segment2D s)
        {
            if (_s0.Equals(s))
                return new Segment2D[] { _s1, _s2 };
            else if (_s1.Equals(s))
                return new Segment2D[] { _s0, _s2 };
            else
                return new Segment2D[] { _s0, _s1 };
        }
        public XYPT GetCircumcenter()
        {
            if (_circum == null)
            {
                _circum = Circle2D.GetCircumscribedCircle(this);
            }
            return _circum.Center;
        }
        public bool ContainsInExternalCircle(Vertex2D v)
        {
            if (_circum == null)
            {
                _circum = Circle2D.GetCircumscribedCircle(this);
            }
            return _circum.ContainsPoint(v.Coordinate);
        }
        private float GetAngle(XYPT from, XYPT to0, XYPT to1)
        {
            var v0 = to0 - from;
            var v1 = to1 - from;
            

            float acos = (float)Math.Acos(v0.dot(v1) / Math.Sqrt(v0.Magnitude() * v0.Magnitude() * v1.Magnitude() * v1.Magnitude()));
            return (acos * 180 / (float)Math.PI);
        }
        public bool IsSkinny (float angle, float threshold)
        {
            if (_s0.Length <= threshold && _s1.Length <= threshold && _s2.Length <= threshold)
                return false;
            if (GetAngle(_a.Coordinate, _b.Coordinate, _c.Coordinate) < angle) //bac    
                return true;
            else if (GetAngle(_b.Coordinate, _a.Coordinate, _c.Coordinate) < angle) //abc
                return true;
            else if (GetAngle(_c.Coordinate, _a.Coordinate, _b.Coordinate) < angle) //acb   
                return true;
            else
                return false;
        }
        public bool Equals(Triangle2D t)
        {
            return HasPoint(t.A) && HasPoint(t.B) && HasPoint(t.C);
        }
        public bool HasCommonSegment(Triangle2D t)
        {
            return HasSegment(t.S0) || HasSegment(t.S1) || HasSegment(t.S2);
        }
        public float GetArea()
        {
            return 0.5f * (float)(_b.Coordinate.y - _c.Coordinate.y - _b.Coordinate.x + _c.Coordinate.x + _b.Coordinate.x * _c.Coordinate.y - _b.Coordinate.y * _c.Coordinate.x);
        }
    }
}
