using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Globalization;

namespace Nolme.WinForms
{
	public enum ChartRenderingMode
	{
		Linear3d,
		BarWith3d,
		Max
	};

	public enum ChartCumulativeMode
	{
		StartFrom0,
		StartFromLastValue,
		Max
	};

	/// <summary>
	/// Summary description for Chart.
	/// </summary>
	public class Chart : CustomPanel	//System.ComponentModel.Component
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private int		m_LeftMargin;
		private int		m_RightMargin;
		private int		m_TopMargin;
		private int		m_BottomMargin;
		private int		m_DeltaDepth;

		private bool	m_DisplayTextOnColumns;
		private bool	m_DisplayHiddenSides;

		private Pen		m_GridPen;
		private Pen		m_GridPenFor0;

		private Pen		m_ColumnPen;
		private Font	m_ColumnFont;
		private Font	m_ColumnTitleFont;
		private Pen		m_HiddenColumnPen;
		
		private Brush	m_LegendBrush;
		private Font	m_LegendFont;

		private int		m_VerticalAxisMinValue;
		private int		m_VerticalAxisMaxValue;
		private int		m_VerticalAxisStep;
		private int		m_MarginBetweenColumn;

		private string	m_MainTitle;
		private Font	m_MainTitleFont;

		private ArrayList	m_Columns;

		private Color	[]m_PredefinedColors = {Color.FromArgb (125, Color.Red),
												   Color.FromArgb (125, Color.Green),
												   Color.FromArgb (125, Color.Blue),
												   Color.FromArgb (125, Color.Yellow),
												   Color.FromArgb (125, Color.Cyan),
												   Color.FromArgb (125, Color.Magenta),
												   Color.FromArgb (125, Color.Maroon),
												   Color.FromArgb (125, Color.Aqua),
												   Color.FromArgb (125, Color.Beige),
												   Color.FromArgb (125, Color.Orange),
												   Color.FromArgb (125, Color.Coral),
												   Color.FromArgb (125, Color.Lavender),
												   Color.FromArgb (125, Color.Gainsboro),
												   Color.FromArgb (125, Color.Ivory),
												   Color.FromArgb (125, Color.LemonChiffon),
												   Color.FromArgb (125, Color.Pink)};

		private	ChartRenderingMode	m_RenderingMode;
		private ChartCumulativeMode	m_CumulativeMode;

		public Chart(System.ComponentModel.IContainer container)
		{
			///
			/// Required for Windows.Forms Class Composition Designer support
			///
			if (container != null)
				container.Add(this);
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			this.InitDefault ();
		}

		public Chart()
		{
			///
			/// Required for Windows.Forms Class Composition Designer support
			///
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			this.InitDefault ();
		}

		private void InitDefault ()
		{
			this.LeftMargin				= 50;
			this.RightMargin			= 20;
			this.TopMargin				= 20;
			this.BottomMargin			= 20;
			this.DeltaDepth				= 10;

			this.GridPen				= new Pen (Color.Black, 1);
			this.GridPenFor0			= new Pen (Color.Violet, 2);
			this.ColumnPen				= new Pen (Color.Black, 1);
			this.HiddenColumnPen		= new Pen (Color.Gray, 1);

			this.LegendFont				= new Font("Arial", 11F, FontStyle.Bold);
			this.LegendBrush			= new SolidBrush(Color.Black);

			this.ColumnFont				= new Font("Arial", 8F, FontStyle.Italic);
			this.ColumnTitleFont		= new Font("Arial", 10F, FontStyle.Underline);

			this.MainTitle				= "Main title";
			this.MainTitleFont			= new Font("Arial", 16F, FontStyle.Underline | FontStyle.Bold);

			this.DisplayTextOnColumns	= true;
			this.DisplayHiddenSides		= true;

			this.VerticalAxisMinValue	= 0;
			this.VerticalAxisMaxValue	= 5000;
			this.VerticalAxisStep		= 1000;
			this.MarginBetweenColumn	= 20;

			this.m_Columns				= new ArrayList ();

			this.RenderingMode			= ChartRenderingMode.Linear3d;
			this.CumulativeMode			= ChartCumulativeMode.StartFrom0;
		}

		#region Properties
		public ChartCumulativeMode	CumulativeMode
		{
			get { return m_CumulativeMode; }
			set { m_CumulativeMode = value; }
		}

		public ChartRenderingMode	RenderingMode
		{
			get { return m_RenderingMode; }
			set { m_RenderingMode = value; }
		}

		public Color[]	GetPredefinedColors ()
		{
			return m_PredefinedColors;
		}
		public Color	GetPredefinedColors (int colorIndex)
		{
			return m_PredefinedColors [colorIndex];
		}
		public void SetPredefinedColors (Color[] value)
		{
			m_PredefinedColors = value;
		}

		public int	NumberOfItemsPerColumn
		{
			get
			{
				int iCount = 0;
				if ((m_Columns != null) && (m_Columns.Count != 0))
				{
					ChartColumn column = (ChartColumn)m_Columns[0];
					iCount = column.Length;
				}
				return iCount;
			}
		}

		public int	LeftMargin
		{
			get {return m_LeftMargin; }
			set {m_LeftMargin = value; }
		}

		public int	RightMargin
		{
			get {return m_RightMargin; }
			set {m_RightMargin = value; }
		}

		public int	TopMargin
		{
			get {return m_TopMargin; }
			set {m_TopMargin = value; }
		}

		public int	BottomMargin
		{
			get {return m_BottomMargin; }
			set {m_BottomMargin = value; }
		}

		public int DeltaDepth
		{
			get {return m_DeltaDepth; }
			set {m_DeltaDepth = value; }
		}

		public bool	DisplayTextOnColumns
		{
			get {return m_DisplayTextOnColumns; }
			set {m_DisplayTextOnColumns = value; }
		}

		public bool	DisplayHiddenSides
		{
			get {return m_DisplayHiddenSides; }
			set {m_DisplayHiddenSides = value; }
		}

		public Pen		GridPen
		{
			get {return m_GridPen; }
			set {m_GridPen = value; }
		}

		public Pen		GridPenFor0
		{
			get {return m_GridPenFor0; }
			set {m_GridPenFor0 = value; }
		}

		public Pen		ColumnPen
		{
			get {return m_ColumnPen; }
			set {m_ColumnPen = value; }
		}

		public Pen		HiddenColumnPen
		{
			get {return m_HiddenColumnPen; }
			set {m_HiddenColumnPen = value; }
		}
		
		public Brush	LegendBrush
		{
			get {return m_LegendBrush; }
			set {m_LegendBrush = value; }
		}
		
		public Font	LegendFont
		{
			get {return m_LegendFont; }
			set {m_LegendFont = value; }
		}
		
		public Font	ColumnFont
		{
			get {return m_ColumnFont; }
			set {m_ColumnFont = value; }
		}

		public Font	ColumnTitleFont
		{
			get {return m_ColumnTitleFont; }
			set {m_ColumnTitleFont = value; }
		}

		public Font	MainTitleFont
		{
			get {return m_MainTitleFont; }
			set {m_MainTitleFont = value; }
		}

		public string	MainTitle
		{
			get {return m_MainTitle; }
			set {m_MainTitle = value; }
		}

		public int		VerticalAxisMinValue
		{
			get {return m_VerticalAxisMinValue; }
			set {m_VerticalAxisMinValue = value; }
		}

		public int		VerticalAxisMaxValue
		{
			get {return m_VerticalAxisMaxValue; }
			set {m_VerticalAxisMaxValue = value; }
		}

		public int		VerticalAxisStep
		{
			get {return m_VerticalAxisStep; }
			set {m_VerticalAxisStep = value; }
		}

		public int		MarginBetweenColumn
		{
			get {return m_MarginBetweenColumn; }
			set {m_MarginBetweenColumn = value; }
		}
		
		public ArrayList	Columns
		{
			get {return m_Columns; }		// Use carefully
		}
		#endregion

		#region Highlevel functions
		public ChartColumn	AddColumn (int value)
		{
			int []localArray = new int [1];
			localArray[0] = value;
			
			return AddColumn (localArray);
		}

		public ChartColumn	AddColumn (int value0, int value1)
		{
			int []localArray = new int [2];
			localArray[0] = value0;
			localArray[1] = value1;
			
			return AddColumn (localArray);
		}

		public ChartColumn	AddColumn (int value0, int value1, int value2)
		{
			int []localArray = new int [3];
			localArray[0] = value0;
			localArray[1] = value1;
			localArray[2] = value2;
			
			return AddColumn (localArray);
		}

		public ChartColumn	AddColumn (int value0, int value1, int value2, int value3)
		{
			int []localArray = new int [4];
			localArray[0] = value0;
			localArray[1] = value1;
			localArray[2] = value2;
			localArray[3] = value3;
			
			return AddColumn (localArray);
		}

		public ChartColumn	AddColumn (int []values)
		{
			return this.AddColumn (values, null);
		}

		public ChartColumn	AddColumn (int []values, string title)
		{
			// Create new column
			ChartColumn	newColumn = this.AllocateColumn (values, title);

			// Resize vertical axis
			if (newColumn.PositiveValuesSum > this.VerticalAxisMaxValue)
				this.VerticalAxisMaxValue = newColumn.PositiveValuesSum;
			if (newColumn.NegativeValuesSum < this.VerticalAxisMinValue)
				this.VerticalAxisMinValue = newColumn.NegativeValuesSum;

			// Add column
			this.Columns.Add (newColumn);
			return newColumn;
		}

		virtual protected ChartColumn	AllocateColumn (int []values, string title)
		{
			ChartColumn	newColumn = new ChartColumn (values, title);
			return newColumn;
		}

		/// <summary>
		/// Automatic detection and adjustment of vertical axis maximum
		/// </summary>
		public void AdjustVerticalAxis ()
		{
			// Reset current max
			this.VerticalAxisMaxValue = 0;
			this.VerticalAxisMinValue = 0;
			int savedVerticalAxisMaxValue = int.MinValue;
			int savedVerticalAxisMinValue = int.MaxValue;

			// Iterate to get highest value
			if (this.CumulativeMode == ChartCumulativeMode.StartFromLastValue)
			{
				IEnumerator Iterator = Columns.GetEnumerator ();
				while (Iterator.MoveNext ())
				{
					ChartColumn currentColumn = (ChartColumn)Iterator.Current;
					if (currentColumn.PositiveValuesSum > savedVerticalAxisMaxValue)
						savedVerticalAxisMaxValue = currentColumn.PositiveValuesSum;
					if (currentColumn.NegativeValuesSum < savedVerticalAxisMinValue)
						savedVerticalAxisMinValue = currentColumn.NegativeValuesSum;
				}
			}
			else if (this.CumulativeMode == ChartCumulativeMode.StartFrom0)
			{
				IEnumerator Iterator = Columns.GetEnumerator ();
				while (Iterator.MoveNext ())
				{
					ChartColumn currentColumn = (ChartColumn)Iterator.Current;
					if (currentColumn.HighestValue > savedVerticalAxisMaxValue)
						savedVerticalAxisMaxValue = currentColumn.HighestValue;
					if (currentColumn.LowestValue < savedVerticalAxisMinValue)
						savedVerticalAxisMinValue = currentColumn.LowestValue;
				}
			}
			else 
			{
				Debug.Assert (this.CumulativeMode == ChartCumulativeMode.Max, "Chart.AdjustVerticalAxis", "Cumulative mode not managed");
			}

			// Round max value
			if (savedVerticalAxisMaxValue != 0)
			{
				string Value = savedVerticalAxisMaxValue.ToString (CultureInfo.InvariantCulture);
				int NbDigits = Value.Length;
				int Round    = (int)Math.Pow (10.0F, (double)(NbDigits - 1));
				// N.B.: Round must be an even number in order to make the following calculation works
				// As Round is a base-10 number, this condition is true.
				Debug.Assert (Round%2 == 0, "Chart.AdjustVerticalAxis ()", "Can't proceed when Round is odd");
				//this.VerticalAxisMaxValue = ((savedVerticalAxisMaxValue + Round/2) / Round) * Round;
				this.VerticalAxisMaxValue = ((savedVerticalAxisMaxValue + Round) / Round) * Round;

				Debug.Assert (this.VerticalAxisMaxValue >= savedVerticalAxisMaxValue, "Chart.AdjustVerticalAxis", string.Format (CultureInfo.InvariantCulture, "Overflow during resize from to {0} (max {1})", this.VerticalAxisMaxValue, savedVerticalAxisMaxValue));
			}

			// Round min value
			if (savedVerticalAxisMinValue != 0)
			{
				string Value = (-savedVerticalAxisMinValue).ToString (CultureInfo.InvariantCulture);
				int NbDigits = Value.Length;
				int Round    = (int)Math.Pow (10.0F, (double)(NbDigits - 1));
				// N.B.: Round must be an even number in order to make the following calculation works
				// As Round is a base-10 number, this condition is true.
				Debug.Assert (Round%2 == 0, "Chart.AdjustVerticalAxis ()", "Can't proceed when Round is odd");
				//this.VerticalAxisMaxValue = ((savedVerticalAxisMaxValue + Round/2) / Round) * Round;
				this.VerticalAxisMinValue = - (((-savedVerticalAxisMinValue + Round) / Round) * Round);

				Debug.Assert (this.VerticalAxisMinValue <= savedVerticalAxisMinValue, "Chart.AdjustVerticalAxis", string.Format (CultureInfo.InvariantCulture, "Overflow during resize from to {0} (max {1})", this.VerticalAxisMinValue, savedVerticalAxisMinValue));
			}
		}
		#endregion

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		/*
	PANEL ALIGNMENT                                  CHART COLUMN
*************************                     |      
*            ^          * TM : Top Margin     |       T3________T2                    
*		     | TM       * BM : Bottom Margin  |        /        /|                     
*            v          * LM : Left Margin    |       /        / |     _________        
*      P3_________P6    * RM : Right Margin   |      /________/  |    /|       /|       
*    P2/|         |  RM *                     |     Top0    T1|  |   / |      / |      
*<--->| |         |<--->*                     |     |         |  |  /________/  |      
*  LM | |         |     *                     |     |         |  | |   |     |  |      
*     | |P1       |P5   *                     |     |  B3_____|B2|_|___|_____|__|____  
*     |/__________/     *                     |     |  /      | /  |  /      | /    /  
*    P0      ^   P4     *                     |     | /       |/   | /       |/    /   
*		     | BM       *                     |     |/________/____|/________/____/    
*            v          *                     |     Base0    B1                        
*************************                     |    <---------><---->
											      ColumnWidth  MarginBetweenColumn
		*/
		protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs pevent) 
		{
			int	verticalAxisMaxValueInPixels;
			int x0, y0, x1, y2, x5;
			//int x2, x3, x4, x6;
			//int y1, y3, y4, y5, y6;

			base.OnPaintBackground(pevent);
			pevent.Graphics.SmoothingMode	= SmoothingMode.None;

			// Diagonal border
			x0 = LeftMargin;
			y0 = this.Height - BottomMargin;
			x1 = x0 + DeltaDepth;
			//y1 = y0 - DeltaDepth;

			//x2= x0;
			y2= TopMargin + DeltaDepth;
			//x3= x1;
			//y3= TopMargin;

			//x4 = this.Width - RightMargin - DeltaDepth;
			//y4 = y0;
			x5 = this.Width - RightMargin;
			//y5 = y1;

			//x6 = this.Width - RightMargin;
			//y6 = TopMargin;

			verticalAxisMaxValueInPixels = y0 - y2;

			// Draw main title
			this.DisplayGrid (pevent);

			// Draw columns
			int numberOfColumns = Columns.Count;
			if (numberOfColumns != 0)
			{
				int	columnWidth			= (x5 - x1) / (numberOfColumns);
				int	columnWidthNoMargin = columnWidth - MarginBetweenColumn;
				int columnStartOffset	= x0 + MarginBetweenColumn / 2;		// Center columns on graph

				// Draw columns title
				this.DisplayColumnsTitle (pevent, columnWidth, columnWidthNoMargin, columnStartOffset, verticalAxisMaxValueInPixels, x0, y0);

				if (this.RenderingMode == ChartRenderingMode.BarWith3d)
				{
					this.DisplayColumnsAsBarWith3d (pevent, columnWidth, columnWidthNoMargin, columnStartOffset, verticalAxisMaxValueInPixels, x0, y0);
				}
				else if (this.RenderingMode == ChartRenderingMode.Linear3d)
				{
					this.DisplayColumnsAsLinear (pevent, columnWidth, columnWidthNoMargin, columnStartOffset, verticalAxisMaxValueInPixels, x0, y0);
				}

				if (this.DisplayTextOnColumns)
				{
					this.DisplayColumnsText (pevent, columnWidth, columnWidthNoMargin, columnStartOffset, verticalAxisMaxValueInPixels, x0, y0-this.DeltaDepth);
				}

			}

			// Draw main title
			this.DisplayMainTitle (pevent);
		}

		virtual protected void DisplayColumnsAsLinear (System.Windows.Forms.PaintEventArgs e, int columnWidth, int columnWidthNoMargin, int columnStartOffset, int verticalAxisMaxValueInPixels, int lowerLeftX, int lowerLeftY)
		{
			Point Base0, Base1, Base2, Base3;

			IEnumerator Iterator		= Columns.GetEnumerator ();
			ChartColumn	previousColumn	= null;
			Point		prevBase0		= new Point (0,0);
			Point		prevBase1		= new Point (0,0);
			Point		prevBase2		= new Point (0,0);
			Point		prevBase3		= new Point (0,0);
			int			columnCurrentOffset = columnStartOffset;
			int			offsetFor0			= (-this.VerticalAxisMinValue) * verticalAxisMaxValueInPixels / (this.VerticalAxisMaxValue - this.VerticalAxisMinValue);
			while (Iterator.MoveNext ())
			{
				ChartColumn	currentColumn = (ChartColumn)Iterator.Current;
				Debug.Assert (currentColumn.PositiveValuesSum <= VerticalAxisMaxValue, "Chart.DisplayColumns_BarWith3d", "Vertical axis overflow, verify why current column value is higher than axis vertical value");
				Debug.Assert (this.VerticalAxisMaxValue > 0, "Chart.DisplayColumns_BarWith3d", "Integer divided by 0");

				Base0 = new Point (columnCurrentOffset, lowerLeftY-offsetFor0);
				Base1 = new Point (columnCurrentOffset + columnWidthNoMargin, lowerLeftY-offsetFor0);
				Base2 = new Point (columnCurrentOffset + columnWidthNoMargin + DeltaDepth, lowerLeftY -offsetFor0 - DeltaDepth);
				Base3 = new Point (columnCurrentOffset + DeltaDepth, lowerLeftY -offsetFor0 - DeltaDepth);

				currentColumn.DisplayAsLinear (e, this, previousColumn, columnWidthNoMargin, verticalAxisMaxValueInPixels, Base0, Base1, Base2, Base3, prevBase0, prevBase1, prevBase2, prevBase3);

				// Next
				prevBase0 = Base0;
				prevBase1 = Base1;
				prevBase2 = Base2;
				prevBase3 = Base3;
				columnCurrentOffset += columnWidth;
				previousColumn		= currentColumn;
			}
		}

		virtual protected void DisplayColumnsAsBarWith3d (System.Windows.Forms.PaintEventArgs e, int columnWidth, int columnWidthNoMargin, int columnStartOffset, int verticalAxisMaxValueInPixels, int lowerLeftX, int lowerLeftY)
		{
			Point Base0, Base1, Base2, Base3;

			IEnumerator Iterator		= Columns.GetEnumerator ();
			ChartColumn	previousColumn	= null;
			Point		prevBase0		= new Point (0,0);
			Point		prevBase1		= new Point (0,0);
			Point		prevBase2		= new Point (0,0);
			Point		prevBase3		= new Point (0,0);
			int			columnCurrentOffset = columnStartOffset;
			int			offsetFor0			= (-this.VerticalAxisMinValue) * verticalAxisMaxValueInPixels / (this.VerticalAxisMaxValue - this.VerticalAxisMinValue);
			while (Iterator.MoveNext ())
			{
				ChartColumn	currentColumn = (ChartColumn)Iterator.Current;
				Debug.Assert (currentColumn.PositiveValuesSum <= VerticalAxisMaxValue, "Chart.DisplayColumns_BarWith3d", "Vertical axis overflow, verify why current column value is higher than axis vertical value");
				Debug.Assert (this.VerticalAxisMaxValue > 0, "Chart.DisplayColumns_BarWith3d", "Integer divided by 0");

				Base0 = new Point (columnCurrentOffset, lowerLeftY-offsetFor0);
				Base1 = new Point (columnCurrentOffset + columnWidthNoMargin, lowerLeftY-offsetFor0);
				Base2 = new Point (columnCurrentOffset + columnWidthNoMargin + DeltaDepth, lowerLeftY - offsetFor0 - DeltaDepth);
				Base3 = new Point (columnCurrentOffset + DeltaDepth, lowerLeftY - offsetFor0 - DeltaDepth);

				currentColumn.DisplayAsBarWith3d (e, this, previousColumn, columnWidthNoMargin, verticalAxisMaxValueInPixels, Base0, Base1, Base2, Base3, prevBase0, prevBase1, prevBase2, prevBase3);

				// Next column
				prevBase0 = Base0;
				prevBase1 = Base1;
				prevBase2 = Base2;
				prevBase3 = Base3;
				columnCurrentOffset += columnWidth;
				previousColumn		= currentColumn;
			}
		}

		virtual protected void DisplayColumnsTitle (System.Windows.Forms.PaintEventArgs e, int columnWidth, int columnWidthNoMargin, int columnStartOffset, int verticalAxisMaxValueInPixels, int lowerLeftX, int lowerLeftY)
		{
			IEnumerator Iterator = Columns.GetEnumerator ();
			int columnCurrentOffset = columnStartOffset;
			while (Iterator.MoveNext ())
			{
				ChartColumn	currentColumn = (ChartColumn)Iterator.Current;

				Point Base0 = new Point (columnCurrentOffset, lowerLeftY);
				Point Base1 = new Point (columnCurrentOffset + columnWidthNoMargin, lowerLeftY);

				currentColumn.DisplayTitle (e, this, Base0, Base1);

				// Jump to next offset
				columnCurrentOffset += columnWidth;
			}
		}

		virtual protected void DisplayColumnsText (System.Windows.Forms.PaintEventArgs e, int columnWidth, int columnWidthNoMargin, int columnStartOffset, int verticalAxisMaxValueInPixels, int lowerLeftX, int lowerLeftY)
		{
			int			offset;
			int			offsetFor0;
			IEnumerator Iterator			= Columns.GetEnumerator ();
			int			columnCurrentOffset = columnStartOffset;
			
			offsetFor0 = (-this.VerticalAxisMinValue) * verticalAxisMaxValueInPixels / (this.VerticalAxisMaxValue - this.VerticalAxisMinValue);
			while (Iterator.MoveNext ())
			{
				ChartColumn	currentColumn = (ChartColumn)Iterator.Current;
				Debug.Assert (currentColumn.PositiveValuesSum <= VerticalAxisMaxValue, "Chart.OnPaintBackground", "Vertical axis overflow, verify why current column value is higher than axis vertical value");
				Debug.Assert (this.VerticalAxisMaxValue > 0, "Chart.OnPaintBackground", "Integer divided by 0");

				if (this.CumulativeMode == ChartCumulativeMode.StartFrom0)
				{
					offset  = offsetFor0 + (verticalAxisMaxValueInPixels * currentColumn.HighestValue / (this.VerticalAxisMaxValue-this.VerticalAxisMinValue));
				}
				else if (this.CumulativeMode == ChartCumulativeMode.StartFromLastValue)
				{
					offset  = offsetFor0 + (verticalAxisMaxValueInPixels * currentColumn.PositiveValuesSum / (this.VerticalAxisMaxValue-this.VerticalAxisMinValue));
				}
				else
				{
					Debug.Assert (this.CumulativeMode == ChartCumulativeMode.Max, "Chart.OnPaintBackground", "Cumulative mode not managed");
					offset  = 0;
				}
				System.Drawing.SizeF    size	= e.Graphics.MeasureString (currentColumn.PositiveValuesSum.ToString (CultureInfo.InvariantCulture), this.ColumnFont);
				PointF					Base0	= new PointF ((float)columnCurrentOffset, (float)(lowerLeftY - offset) - size.Height);
				RectangleF backgroundRectangle	= new RectangleF (Base0, size);

				// Align rectangle on column width
				backgroundRectangle.X = columnCurrentOffset + this.MarginBetweenColumn / 2 + ((columnWidthNoMargin - backgroundRectangle.Width) / 2);
				currentColumn.DisplayText (e, this, backgroundRectangle);

				// Jump to next offset
				columnCurrentOffset += columnWidth;
			}
		}

		virtual protected void DisplayGrid (System.Windows.Forms.PaintEventArgs e)
		{
			int x0, y0, x1, y1, x2, y2, x3, y3, x4, y4, x5, y5, x6, y6;

			// Diagonal border from front side to back side
			x0 = LeftMargin;
			y0 = this.Height - BottomMargin;
			x1 = x0 + DeltaDepth;
			y1 = y0 - DeltaDepth;
			e.Graphics.DrawLine (GridPen, x0, y0, x1, y1);

			x2= x0;
			y2= TopMargin + DeltaDepth;
			x3= x1;
			y3= TopMargin;
			e.Graphics.DrawLine (GridPen, x2, y2, x3, y3);

			x4 = this.Width - RightMargin - DeltaDepth;
			y4 = y0;
			x5 = this.Width - RightMargin;
			y5 = y1;
			e.Graphics.DrawLine (GridPen, x4, y4, x5, y5);

			x6 = this.Width - RightMargin;
			y6 = TopMargin;

			// Horizontal & vertical borders
			e.Graphics.DrawLine (GridPen, x0, y0, x4, y4);
			e.Graphics.DrawLine (GridPen, x0, y0, x2, y2);
			e.Graphics.DrawLine (GridPen, x1, y1, x3, y3);
			e.Graphics.DrawLine (GridPen, x6, y6, x3, y3);
			e.Graphics.DrawLine (GridPen, x6, y6, x5, y5);
			e.Graphics.DrawLine (GridPen, x1, y1, x5, y5);

			// Vertical Axis
			int	currentNumber	= 0;
			int	numberOfSteps	= (VerticalAxisMaxValue-VerticalAxisMinValue) / VerticalAxisStep;
			int tmpY			= y0;
			int deltaStep		= (y0 - y2) / numberOfSteps;

			// Loop for all steps but avoid rounding error & overdraw by not drawing last step
			for (int iVertical = 0; iVertical < numberOfSteps-1; iVertical++)
			{
				tmpY			-= deltaStep;
				currentNumber	= this.VerticalAxisMinValue + (iVertical+1) * VerticalAxisStep;

				if ((currentNumber == 0) && (this.VerticalAxisMinValue < 0))
				{
					// Draw diagonal
					e.Graphics.DrawLine (GridPenFor0, x0, tmpY, x1, tmpY - DeltaDepth);
					//e.Graphics.DrawLine (GridPenFor0, x4, tmpY, x5, tmpY - DeltaDepth);

					// Draw horizontal on chart
					e.Graphics.DrawLine (GridPenFor0, x5, tmpY - DeltaDepth, x1, tmpY - DeltaDepth);
					//e.Graphics.DrawLine (GridPenFor0, x0, tmpY, x4, tmpY);
				}
				else
				{
					// Draw diagonal
					e.Graphics.DrawLine (GridPen, x0, tmpY, x1, tmpY - DeltaDepth);

					// Draw horizontal on chart
					e.Graphics.DrawLine (GridPen, x5, tmpY - DeltaDepth, x1, tmpY - DeltaDepth);
				}
			}

			// Draw vertical legend 
			StringFormat legendDrawFormat	= new StringFormat();
			legendDrawFormat.Alignment		= StringAlignment.Far;
			legendDrawFormat.LineAlignment	= StringAlignment.Center;

			tmpY			= y0;
			for (int iVertical = 0; iVertical < numberOfSteps; iVertical++)
			{
				tmpY	-= deltaStep;

				// Draw horizontal on scaling
				e.Graphics.DrawLine (GridPen, x0, tmpY, x0-5, tmpY);
				
				// Draw legend
				currentNumber = VerticalAxisMinValue + (iVertical+1) * VerticalAxisStep;
				PointF point = new PointF (x0-5, tmpY);		// remove few pixels from vertical line
				e.Graphics.DrawString (currentNumber.ToString (CultureInfo.InvariantCulture), LegendFont, LegendBrush, point, legendDrawFormat);
			}
		}

		virtual protected void DisplayMainTitle (System.Windows.Forms.PaintEventArgs e)
		{
			if (this.MainTitle != null)
			{
				Point			point				= new Point (this.LeftMargin, this.Height - this.MainTitleFont.Height);
				Size			size				= new Size (this.Width - this.LeftMargin - this.RightMargin, this.MainTitleFont.Height) ; //e.Graphics.MeasureString (this.MainTitle, this.MainTitleFont);
				StringFormat	legendDrawFormat	= new StringFormat();
				legendDrawFormat.Alignment		= StringAlignment.Center;
				legendDrawFormat.LineAlignment	= StringAlignment.Center;
				Rectangle textRectangle			= new Rectangle (point, size);

				e.Graphics.DrawString (this.MainTitle, this.MainTitleFont, this.LegendBrush, textRectangle, legendDrawFormat);
			}
		}


		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		#endregion
	}
}
