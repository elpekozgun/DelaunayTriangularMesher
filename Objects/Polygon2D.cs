using MathUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathEntities
{
    public class Polygon2D
    {
        private List<Vertex2D> _vertices;
        private List<Segment2D> _segments;

        public Vertex2D[] Vertices
        {
            get
            {
                return _vertices.ToArray();
            }
        }
        public Segment2D[] Segments
        {
            get
            {
                return _segments.ToArray();
            }
        }

        public Polygon2D(XYPT[] contour)
        {
            _vertices = contour.Select(p => new Vertex2D(p)).ToList();
            _segments = new List<Segment2D>();
            for (int i = 0; i < _vertices.Count; i++)
            {
                var v0 = _vertices[i];
                var v1 = _vertices[(i + 1) % _vertices.Count];
                _segments.Add(new Segment2D(v0, v1));
            }
        }


        public static Polygon2D ConvexHull(XYPT[] points)
        {
            var ordered = points.ToList().OrderBy(p => p.x).ToList();

            var upper = new List<XYPT>();
            upper.Add(ordered[0]);
            upper.Add(ordered[1]);
            for (int i = 2,n = ordered.Count; i < n; i++)
            {
                upper.Add(ordered[i]);
                int k = upper.Count;
                if (k > 2)
                {
                    var p = upper[k - 3];
                    var r = upper[k - 2];
                    var q = upper[k - 1];
                    if (Utilities2D.IsLeft(p, q, r))
                        upper.RemoveAt(k - 2);
                }
            }

            var lower = new List<XYPT>();
            lower.Add(ordered[ordered.Count - 1]);
            lower.Add(ordered[ordered.Count - 2]);
            for (int i = ordered.Count - 3; i >= 0  ; i--)
            {
                lower.Add(ordered[i]);
                int k = lower.Count;
                if (k > 2)
                {
                    var p = lower[k - 3];
                    var r = lower[k - 2];
                    var q = lower[k - 1];
                    if (Utilities2D.IsLeft(p, q, r))
                        lower.RemoveAt(k - 2);
                }
            }

            lower.RemoveAt(lower.Count - 1);
            lower.RemoveAt(0);

            upper.AddRange(lower);

            return new Polygon2D(upper.ToArray());

        }
        public static Polygon2D Contour(XYPT[] points)
        {
            var n = points.Length;
            var edges = new List<HalfSegment2D>();

            for (int i = 0; i < n; i++)
                edges.Add(new HalfSegment2D(points[i]));
            for (int i = 0; i < n; i++)
            {
                var e = edges[i];
                e.From = edges[(i == 0) ? (n - 1) : (i - 1)];
                e.To = edges[(i + 1) % n];
            }
            edges = SplitEdges(edges);

            var result = new List<XYPT>();

            HalfSegment2D start = edges[0];
            result.Add(start.P);

            HalfSegment2D current = start;

            while (true)
            {
                HalfSegment2D from = current, to = current.To;
                HalfSegment2D from2 = to.To, to2 = from2.To;

                bool flag = false;

                while (from2 != start && to2 != from)
                {
                    if (flag = Utilities2D.DoesIntersect(from.P, to.P, from2.P, to2.P))
                        break;
                    from2 = to2;
                    to2 = to2.To;
                }
                if (!flag)
                {
                    result.Add(to.P);
                    current = to;
                }
                else
                {
                    result.Add(from2.P);

                    from.To = from2;
                    from2.To = from;

                    to.From = to2;
                    to.Invert();
                    to2.From = to;

                    HalfSegment2D e = from2;
                    while (e != to)
                    {
                        e.Invert();
                        e = e.To;
                    }

                    current = from2;
                }
                if (current == start) break;
            }
            result.RemoveAt(result.Count - 1);
            return new Polygon2D(result.ToArray());
        }
        private static List<HalfSegment2D> SplitEdges(List<HalfSegment2D> edges)
        {
            HalfSegment2D start = edges[0];
            HalfSegment2D current = start;

            while (true)
            {
                HalfSegment2D from = current, to = from.To;
                HalfSegment2D from2 = to.To, to2 = from2.To;

                int intersections = 0;

                while (to2 != from.From)
                {
                    if (Utilities2D.DoesIntersect(from.P,to.P, from2.P, to2.P))
                    {
                        intersections++;
                        if (intersections >= 2)
                        {
                            break;
                        }
                    }

                    from2 = from2.To;
                    to2 = to2.To;
                }

                if (intersections >= 2)
                    edges.Add(current.Split());
                else
                {
                    current = current.To;
                    if (current == start)
                        break;
                }

            }
            return edges;
        }
        public bool Contains(XYPT p)
        {
            return Utilities2D.Contains(p, _vertices);
        }

    }
}
