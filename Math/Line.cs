using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathUtils
{
    public class Line 
    {
        #region Ctor

        public Line()
        {
            m_points = new List<XYPT>();
        }

        public Line(List<XYPT> points)
        {
            m_points = points;
        }
        public Line(XYPT pt1 ,XYPT pt2)
        {
            m_points = new List<XYPT>() { pt1, pt2 };
        }

        #endregion

        #region Private Fields

        private List<XYPT> m_points;

        #endregion

        #region Public Properties

        public List<XYPT> Points
        {
            get
            {
                return m_points;
            }

            set
            {
                m_points = value;
            }
        }

        public List<XYPT> BoundingPoints
        {
            get
            {
                var ret = new List<XYPT>();
                float xMin = (float)m_points[0].x;
                float yMin = (float)m_points[0].y;
                float xMax = (float)m_points[0].x;
                float yMax = (float)m_points[0].y;

                foreach (var pt in m_points)
                {
                    if (pt.x < xMin)
                        xMin = (float)pt.x;
                    if (pt.y < yMin)
                        yMin = (float)pt.y;
                    if (pt.x > xMax)
                        xMax = (float)pt.x;
                    if (pt.y > yMax)
                        yMax = (float)pt.y;
                }
                ret.Add(new XYPT(xMin, yMin));
                ret.Add(new XYPT(xMax, yMax));
                return ret;
            }
        }

        #endregion

        #region Public Methods

        public static Line operator + (Line p, Line q)
        {
            Line r = new Line();
            foreach (var point in p.m_points)
            {
                r.Points.Add(point);
            }
            foreach (var point in q.m_points)
            {
                r.m_points.Add(point);
            }

            return r;
        }

        public bool PointOnLineSegment(XYPT pt1, XYPT pt2, XYPT pt, double epsilon = 0.001)
        {
            if (pt.XX - Math.Max(pt1.XX, pt2.XX) > epsilon ||
                Math.Min(pt1.XX, pt2.XX) - pt.XX > epsilon ||
                pt.YY - Math.Max(pt1.YY, pt2.YY) > epsilon ||
                Math.Min(pt1.YY, pt2.YY) - pt.YY > epsilon)
                return false;

            if (Math.Abs(pt2.XX - pt1.XX) < epsilon)
                return Math.Abs(pt1.XX - pt.XX) < epsilon || Math.Abs(pt2.XX - pt.XX) < epsilon;
            if (Math.Abs(pt2.YY - pt1.YY) < epsilon)
                return Math.Abs(pt1.YY - pt.YY) < epsilon || Math.Abs(pt2.YY - pt.YY) < epsilon;

            double x = pt1.XX + (pt.YY - pt1.YY) * (pt2.XX - pt1.XX) / (pt2.YY - pt1.YY);
            double y = pt1.YY + (pt.XX - pt1.XX) * (pt2.YY - pt1.YY) / (pt2.XX - pt1.XX);

            return Math.Abs(pt.XX - x) < epsilon || Math.Abs(pt.YY - y) < epsilon;
        }

        public bool Pt2LinePt(XYPT pt1, XYPT pt2,  XYPT pPerp, out XYPT pDrop, double epsilon = 0.001)
        {
            double dx = pt2.x - pt1.x;
            double dy = pt2.y - pt1.y;

            if ((dx == 0.0) && (dy == 0.0))
            {
                pDrop.x = pt1.x;
                pDrop.y = pt1.y;
                return true;
            }

            if (Math.Abs(dy) <= Math.Abs(dx))
            {
                double m = dy / dx;             // Slope of line
                double b = pt1.y - m * pt1.x;     // Y-Intercept

                // Find the point on (x1,y1)-(x2,y2) that is closest to (x,y)

                double tmp = (pPerp.y - b) * m + pPerp.x;
                pDrop.x = tmp / (m * m + 1);
                pDrop.y = m * pDrop.x + b;
            }
            else
            {
                double m = dx / dy;             // Inverse slope of line
                double b = pt1.x - m * pt1.y;     // X-Intercept

                // Find the point on (x1,y1)-(x2,y2) that is closest to (x,y).

                double tmp = (pPerp.x - b) * m + pPerp.y;
                pDrop.y = tmp / (m * m + 1);
                pDrop.x = m * pDrop.y + b;

            }

            // x and y are the points on (x1,y1)-(x2,y2) such that
            // (x,y)-(xr,yr) is perpendicular to (x1,y1)-(x2,y2).

            bool b1 = (pt1.x <= pDrop.x + epsilon) && (pDrop.x <= pt2.x + epsilon);
            bool b2 = (pt2.x <= pDrop.x + epsilon) && (pDrop.x <= pt1.x + epsilon);
            bool b3 = (pt1.y <= pDrop.y + epsilon) && (pDrop.y <= pt2.y + epsilon);
            bool b4 = (pt2.y <= pDrop.y + epsilon) && (pDrop.y <= pt1.y + epsilon);
            if ((b1 || b2) && (b3 || b4)) return true;
            else return false;

        }

        public void SortPoints()
        {
            if (m_points.Count == 0 || m_points.Count == 1)
                return;
            var center = (m_points[0] + m_points[1])/2;

            m_points = m_points.OrderByDescending(x => x.AngleTo(center)).ToList();
        }


        #endregion

    }
}
