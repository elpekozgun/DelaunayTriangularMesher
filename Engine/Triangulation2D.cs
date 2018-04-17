using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathEntities;
using MathUtils;

namespace Engine
{
    /// <summary>
    /// Based on the Ruppert's Delaunay Refinement Algorithm.
    /// </summary>
    public class Triangulation2D
    {
        const float ANGLE_MIN = 15f;
        const float ANGLE_MAX = 35f;

        private List<Triangle2D> _triangles = new List<Triangle2D>();
        private Polygon2D _pslg;
        private List<Vertex2D> _ver = new List<Vertex2D>();
        private List<Segment2D> _seg = new List<Segment2D>();
        private List<Vertex2D> _pVer = new List<Vertex2D>();
        private List<Segment2D> _pSeg = new List<Segment2D>();

        public Triangle2D[] Triangles
        {
            get
            {
                return _triangles.ToArray();
            }
        }

        public Triangulation2D(Polygon2D polygon, float angle = 25f , float threshold = 1f)
        {
            if (angle > ANGLE_MAX)
                angle = ANGLE_MAX;
            else if (angle < ANGLE_MIN)
                angle = ANGLE_MIN;

            _pslg = polygon;
            _ver = _pslg.Vertices.ToList();
            _seg = _pslg.Segments.ToList();

            Triangulate(polygon.Vertices.Select(v => v.Coordinate).ToArray(), angle, threshold);
        }


        private bool PointOnLineSegment(XYPT pt1, XYPT pt2, XYPT pt, double epsilon = 0.001)
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
        public List<XYPT> GetPointsOnBoundary()
        {
            var list = GetTrianglePointList();
            List<XYPT> result = new List<XYPT>();
            foreach (var p in _pslg.Segments)
            {
                foreach (var point in list)
                {
                    if (PointOnLineSegment(p.StartPt.Coordinate, p.EndPt.Coordinate, point))
                    {
                        result.Add(point);
                    }
                }
            }
            result = result.Distinct().ToList();
            return result;
        }
        private List<XYPT> GetTrianglePointList()
        {
            List<XYPT> list = new List<XYPT>();
            foreach (var triangle in _triangles)
            {
                list.Add(triangle.A.Coordinate);
                list.Add(triangle.B.Coordinate);
                list.Add(triangle.C.Coordinate);
            }
            list = list.Distinct().ToList();
            return list;
        }

        private void Triangulate(XYPT[] points, float angle, float threshold)
        {
            XYPT min, max;
            Bound(points, out min, out max);

            AddExternalTriangle(min, max);

            for (int i = 0; i < points.Length; i++)
            {
                var v = points[i];
                UpdateTriangulation(v);
            }

            Refine(angle, threshold);
            RemoveExternalPSLG();
        }
        private void Bound(XYPT[] points, out XYPT min, out XYPT max)
        {
            min = new XYPT(1, 1) * float.MaxValue;
            max = new XYPT(1, 1) * float.MinValue;
            for (int i = 0; i < points.Length; i++)
            {
                var p = points[i];
                min.x = Math.Min(min.x, p.x);
                min.y = Math.Min(min.y, p.y);
                max.x = Math.Max(max.x, p.x);
                max.y = Math.Max(max.y, p.y);
            }
        }
        private Triangle2D AddExternalTriangle(XYPT min , XYPT max)
        {
            var center = (max + min) * 0.5f;
            var diagonal = max.DistTo(min);
            var dh = diagonal * 0.5f;
            var rdh = Math.Sqrt(3f) * dh;
            return AddTriangle(CheckAndAddVertex(center + new XYPT(-rdh, -dh) * 3f),
                               CheckAndAddVertex(center + new XYPT(rdh, -dh) * 3f),
                               CheckAndAddVertex(center + new XYPT(0f, diagonal) * 3f));
        }
        private Triangle2D AddTriangle(Vertex2D a, Vertex2D b, Vertex2D c)
        {
            var s0 = CheckAndAddSegment(a, b);
            var s1 = CheckAndAddSegment(b, c);
            var s2 = CheckAndAddSegment(c, a);
            var t = new Triangle2D(s0, s1, s2);
            _triangles.Add(t);
            return t;
        }
        private Segment2D CheckAndAddSegment(Vertex2D a, Vertex2D b)
        {
            var idx = FindSegment(a, b, _pSeg);
            Segment2D s;
            if (idx < 0)
            {
                s = new Segment2D(a, b);
                _pSeg.Add(s);
            }
            else
            {
                s = _pSeg[idx];
            }
            s.Increase();
            return s;
        }
        private Vertex2D CheckAndAddVertex(XYPT coord)
        {
            var idx = FindVertex(coord, _pVer);
            if (idx <0)
            {
                var v = new Vertex2D(coord);
                _pVer.Add(v);
                return v;
            }
            return _pVer[idx];
        }
        private int FindSegment(Vertex2D a, Vertex2D b, List<Segment2D> segments)
        {
            return segments.FindIndex(s => s.StartPt == a && s.EndPt == b || (s.StartPt == b && s.EndPt == a));
        }
        private int FindVertex(XYPT p, List<Vertex2D> vertices)
        {
            return vertices.FindIndex(v => v.Coordinate == p);
        }
        private void UpdateTriangulation(XYPT p)
        {
            var tmpT = new List<Triangle2D>();
            var tmpS = new List<Segment2D>();

            var v = CheckAndAddVertex(p);
            tmpT = _triangles.FindAll(t => t.ContainsInExternalCircle(v));
            foreach (var t in tmpT)
            {
                tmpS.Add(t.S0);
                tmpS.Add(t.S1);
                tmpS.Add(t.S2);

                AddTriangle(t.A, t.B, v);
                AddTriangle(t.B, t.C, v);
                AddTriangle(t.C, t.A, v);
                RemoveTriangle(t);
            }

            while(tmpS.Count != 0)
            {
                var s = tmpS.Last();
                tmpS.RemoveAt(tmpS.Count - 1);

                var commonTri = _triangles.FindAll(t => t.HasSegment(s));
                if (commonTri.Count <= 1)
                    continue;
                var abc = commonTri[0];
                var abd = commonTri[1];

                if (abc.Equals(abd))
                {
                    RemoveTriangle(abc);
                    RemoveTriangle(abd);
                    continue;
                }

                var a = s.StartPt;
                var b = s.EndPt;
                var c = abc.ExcludePoint(s);
                var d = abd.ExcludePoint(s);

                var ec = Circle2D.GetCircumscribedCircle(abc);

                if (ec.ContainsPoint(d.Coordinate))
                {
                    RemoveTriangle(abc);
                    RemoveTriangle(abd);

                    AddTriangle(a, c, d);
                    AddTriangle(b, c, d);

                    var segment0 = abc.ExcludeSegment(s);
                    tmpS.Add(segment0[0]);
                    tmpS.Add(segment0[1]);

                    var segment1 = abd.ExcludeSegment(s);
                    tmpS.Add(segment1[0]);
                    tmpS.Add(segment1[1]);
                }
            }

        }
        private void RemoveTriangle(Triangle2D t)
        {
            _triangles.Remove(t);
            if (t.S0.Decrease() <= 0)
                RemoveSegment(t.S0);
            if (t.S1.Decrease() <= 0)
                RemoveSegment(t.S1);
            if (t.S2.Decrease() <= 0)
                RemoveSegment(t.S2);
        }
        private void RemoveSegment(Segment2D s)
        {
            _pSeg.Remove(s);
            if (s.StartPt.Reference <= 0)
                _pVer.Remove(s.StartPt);
            if (s.EndPt.Reference <= 0)
                _pVer.Remove(s.EndPt);
        }
        private void Refine(float angle, float threshold)
        {
            while (_triangles.Any(t => !ExternalPSLG(t) && t.IsSkinny(angle, threshold)))
                RefineSubRoutine(angle, threshold);
        }
        private void RefineSubRoutine(float angle, float threshold)
        {
            while (true)
            {
                if (!FindAndSplit(threshold))
                    break;
            }

            var skinny = _triangles.Find(t => !ExternalPSLG(t) && t.IsSkinny(angle, threshold));
            if (skinny != null)
            {
                var p = skinny.GetCircumcenter();

                var segments = _seg.FindAll(s => s.HasEncroached(p));
                if (segments.Count > 0)
                    segments.ForEach(s => SplitSegment(s));
                else
                    SplitTriangle(skinny);
            }
        }
        private void SplitSegment(Segment2D s)
        {
            Vertex2D a = s.StartPt;
            Vertex2D b = s.EndPt;

            var mv = new Vertex2D(s.GetMidPoint());

            var idxA = _ver.IndexOf(a);
            var idxB = _ver.IndexOf(b);
            if (Math.Abs(idxA - idxB) == 1)
            {
                var idx = (idxA > idxB) ? idxA : idxB;
                _ver.Insert(idx, mv);
            }
            else
                _ver.Add(mv);

            UpdateTriangulation(mv.Coordinate);

            var sidx = _seg.IndexOf(s);
            _seg.RemoveAt(sidx);

            _seg.Add(new Segment2D(s.StartPt, mv));
            _seg.Add(new Segment2D(mv,  s.EndPt));

        }
        private void SplitTriangle(Triangle2D t)
        {
            var c = t.GetCircumcenter();
            UpdateTriangulation(c);
        }
        private bool FindAndSplit(float threshold)
        {
            for (int i = 0; i < _seg.Count; i++)
            {
                var s = _seg[i];
                if (s.Length < threshold) continue;

                for (int j = 0, m = _pVer.Count; j < m; j++)
                {
                    if (s.HasEncroached(_pVer[j].Coordinate))
                    {
                        SplitSegment(s);
                        return true;
                    }
                }
            }
            return false;
        }
        private bool ExternalPSLG(XYPT p)
        {
            return !Utilities2D.Contains(p, _ver);
        }
        private bool ExternalPSLG(Segment2D s)
        {
            return ExternalPSLG(s.GetMidPoint());
        }
        private bool ExternalPSLG(Triangle2D t)
        {
            return ExternalPSLG(t.A.Coordinate) ||
                   ExternalPSLG(t.B.Coordinate) ||
                   ExternalPSLG(t.C.Coordinate);
        }
        private void RemoveExternalPSLG()
        {
            for (int i = 0, n = _triangles.Count; i < n ; i++)
            {
                var t = _triangles[i];
                if (ExternalPSLG(t) || HasOuterSegments(t))
                {
                    RemoveTriangle(t);
                    i--;
                    n--;
                }
            }
        }

        private bool HasOuterSegments(Triangle2D t)
        {
            if (!ContainsSegment(t.S0,_seg))
            {
                return ExternalPSLG(t.S0);
            }
            if (!ContainsSegment(t.S1, _seg))
            {
                return ExternalPSLG(t.S1);
            }
            if (!ContainsSegment(t.S2, _seg))
            {
                return ExternalPSLG(t.S2);
            }
            return false;
        }
        
        private bool ContainsSegment(Segment2D s, List<Segment2D> segments)
        {
            return segments.FindIndex(ss => 
            (ss.StartPt.Coordinate == s.StartPt.Coordinate && ss.EndPt.Coordinate == s.EndPt.Coordinate) ||
            (ss.StartPt.Coordinate == s.EndPt.Coordinate && ss.EndPt.Coordinate == s.StartPt.Coordinate)) >= 0;
        
            }


    }
}
