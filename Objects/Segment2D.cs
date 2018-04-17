using MathUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MathEntities
{
    public class Segment2D
    {
        private int _reference;
        private float _length;
        private Vertex2D _startPt; 
        private Vertex2D _endPt;

        public int Reference
        {
            get
            {
                return _reference;
            }
        }
        public Vertex2D EndPt
        {
            get
            {
                return _endPt;
            }

            set
            {
                _endPt = value;
            }
        }
        public Vertex2D StartPt
        {
            get
            {
                return _startPt;
            }

            set
            {
                _startPt = value;
            }
        }
        public float Length
        {
            get
            {
                if (_length <= 0)
                {
                    _length = (float)(_startPt.Coordinate.DistTo(_endPt.Coordinate));
                }
                return _length;
            }
        }
        public float ProjectedLengthOverX
        {
            get
            {
                return (float)Math.Abs(_endPt.Coordinate.x -_startPt.Coordinate.x);
            }
        }
        public float ProjectedLengthOverY
        {
            get
            {
                return (float)Math.Abs(_endPt.Coordinate.y - _startPt.Coordinate.y);
            }
        }

        public Segment2D(Vertex2D start, Vertex2D end)
        {
            _startPt = start;
            _endPt = end;
        }


        public XYPT GetMidPoint()
        {
            return (_startPt.Coordinate + _endPt.Coordinate) * 0.5f;
        }
        public bool HasEncroached(XYPT p)
        {
            if (p == _startPt.Coordinate || p == _endPt.Coordinate)
                return false;
            var radius = this._length * 0.5f;
            return GetMidPoint().DistTo(p) < radius;
        }
        public bool HasPoint(Vertex2D v)
        {
            return _startPt == v || _endPt == v;
        }
        public bool HasPoint(XYPT p)
        {
            return _startPt.Coordinate == p || _endPt.Coordinate == p;
        }
        public int Increase()
        {
            _startPt.Increase();
            _endPt.Increase();
            return ++_reference;
        }
        public int Decrease ()
        {
            _startPt.Decrease();
            _endPt.Decrease();
            return ++_reference;
        }

    }
}
