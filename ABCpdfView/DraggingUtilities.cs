// ===========================================================================
//	©2013-2024 WebSupergoo. All rights reserved.
//
//	This source code is for use exclusively with the ABCpdf product with
//	which it is distributed, under the terms of the license for that
//	product. Details can be found at
//
//		http://www.websupergoo.com/
//
//	This copyright notice must not be deleted and must be reproduced alongside
//	any sections of code extracted from this module.
// ===========================================================================

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ABCpdfControls {
	/// <summary>
	/// Summary description for DraggingUtils.
	/// </summary>
	internal class DraggingUtilities {
		private const int kDragWindowMimimumSize = 10;	

		/// <summary>
		/// Object returned by the GetDragWindowSize() Method.
		/// </summary>
		internal struct DragWindowInfo {
			/// <summary>
			/// Creates a new object with the size, and point already initialized.
			/// </summary>
			/// <param name="size">Size of the drag window drawn.</param>
			/// <param name="topLeft">Top left point of the drag window created.</param>
			public DragWindowInfo(Size size, Point topLeft) {
				WindowSize = size;
				TopLeft = topLeft;
			}

			/// <summary>
			/// Size of the drag window drawn.
			/// </summary>
			public Size WindowSize;
			/// <summary>
			/// Top left point of the drag window created.
			/// </summary>
			public Point TopLeft;

			public Rectangle GetRectangle() {
				Rectangle theRect = new Rectangle(TopLeft, WindowSize);
				return theRect;
			}
		}

		/// <summary>
		/// Gets the size of the drag window drawn by the user.
		/// </summary>
		/// <param name="dragCurrent">The current position of the curser.</param>
		/// <param name="dragStart">The position of the cursor where the drag was started.</param>
		/// <returns>Drag Window Information.</returns>
		static public DragWindowInfo GetDragWindowSize(Point dragCurrent, Point dragStart) {
			Point topLeft	  = new Point(0,0);
			Point bottomRight = new Point(0,0);
			
			if ( dragStart.X < dragCurrent.X ) {
			 	// Dragging to the right.
				if ( dragStart.Y < dragCurrent.Y ) {
				 	// Right & Down
					topLeft		= dragStart;
					bottomRight = dragCurrent;
				}
				else {
				 	// Right & Up
					topLeft		= new Point(dragStart.X, dragCurrent.Y);
					bottomRight = new Point(dragCurrent.X, dragStart.Y);
				}
			}
			else if ( dragCurrent.X < dragStart.X ) {
			 	// Dragging to the left.
				if ( dragStart.Y < dragCurrent.Y ) {
				 	// Left & Down
					topLeft		= new Point(dragCurrent.X, dragStart.Y);
					bottomRight	= new Point(dragStart.X, dragCurrent.Y);
				}
				else {
				 	// Left & Up
					topLeft		= dragCurrent;
					bottomRight = dragStart;
				}
			}

			int width	= bottomRight.X - topLeft.X;
			int height	= bottomRight.Y - topLeft.Y;

			DragWindowInfo dwi = new DragWindowInfo(new Size(width, height), topLeft);

			return dwi;
		}

		/// <summary>
		/// Make sure that dragged rect is big enough
		/// </summary>
		/// <param name="dragStart">Start dragging point</param>
		/// <param name="dragEnd">End dragging point</param>
		/// <returns>True if height and witdh of the rectangle are bigger than kDragWindowMimimumSize</returns>
		static public bool CheckDragBoxSize(Point dragStart, Point dragEnd) {
			DragWindowInfo dwi = GetDragWindowSize(dragEnd, dragStart);

			Size size = dwi.WindowSize;
            
			int height = size.Height;
			int width  = size.Width;

			if ( height > kDragWindowMimimumSize && width > kDragWindowMimimumSize )
				return true;
			else
				return false;
		}

		/// <summary>
		/// Invert 3-dimensional matrix
		/// </summary>
		/// <param name="a">Original matrix</param>
		/// <param name="r">on output: Inverted matrix</param>
		static void matrix3Invert(float[,] a, float[,] r) {
			float a11 = a[0,0],a12=a[0,1],a13=a[0,2];
			float a21 = a[1,0],a22=a[1,1],a23=a[1,2];
			float a31 = a[2,0],a32=a[2,1],a33=a[2,2];
			float d =(a11*a22*a33-a11*a23*a32-a21*a12*a33+a21*a13*a32+a31*a12*a23-a31*a13*a22);
			if (d != 0)
				r[0,0]= (a22*a33-a23*a32)/d;
			r[0,1]=-(a12*a33-a13*a32)/d;
			r[0,2]= (a12*a23-a13*a22)/d;
			r[1,0]=-(a21*a33-a23*a31)/d;
			r[1,1]= (a11*a33-a13*a31)/d;
			r[1,2]=-(a11*a23-a13*a21)/d;
			r[2,0]= (a21*a32-a22*a31)/d;
			r[2,1]=-(a11*a32-a12*a31)/d;
			r[2,2]= (a11*a22-a12*a21)/d;
		}

		static public Matrix GetDraggingMatrix(Point dragNewPoint, GraphicsPath path, int draggingPointIndex) {

			PointF[] points = path.PathPoints;

			//A bit of mathematics to calculate transformation factors
			PointF pointA = points[(draggingPointIndex + 2) % 4];
			PointF pointB = points[(draggingPointIndex + 1) % 4];
			PointF pointC = points[(draggingPointIndex + 3) % 4];
			PointF pointM  = dragNewPoint;

			//Calculate new points positions
			float ub = ((pointA.X - pointM.X)*(pointC.Y - pointA.Y) - (pointA.Y - pointM.Y)*(pointC.X - pointA.X)) /
				((pointB.Y - pointA.Y)*(pointC.X - pointA.X) - (pointB.X - pointA.X)*(pointC.Y - pointA.Y));

			PointF pointBnew = new PointF(pointA.X + (pointB.X - pointA.X )*ub, pointA.Y + (pointB.Y - pointA.Y )*ub);

			float uc = ((pointA.X - pointM.X)*(pointB.Y - pointA.Y) - (pointA.Y - pointM.Y)*(pointB.X - pointA.X)) /
				((pointC.Y - pointA.Y)*(pointB.X - pointA.X) - (pointC.X - pointA.X)*(pointB.Y - pointA.Y));

			PointF pointCnew = new PointF(pointA.X + (pointC.X - pointA.X )*uc, pointA.Y + (pointC.Y - pointA.Y )*uc);

			//Find the transformation Matrix
			float[,] m = new float[3,3];
			m[0,0] = pointA.X;
			m[0,1] = pointA.Y;
			m[0,2] = 1;

			m[1,0] = pointB.X;
			m[1,1] = pointB.Y;
			m[1,2] = 1;

			m[2,0] = pointC.X;
			m[2,1] = pointC.Y;
			m[2,2] = 1;

			float[,] r = new float[3,3];
			matrix3Invert(m, r);

			float a = r[0,0]*pointA.X + r[0,1]*pointBnew.X + r[0,2]*pointCnew.X;
			float c = r[1,0]*pointA.X + r[1,1]*pointBnew.X + r[1,2]*pointCnew.X;
			float e = r[2,0]*pointA.X + r[2,1]*pointBnew.X + r[2,2]*pointCnew.X;

			float b = r[0,0]*pointA.Y + r[0,1]*pointBnew.Y + r[0,2]*pointCnew.Y;
			float d = r[1,0]*pointA.Y + r[1,1]*pointBnew.Y + r[1,2]*pointCnew.Y;
			float f = r[2,0]*pointA.Y + r[2,1]*pointBnew.Y + r[2,2]*pointCnew.Y;

			Matrix theMatrix = new Matrix(a, b, c, d, e, f);

			return theMatrix;
		}


		static double GetVectorAngle(PointF p1, PointF p2) {
			double angle = Math.Atan2(p1.Y - p2.Y, p2.X - p1.X);
			return angle*180/Math.PI;
		}


		public static Cursor GetGrabBarCursor(int index, GraphicsPath inPath) {
			PointF pointA = inPath.PathPoints[index];
			PointF pointB = inPath.PathPoints[(index + 1) % 4];
			PointF pointC = inPath.PathPoints[(index + 3) % 4];

			double bisectorAngle = 180 + (GetVectorAngle(pointB, pointA) + GetVectorAngle(pointC, pointA)) / 2;

			Cursor theCursor = Cursors.Default;
			if ( (0 <= bisectorAngle && bisectorAngle <= 45/2  ) || (315/2 <= bisectorAngle && bisectorAngle <= 405/2) || bisectorAngle >=675/2)
				theCursor = Cursors.SizeWE;
			else
				if ( (45/2 <= bisectorAngle && bisectorAngle <= 135/2) || (405/2 <=bisectorAngle && bisectorAngle <= 495/2))
				theCursor = Cursors.SizeNESW;
			else
				if ( (135/2 <= bisectorAngle && bisectorAngle <= 225/2) || (495/2 <= bisectorAngle && bisectorAngle <= 585/2))
				theCursor = Cursors.SizeNS;
			else
				if ( (225/2 <= bisectorAngle && bisectorAngle <= 315/2) || (585/2 <= bisectorAngle && bisectorAngle <= 675/2))
				theCursor = Cursors.SizeNWSE;

			return theCursor;
		}

		public static bool IsOverGrabBars(Point p, GraphicsPath path, int grabBarWidth, out int grabPoint) {
			for (int i = 0; i < path.PathPoints.Length; i++) {
				Rectangle theRect = new Rectangle((int)path.PathPoints[i].X - grabBarWidth / 2 , (int)path.PathPoints[i].Y - grabBarWidth / 2, grabBarWidth, grabBarWidth);
				if (theRect.Contains(p)	) {
					grabPoint = i;
					return true;
				}
			}
			grabPoint = -1;
			return false;
		}
	}
}
