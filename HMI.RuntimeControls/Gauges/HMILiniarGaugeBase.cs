// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
namespace HMI.RuntimeControls.Gauges
{
	public abstract class HMILiniarGaugeBase : HMIGaugeBase
	{
        class GaugeValuePercentageConverter
        {
            private double minimumValue;
            private double maximumValue;
            internal GaugeValuePercentageConverter(double value, double minimumValue)
            {
                this.minimumValue = minimumValue;
                this.maximumValue = value;
            }
            internal double GetPercentage(double value)
            {
                if (double.IsNaN(value))
                {
                    return 0.0;
                }
                return this.ClampToRange(value);
            }
            private double ClampToRange(double value)
            {
                if (value < this.minimumValue)
                {
                    return 0.0;
                }
                if (value > this.maximumValue)
                {
                    return 100.0;
                }
                return this.CalculatePercentage(value);
            }
            private double CalculatePercentage(double value)
            {
                double valueOffset = value - this.minimumValue;
                double valueRange = this.maximumValue - this.minimumValue;
                return Convert.ToDouble(valueOffset / valueRange * 100.0);
            }
        }
        class GaugeFillAnimation
        {
            private double fillAmount;
            private Storyboard storyboard;
            public GaugeFillAnimation(Storyboard storyboard, double fillAmount)
            {
                this.storyboard = storyboard;
                this.fillAmount = fillAmount;
                this.InitializeAnimation();
                this.BeginAnimation(this.storyboard);
            }
            private void InitializeAnimation()
            {
                this.SetAnimationKeyFrames(this.storyboard, 1);
                this.BeginAnimation(this.storyboard);
            }
            private void BeginAnimation(Storyboard storyboard)
            {
                storyboard.Completed += (new EventHandler(this.OnAnimationCompleted));
                storyboard.Begin();
            }
            private void OnAnimationCompleted(object? sender, EventArgs e)
            {
                Storyboard storyboard_ = sender as Storyboard;
                this.SetAnimationKeyFrames(storyboard_, 0);
            }
            private void SetAnimationKeyFrames(Storyboard storyboard, int keyFrameIndex)
            {
                GaugeFillAnimation.SetFirstAnimationTrackValue(this.fillAmount - 0.001, storyboard, keyFrameIndex);
                GaugeFillAnimation.SetSecondAnimationTrackValue(this.fillAmount, storyboard, keyFrameIndex);
            }
            private static void SetFirstAnimationTrackValue(double fillAmount, Storyboard storyboard, int keyFrameIndex)
            {
                Timeline animationTimeline = storyboard.Children[0];
                DoubleAnimationUsingKeyFrames keyFrames = animationTimeline as DoubleAnimationUsingKeyFrames;
                keyFrames.KeyFrames[keyFrameIndex].Value = (fillAmount);
            }
            private static void SetSecondAnimationTrackValue(double fillAmount, Storyboard storyboard, int keyFrameIndex)
            {
                Timeline animationTimeline = storyboard.Children[1];
                DoubleAnimationUsingKeyFrames keyFrames = animationTimeline as DoubleAnimationUsingKeyFrames;
                keyFrames.KeyFrames[keyFrameIndex].Value = (fillAmount);
            }
        }


        private Grid scaleGrid;
        private Shape fillShape;
        private Shape marginShape;
		private TextBlock maximumValueLabel;
		private TextBlock minimumValueLabel;
		private Shape backgroundShape;
		private Storyboard fillAnimation;
		private GaugeValuePercentageConverter valuePercentageConverter;

		public static readonly DependencyProperty FillColorProperty = DependencyProperty.Register("FillColor", typeof(Color), typeof(HMILiniarGaugeBase), new PropertyMetadata(Color.FromArgb(255, 0, 147, 196), new PropertyChangedCallback(HMILiniarGaugeBase.OnFillColorChanged)));
		public static readonly DependencyProperty StrokeThiknessProperty = DependencyProperty.Register("StrokeThikness", typeof(double), typeof(HMILiniarGaugeBase), new PropertyMetadata(2.0, new PropertyChangedCallback(HMILiniarGaugeBase.OnStrokeThicknessChanged)));
		public static readonly DependencyProperty StrokeColorProperty = DependencyProperty.Register("StrokeColor", typeof(Brush), typeof(HMILiniarGaugeBase), new PropertyMetadata(new SolidColorBrush(Colors.White), new PropertyChangedCallback(HMILiniarGaugeBase.OnStrokeColorChanged)));
		public static readonly DependencyProperty IntervalsNumberProperty = DependencyProperty.Register("IntervalsNumber", typeof(int), typeof(HMILiniarGaugeBase), new PropertyMetadata(3, new PropertyChangedCallback(HMILiniarGaugeBase.OnIntervalsNumberChanged)));
		
		[Category("HMI")]
		public Color FillColor
		{
			get
			{
				return (Color)base.GetValue(HMILiniarGaugeBase.FillColorProperty);
			}
			set
			{
				base.SetValue(HMILiniarGaugeBase.FillColorProperty, value);
			}
		}
		[Category("HMI")]
		public double StrokeThikness
		{
			get
			{
				return (double)base.GetValue(HMILiniarGaugeBase.StrokeThiknessProperty);
			}
			set
			{
				base.SetValue(HMILiniarGaugeBase.StrokeThiknessProperty, value);
			}
		}
		[Category("HMI")]
		public Brush StrokeColor
		{
			get
			{
				return (Brush)base.GetValue(HMILiniarGaugeBase.StrokeColorProperty);
			}
			set
			{
				base.SetValue(HMILiniarGaugeBase.StrokeColorProperty, value);
			}
		}
		[Category("HMI")]
		public int IntervalsNumber
		{
			get
			{
				return (int)base.GetValue(HMILiniarGaugeBase.IntervalsNumberProperty);
			}
			set
			{
				base.SetValue(HMILiniarGaugeBase.IntervalsNumberProperty, value);
			}
		}
		private static void OnFillColorChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
			HMILiniarGaugeBase gauge = dependencyObject as HMILiniarGaugeBase;
			if (gauge != null && gauge.fillShape != null)
			{
				LinearGradientBrush linearGradientBrush = gauge.fillShape.Fill as LinearGradientBrush;
				if (linearGradientBrush != null && linearGradientBrush.GradientStops[0] != null)
				{
					if (linearGradientBrush == null)
					{
						GradientStop fallbackFirstGradientStop = linearGradientBrush.GradientStops[0];
						GradientStop secondGradientStop = linearGradientBrush.GradientStops[1];
						GradientStop fallbackThirdGradientStop = linearGradientBrush.GradientStops[2];
						Color color;
						linearGradientBrush.GradientStops[3].Color=(color = default(Color));
						Color color2;
						fallbackThirdGradientStop.Color=(color2 = color);
						Color color3;
						secondGradientStop.Color=(color3 = color2);
						fallbackFirstGradientStop.Color=(color3);
						return;
					}
					GradientStop firstGradientStop = linearGradientBrush.GradientStops[0];
					Color color4;
					linearGradientBrush.GradientStops[1].Color=(color4 = default(Color));
					firstGradientStop.Color=(color4);
					GradientStop thirdGradientStop = linearGradientBrush.GradientStops[2];
					Color color5;
					linearGradientBrush.GradientStops[3].Color=(color5 = (Color)change.NewValue);
					thirdGradientStop.Color=(color5);
				}
			}
		}
		private static void OnStrokeThicknessChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
			HMILiniarGaugeBase gauge = dependencyObject as HMILiniarGaugeBase;
			if (gauge != null)
			{
				double? strokeThickness = HMIGaugeBase.ClampDouble((double)change.NewValue, 0.0, 20.0);
				if (strokeThickness.HasValue)
				{
					gauge.StrokeThikness = strokeThickness.Value;
					return;
				}
				if (gauge.levelValue != null)
				{
					if ((Rectangle)gauge.marginShape != null)
					{
						((Rectangle)gauge.marginShape).StrokeThickness=((double)change.NewValue);
					}
					if (gauge.backgroundShape != null)
					{
						gauge.backgroundShape.StrokeThickness=((double)change.NewValue);
					}
				}
			}
		}
		private static void OnStrokeColorChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
			HMILiniarGaugeBase gauge = dependencyObject as HMILiniarGaugeBase;
			if (gauge != null)
			{
				if ((Rectangle)gauge.marginShape != null)
				{
					((Rectangle)gauge.marginShape).Stroke=((Brush)change.NewValue);
				}
				if (gauge.backgroundShape != null)
				{
					gauge.backgroundShape.Stroke=((Brush)change.NewValue);
				}
			}
		}
		private static void OnIntervalsNumberChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
			HMILiniarGaugeBase gauge = dependencyObject as HMILiniarGaugeBase;
			if (gauge != null)
			{
				int? intervalCount = HMIGaugeBase.ClampInteger((int)change.NewValue, 0, 30);
				if (intervalCount.HasValue)
				{
					gauge.IntervalsNumber = intervalCount.Value;
					return;
				}
				if (gauge.levelValue != null)
				{
					gauge.BuildScaleIntervals(0.0, 0.0);
				}
			}
		}
		public HMILiniarGaugeBase()
		{
			base.DefaultStyleKey=(typeof(HMILiniarGaugeBase));
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
		}
        protected override void GetGaugeTemplateParts()
		{
			base.GetGaugeTemplateParts();
			this.scaleGrid = (Grid)base.GetTemplateChild("ScaleGrid");
			this.fillShape = (Shape)base.GetTemplateChild("ShapeFill");
			this.marginShape = (Shape)base.GetTemplateChild("MarginShape");
			this.maximumValueLabel = (TextBlock)base.GetTemplateChild("MaxLabel");
			this.minimumValueLabel = (TextBlock)base.GetTemplateChild("MinLabel");
			this.backgroundShape = (Shape)base.GetTemplateChild("ShapeBackground");
			this.fillAnimation = (Storyboard)base.GetTemplateChild("BackgroundFillAnimation");
		}
		protected override void OnSignalValueChangedHandler(string signalName, object signalValue)
		{
			base.OnSignalValueChangedHandler(signalName, signalValue);
			this.UpdateGaugeValueDisplay(base.GetCurrentValue());
			new GaugeFillAnimation(this.fillAnimation, this.GetGaugeFillRatio());
		}
		private double GetGaugeFillRatio()
		{
			this.valuePercentageConverter = new GaugeValuePercentageConverter(base.MaximumValue, base.MinimumValue);
			return 1.0 - this.valuePercentageConverter.GetPercentage(base.GetCurrentValue()) / 100.0;
		}
        protected override void RefreshGaugeAppearance()
		{
			base.RefreshGaugeAppearance();
			this.OnGaugeRangeChanged();
		}
		protected override void OnGaugeRangeChanged()
		{
			this.UpdateScaleEndpointLabels();
			this.UpdateBackgroundShapeAppearance();
			this.UpdateFillGradient();
		}
		private void UpdateBackgroundShapeAppearance()
		{
			if (this.backgroundShape != null)
			{
				this.backgroundShape.StrokeThickness=(this.StrokeThikness);
				this.backgroundShape.Stroke=(this.StrokeColor);
			}
		}
		private void UpdateScaleEndpointLabels()
		{
			if (this.maximumValueLabel != null && this.minimumValueLabel != null)
			{
				this.maximumValueLabel.Text=(base.MaximumValue.ToString());
				this.minimumValueLabel.Text=(base.MinimumValue.ToString());
			}
		}
		private void UpdateFillGradient()
		{
			if (this.fillShape != null)
			{
				LinearGradientBrush linearGradientBrush = (LinearGradientBrush)this.fillShape.Fill;
				if (linearGradientBrush != null)
				{
					GradientStop firstGradientStop = linearGradientBrush.GradientStops[0];
					Color color;
					linearGradientBrush.GradientStops[1].Color=(color = default(Color));
					firstGradientStop.Color=(color);
					GradientStop thirdGradientStop = linearGradientBrush.GradientStops[2];
					Color fillColor;
					linearGradientBrush.GradientStops[3].Color=(fillColor = this.FillColor);
					thirdGradientStop.Color=(fillColor);
				}
			}
		}
        protected virtual void BuildScaleIntervals(double scaleStart, double scaleLength)
		{
			if (this.fillShape != null && this.scaleGrid != null && this.IntervalsNumber >= 2)
			{
				this.ClearScaleGrid();
				double intervalSpacing = scaleLength / this.IntervalsNumber;
				double scalePosition = 0.0;
				double valueStep = (base.MaximumValue - base.MinimumValue) / this.IntervalsNumber;
				double? intervalValue = base.MinimumValue;
				for (int intervalIndex = 0; intervalIndex < this.IntervalsNumber; intervalIndex++)
				{
					bool isMajorInterval = this.IsMajorScaleInterval(scalePosition, scaleLength);
					this.AddScaleInterval(isMajorInterval, scalePosition, intervalValue);
					intervalValue = intervalValue.HasValue
						? intervalValue.Value + valueStep
						: default(double?);
					scalePosition += intervalSpacing;
				}
				this.CompleteScaleRendering();
				return;
			}
		}
        protected abstract void CompleteScaleRendering();
		private void ClearScaleGrid()
		{
			if (this.scaleGrid != null)
			{
				this.scaleGrid.Children.Clear();
			}
		}
		private bool IsMajorScaleInterval(double scalePosition, double scaleLength)
		{
			return HMILiniarGaugeBase.IsFirstScaleInterval(scalePosition) || this.IsMiddleScaleInterval(scalePosition, scaleLength) || HMILiniarGaugeBase.IsFinalScaleInterval(scalePosition, scaleLength);
		}
		private static bool IsFinalScaleInterval(double scalePosition, double scaleLength)
		{
			return scalePosition == scaleLength;
		}
		private bool IsMiddleScaleInterval(double scalePosition, double scaleLength)
		{
			return scalePosition == scaleLength / 2.0;
		}
		private static bool IsFirstScaleInterval(double scalePosition)
		{
			return scalePosition == 0.0;
		}
        protected virtual void AddScaleInterval(bool isMajorInterval, double scalePosition, double? intervalValue)
		{
			Rectangle rectangle = this.CreateScaleInterval(isMajorInterval, scalePosition);
			this.scaleGrid.Children.Add(rectangle);
		}
        protected abstract Rectangle CreateScaleInterval(bool isMajorInterval, double scalePosition);
	}
}
