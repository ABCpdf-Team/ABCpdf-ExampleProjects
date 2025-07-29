using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Diagnostics;

namespace Nolme.WinForms
{
	/// <summary>
	/// Summary description for ChartColumn.
	/// </summary>
	public class ChartColumn
	{
		private int[]	m_Values;				/// Values in column
		private int		m_PositiveValuesSum;	/// Sum of all positives values stored (usd for debug & scaling)
		private int		m_NegativeValuesSum;	/// Sum of all negatives values stored (usd for debug & scaling)
		private int		m_HighestValue;			/// Highest value in all values stored
		private int		m_LowestValue;			/// Lowest value in all values stored
		private string	m_Title;				/// Title of column

		public ChartColumn(int []values)
		{
			this.Init (values);
		}

		public ChartColumn(int []values, string title)
		{
			this.Title = title;
			this.Init (values);
		}

		void Init (int []values)
		{
			SetValues (values);
		}

		#region Properties
		public int HighestValue
		{
			get {return m_HighestValue;}
		}

		public int LowestValue
		{
			get {return m_LowestValue;}
		}

		public int PositiveValuesSum
		{
			get {return m_PositiveValuesSum;}
		}

		public int NegativeValuesSum
		{
			get {return m_NegativeValuesSum;}
		}

		public int Length
		{
			get {return m_Values.Length;}
		}

		public string Title
		{
			get {return m_Title; }
			set {m_Title = value; }
		}

		public int[] GetValues ()
		{
			return m_Values;
		}

		public void SetValues (int[] value)
		{
			m_Values			= value;
			m_NegativeValuesSum	= 0;
			m_PositiveValuesSum	= 0;
			m_HighestValue		= int.MinValue;
			m_LowestValue		= int.MaxValue;
			for (int i = 0; i < value.Length; i++)
			{
				if (value [i] >= 0)
					m_PositiveValuesSum	+= value [i];
				else
					m_NegativeValuesSum	+= value [i];

				if (value [i] > m_HighestValue)
				{
					m_HighestValue = value [i];
				}
				if (value [i] < m_LowestValue)
				{
					m_LowestValue = value [i];
				}
			}
		}
		#endregion

		#region Drawing
		virtual internal void DisplayAsLinear (System.Windows.Forms.PaintEventArgs pevent, Chart parent, ChartColumn	previousColumn, 
												int columnWidthNoMargin, int verticalAxisMaxValueInPixels, 
												Point base0, Point base1, Point base2, Point base3,
												Point prevBase0, Point prevBase1, Point prevBase2, Point prevBase3)
		{
			Debug.Assert (parent.RenderingMode == ChartRenderingMode.Linear3d, "ChartColumn.DisplayBars", "Rendering mode not yet managed");

			// First column, nothing to draw
			if (previousColumn == null)
				return;

			// Draw all values
			for (int i = 0; i < this.m_Values.Length; i++)
			{
				Color		currentColor		= parent.GetPredefinedColors (i);
				Brush		currentBrush		= new SolidBrush (currentColor);
				Brush		currentBrushDarken	= new SolidBrush (Color.FromArgb (currentColor.A, currentColor.R  /2, currentColor.G  /2, currentColor.B  /2));

				// Compute points
				int			prevY			= prevBase0.Y - (previousColumn.m_Values[i] * verticalAxisMaxValueInPixels / (parent.VerticalAxisMaxValue-parent.VerticalAxisMinValue));
				int			y				= base0.Y - (this.m_Values[i] * verticalAxisMaxValueInPixels / (parent.VerticalAxisMaxValue-parent.VerticalAxisMinValue));
				Point		currentPoint0	= new Point (prevBase0.X + columnWidthNoMargin / 2, prevY);
				Point		currentPoint1	= new Point (base0.X + columnWidthNoMargin / 2, y);
				Point		currentPoint2	= new Point (base3.X + columnWidthNoMargin / 2, y - parent.DeltaDepth);
				Point		currentPoint3	= new Point (prevBase3.X + columnWidthNoMargin / 2, prevY - parent.DeltaDepth);
			
				// Local top value path
				GraphicsPath pathSubValueTop = new GraphicsPath ();

				pathSubValueTop.AddLine (currentPoint0, currentPoint1);
				pathSubValueTop.AddLine (currentPoint1, currentPoint2);
				pathSubValueTop.AddLine (currentPoint2, currentPoint3);
				pathSubValueTop.AddLine (currentPoint3, currentPoint0);
				if (prevY > y)
					pevent.Graphics.FillPath (currentBrushDarken, pathSubValueTop);
				else
					pevent.Graphics.FillPath (currentBrush, pathSubValueTop);
			}
		}

		virtual internal void DisplayAsBarWith3d (System.Windows.Forms.PaintEventArgs pevent, Chart parent, ChartColumn	previousColumn, 
													int columnWidthNoMargin, int verticalAxisMaxValueInPixels, 
													Point base0, Point base1, Point base2, Point base3,
													Point prevBase0, Point prevBase1, Point prevBase2, Point prevBase3)
		{
			bool  FirstNegativeBarToDraw = true;

			// Draw all values
			Point currentBase0 = base0;
			Point currentBase1 = base1;
			Point currentBase2 = base2;
			Point currentBase3 = base3;

			Point currentBasePositive0 = base0;
			Point currentBasePositive1 = base1;
			Point currentBasePositive2 = base2;
			Point currentBasePositive3 = base3;

			Point currentBaseNegative0 = base0;
			Point currentBaseNegative1 = base1;
			Point currentBaseNegative2 = base2;
			Point currentBaseNegative3 = base3;

			for (int i = 0; i < this.m_Values.Length; i++)
			{
				if (this.m_Values[i] != 0)
				{
					int   offset      = this.m_Values[i] * (verticalAxisMaxValueInPixels) / (parent.VerticalAxisMaxValue-parent.VerticalAxisMinValue);

					if (this.m_Values[i] >= 0)
					{
						currentBase0 = currentBasePositive0;
						currentBase1 = currentBasePositive1;
						currentBase2 = currentBasePositive2;
						currentBase3 = currentBasePositive3;
					}
					else
					{
						currentBase0 = currentBaseNegative0;
						currentBase1 = currentBaseNegative1;
						currentBase2 = currentBaseNegative2;
						currentBase3 = currentBaseNegative3;
					}

					Point	currentTop0			= new Point (currentBase0.X, currentBase0.Y - offset);
					Point	currentTop1			= new Point (currentBase1.X, currentBase1.Y - offset);
					Point	currentTop2			= new Point (currentBase2.X, currentBase2.Y - offset);
					Point	currentTop3			= new Point (currentBase3.X, currentBase3.Y - offset);
					Color	currentColor		= parent.GetPredefinedColors (i);
					Brush	currentBrush		= new SolidBrush (currentColor);
					//Brush	currentBrushDarken	= new SolidBrush (Color.FromArgb (currentColor.A, currentColor.R  /2, currentColor.G  /2, currentColor.B  /2));

					if (parent.RenderingMode == ChartRenderingMode.BarWith3d)
					{
						if (parent.DisplayHiddenSides)
						{
							if (this.m_Values[i] >= 0)
							{
								pevent.Graphics.DrawLine (parent.HiddenColumnPen, currentBase0, currentBase3);
								pevent.Graphics.DrawLine (parent.HiddenColumnPen, currentBase3, currentTop3);
								pevent.Graphics.DrawLine (parent.HiddenColumnPen, currentBase3, currentBase2);
							}
							else
							{
								pevent.Graphics.DrawLine (parent.HiddenColumnPen, currentTop0, currentTop3);
								pevent.Graphics.DrawLine (parent.HiddenColumnPen, currentBase3, currentTop3);
								pevent.Graphics.DrawLine (parent.HiddenColumnPen, currentTop3, currentTop2);
							}
						}

						if (this.m_Values[i] >= 0)
						{
							// Local top value path
							GraphicsPath pathSubValueTop = new GraphicsPath ();
							pathSubValueTop.AddLine (currentTop0, currentTop1);
							pathSubValueTop.AddLine (currentTop1, currentTop2);
							pathSubValueTop.AddLine (currentTop2, currentTop3);
							pathSubValueTop.AddLine (currentTop3, currentTop0);
							//if (i == this.m_Values.Length-1)
							{
								pevent.Graphics.FillPath (currentBrush, pathSubValueTop);
							}
						}
						else 
						{
							if ( FirstNegativeBarToDraw)
							{
								// Local bottom value path
								GraphicsPath pathSubValueBottom = new GraphicsPath ();
								pathSubValueBottom.AddLine (currentBase0, currentBase1);
								pathSubValueBottom.AddLine (currentBase1, currentBase2);
								pathSubValueBottom.AddLine (currentBase2, currentBase3);
								pathSubValueBottom.AddLine (currentBase3, currentBase0);
								pevent.Graphics.FillPath (currentBrush, pathSubValueBottom);
								FirstNegativeBarToDraw = false;
							}
						}

						// Left
						/*
						GraphicsPath pathSubValueLeft = new GraphicsPath ();
						pathSubValueLeft.AddLine (currentBase0, currentBase3);
						pathSubValueLeft.AddLine (currentBase3, currentTop3);
						pathSubValueLeft.AddLine (currentTop3, currentTop0);
						pathSubValueLeft.AddLine (currentTop0, currentBase0);
						pevent.Graphics.FillPath (currentBrushDarken, pathSubValueLeft);
						*/

						// Back
						/*
						GraphicsPath pathSubValueBack = new GraphicsPath ();
						pathSubValueBack.AddLine (currentBase3, currentBase2);
						pathSubValueBack.AddLine (currentBase2, currentTop2);
						pathSubValueBack.AddLine (currentTop2, currentTop3);
						pathSubValueBack.AddLine (currentTop3, currentBase3);
						pevent.Graphics.FillPath (currentBrushDarken, pathSubValueBack);
						*/

						// Front
						GraphicsPath pathSubValueFront = new GraphicsPath ();
						pathSubValueFront.AddLine (currentBase0, currentBase1);
						pathSubValueFront.AddLine (currentBase1, currentTop1);
						pathSubValueFront.AddLine (currentTop1, currentTop0);
						pathSubValueFront.AddLine (currentTop0, currentBase0);
						pevent.Graphics.FillPath (currentBrush, pathSubValueFront);
						pevent.Graphics.DrawPath (parent.ColumnPen, pathSubValueFront);

						// Right
						GraphicsPath pathSubValueRightSide = new GraphicsPath ();
						pathSubValueRightSide.AddLine (currentBase1, currentBase2);
						pathSubValueRightSide.AddLine (currentBase2, currentTop2);
						pathSubValueRightSide.AddLine (currentTop2, currentTop1);
						pathSubValueRightSide.AddLine (currentTop1, currentBase1);
						pevent.Graphics.FillPath (currentBrush, pathSubValueRightSide);
						pevent.Graphics.DrawPath (parent.ColumnPen, pathSubValueRightSide);
					}
					else
					{
						Debug.Assert (parent.RenderingMode == ChartRenderingMode.Linear3d, "ChartColumn.DisplayBars", "Rendering mode not yet managed");
					}

					// Next
					if (parent.CumulativeMode == ChartCumulativeMode.StartFromLastValue)
					{
						if (this.m_Values[i] >= 0)
						{
							currentBasePositive0 = currentTop0;
							currentBasePositive1 = currentTop1;
							currentBasePositive2 = currentTop2;
							currentBasePositive3 = currentTop3;
						}
						else
						{
							currentBaseNegative0 = currentTop0;
							currentBaseNegative1 = currentTop1;
							currentBaseNegative2 = currentTop2;
							currentBaseNegative3 = currentTop3;
						}
					}
				}
			}

			// Redraw top to override hidden pen color
			GraphicsPath pathTop = new GraphicsPath ();
			pathTop.AddLine (currentBasePositive0, currentBasePositive1);
			pathTop.AddLine (currentBasePositive1, currentBasePositive2);
			pathTop.AddLine (currentBasePositive2, currentBasePositive3);
			pathTop.AddLine (currentBasePositive3, currentBasePositive0);
			pevent.Graphics.DrawPath (parent.ColumnPen, pathTop);
		}

		virtual internal void DisplayTitle (System.Windows.Forms.PaintEventArgs pevent, Chart parent, Point base0, Point base1)
		{
			//System.Drawing.SizeF    size				= new SizeF (base1.X - base0.X + parent.DeltaDepth + parent.MarginBetweenColumn, base1.Y - base0.Y);
			//RectangleF				backgroundRectangle = new RectangleF (base0, size);

			if (this.Title != null)
			{
				StringFormat legendDrawFormat	= new StringFormat();
				legendDrawFormat.Alignment		= StringAlignment.Far;
				legendDrawFormat.LineAlignment	= StringAlignment.Center;

				// Draw oblique
				Matrix oldMatrix = pevent.Graphics.Transform.Clone();
				pevent.Graphics.TranslateTransform( base0.X + (base1.X-base0.X)/2, base0.Y + 5);	// Add few pixels
				pevent.Graphics.RotateTransform(-45);
				pevent.Graphics.DrawString(this.Title, parent.ColumnTitleFont, parent.LegendBrush, 0, 0, legendDrawFormat);
				pevent.Graphics.Transform = oldMatrix;

				// Draw horizontal
				// pevent.Graphics.DrawString (this.Title.ToString (), parent.ColumnTitleFont, parent.LegendBrush, backgroundRectangle);
			}
		}

		virtual internal void DisplayText (System.Windows.Forms.PaintEventArgs pevent, Chart parent, RectangleF backgroundRectangle)
		{
			// Draw legend background
			LinearGradientBrush	backgroundBrush = new LinearGradientBrush (backgroundRectangle, Color.White, Color.Gray, 45.0f, true);
			pevent.Graphics.FillRectangle (backgroundBrush, backgroundRectangle);
			pevent.Graphics.DrawRectangle (parent.ColumnPen, backgroundRectangle.X, backgroundRectangle.Y, backgroundRectangle.Width, backgroundRectangle.Height);

			// Draw legend
			int TotalValues = this.PositiveValuesSum - this.NegativeValuesSum;
			pevent.Graphics.DrawString (TotalValues.ToString (CultureInfo.InvariantCulture), parent.ColumnFont, parent.LegendBrush, backgroundRectangle);
		}
		#endregion
	}
}
