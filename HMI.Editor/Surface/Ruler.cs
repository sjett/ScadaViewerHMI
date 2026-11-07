// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
namespace HMI.Editor.Surface
{
    /// <summary>
    /// 标尺，显示在设计器的边缘，显示单位刻度和子单位刻度，并且可以显示一个芯片位置
    /// </summary>
    public class Ruler : FrameworkElement
	{
		private readonly double SegmentHeight;
		public static readonly DependencyProperty UnitTickMarksPenProperty;
		public static readonly DependencyProperty PaddingProperty;
		public static readonly DependencyProperty ChipPenProperty;
		public static readonly DependencyProperty ThinPenProperty;
		public static readonly DependencyProperty BorderPenProperty;
		public static readonly DependencyProperty BorderBrushProperty;
		public static readonly DependencyProperty LengthProperty;
		public static readonly DependencyProperty ChipProperty;
		public static readonly DependencyProperty CountShiftProperty;
		public static readonly DependencyProperty MarksProperty;
		public static readonly DependencyProperty ZoomFactorProperty;
		public static readonly DependencyProperty GridSizeProperty;
		public static readonly DependencyProperty OrientationProperty;
		public Pen UnitTickMarksPen
		{
			get
			{
				return (Pen)GetValue(UnitTickMarksPenProperty);
			}
			set
			{
                SetValue(UnitTickMarksPenProperty, value);
			}
		}
		public Thickness Padding
		{
			get
			{
				return (Thickness)GetValue(PaddingProperty);
			}
			set
			{
                SetValue(PaddingProperty, value);
			}
		}
		public Pen ChipPen
		{
			get
			{
				return (Pen)GetValue(ChipPenProperty);
			}
			set
			{
                SetValue(ChipPenProperty, value);
			}
		}
		public Pen ThinPen
		{
			get
			{
				return (Pen)GetValue(ThinPenProperty);
			}
			set
			{
                SetValue(ThinPenProperty, value);
			}
		}
		public Pen BorderPen
		{
			get
			{
				return (Pen)GetValue(BorderPenProperty);
			}
			set
			{
                SetValue(BorderPenProperty, value);
			}
		}
		public Brush BorderBrush
		{
			get
			{
				return (Brush)GetValue(BorderBrushProperty);
			}
			set
			{
                SetValue(BorderBrushProperty, value);
			}
		}
		[Obsolete("Use GetRulerSize instead")]
		public double Length
		{
			get
			{
				return (double)GetValue(LengthProperty);
			}
			set
			{
                SetValue(LengthProperty, value);
			}
		}
		public double Chip
		{
			get
			{
				return (double)GetValue(ChipProperty);
			}
			set
			{
                SetValue(ChipProperty, value);
			}
		}
		public int CountShift
		{
			get
			{
				return (int)GetValue(CountShiftProperty);
			}
			set
			{
                SetValue(CountShiftProperty, value);
			}
		}
		public MarksLocation Marks
		{
			get
			{
				return (MarksLocation)GetValue(MarksProperty);
			}
			set
			{
                SetValue(MarksProperty, value);
			}
		}
		public double ZoomFactor
		{
			get
			{
				return (double)GetValue(ZoomFactorProperty);
			}
			set
			{
                SetValue(ZoomFactorProperty, value);
			}
		}
		public double GridSize
		{
			get
			{
				return (double)GetValue(GridSizeProperty);
			}
			set
			{
                SetValue(GridSizeProperty, value);
			}
		}
		public Orientation Orientation
		{
			get
			{
				return (Orientation)GetValue(OrientationProperty);
			}
			set
			{
                SetValue(OrientationProperty, value);
			}
		}
		private double RulerLength
		{
			get;
			set;
		}
		static Ruler()
		{
            UnitTickMarksPenProperty = DependencyProperty.Register("UnitTickMarksPen", typeof(Pen), typeof(Ruler), new FrameworkPropertyMetadata(new Pen(Brushes.Black, 1.0), FrameworkPropertyMetadataOptions.AffectsRender));
            PaddingProperty = DependencyProperty.Register("Padding", typeof(Thickness), typeof(Ruler), new FrameworkPropertyMetadata(new Thickness(0.0, 0.0, 0.0, 0.0), FrameworkPropertyMetadataOptions.AffectsRender));
            ChipPenProperty = DependencyProperty.Register("ChipPen", typeof(Pen), typeof(Ruler), new FrameworkPropertyMetadata(new Pen(Brushes.Red, 1.0), FrameworkPropertyMetadataOptions.AffectsRender));
            ThinPenProperty = DependencyProperty.Register("ThinPen", typeof(Pen), typeof(Ruler), new FrameworkPropertyMetadata(new Pen(Brushes.Black, 0.5), FrameworkPropertyMetadataOptions.AffectsRender));
            BorderPenProperty = DependencyProperty.Register("BorderPen", typeof(Pen), typeof(Ruler), new FrameworkPropertyMetadata(new Pen(Brushes.Gray, 1.0), FrameworkPropertyMetadataOptions.AffectsRender));
            BorderBrushProperty = DependencyProperty.Register("BorderBrush", typeof(Brush), typeof(Ruler), new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromRgb(255, 255, 255))));
            LengthProperty = DependencyProperty.Register("Length", typeof(double), typeof(Ruler), new FrameworkPropertyMetadata(100.0, FrameworkPropertyMetadataOptions.AffectsRender));
            ChipProperty = DependencyProperty.Register("Chip", typeof(double), typeof(Ruler), new FrameworkPropertyMetadata(-1000.0, FrameworkPropertyMetadataOptions.AffectsRender));
            CountShiftProperty = DependencyProperty.Register("CountShift", typeof(int), typeof(Ruler), new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsRender));
            MarksProperty = DependencyProperty.Register("Marks", typeof(MarksLocation), typeof(Ruler), new FrameworkPropertyMetadata(MarksLocation.Up, FrameworkPropertyMetadataOptions.AffectsRender));
            ZoomFactorProperty = DependencyProperty.Register("ZoomFactor", typeof(double), typeof(Ruler), new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsParentMeasure | FrameworkPropertyMetadataOptions.AffectsRender));
            GridSizeProperty = DependencyProperty.Register("GridSize", typeof(double), typeof(Ruler), new FrameworkPropertyMetadata(100.0, FrameworkPropertyMetadataOptions.AffectsRender));
            OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(Ruler), new FrameworkPropertyMetadata(Orientation.Horizontal, FrameworkPropertyMetadataOptions.None));
            HeightProperty.OverrideMetadata(typeof(Ruler), new FrameworkPropertyMetadata(20.0));
		}
		public Ruler()
		{
            SegmentHeight = Height - 10.0;
		}
		protected override void OnRender(DrawingContext drawingContext)
		{
			base.OnRender(drawingContext);
            RulerLength = GetRulerLength();
            ClearRulerBackground(drawingContext);
            DrawChip(drawingContext);
			double unitIndex = 0.0;
			while (true)
			{
				double unitPosition = DrawSubunitMarks(drawingContext, unitIndex);
                DrawUnitMarks(drawingContext, unitPosition);
				if (unitPosition != 0.0 && unitPosition < RulerLength)
				{
                    DrawUnitText(drawingContext, unitPosition);
				}
				if (unitPosition >= RulerLength)
				{
					break;
				}
				unitIndex += 1.0;
			}
		}
		private void DrawUnitMarks(DrawingContext drawingContext, double positionOnRuler)
		{
			if (positionOnRuler < RulerLength)
			{
				if (Marks != MarksLocation.Up)
				{
                    DrawVerticalMarks(drawingContext, positionOnRuler);
					return;
				}
                DrawHorizontalMarks(drawingContext, positionOnRuler);
			}
		}
		private double DrawSubunitMarks(DrawingContext drawingContext, double positionUnit)
		{
			double unitPosition = positionUnit * GridSize;
			if (unitPosition < RulerLength)
			{
				double firstQuarterPosition = unitPosition + GridSize * 0.25;
				double midpointPosition = unitPosition + GridSize * 0.5;
				double thirdQuarterPosition = unitPosition + GridSize * 0.75;
				double shortTickHeight = SegmentHeight / 3.0;
				double mediumTickHeight = SegmentHeight * 2.0 / 3.0;
				if (Marks == MarksLocation.Up)
				{
					if (firstQuarterPosition < RulerLength)
					{
						drawingContext.DrawLine(ThinPen, new Point((firstQuarterPosition + Padding.Left) * ZoomFactor, Padding.Top), new Point((firstQuarterPosition + Padding.Left) * ZoomFactor, shortTickHeight + Padding.Top));
					}
					if (midpointPosition < RulerLength)
					{
						drawingContext.DrawLine(UnitTickMarksPen, new Point((midpointPosition + Padding.Left) * ZoomFactor, Padding.Top), new Point((midpointPosition + Padding.Left) * ZoomFactor, mediumTickHeight + Padding.Top));
					}
					if (thirdQuarterPosition < RulerLength)
					{
						drawingContext.DrawLine(ThinPen, new Point((thirdQuarterPosition + Padding.Left) * ZoomFactor, Padding.Top), new Point((thirdQuarterPosition + Padding.Left) * ZoomFactor, shortTickHeight + Padding.Top));
					}
				}
				else
				{
					if (firstQuarterPosition < RulerLength)
					{
						drawingContext.DrawLine(ThinPen, new Point(firstQuarterPosition * ZoomFactor, Height), new Point(firstQuarterPosition * ZoomFactor, Height - shortTickHeight));
					}
					if (midpointPosition < RulerLength)
					{
						drawingContext.DrawLine(UnitTickMarksPen, new Point(midpointPosition * ZoomFactor, Height), new Point(midpointPosition * ZoomFactor, Height - mediumTickHeight));
					}
					if (thirdQuarterPosition < RulerLength)
					{
						drawingContext.DrawLine(ThinPen, new Point(thirdQuarterPosition * ZoomFactor, Height), new Point(thirdQuarterPosition * ZoomFactor, Height - shortTickHeight));
					}
				}
			}
			return unitPosition;
		}
		private void DrawHorizontalMarks(DrawingContext drawingContext, double positionOnRuler)
		{
			drawingContext.DrawLine(UnitTickMarksPen, new Point((positionOnRuler + Padding.Left) * ZoomFactor, Padding.Top), new Point((positionOnRuler + Padding.Left) * ZoomFactor, SegmentHeight + Padding.Top));
		}
		private void DrawVerticalMarks(DrawingContext drawingContext, double positionOnRuler)
		{
			drawingContext.DrawLine(UnitTickMarksPen, new Point((positionOnRuler + Padding.Left) * ZoomFactor, Height + Padding.Top), new Point((positionOnRuler + Padding.Left) * ZoomFactor, Height + Padding.Top - SegmentHeight));
		}
		private void DrawUnitText(DrawingContext drawingContext, double positionOnRuler)
		{
			FormattedText formattedText = new FormattedText((positionOnRuler + CountShift).ToString(CultureInfo.CurrentCulture), CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), DipHelper.PtToDip(6.0), Brushes.DimGray);
			formattedText.SetFontWeight(FontWeights.Regular);
			formattedText.TextAlignment = TextAlignment.Center;
			if (Marks == MarksLocation.Up)
			{
				drawingContext.DrawText(formattedText, new Point((positionOnRuler + Padding.Left) * ZoomFactor, Height + Padding.Top - formattedText.Height));
				return;
			}
			drawingContext.DrawText(formattedText, new Point((positionOnRuler + Padding.Left) * ZoomFactor, Height + Padding.Top - SegmentHeight - formattedText.Height));
		}
        /// <summary>
        /// 绘制芯片位置
        /// </summary>
        /// <param name="drawingContext"></param>
        private void DrawChip(DrawingContext drawingContext)
		{
			double chip = Chip;
			drawingContext.DrawLine(ChipPen, new Point((chip + Padding.Left) * ZoomFactor, Padding.Top), new Point((chip + Padding.Left) * ZoomFactor, Height));
		}
		private void ClearRulerBackground(DrawingContext drawingContext)
		{
			drawingContext.DrawRectangle(BorderBrush, BorderPen, new Rect(new Point(0.0, 0.0), new Point((RulerLength + Padding.Left + Padding.Right) * ZoomFactor, Height + Padding.Bottom + Padding.Top)));
		}
		private double GetRulerLength()
		{
			double result;
			if (Orientation == Orientation.Horizontal)
			{
				result = ActualWidth;
			}
			else
			{
				result = ActualHeight;
			}
			return result;
		}
		public void SetChipPosition(double position)
		{
            Chip = position;
		}
	}
}
