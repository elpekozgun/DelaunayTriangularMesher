using MathUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathEntities
{
    public class HalfSegment2D
    {
        private XYPT p;
        private HalfSegment2D from, to;

        public XYPT P
        {
            get
            {
                return p;
            }

            set
            {
                p = value;
            }
        }
        public HalfSegment2D From
        {
            get
            {
                return from;
            }

            set
            {
                from = value;
            }
        }
        public HalfSegment2D To
        {
            get
            {
                return to;
            }

            set
            {
                to = value;
            }
        }

        public HalfSegment2D(XYPT point)
        {
            p = point;
        }


        public void Invert()
        {
            var tmp = from;
            from = to;
            to = tmp;
        }
        public HalfSegment2D Split()
        {
            var m = (to.p + p) * 0.5f;
            var e = new HalfSegment2D(m);
            to.from = e;
            e.to = to;
            to = e;
            e.from = this;
            return e;
        }

    }
}
