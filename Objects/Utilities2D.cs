using MathUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathEntities
{
    public class Utilities2D
    {
        /// <summary>
        /// Constrains the distance between 2 points by a threshold
        /// </summary>
        /// <param name="points"></param>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public static List<XYPT> ConstrainDistance(List<XYPT> points, float threshold = 1f)
        {
            var result = new List<XYPT>();

            var n = points.Count;
            for (int i = 0, j = 1; i < n && j < n; j++)
            {
                var from = points[i];
                var to = points[j];
                if (from.DistTo(to) > threshold)
                {
                    result.Add(from);
                    i = j;
                }
            }

            var p0 = result.Last();
            var p1 = result.First();
            if (p0.DistTo(p1) > threshold)
            {
                result.Add((p0 + p1) * 0.5f);
            }

            return result;
        }
        public static bool DoesIntersect(XYPT p0, XYPT p1, XYPT p2, XYPT p3)
        {
            var s1 = p1 - p0;
            var s2 = p3 - p2;
            var s = (-s1.y * (p0.x - p2.x) + s1.x * (p0.y - p2.y)) / (-s2.x * s1.y + s1.x * s2.y);
            var t = ( s2.x * (p0.y - p2.y) - s2.y * (p0.x - p2.x)) / (-s2.x * s1.y + s1.x * s2.y);
            return (s >= 0 && s <= 1 && t >= 0 && t <= 1);
        }

        public static bool Contains(XYPT point, List<Vertex2D> vertices)
        {
            var n = vertices.Count;
            bool c = false;
            for (int i = 0, j = n - 1; i < n; j = i++)
            {
                if (vertices[i].Coordinate == point) return true;
                if (
                    ((vertices[i].Coordinate.y > point.y) != (vertices[j].Coordinate.y > point.y)) &&
                    (point.x < (vertices[j].Coordinate.x - vertices[i].Coordinate.x) * (point.y - vertices[i].Coordinate.y) / (vertices[j].Coordinate.y - vertices[i].Coordinate.y) + vertices[i].Coordinate.x)
                )
                {
                    c = !c;
                }
            }
            // c == true means odd, c == false means even
            return c;
        }

        public static bool Contains(XYPT point, List<XYPT> polygon)
        {
            for (int i = 0,n = polygon.Count; i < n; i++)
            {
                if ((polygon[(i + 1) % n] - polygon[i]).cross(point) <= 0)
                    return false;
            }
            return true;
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
        public static bool IsLeft(XYPT from, XYPT to, XYPT p)
        {
            float cross = (float)((to.x - from.x) * (p.y - from.y) - (to.y - from.y) * (p.x - from.x));
            return cross > 0;
        }
        public static bool IsEqual(Vertex2D v0, Vertex2D v1)
        {
            return v0.Coordinate == v1.Coordinate;
        }
        public static bool IsEqual(Segment2D s0, Segment2D s1)
        {
            return IsEqual(s0.StartPt, s1.StartPt) && IsEqual(s0.EndPt, s1.EndPt) ||
                   IsEqual(s0.StartPt, s1.EndPt) && IsEqual(s0.EndPt, s1.StartPt);
        }
        public static bool IsEqual(Triangle2D t0, Triangle2D t1)
        {
            return IsEqual(t0.S0, t1.S0) && IsEqual(t0.S1, t1.S1) && IsEqual(t0.S2, t1.S2) ||
                   IsEqual(t0.S0, t1.S0) && IsEqual(t0.S1, t1.S2) && IsEqual(t0.S2, t1.S1) ||
                   IsEqual(t0.S0, t1.S1) && IsEqual(t0.S1, t1.S0) && IsEqual(t0.S2, t1.S2) ||
                   IsEqual(t0.S0, t1.S1) && IsEqual(t0.S1, t1.S2) && IsEqual(t0.S2, t1.S0) ||
                   IsEqual(t0.S0, t1.S2) && IsEqual(t0.S1, t1.S0) && IsEqual(t0.S2, t1.S1) ||
                   IsEqual(t0.S0, t1.S2) && IsEqual(t0.S1, t1.S1) && IsEqual(t0.S2, t1.S0);

        }
    }
}
