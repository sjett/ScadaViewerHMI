// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
namespace HMI.RuntimeControls.Gauges
{
	public abstract class HMIRadialGaugeBase : HMIGaugeBase
	{
		private Grid needleGrid;
		private int currentIntervalIndex;
		private double previousNeedleAngle;
		private Path needleLine;
		private double currentIntervalOffset;
		private Grid intervalGrid;
		private Grid labelsGrid;
		internal static double currentNeedleAngle;
		internal static bool isMajorInterval;
		private Ellipse backgroundEllipse;
		private Storyboard needleAnimation;
		private Ellipse needleCenterEllipse;
		private Storyboard maximumArrowAnimation;
		private Storyboard minimumArrowAnimation;
		private Ellipse needleOuterCenterEllipse;
		private EasingDoubleKeyFrame endAngleKeyFrame;
		private EasingDoubleKeyFrame startAngleKeyFrame;

		public static readonly DependencyProperty IntervalRadiusProperty = DependencyProperty.Register("IntervalRadius", typeof(double), typeof(HMIRadialGaugeBase), new PropertyMetadata(0.0, new PropertyChangedCallback(HMIRadialGaugeBase.OnIntervalRadiusChanged)));
		public static readonly DependencyProperty BigIntervalsNumberProperty = DependencyProperty.Register("BigIntervalsNumber", typeof(int), typeof(HMIRadialGaugeBase), new PropertyMetadata(10, new PropertyChangedCallback(HMIRadialGaugeBase.OnBigIntervalsNumberChanged)));
		public static readonly DependencyProperty NeedleLineBackgroundProperty = DependencyProperty.Register("NeedleLineBackground", typeof(Brush), typeof(HMIRadialGaugeBase), new PropertyMetadata(new SolidColorBrush(Colors.LightGray), new PropertyChangedCallback(HMIRadialGaugeBase.OnNeedleLineBackgroundChanged)));
		public static readonly DependencyProperty BigIntervalBackgroundProperty = DependencyProperty.Register("BigIntervalBackground", typeof(Brush), typeof(HMIRadialGaugeBase), new PropertyMetadata(new SolidColorBrush(Colors.White), new PropertyChangedCallback(HMIRadialGaugeBase.OnBigIntervalBackgroundChanged)));
		public static readonly DependencyProperty NeedleCenterBackgroundProperty = DependencyProperty.Register("NeedleCenterBackground", typeof(Brush), typeof(HMIRadialGaugeBase), new PropertyMetadata(new SolidColorBrush(Colors.Gray), new PropertyChangedCallback(HMIRadialGaugeBase.OnNeedleCenterBackgroundChanged)));
		public static readonly DependencyProperty NeedleOuterCenterEllipseBackgroundProperty = DependencyProperty.Register("NeedleOuterCenterEllipseBackground", typeof(Brush), typeof(HMIRadialGaugeBase), new PropertyMetadata(new SolidColorBrush(Colors.LightGray), new PropertyChangedCallback(HMIRadialGaugeBase.OnNeedleOuterCenterBackgroundChanged)));
		
		private bool isLinearGauge;
		
		private double accumulatedIntervalValue;
		
		public bool IsLiniarGauge
		{
			
			get
			{
				return this.isLinearGauge;
			}
		}
		public double ScaleValue
		{
			get
			{
				return base.MaximumValue - base.MinimumValue;
			}
		}
		[Category("HMI")]
		public ObservableCollection<IFillRange> FillRange
		{
			get;
			set;
		}
		[Category("HMI")]
		public double IntervalRadius
		{
			get
			{
				return (double)base.GetValue(HMIRadialGaugeBase.IntervalRadiusProperty);
			}
			set
			{
				base.SetValue(HMIRadialGaugeBase.IntervalRadiusProperty, value);
			}
		}
		[Category("HMI")]
		public int BigIntervalsNumber
		{
			get
			{
				return (int)base.GetValue(HMIRadialGaugeBase.BigIntervalsNumberProperty);
			}
			set
			{
				base.SetValue(HMIRadialGaugeBase.BigIntervalsNumberProperty, value);
			}
		}
		[Category("HMI")]
		public Brush NeedleLineBackground
		{
			get
			{
				return (Brush)base.GetValue(HMIRadialGaugeBase.NeedleLineBackgroundProperty);
			}
			set
			{
				base.SetValue(HMIRadialGaugeBase.NeedleLineBackgroundProperty, value);
			}
		}
		[Category("HMI")]
		public Brush BigIntervalBackground
		{
			get
			{
				return (Brush)base.GetValue(HMIRadialGaugeBase.BigIntervalBackgroundProperty);
			}
			set
			{
				base.SetValue(HMIRadialGaugeBase.BigIntervalBackgroundProperty, value);
			}
		}
		[Category("HMI")]
		public Brush NeedleCenterBackground
		{
			get
			{
				return (Brush)base.GetValue(HMIRadialGaugeBase.NeedleCenterBackgroundProperty);
			}
			set
			{
				base.SetValue(HMIRadialGaugeBase.NeedleCenterBackgroundProperty, value);
			}
		}
		[Category("HMI")]
		public Brush NeedleOuterCenterEllipseBackground
		{
			get
			{
				return (Brush)base.GetValue(HMIRadialGaugeBase.NeedleOuterCenterEllipseBackgroundProperty);
			}
			set
			{
				base.SetValue(HMIRadialGaugeBase.NeedleOuterCenterEllipseBackgroundProperty, value);
			}
		}
		internal bool HasNegativeMinimumValue()
		{
			return base.MinimumValue < 0.0;
		}
		
		internal double GetAccumulatedIntervalValue()
		{
			return this.accumulatedIntervalValue;
		}
		
		internal void SetAccumulatedIntervalValue(double intervalValue)
		{
			this.accumulatedIntervalValue = intervalValue;
		}
		private static void OnIntervalRadiusChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
			HMIRadialGaugeBase gauge = dependencyObject as HMIRadialGaugeBase;
			if (gauge != null)
			{
				double? intervalRadius = HMIGaugeBase.ClampDouble((double)change.NewValue, 0.0, 13.0);
				if (intervalRadius.HasValue)
				{
					gauge.IntervalRadius = intervalRadius.Value;
					return;
				}
				HMIRadialGaugeBase.UpdateMatchingIntervalCornerRadius(change, gauge, "BigIntervalRectangle");
			}
		}
		private static void OnBigIntervalsNumberChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
			HMIRadialGaugeBase gauge = dependencyObject as HMIRadialGaugeBase;
			if (gauge != null)
			{
				int? intervalCount = HMIGaugeBase.ClampInteger((int)change.NewValue, 0, 50);
				if (intervalCount.HasValue)
				{
					gauge.BigIntervalsNumber = intervalCount.Value;
					return;
				}
				gauge.RebuildGaugeScale();
			}
		}
		private static void OnNeedleLineBackgroundChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
			HMIRadialGaugeBase gauge = dependencyObject as HMIRadialGaugeBase;
			if (gauge != null && gauge.needleLine != null)
			{
				gauge.needleLine.Fill=((Brush)change.NewValue);
			}
		}
		private static void OnBigIntervalBackgroundChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
			HMIRadialGaugeBase gauge = dependencyObject as HMIRadialGaugeBase;
			if (gauge != null && gauge.intervalGrid != null)
			{
				HMIRadialGaugeBase.UpdateMatchingIntervalBrushes(change, gauge, "BigIntervalRectangle");
			}
		}
		private static void OnNeedleCenterBackgroundChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
			HMIRadialGaugeBase gauge = dependencyObject as HMIRadialGaugeBase;
			if (gauge != null && gauge.needleCenterEllipse != null)
			{
				gauge.needleCenterEllipse.Fill=((Brush)change.NewValue);
			}
		}
		private static void OnNeedleOuterCenterBackgroundChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
			HMIRadialGaugeBase gauge = dependencyObject as HMIRadialGaugeBase;
			if (gauge != null && gauge.needleOuterCenterEllipse != null)
			{
				gauge.needleOuterCenterEllipse.Fill=((Brush)change.NewValue);
			}
		}
		public HMIRadialGaugeBase()
		{
			this.FillRange = new ObservableCollection<IFillRange>();
			this.FillRange.CollectionChanged+=(new NotifyCollectionChangedEventHandler(this.OnFillRangeCollectionChanged));
			base.DefaultStyleKey=(typeof(HMIRadialGaugeBase));
		}
		~HMIRadialGaugeBase()
		{
			base.UnregisterGauge(false);
		}
		public override void OnApplyTemplate()
		{
			this.InitializeGaugeConnector();
			base.OnApplyTemplate();
			this.GetGaugeTemplateParts();
			this.RebuildGaugeScale();
		}
		private void OnFillRangeCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
			case NotifyCollectionChangedAction.Add:
			{
				IEnumerator enumerator = e.NewItems.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						IFillRange fillRange = (IFillRange)enumerator.Current;
						fillRange.PropertyChanged+=(new PropertyChangedEventHandler(this.OnFillRangePropertyChanged));
						fillRange.Fill = new SolidColorBrush(Colors.White);
					}
					return;
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					if (disposable != null)
					{
						disposable.Dispose();
					}
				}
				break;
			}
			case NotifyCollectionChangedAction.Remove:
				break;
			default:
				return;
			}
			IEnumerator removedRangeEnumerator = e.OldItems.GetEnumerator();
			try
			{
				while (removedRangeEnumerator.MoveNext())
				{
					IFillRange removedRange = (IFillRange)removedRangeEnumerator.Current;
					removedRange.PropertyChanged-=(new PropertyChangedEventHandler(this.OnFillRangePropertyChanged));
				}
			}
			finally
			{
				IDisposable disposableRemovedRanges = removedRangeEnumerator as IDisposable;
				if (disposableRemovedRanges != null)
				{
					disposableRemovedRanges.Dispose();
				}
			}
			this.OnFillRangePropertyChanged(null, null);
		}
		private void OnFillRangePropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			this.OnGaugeRangeChanged();
			this.RefreshGaugeAppearance();
		}
        protected virtual void RebuildGaugeScale()
		{
			this.OnGaugeRangeChanged();
			this.RefreshGaugeAppearance();
		}
		protected override void RefreshGaugeAppearance()
		{
			base.RefreshGaugeAppearance();
			this.RefreshNeedleCenterBackground();
			this.RefreshNeedleLineBackground();
		}
		private void RefreshNeedleCenterBackground()
		{
			Brush needleCenterBackground = this.NeedleCenterBackground;
			this.NeedleCenterBackground = new LinearGradientBrush();
			this.NeedleCenterBackground = needleCenterBackground;
		}
		private void RefreshNeedleLineBackground()
		{
			Brush needleLineBackground = this.NeedleLineBackground;
			this.NeedleLineBackground = new LinearGradientBrush();
			this.NeedleLineBackground = needleLineBackground;
		}
        protected override void GetGaugeTemplateParts()
		{
			base.GetGaugeTemplateParts();
			this.needleGrid = (Grid)base.GetTemplateChild("NeedleGrid");
			this.needleLine = (Path)base.GetTemplateChild("NeedleLine");
			this.intervalGrid = (Grid)base.GetTemplateChild("ScaleIntervals");
			this.labelsGrid = (Grid)base.GetTemplateChild("LabelsContainer");
			this.needleAnimation = (Storyboard)base.GetTemplateChild("NeedleAnimation");
			this.backgroundEllipse = (Ellipse)base.GetTemplateChild("BackgroundEllipse");
			this.maximumArrowAnimation = (Storyboard)base.GetTemplateChild("MaxArrowAnimation");
			this.minimumArrowAnimation = (Storyboard)base.GetTemplateChild("MinArrowAnimation");
			this.needleCenterEllipse = (Ellipse)base.GetTemplateChild("NeedleCenterEllipse");
			this.needleOuterCenterEllipse = (Ellipse)base.GetTemplateChild("NeedleOuterCenterEllipse");
			this.endAngleKeyFrame = (EasingDoubleKeyFrame)base.GetTemplateChild("EndAngle");
			this.startAngleKeyFrame = (EasingDoubleKeyFrame)base.GetTemplateChild("StartAngle");
		}
        protected override void OnGaugeRangeChanged()
		{
            this.SetAccumulatedIntervalValue(0.0);
			this.InitializeGaugeValueDisplay();
			if (this.intervalGrid != null)
			{
				this.ClearIntervalGrid();
				this.ClearScaleLabels();
				this.currentIntervalIndex = 0;
				double rotationAngle = 0.0;
				double valueStep = (base.MaximumValue - base.MinimumValue) / this.BigIntervalsNumber;
				double intervalValue = base.MinimumValue;
				double majorIntervalCount = this.GetMajorIntervalCount();
				double intervalAngleStep = this.ScaleValue / majorIntervalCount;
				double rotationAngleStep = 260.0 / this.ScaleValue * intervalAngleStep;
				for (int intervalIndex = 0; intervalIndex <= this.BigIntervalsNumber + 1; intervalIndex++)
				{
					HMIRadialGaugeBase.isMajorInterval = true;
					this.currentIntervalOffset = intervalAngleStep * this.currentIntervalIndex;
					this.BuildMajorInterval(ref rotationAngle, valueStep, ref intervalValue, intervalAngleStep, rotationAngleStep);
					if (rotationAngle > 260.0)
					{
						return;
					}
					HMIRadialGaugeBase.isMajorInterval = false;
					this.BuildMinorIntervals(ref rotationAngle, intervalAngleStep, rotationAngleStep, ref this.currentIntervalIndex);
					this.currentIntervalOffset = intervalAngleStep * this.currentIntervalIndex;
				}
			}
		}
        protected virtual void BuildMajorInterval(ref double rotationAngle, double valueStep, ref double intervalValue, double intervalAngleStep, double rotationAngleStep)
		{
			Grid grid = this.CreateIntervalGrid(rotationAngle);
			if (this.intervalGrid != null)
			{
				this.intervalGrid.Children.Add(grid);
			}
			if (this.labelsGrid != null)
			{
				this.labelsGrid.Children.Add(this.CreateScaleLabelGrid(rotationAngle - 40.0, intervalValue.ToString()));
			}
			intervalValue += valueStep;
			this.SetAccumulatedIntervalValue(this.GetAccumulatedIntervalValue() + intervalAngleStep);
			rotationAngle += rotationAngleStep;
			this.currentIntervalIndex++;
			this.currentIntervalOffset = intervalAngleStep * this.currentIntervalIndex;
		}
        protected virtual int GetMajorIntervalCount()
		{
			return this.BigIntervalsNumber;
		}
        protected abstract void BuildMinorIntervals(ref double rotationAngle, double intervalValueStep, double degreesPerInterval, ref int intervalIndex);
		public virtual void IncrementVariables(ref double angleRotation, double intervalStep, double angleStep)
		{
			angleRotation += angleStep;
			this.SetAccumulatedIntervalValue(this.GetAccumulatedIntervalValue() + intervalStep);
			this.currentIntervalIndex++;
			this.currentIntervalOffset = intervalStep * (double)this.currentIntervalIndex;
		}
		private void ClearIntervalGrid()
		{
			if (this.intervalGrid.Children.Count > 0)
			{
				this.intervalGrid.Children.Clear();
			}
		}
		private void ClearScaleLabels()
		{
			if (this.labelsGrid.Children.Count > 0)
			{
				this.labelsGrid.Children.Clear();
			}
		}
        protected virtual Grid CreateIntervalGrid(double rotationAngle)
		{
			Grid grid = new Grid();
			grid.Children.Add(this.CreateIntervalRectangle());
			grid.Width=(290.0);
			grid.Height=(4.0);
			grid.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
			grid.VerticalAlignment= System.Windows.VerticalAlignment.Center;
			HMIRadialGaugeBase.ApplyIntervalGridTransform(rotationAngle, grid);
			grid.RenderTransformOrigin=(new Point(0.586, 0.5));
			grid.Background=(new SolidColorBrush(Colors.Transparent));
			this.ConfigureIntervalRectangle(grid);
			return grid;
		}
		private static void ApplyIntervalGridTransform(double rotationAngle, Grid intervalGrid)
		{
			TransformGroup transformGroup = new TransformGroup();
			RotateTransform rotateTransform = new RotateTransform();
			rotateTransform.Angle=(rotationAngle);
			transformGroup.Children.Add(rotateTransform);

			TranslateTransform translateTransform = new TranslateTransform();
			translateTransform.X=(-25.0);
			transformGroup.Children.Add(translateTransform);
			intervalGrid.RenderTransform=(transformGroup);
		}
        protected virtual void ConfigureIntervalRectangle(Grid intervalGrid)
		{
			Rectangle rectangle = (Rectangle)intervalGrid.Children[0];
			Guid intervalId = Guid.NewGuid();
			if (rectangle != null)
			{
				if (HMIRadialGaugeBase.isMajorInterval)
				{
					this.ConfigureMajorIntervalRectangle(ref intervalGrid, ref rectangle, intervalId);
					return;
				}
				this.ConfigureMinorIntervalRectangle(ref intervalGrid, ref rectangle, intervalId);
			}
		}
        protected abstract void ConfigureMinorIntervalRectangle(ref Grid intervalGrid, ref Rectangle intervalRectangle, Guid intervalId);
        protected virtual void ConfigureMajorIntervalRectangle(ref Grid intervalGrid, ref Rectangle intervalRectangle, Guid intervalId)
		{
			intervalGrid.SetValue(Canvas.ZIndexProperty, 1);
			intervalGrid.Name=("BigIntervalGrid" + intervalId);
			intervalRectangle.Name=("BigIntervalRectangle" + intervalId);
			intervalRectangle.RadiusX=(this.IntervalRadius);
			intervalRectangle.RadiusY=(this.IntervalRadius);
		}
        protected virtual Shape CreateIntervalRectangle()
		{
			Rectangle rectangle = new Rectangle();
			rectangle.Width=((double)(HMIRadialGaugeBase.isMajorInterval ? 25 : 17));
			rectangle.Height=(4.0);
			rectangle.Fill=(this.GetIntervalBrush());
			rectangle.HorizontalAlignment=(0);
			rectangle.VerticalAlignment= System.Windows.VerticalAlignment.Center;
			return rectangle;
		}
        protected virtual Brush GetIntervalBrush()
		{
			if (this.FillRange != null)
			{
				foreach (IFillRange fillRange in this.FillRange)
				{
					if (fillRange.Begin <= this.currentIntervalOffset && fillRange.End >= this.currentIntervalOffset)
					{
						return fillRange.Fill;
					}
				}
			}
			return this.GetDefaultIntervalBrush();
		}
        protected virtual Brush GetDefaultIntervalBrush()
		{
			return this.BigIntervalBackground;
		}
        protected virtual Grid CreateScaleLabelGrid(double rotationAngle, string labelText)
		{
			TransformGroup transformGroup = new TransformGroup();
			RotateTransform rotateTransform = new RotateTransform();
			rotateTransform.Angle=(rotationAngle);
			transformGroup.Children.Add(rotateTransform);
			Grid grid = new Grid();
			grid.RenderTransformOrigin=(new Point(1.0, 0.5));
			grid.Width=(135.0);
			grid.Margin=(new Thickness(0.0, 0.0, 146.0, 0.0));
			grid.HorizontalAlignment= System.Windows.HorizontalAlignment.Center;
			grid.VerticalAlignment= System.Windows.VerticalAlignment.Center;
			grid.RenderTransform=(transformGroup);
			Grid grid2 = grid;
			grid2.Children.Add(this.CreateScaleLabel(labelText, rotationAngle * -1.0));
			return grid2;
		}
        protected virtual TextBlock CreateScaleLabel(string labelText, double rotationAngle)
		{
			TransformGroup transformGroup = new TransformGroup();
			RotateTransform rotateTransform = new RotateTransform();
			rotateTransform.Angle=(rotationAngle);
			transformGroup.Children.Add(rotateTransform);
			TranslateTransform translateTransform = new TranslateTransform();
			translateTransform.X=(-60.0);
			transformGroup.Children.Add(translateTransform);
			TextBlock textBlock = new TextBlock();
			textBlock.RenderTransformOrigin=(new Point(0.5, 0.5));
			textBlock.Text=(base.FormatValueToPrecision(labelText));
			textBlock.VerticalAlignment= System.Windows.VerticalAlignment.Center;
			textBlock.HorizontalAlignment= System.Windows.HorizontalAlignment.Center;
			textBlock.RenderTransform=(transformGroup);
			return textBlock;
		}
		protected override void OnSignalValueChangedHandler(string signalName, object signalValue)
		{
			base.OnSignalValueChangedHandler(signalName, signalValue);
			this.UpdateNeedleForSignal(signalName, signalValue);
		}
		private void UpdateNeedleForSignal(string signalName, object signalValue)
		{
			if (HMIRadialGaugeBase.HasSignalName(signalName))
			{
				this.UpdateNeedleValue(signalValue);
				return;
			}
			HMIRadialGaugeBase.currentNeedleAngle = (double)(this.HasNegativeMinimumValue() ? 0 : -130);
		}
        protected virtual void UpdateNeedleValue(object signalValue)
		{
			this.CalculateNeedleAngle();
			this.UpdateGaugeValueDisplay(signalValue);
			this.AnimateNeedleRotation();
		}
        protected virtual void AnimateNeedleRotation()
		{
			if (this.startAngleKeyFrame != null && this.endAngleKeyFrame != null && this.needleAnimation != null)
			{
				this.startAngleKeyFrame.Value=(this.previousNeedleAngle);
				this.endAngleKeyFrame.Value=(HMIRadialGaugeBase.currentNeedleAngle);
				this.needleAnimation.Begin();
				this.previousNeedleAngle = HMIRadialGaugeBase.currentNeedleAngle;
				return;
			}
		}
        protected void CalculateNeedleAngle()
		{
			ICalculateAngle calculateAngle = new CalculateAngle
			{
				IsSinusoidal = this.HasNegativeMinimumValue(),
				MaximumAngle = 260.0,
				MinimunAngle = -130.0,
				MaximumValue = base.MaximumValue,
				MinimumValue = base.MinimumValue
			};
			HMIRadialGaugeBase.currentNeedleAngle = calculateAngle.GetAngleValue(base.GetCurrentValue());
		}
		public override void Dispose()
		{
			base.Dispose();
		}
        protected static void UpdateMatchingIntervalBrushes(DependencyPropertyChangedEventArgs change, HMIRadialGaugeBase gauge, string intervalName)
		{
			if (gauge.intervalGrid != null)
			{
				foreach (var intervalElement in gauge.intervalGrid.Children)
				{
					Grid intervalGrid = (Grid)intervalElement;
					Rectangle rectangle = (Rectangle)intervalGrid.Children[0];
					if (rectangle != null && rectangle.Name.Contains(intervalName))
					{
						rectangle.Fill = ((Brush)change.NewValue);
					}
				}
			}
		}
        protected static void UpdateMatchingIntervalCornerRadius(DependencyPropertyChangedEventArgs change, HMIRadialGaugeBase gauge, string intervalName)
		{
			if (gauge.intervalGrid != null)
			{
				double cornerRadius = (double)change.NewValue;
				foreach (var intervalElement in gauge.intervalGrid.Children)
				{
					Grid intervalGrid = (Grid)intervalElement;
					Rectangle rectangle = (Rectangle)intervalGrid.Children[0];
					if (rectangle != null && rectangle.Name.Contains(intervalName))
					{
						rectangle.RadiusX = (cornerRadius);
						rectangle.RadiusY = (cornerRadius);
					}
				}
			}
		}
		private static bool HasSignalName(string signalName)
		{
			return !string.IsNullOrEmpty(signalName);
		}
	}
}
