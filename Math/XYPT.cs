using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathUtils
{
    [Serializable()]
    public struct XYPT : IComparable
    {

        #region Ctor

        public XYPT(XYPT q)
        {
            this = q;
        }

        public XYPT(double xx, double yy)
        {
            x = xx;
            y = yy;
        }

        public XYPT(float xx, float yy)
        {
            x = xx;
            y = yy;
        }
        #endregion

        #region Public Fields

        public double x;
        public double y;

        #endregion

        #region Public Properties
        public double XX
        {
            get { return x; }
            set { x = value; }
        }

        public double YY
        {
            get { return y; }
            set { y = value; }
        }
        #endregion


        // Vector Operations


        /// <summary>
        /// negative of a point
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static XYPT operator -(XYPT p)
        {
            XYPT q;
            q.x = -p.x;
            q.y = -p.y;
            return q;
        }

        /// <summary>
        /// subtraction of 2 points
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        public static XYPT operator -(XYPT p, XYPT q)
        {
            XYPT r;
            r.x = p.x - q.x;
            r.y = p.y - q.y;
            return r;
        }

        /// <summary>
        ///point itself
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static XYPT operator +(XYPT p)
        {
            return p;
        }

        /// <summary>
        /// addition of 2 points
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        public static XYPT operator +(XYPT p, XYPT q)
        {
            XYPT r;
            r.x = p.x + q.x;
            r.y = p.y + q.y;
            return r;
        }

        /// <summary>
        /// scalar x point
        /// </summary>
        /// <param name="p"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public static XYPT operator *(XYPT p, double m)
        {
            XYPT q;
            q.x = p.x * m;
            q.y = p.y * m;
            return q;
        }

        /// <summary>
        /// point x scalar
        /// </summary>
        /// <param name="m"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static XYPT operator *(double m, XYPT p)
        {
            XYPT q;
            q.x = p.x * m;
            q.y = p.y * m;
            return q;
        }

        /// <summary>
        /// point / scalar
        /// </summary>
        /// <param name="p"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        public static XYPT operator /(XYPT p, double m)
        {
            XYPT q;
            q.x = p.x / m;
            q.y = p.y / m;
            return q;
        }



        /// <summary>
        /// returns true if points are the same
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        public static bool operator ==(XYPT p, XYPT q)
        {
            return (p.x == q.x && p.y == q.y);
        }

        /// <summary>
        /// returns true if points are not the same
        /// </summary>
        /// <param name="p"></param>
        /// <param name="q"></param>
        /// <returns></returns>
        public static bool operator !=(XYPT p, XYPT q)
        {
            return (p.x != q.x || p.y != q.y);
        }

        /// <summary>
        /// assigns a double scalar value to point data
        /// </summary>
        /// <param name="dScalar"></param>
        public static explicit operator XYPT(double dScalar)
        {
            XYPT p;
            p.x = p.y = dScalar;
            return p;
        }

        /// <summary>
        /// assigns a integer scalar value to point data
        /// </summary>
        /// <param name="nScalar"></param>
        public static explicit operator XYPT(int nScalar)
        {
            XYPT p;
            p.x = p.y = nScalar;
            return p;
        }


        /// <summary>
        /// checks if 2 points are the same
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            if (obj is XYPT)
            {
                XYPT xyPoint = (XYPT)obj;
                return (this.x == xyPoint.x
                        && this.y == xyPoint.y);
            }
            else return false;
        }

        // Utilities


        public double Angle()
        {
            return Math.Atan2(y, x);
        }

        public double AngleTo(XYPT p)
        {
            XYPT q = p - this;
            return Math.Atan2(q.y, q.x);
        }
        public double AngleTo02π(XYPT p)
        {
            XYPT q = p - this; double φ = Math.Atan2(q.y, q.x);
            if (φ < 0) φ += Math.PI * 2; return φ;
        }
        public double cross(XYPT q)
        {
            return (x * q.y - y * q.x);
        }

        public double DistOrig()
        {
            return Math.Sqrt(x * x + y * y);
        }

        public double DistTo(double x2, double y2)
        {
            double Δx = x2 - x; double Δy = y2 - y;
            return Math.Sqrt(Δx * Δx + Δy * Δy);
        }
        public double DistTo(XYPT p)
        {
            XYPT q = p - this; return q.Magnitude();
        }
        public double DistTo2(XYPT p)
        {
            XYPT q = p - this; return (q.x * q.x + q.y * q.y);
        }

        public double dot(XYPT q)
        {
            return (x * q.x + y * q.y);
        }

        public bool isfEqual(XYPT q)
        {
            bool bxeq = (x == q.x ? true : false);
            bool byeq = (y == q.y ? true : false);
            return (bxeq && byeq);
        }

        public bool isdoubleEQ(XYPT p2)
        {
            return (((double)this.x == (double)p2.x) && ((double)this.y == (double)p2.y));
        }

        public bool isNearlySameAs(XYPT q, double sepToler)
        {
            return (this.DistTo(q) <= sepToler);
        }


        public void maximize(XYPT q)
        {
            if (this.x < q.x) this.x = q.x;
            if (this.y < q.y) this.x = q.y;
        }
        public void minimize(XYPT q)
        {
            if (q.x < this.x) this.x = q.x;
            if (q.y < this.y) this.x = q.y;
        }
        public double Magnitude()
        {
            return Math.Sqrt(x * x + y * y);
        }

        public XYPT Midpt(XYPT p2)
        {
            XYPT p;
            p.x = 0.5f * (this.x + p2.x);
            p.y = 0.5f * (this.y + p2.y);
            return p;
        }

        public XYPT MidwayPointTo(XYPT p2)
        {
            XYPT p;
            p.x = 0.5f * (this.x + p2.x);
            p.y = 0.5f * (this.y + p2.y);
            return p;
        }

        public void move(XYPT displacement)
        {
            x += displacement.x;
            y += displacement.y;
        }

        public void move(double r, double phi)
        {
            x = x + r * Math.Cos(phi);
            y = y + r * Math.Sin(phi);
        }


        public XYPT Moved(XYPT displacement)
        {
            XYPT asif = this; // asif == "As If"
            asif.x += displacement.x;
            asif.y += displacement.y;
            return asif;
        }

        public XYPT Moved(double r, double phi)
        {
            XYPT asif = this; // asif == "As If"
            asif.x += r * Math.Cos(phi);
            asif.y += r * Math.Sin(phi);
            return asif;
        }


        public bool NearlyEquals(XYPT q, double sepToler)
        {
            return (this.DistTo(q) <= sepToler);
        }

        public double Norm()
        {
            return Math.Sqrt(x * x + y * y);
        }

        public void normalize()
        {
            double d = Math.Sqrt(x * x + y * y);
            if (d == 0)
            {
                x = y = 0;
            }
            else
            {
                x /= d; y /= d;
            }
        }

        /// <summary>
        ///		Rotates p relative (0, 0).
        /// </summary>
        /// <param name="phi">	The angle of rotation (in radians).	</param>
        /// <returns></returns>
        public void rotate(double phi)
        {
            XYPT q;
            double cp = Math.Cos(phi);
            double sp = Math.Sin(phi);
            q.x = (x * cp) - (y * sp);
            q.y = (x * sp) + (y * cp);
            x = q.x;
            y = q.y;
        }

        /// <summary>
        ///		Rotates q relative p.
        /// </summary>
        /// <param name="q">	The point being rotated.			</param>
        /// <param name="phi">	The angle of rotation (in radians).	</param>
        /// <returns></returns>
        public void rotate(ref XYPT q, double phi)
        {
            double cp = Math.Cos(phi);
            double sp = Math.Sin(phi);
            double dx = q.x - x;
            double dy = q.y - y;
            q.x = x + (dx * cp) - (dy * sp);
            q.y = y + (dx * sp) + (dy * cp);
        }

        /// <summary>
        ///		Returns p rotated relative (0, 0).
        /// </summary>
        /// <param name="phi">	The angle of rotation (in radians).	</param>
        /// <returns></returns>
        /// 
        public XYPT Rotated(double phi)
        {
            double cp = Math.Cos(phi);
            double sp = Math.Sin(phi);
            return new XYPT((x * cp) - (y * sp),
                                (x * sp) + (y * cp));
        }

        /// <summary>
        ///		Returns q rotated relative p (origin).
        /// </summary>
        /// <param name="q">	The point being rotated.			</param>
        /// <param name="phi">	The angle of rotation (in radians).	</param>
        /// <returns></returns>
        public XYPT Rotated(XYPT q, double phi)
        {
            double cp = Math.Cos(phi);
            double sp = Math.Sin(phi);
            double dx = q.x - x;
            double dy = q.y - y;
            return new XYPT(x + (dx * cp) - (dy * sp),
                                y + (dx * sp) + (dy * cp));
        }

        /// <summary>
        ///		Rotates p relative q.
        /// </summary>
        /// <param name="q">	The point being rotated.			</param>
        /// <param name="phi">	The angle of rotation (in radians).	</param>
        /// <returns></returns>
        public void rotateRel(XYPT q, double phi)
        {
            double cp = Math.Cos(phi);
            double sp = Math.Sin(phi);
            double dx = x - q.x;
            double dy = y - q.y;
            x = q.x + (dx * cp) - (dy * sp);
            y = q.y + (dx * sp) + (dy * cp);
        }

        public void rotate90ACW() { double t = y; y = x; x = -t; }
        public void rotate90CCW() { double t = y; y = x; x = -t; }
        public void rotate90CW() { double t = x; x = y; y = -t; }
        public void swapXY() { double t = x; x = y; y = t; }
        public void swapWith(ref XYPT q) { XYPT t = this; this = q; q = t; }
        public void zero() { this.x = this.y = 0; }

        public void zeroBelow(double ZeroLevel)
        {
            if (Math.Abs(this.x) < ZeroLevel) this.x = 0;
            if (Math.Abs(this.y) < ZeroLevel) this.y = 0;
        }
        //public static XYPT ZeroZero		()								{ return new XYPT(0,0); }

        public override string ToString() { return (x.ToString() + ", " + y.ToString()); }
        public string ToString(string format) { return (x.ToString(format) + ", " + y.ToString(format)); }

        /// <summary>
        ///		Static property, returns constant (x,y) = (double.MaxValue, double.MaxValue).
        /// </summary>
        public static XYPT MaxValue
        {
            get
            {
                return new XYPT(double.MaxValue, double.MaxValue);
            }
        }

        /// <summary>
        ///		Static property, returns constant (x,y) = (double.MinValue, double.MinValue).
        /// </summary>
        public static XYPT MinValue
        {
            get
            {
                return new XYPT(double.MinValue, double.MinValue);
            }
        }

        /// <summary>
        ///		Static property, returns constant (x,y) = (0,0).
        /// </summary>
        public static XYPT ZeroZero
        {
            get
            {
                return new XYPT(0, 0);
            }
        }

        /// <summary>
        ///		Static property, returns constant (x,y) = (+∞, +∞).
        /// </summary>
        public static XYPT Infinity
        {
            get
            {
                return new XYPT(double.PositiveInfinity, double.PositiveInfinity);
            }
        }

        public static XYPT average(XYPT[] array)
        {
            int n = array.GetLength(0);
            XYPT sum = new XYPT(0, 0);

            if (n <= 0) return sum;

            for (int i = 0; i < n; i++) sum += array[i];
            return (sum / n);
        }

        public static XYPT average(List<XYPT> listXYPT)
        {
            int n = listXYPT.Count;
            XYPT sum = new XYPT(0, 0);

            if (n <= 0) return sum;

            for (int i = 0; i < n; i++) sum += listXYPT[i];
            return (sum / n);
        }

        public static XYPT sum(XYPT[] array)
        {
            int n = array.GetLength(0);
            XYPT s = new XYPT(0, 0);

            if (n <= 0) return s;

            for (int i = 0; i < n; i++) s += array[i];
            return s;
        }

        public static XYPT sum(List<XYPT> listXYPT)
        {
            int n = listXYPT.Count;
            XYPT s = new XYPT(0, 0);

            if (n <= 0) return s;

            for (int i = 0; i < n; i++) s += listXYPT[i];
            return s;
        }

        /// <summary>
        ///		Gets Minimum of ( x, y )
        /// </summary>
        public double minOfXY
        {
            get
            {
                if (this.x <= this.y) return this.x;
                else return this.y;
            }
        }
        /// <summary>
        ///		Gets Maxmimum of ( x, y )
        /// </summary>
        public double maxOfXY
        {
            get
            {
                if (this.x >= this.y) return this.x;
                else return this.y;
            }
        }

        /// <summary>
        ///		<para> Imagine a list of column vectors, each bearing a list of XY-points, essentially a 2d matrix.	</para>
        ///		<para> This utility averages points in the same row.												</para>
        /// </summary>
        /// <param name="list"> A list of lists of XYPTs </param>
        /// <returns></returns>
        public static XYPT[] averageVectors(List<List<XYPT>> list)
        {
            /*	Example:
             * 
             *	List[0]		List[1]		List[2]					XYPT[] Array
             *	
             *	List[0][0]		[1][0]		[2][0]		==>		[0] = ( [0][0] + [1][0] + [2][0] ) / 3
             *		[0][1]		[1][1]		[2][1]		==>		[1] = ( [0][1] + [1][1] + [2][1] ) / 3
             *		[0][2]		[1][2]		[2][2]		==>		[2] = ( [0][2] + [1][2] + [2][2] ) / 3
             *		[0][3]		[1][3]		[2][3]		==>		[3] = ( [0][3] + [1][3] + [2][3] ) / 3
             *		[0][4]		[1][4]		[2][4]		==>		[4] = ( [0][4] + [1][4] + [2][4] ) / 3
             */
            int mVectors = list.Count;
            XYPT[] sum = new XYPT[0];
            int nRowsMax = 0;
            int[] n;

            if (mVectors <= 0) return sum;

            for (int iRow = 0; iRow < mVectors; iRow++)
                if (nRowsMax < list[iRow].Count) nRowsMax = list[iRow].Count;

            // Create the array of sums
            sum = new XYPT[nRowsMax];
            if (nRowsMax <= 0) return sum;
            n = new int[nRowsMax];


            for (int iRow = 0; iRow < nRowsMax; iRow++)
            {
                sum[iRow] = XYPT.ZeroZero;                      // Zero the sums.
                for (int jCol = 0; jCol < mVectors; jCol++)
                {
                    int nXYPTs = list[jCol].Count;
                    if (1 <= nXYPTs && nXYPTs <= nRowsMax)
                    {
                        n[iRow]++;
                        sum[iRow] += list[jCol][iRow];
                    }
                }
            }
            for (int iRow = 0; iRow < nRowsMax; iRow++) sum[iRow] /= n[iRow];

            return sum;

        }

        /// <summary>
        ///		<para> Imagine a list of column vectors, each bearing a list of XY-points, essentially a 2d matrix.	</para>
        ///		<para> This utility averages y values in the same row.												</para>
        /// </summary>
        /// <param name="list"> A list of lists of XYPTs </param>
        /// <returns></returns>
        public static double[] averageVectorYs(List<List<XYPT>> list)
        {
            /*	Example:
             * 
             *	List[0]		List[1]		List[2]					double[] Array
             *	List[0][0]		[1][0]		[2][0]		==>		[0] = ( [0][0].y + [1][0].y + [2][0].y ) / 3
             *		[0][1]		[1][1]		[2][1]		==>		[1] = ( [0][1].y + [1][1].y + [2][1].y ) / 3
             *		[0][2]		[1][2]		[2][2]		==>		[2] = ( [0][2].y + [1][2].y + [2][2].y ) / 3
             *		[0][3]		[1][3]		[2][3]		==>		[3] = ( [0][3].y + [1][3].y + [2][3].y ) / 3
             *		[0][4]		[1][4]		[2][4]		==>		[4] = ( [0][4].y + [1][4].y + [2][4].y ) / 3
             */
            int mVectors = list.Count;
            double[] sum = new double[0];
            int nRowsMax = 0;
            int[] n;

            if (mVectors <= 0) return sum;

            for (int iRow = 0; iRow < mVectors; iRow++)
                if (nRowsMax < list[iRow].Count) nRowsMax = list[iRow].Count;

            // Create the array of sums
            sum = new double[nRowsMax];
            if (nRowsMax <= 0) return sum;
            n = new int[nRowsMax];


            for (int iRow = 0; iRow < nRowsMax; iRow++)
            {
                sum[iRow] = 0;                              // Zero the sums.
                for (int jCol = 0; jCol < mVectors; jCol++)
                {
                    int nXYPTs = list[jCol].Count;
                    if (1 <= nXYPTs && nXYPTs <= nRowsMax)
                    {
                        n[iRow]++;
                        sum[iRow] += list[jCol][iRow].y;
                    }
                }
            }
            for (int iRow = 0; iRow < nRowsMax; iRow++) sum[iRow] /= n[iRow];

            return sum;

        } 

        /// <summary>
        ///		<para> Imagine a list of column vectors, each bearing a list of XY-points, essentially a 2d matrix.	</para>
        ///		<para> This utility returns the largest X value in each row.										</para>
        /// </summary>
        /// <param name="list"> A list of lists of XYPTs </param>
        /// <returns></returns>
        public static double[] MaximumVectorXs(List<List<XYPT>> list)
        {
            /*	Example:
             * 
             *	List[0]		List[1]		List[2]					double[] Array
             *	List[0][0]		[1][0]		[2][0]		==>		[0] = Max{ [0][0].x,  [1][0].x,  [2][0].x }
             *		[0][1]		[1][1]		[2][1]		==>		[1] = Max{ [0][1].x,  [1][1].x,  [2][1].x }
             *		[0][2]		[1][2]		[2][2]		==>		[2] = Max{ [0][2].x,  [1][2].x,  [2][2].x }
             *		[0][3]		[1][3]		[2][3]		==>		[3] = Max{ [0][3].x,  [1][3].x,  [2][3].x }
             *		[0][4]		[1][4]		[2][4]		==>		[4] = Max{ [0][4].x,  [1][4].x,  [2][4].x }
             */
            int mVectors = list.Count;
            double[] maximumX = new double[0];
            int nRowsMax = 0;

            if (mVectors <= 0) return maximumX;

            for (int iRow = 0; iRow < mVectors; iRow++)
                if (nRowsMax < list[iRow].Count) nRowsMax = list[iRow].Count;

            // Create the array of sums
            maximumX = new double[nRowsMax];
            if (nRowsMax <= 0) return maximumX;


            for (int iRow = 0; iRow < nRowsMax; iRow++)
            {
                maximumX[iRow] = -double.MaxValue;              // Set to the largest negative double.
                for (int jCol = 0; jCol < mVectors; jCol++)
                {
                    int nXYPTs = list[jCol].Count;
                    if (1 <= nXYPTs && nXYPTs <= nRowsMax)
                        if (maximumX[iRow] < list[jCol][iRow].x) maximumX[iRow] = list[jCol][iRow].x;
                }
            }

            return maximumX;

        } 
        
        /// <summary>
        ///		<para> Imagine a list of column vectors, each bearing a list of XY-points, essentially a 2d matrix.	</para>
        ///		<para> This utility returns the largest Y value in each row.										</para>
        /// </summary>
        /// <param name="list"> A list of lists of XYPTs </param>
        /// <returns></returns>
        /// 
        public static double[] MaximumVectorYs(List<List<XYPT>> list)
        {
            /*	Example:
             * 
             *	List[0]		List[1]		List[2]					double[] Array
             *	List[0][0]		[1][0]		[2][0]		==>		[0] = Max{ [0][0].y,  [1][0].y,  [2][0].y }
             *		[0][1]		[1][1]		[2][1]		==>		[1] = Max{ [0][1].y,  [1][1].y,  [2][1].y }
             *		[0][2]		[1][2]		[2][2]		==>		[2] = Max{ [0][2].y,  [1][2].y,  [2][2].y }
             *		[0][3]		[1][3]		[2][3]		==>		[3] = Max{ [0][3].y,  [1][3].y,  [2][3].y }
             *		[0][4]		[1][4]		[2][4]		==>		[4] = Max{ [0][4].y,  [1][4].y,  [2][4].y }
             */
            int mVectors = list.Count;
            double[] maximumY = new double[0];
            int nRowsMax = 0;

            if (mVectors <= 0) return maximumY;

            for (int iRow = 0; iRow < mVectors; iRow++)
                if (nRowsMax < list[iRow].Count) nRowsMax = list[iRow].Count;

            // Create the array of sums
            maximumY = new double[nRowsMax];
            if (nRowsMax <= 0) return maximumY;


            for (int iRow = 0; iRow < nRowsMax; iRow++)
            {
                maximumY[iRow] = -double.MaxValue;              // Set to the largest negative double.
                for (int jCol = 0; jCol < mVectors; jCol++)
                {
                    int nXYPTs = list[jCol].Count;
                    if (1 <= nXYPTs && nXYPTs <= nRowsMax)
                        if (maximumY[iRow] < list[jCol][iRow].y) maximumY[iRow] = list[jCol][iRow].y;
                }
            }

            return maximumY;

        } 

        /// <summary>
        ///		<para> Imagine a list of column vectors, each bearing a list of XY-points, essentially a 2d matrix.	</para>
        ///		<para> This utility returns the largest X and largest Y value in each row.							</para>
        /// </summary>
        /// <param name="list"> A list of lists of XYPTs </param>
        /// <returns></returns>
        public static XYPT[] MaximumVector(List<List<XYPT>> list)
        {
            /*	Example:
             * 
             *	List[0]		List[1]		List[2]					double[] Array
             *	List[0][0]		[1][0]		[2][0]		==>		[0].x = Max{ [0][0].x,  [1][0].x,  [2][0].x };   [0].y = Max{ [0][0].y,  [1][0].y,  [2][0].y }
             *		[0][1]		[1][1]		[2][1]		==>		[1].x = Max{ [1][0].x,  [1][1].x,  [2][1].x };   [1].y = Max{ [0][1].y,  [1][1].y,  [2][1].y }
             *		[0][2]		[1][2]		[2][2]		==>		[2].x = Max{ [2][0].x,  [1][2].x,  [2][2].x };   [2].y = Max{ [0][2].y,  [1][2].y,  [2][2].y }
             *		[0][3]		[1][3]		[2][3]		==>		[3].x = Max{ [3][0].x,  [1][3].x,  [2][3].x };   [3].y = Max{ [0][3].y,  [1][3].y,  [2][3].y }
             *		[0][4]		[1][4]		[2][4]		==>		[4].x = Max{ [4][0].x,  [1][4].x,  [2][4].x };   [4].y = Max{ [0][4].y,  [1][4].y,  [2][4].y }
             */
            int mVectors = list.Count;
            XYPT[] maximum = new XYPT[0];
            int nRowsMax = 0;

            if (mVectors <= 0) return maximum;

            for (int iRow = 0; iRow < mVectors; iRow++)
                if (nRowsMax < list[iRow].Count) nRowsMax = list[iRow].Count;

            // Create the array of sums
            maximum = new XYPT[nRowsMax];
            if (nRowsMax <= 0) return maximum;


            for (int iRow = 0; iRow < nRowsMax; iRow++)
            {
                maximum[iRow] = -XYPT.MaxValue;             // Set to the largest negative double.
                for (int jCol = 0; jCol < mVectors; jCol++)
                {
                    int nXYPTs = list[jCol].Count;
                    if (1 <= nXYPTs && nXYPTs <= nRowsMax)
                    {
                        XYPT p = list[jCol][iRow];
                        if (maximum[iRow].x < p.x) maximum[iRow].x = p.x;
                        if (maximum[iRow].y < p.y) maximum[iRow].y = p.y;
                    }
                }

            } // for( int iRow = 0 ; iRow < nRowsMax ; iRow++ )

            return maximum;

        } // public static double[] MaximumVectorYs( List<List<XYPT>> list )

        public int CompareTo(object obj)
        {
            if (obj.GetType() == this.GetType())
            {
                return 1;
            }
            return 0;
        }
    }

}
