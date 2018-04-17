using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathUtils;

namespace MathEntities
{
    public class Vertex2D
    {
        private XYPT coordinate;
        private int reference;

        public XYPT Coordinate
        {
            get
            {
                return coordinate;
            }
        }
        public int Reference
        {
            get
            {
                return reference;
            }
        }

        public Vertex2D(XYPT coord)
        {
            coordinate = coord;
        }


        public int Increase()
        {
            return ++reference;
        }
        public int Decrease()
        {
            return --reference;
        }

    }
}
