// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
namespace HMI.Runtime.Shared
{
    /// <summary>



    /// </summary>
    public class HMIStaticStateCondition : HMIStateConditionBase
	{
		public static readonly DependencyProperty AlternateBackgroundProperty = DependencyProperty.Register("AlternateBackground", typeof(Brush), typeof(HMIStaticStateCondition), new PropertyMetadata(new SolidColorBrush(Colors.White), new PropertyChangedCallback(OnAlternateBackgroundPropertyChanged)));
		public static readonly DependencyProperty AlternateForegroundProperty = DependencyProperty.Register("AlternateForeground", typeof(Brush), typeof(HMIStaticStateCondition), new PropertyMetadata(new SolidColorBrush(Colors.Black), new PropertyChangedCallback(OnAlternateForegroundPropertyChanged)));
		public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register("Background", typeof(Brush), typeof(HMIStaticStateCondition), new PropertyMetadata(new SolidColorBrush(Colors.White), new PropertyChangedCallback(OnBackgroundPropertyChanged)));
		public static readonly DependencyProperty BlinkingFrequencyProperty = DependencyProperty.Register("BlinkingFrequency", typeof(double), typeof(HMIStaticStateCondition), new PropertyMetadata(0.1, new PropertyChangedCallback(OnBlinkingFrequencyPropertyChanged)));
		public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register("Foreground", typeof(Brush), typeof(HMIStaticStateCondition), new PropertyMetadata(new SolidColorBrush(Colors.Black), new PropertyChangedCallback(OnForegroundPropertyChanged)));
		public static readonly DependencyProperty IsBlinkingProperty = DependencyProperty.Register("IsBlinking", typeof(bool), typeof(HMIStaticStateCondition), new PropertyMetadata(false, new PropertyChangedCallback(OnIsBlinkingPropertyChanged)));
		public static readonly DependencyProperty IsVisibleProperty = DependencyProperty.Register("IsVisible", typeof(bool), typeof(HMIStaticStateCondition), new PropertyMetadata(true, new PropertyChangedCallback(OnIsVisiblePropertyChanged)));
		public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register("IsEnabled", typeof(bool), typeof(HMIStaticStateCondition), new PropertyMetadata(true));

        /// <summary>


        /// </summary>
        [Category("HMI")]
		public Brush AlternateBackground
		{
			get
			{
				return (Brush)base.GetValue(AlternateBackgroundProperty);
			}
			set
			{
				base.SetValue(AlternateBackgroundProperty, value);
			}
		}
        /// <summary>


        /// </summary>
        [Category("HMI")]
		public Brush AlternateForeground
		{
			get
			{
				return (Brush)base.GetValue(AlternateForegroundProperty);
			}
			set
			{
				base.SetValue(AlternateForegroundProperty, value);
			}
		}
        /// <summary>

        /// </summary>
        [Category("HMI")]
		public Brush Background
		{
			get
			{
				return (Brush)base.GetValue(BackgroundProperty);
			}
			set
			{
				base.SetValue(BackgroundProperty, value);
			}
		}
        /// <summary>

        /// </summary>
        [Category("HMI")]
		public double BlinkingFrequency
		{
			get
			{
				return (double)base.GetValue(BlinkingFrequencyProperty);
			}
			set
			{
				base.SetValue(BlinkingFrequencyProperty, value);
			}
		}
        /// <summary>

        /// </summary>
        [Category("HMI")]
		public Brush Foreground
		{
			get
			{
				return (Brush)base.GetValue(ForegroundProperty);
			}
			set
			{
				base.SetValue(ForegroundProperty, value);
			}
		}
        /// <summary>

        /// </summary>
        [Category("HMI")]
		public bool IsBlinking
		{
			get
			{
				return (bool)base.GetValue(IsBlinkingProperty);
			}
			set
			{
				base.SetValue(IsBlinkingProperty, value);
			}
		}
        /// <summary>

        /// </summary>
        [Category("HMI")]
		public bool IsVisible
		{
			get
			{
				return (bool)base.GetValue(IsVisibleProperty);
			}
			set
			{
				base.SetValue(IsVisibleProperty, value);
			}
		}
        /// <summary>

        /// </summary>
        [Category("HMI")]
		public bool IsEnabled
		{
			get
			{
				return (bool)base.GetValue(IsEnabledProperty);
			}
			set
			{
				base.SetValue(IsEnabledProperty, value);
			}
		}
		private static void OnAlternateBackgroundPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnAlternateForegroundPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnBackgroundPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnBlinkingFrequencyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnForegroundPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnIsBlinkingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnIsVisiblePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		~HMIStaticStateCondition()
		{
            Dispose(false);
		}
		public bool Equals(HMIStaticStateCondition state)
		{
			return state != null && AlternateBackground.Equals(state.AlternateBackground) && AlternateForeground.Equals(state.AlternateForeground) && Background.Equals(state.Background) && Foreground.Equals(state.Foreground) && IsBlinking.Equals(state.IsBlinking) && IsEnabled.Equals(state.IsEnabled) && BlinkingFrequency.Equals(state.BlinkingFrequency) && IsVisible.Equals(state.IsVisible);
		}
		public override string ToString()
		{
			return base.StateName;
		}
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}
	}
}
