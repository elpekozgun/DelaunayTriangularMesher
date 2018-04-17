using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using Engine;
using MathEntities;
using MathUtils;

namespace TriangularMesher
{
    public partial class frmUI: Form
    {

        public frmUI()
        {
            InitializeComponent();
            PrepareUI();
            SubscribeEvents();
        }

        private void PrepareUI()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            CenterToScreen();
            this.BackColor = Color.FromArgb(255, 60, 60, 60);
            canvas.BackColor = Color.FromArgb(255, 30, 30, 30);
            this.KeyPreview = true;
        }
         
        private void SubscribeEvents()
        {
            canvas.Paint += Canvas_Paint;
        }

        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighSpeed;
            var g = e.Graphics;

            Width = canvas.ClientSize.Width;
            Height = canvas.ClientSize.Height;


            //DrawCalls
            DrawVertices(g);
        }

        private void DrawVertices(Graphics g)
        {
            var poly = new List<XYPT>();

            poly.Add(new XYPT(0, 0));
            poly.Add(new XYPT(5, 0));
            poly.Add(new XYPT(5, 5));
            poly.Add(new XYPT(0, 5));

            var pt = new XYPT(2, 2);

            Utilities2D.Contains(pt, poly);


            g.TranslateTransform(60, -200);
            g.ScaleTransform(2.5f, 2.5f);

            List<XYPT> listLeft = new List<XYPT>();
            listLeft.Add(new XYPT(100, 100));
            listLeft.Add(new XYPT(150, 100));
            listLeft.Add(new XYPT(150, 125));
            listLeft.Add(new XYPT(125, 125));
            listLeft.Add(new XYPT(125, 135));
            listLeft.Add(new XYPT(145, 135));
            listLeft.Add(new XYPT(145, 165));
            listLeft.Add(new XYPT(125, 165));
            listLeft.Add(new XYPT(125, 175));
            listLeft.Add(new XYPT(150, 175));
            listLeft.Add(new XYPT(150, 200));
            listLeft.Add(new XYPT(100, 200));

            var listRight = new List<XYPT>();
            listRight.Add(new XYPT(150, 100));
            listRight.Add(new XYPT(200, 100));
            //listRight.Add(new XYPT(190, 110));  //
            //listRight.Add(new XYPT(200, 110));  //
            listRight.Add(new XYPT(200, 200));
            //listRight.Add(new XYPT(190, 190));  //
            //listRight.Add(new XYPT(200, 200));  //
            listRight.Add(new XYPT(150, 200));
            //listRight.Add(new XYPT(150, 190));  //
            listRight.Add(new XYPT(150, 175));
            listRight.Add(new XYPT(175, 175));
            listRight.Add(new XYPT(175, 165));
            listRight.Add(new XYPT(155, 165));
            listRight.Add(new XYPT(155, 135));
            listRight.Add(new XYPT(175, 135));
            listRight.Add(new XYPT(175, 125));
            listRight.Add(new XYPT(150, 125));
            //listRight.Add(new XYPT(150, 110));  //

            var listProfile = new List<XYPT>();
            listProfile.Add(new XYPT(125,125));
            listProfile.Add(new XYPT(175,125));
            listProfile.Add(new XYPT(175,135));
            listProfile.Add(new XYPT(155,135));
            listProfile.Add(new XYPT(155,165));
            listProfile.Add(new XYPT(175,165));
            listProfile.Add(new XYPT(175,175));
            listProfile.Add(new XYPT(125,175));
            listProfile.Add(new XYPT(125,165));
            listProfile.Add(new XYPT(145,165));
            listProfile.Add(new XYPT(145,135));
            listProfile.Add(new XYPT(125,135));

            var leftReinfs = new List<XYPT>();
            leftReinfs.Add(new XYPT(110, 110));
            leftReinfs.Add(new XYPT(150, 110));
            leftReinfs.Add(new XYPT(110, 190));
            leftReinfs.Add(new XYPT(150, 190));

            var rightReinfs = new List<XYPT>();
            rightReinfs.Add(new XYPT(150, 110));
            rightReinfs.Add(new XYPT(190, 110));
            rightReinfs.Add(new XYPT(150, 190));
            rightReinfs.Add(new XYPT(190, 190));

            //listRight.AddRange(rightReinfs);
            Polygon2D rightPolygon = new Polygon2D(listRight.ToArray());
            Triangulation2D trianglesRight = new Triangulation2D(rightPolygon, 35, 5);

            //listLeft.AddRange(leftReinfs);
            Polygon2D leftPolygon = new Polygon2D(listLeft.ToArray());
            Triangulation2D trianglesLeft = new Triangulation2D(leftPolygon, 35, 5);

            Polygon2D profilePolygon = new Polygon2D(listProfile.ToArray());
            Triangulation2D trianglesProfile = new Triangulation2D(profilePolygon, 35, 5);

            foreach (var triangle in trianglesLeft.Triangles)
            {
                g.DrawLine(new Pen(Color.Red, 0.2f), new PointF((float)triangle.A.Coordinate.x, (float)triangle.A.Coordinate.y), new PointF((float)triangle.B.Coordinate.x, (float)triangle.B.Coordinate.y));
                g.DrawLine(new Pen(Color.Red, 0.2f), new PointF((float)triangle.B.Coordinate.x, (float)triangle.B.Coordinate.y), new PointF((float)triangle.C.Coordinate.x, (float)triangle.C.Coordinate.y));
                g.DrawLine(new Pen(Color.Red, 0.2f), new PointF((float)triangle.C.Coordinate.x, (float)triangle.C.Coordinate.y), new PointF((float)triangle.A.Coordinate.x, (float)triangle.A.Coordinate.y));
            }

            foreach (var triangle in trianglesRight.Triangles)
            {
                g.DrawLine(new Pen(Color.Red, 0.2f), new PointF((float)triangle.A.Coordinate.x, (float)triangle.A.Coordinate.y), new PointF((float)triangle.B.Coordinate.x, (float)triangle.B.Coordinate.y));
                g.DrawLine(new Pen(Color.Red, 0.2f), new PointF((float)triangle.B.Coordinate.x, (float)triangle.B.Coordinate.y), new PointF((float)triangle.C.Coordinate.x, (float)triangle.C.Coordinate.y));
                g.DrawLine(new Pen(Color.Red, 0.2f), new PointF((float)triangle.C.Coordinate.x, (float)triangle.C.Coordinate.y), new PointF((float)triangle.A.Coordinate.x, (float)triangle.A.Coordinate.y));
            }

            foreach (var triangle in trianglesProfile.Triangles)
            {
                g.DrawLine(new Pen(Color.Green, 0.2f), new PointF((float)triangle.A.Coordinate.x, (float)triangle.A.Coordinate.y), new PointF((float)triangle.B.Coordinate.x, (float)triangle.B.Coordinate.y));
                g.DrawLine(new Pen(Color.Green, 0.2f), new PointF((float)triangle.B.Coordinate.x, (float)triangle.B.Coordinate.y), new PointF((float)triangle.C.Coordinate.x, (float)triangle.C.Coordinate.y));
                g.DrawLine(new Pen(Color.Green, 0.2f), new PointF((float)triangle.C.Coordinate.x, (float)triangle.C.Coordinate.y), new PointF((float)triangle.A.Coordinate.x, (float)triangle.A.Coordinate.y));
            }

            foreach (var item in rightReinfs)
                g.DrawEllipse(new Pen(Color.Cyan, 0.2f),new Rectangle((int)item.x - 2,(int)item.y - 2 ,4,4));
            foreach (var item in leftReinfs)
                g.DrawEllipse(new Pen(Color.Cyan, 0.2f), new Rectangle((int)item.x - 2, (int)item.y - 2, 4, 4));


        }

    }
}
