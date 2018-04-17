using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathUtils
{
    public class Polygon
    {
        #region Ctor

        public Polygon()
        {
            InnerPolygons = new List<Polygon>();
            m_points = new List<XYPT>();
        }

        public Polygon(List<XYPT> points)
        {
            m_points = points;
        }

        #endregion

        #region Private Fields

        private List<XYPT> m_points;

        #endregion

        #region Public Fields and Properties

        public List<Polygon> InnerPolygons;
        public List<XYPT> Points
        {
            get
            {
                return m_points;
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
        public bool IsCCW { get { return (PolygonArea() > 0); } }
        public bool IsCWW { get { return (PolygonArea() < 0); } }

        #endregion

        #region Public Methods

        public float PolygonArea()
        {
            int i, j;
            float area = 0;

            for (i = 0; i < m_points.Count; i++)
            {
                j = (i + 1) % m_points.Count;

                area += (float)(m_points[i].XX * m_points[j].YY);
                area -= (float)(m_points[i].YY * m_points[j].XX);
            }
            area /= 2;

            return area;
        }

        public void AddPoint(XYPT p)
        {
            if (m_points == null)
            {
                m_points = new List<XYPT>();
            }

            m_points.Add(p);
        }

        public void RemovePoint(XYPT p)
        {
            if (ContainsPoint(p))
            {
                m_points.Remove(p);
            }
        }

        public bool ContainsPoint(XYPT p)
        {
            if (m_points == null)
            {
                m_points = new List<XYPT>();
                return false;
            }

            return m_points.Contains(p);
        }

        public int Count
        {
            get
            {
                if (m_points == null)
                {
                    m_points = new List<XYPT>();
                }

                return m_points.Count;
            }
        }

        public bool PolygonInPolygon(Polygon poly, float percent = .6f)
        {
            var t = 0;
            for (var i = 0; i < poly.Count; ++i)
            {
                if (this.PointInPolygon(poly.Points[i]))
                    t++;
            }

            return ((float)t) >= (poly.Count * percent) ? true : false;
        }

        public bool PointInPolygon(XYPT point)
        {
            int i, j;
            bool inside = false;
            int nvert = m_points.Count;

            for (i = 0, j = nvert - 1; i < nvert; j = i++)
            {
                if (((m_points[i].YY > point.YY) != (m_points[j].YY > point.YY)) &&
                 (point.XX < (m_points[j].XX - m_points[i].XX) * (point.YY - m_points[i].YY) / (m_points[j].YY - m_points[i].YY) + m_points[i].XX))
                {
                    inside = !inside;
                }
            }
            // check if point is over segments of polygon
            if (inside != true)
            {
                for (int k = 0; k < nvert; k++)
                {
                    if (PointOnLineSegment(m_points[(k + 1) % nvert], m_points[k], point))
                        return true;
                }
            }
            return inside;
        }

        public static bool PointOnLineSegment(XYPT pt1, XYPT pt2, XYPT pt, double epsilon = 0.001)
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

        private XYPT GetCenter()
        {
            XYPT center = new XYPT();
            foreach (var point in m_points)
            {
                center.x += point.x;
                center.y += point.y;
            }
            center /= Count;
            return center;
        }

        public void SortPoints()
        {
            if (m_points.Count == 0 || m_points.Count == 1)
                return;
            var center = GetCenter();
            
            m_points = m_points.OrderBy(x=>x.AngleTo(center)).ToList();
            int upper = m_points.Where(x => x.y > center.y).Count();
            int lower = m_points.Where(x => x.y < center.y).Count();

            if (lower >= upper)
            {
                //Left Shift Algorithm
                for (int j = 0; j < upper; j++)
                {
                    var temp = m_points[0];
                    for (int i = 1; i < Count; i++)
                    {
                        m_points[i - 1] = m_points[i];
                    }
                    m_points[Count - 1] = temp;
                }
            }
            else
            { 
                //Rigth Shift Algorithm
                for (int j = 0; j < lower; j++)
                {
                    var temp = m_points[Count - 1];
                    for (int i = Count - 2; i >= 0; i--)
                    {
                        m_points[i + 1] = m_points[i];
                    }
                    m_points[0] = temp;
                }
            }
        }

        public bool IsConvex()
        {
            // For each set of three adjacent points A, B, C,
            // find the dot product AB · BC. If the sign of
            // all the dot products is the same, the angles
            // are all positive or negative (depending on the
            // order in which we visit them) so the polygon
            // is convex.
            bool got_negative = false;
            bool got_positive = false;
            int num_points = Points.Count;
            int B, C;
            for (int A = 0; A < num_points; A++)
            {
                B = (A + 1) % num_points;
                C = (B + 1) % num_points;

                float cross_product =
                    CrossProductLength(
                        (float)Points[A].XX, (float)Points[A].YY,
                        (float)Points[B].XX, (float)Points[B].YY,
                        (float)Points[C].XX, (float)Points[C].YY);
                if (cross_product < 0)
                {
                    got_negative = true;
                }
                else if (cross_product > 0)
                {
                    got_positive = true;
                }
                if (got_negative && got_positive) return false;
            }

            // If we got this far, the polygon is convex.
            return true;
        }

        public static float CrossProductLength(float Ax, float Ay,
    float Bx, float By, float Cx, float Cy)
        {
            // Get the vectors' coordinates.
            float BAx = Ax - Bx;
            float BAy = Ay - By;
            float BCx = Cx - Bx;
            float BCy = Cy - By;

            // Calculate the Z coordinate of the cross product.
            return (BAx * BCy - BAy * BCx);
        }


        #endregion
    }
}
