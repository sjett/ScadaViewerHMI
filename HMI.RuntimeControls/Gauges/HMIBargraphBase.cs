// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using HMI.Runtime.Core;
using HMI.Runtime.Shared;
using HMI.Runtime.Shared.Common;
using HMI.Runtime.Shared.Common.Converters;

namespace HMI.RuntimeControls.Gauges
{
    /// <summary>



    /// </summary>
    [
        TemplatePart(Name = "Scale100Element", Type = typeof(TextBlock)), 
		TemplatePart(Name = "Scale0Element", Type = typeof(TextBlock)), 
		TemplatePart(Name = "ScalePathElement", Type = typeof(Path)), 
		TemplatePart(Name = "Scale50Element", Type = typeof(TextBlock)), 
		TemplatePart(Name = "Bar", Type = typeof(Shape)), 
		TemplatePart(Name = "ScaleGridElement", Type = typeof(Grid))]
	public abstract class HMIBargraphBase : Control, IDisposable
	{
        private const string BarElementName = "Bar";
        private const string ScalePathElement = "ScalePathElement";
        private const string ScaleGridElement = "ScaleGridElement";
        private const string Scale100Element = "Scale100Element";
        private const string Scale50Element = "Scale50Element";
        private const string Scale0Element = "Scale0Element";

		private Shape? barElement;
		private Path? scalePathElement;
		private Grid? scaleGridElement;
		private TextBlock? scale100Element;
		private TextBlock? scale50Element;
		private TextBlock? scale0Element;
		private IHMIConnector connector;
		private OnSignalChangedDelegate signalChangedHandler;
		private OnSignalChangedDelegate visibilitySignalChangedHandler;

		private double signalValue;
		private double signalValueProcentage;
		private bool canShowControlFromVisibilitySignal;
		private bool isSecurityAvailable;
		private bool isAuthorized;
		private double oldRangeMaxValue;
		private double newRangeMaxValue;
		private double newRangeMinValue;
		private double oldRangeMinValue;

        public static readonly DependencyProperty ObjectIDProperty = DependencyProperty.Register("ObjectID", typeof(string), typeof(HMIBargraphBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIBargraphBase.OnObjectIDPropertyChanged)));
		public static readonly DependencyProperty RangeMinProperty = DependencyProperty.Register("RangeMin", typeof(double), typeof(HMIBargraphBase), new PropertyMetadata(0.0, new PropertyChangedCallback(HMIBargraphBase.OnRangeMinPropertyChanged)));
		public static readonly DependencyProperty RangeMaxProperty = DependencyProperty.Register("RangeMax", typeof(double), typeof(HMIBargraphBase), new PropertyMetadata(100.0, new PropertyChangedCallback(HMIBargraphBase.OnRangeMaxPropertyChanged)));
		public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register("Scale", typeof(bool), typeof(HMIBargraphBase), new PropertyMetadata(false, new PropertyChangedCallback(HMIBargraphBase.OnScalePropertyChanged)));
		public static readonly DependencyProperty SecurityAuthorizationNameProperty = DependencyProperty.Register("SecurityAuthorizationName", typeof(string), typeof(HMIBargraphBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIBargraphBase.OnSecurityAuthorizationNamePropertyChanged)));
		public static readonly DependencyProperty SecurityDenyAccessBehaviourProperty = DependencyProperty.Register("SecurityDenyAccessBehaviour", typeof(DenyAccessBehaviour), typeof(HMIBargraphBase), new PropertyMetadata(DenyAccessBehaviour.Hide, new PropertyChangedCallback(HMIBargraphBase.OnSecurityDenyAccessBehaviourPropertyChanged)));
		public static readonly DependencyProperty ServerNameProperty = DependencyProperty.Register("ServerName", typeof(string), typeof(HMIBargraphBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIBargraphBase.OnServerNamePropertyChanged)));
		public static readonly DependencyProperty SignalNameProperty = DependencyProperty.Register("SignalName", typeof(string), typeof(HMIBargraphBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIBargraphBase.OnSignalNamePropertyChanged)));
		public static readonly DependencyProperty SignalPrefixProperty = DependencyProperty.Register("SignalPrefix", typeof(string), typeof(HMIBargraphBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIBargraphBase.OnSignalPrefixPropertyChanged)));
		public static readonly DependencyProperty VisibilityMaskProperty = DependencyProperty.Register("VisibilityMask", typeof(int?), typeof(HMIBargraphBase), new PropertyMetadata(null, new PropertyChangedCallback(HMIBargraphBase.OnVisibilityMaskPropertyChanged)));
		public static readonly DependencyProperty VisibilitySignalNameProperty = DependencyProperty.Register("VisibilitySignalName", typeof(string), typeof(HMIBargraphBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIBargraphBase.OnVisibilitySignalNamePropertyChanged)));
		
		
		protected bool IsTemplateApplied
		{
			get;
			set;
		}
		protected IHMIConnector Connector
		{
			get
			{
				return this.connector;
			}
		}
		protected bool IsAuthorized
		{
			get
			{
				return this.isAuthorized;
			}
		}
		protected Shape? BarElement
		{
			get
			{
				return this.barElement;
			}
		}
		protected double SignalValueProcentage
		{
			get
			{
				return this.signalValueProcentage;
			}
		}

		[Category("HMI")]
		public string ObjectID
		{
			get
			{
				return (string)base.GetValue(HMIBargraphBase.ObjectIDProperty);
			}
			set
			{
				base.SetValue(HMIBargraphBase.ObjectIDProperty, value);
			}
		}
		[Category("HMI"), TypeConverter(typeof(DoubleTypeConverter))]
		public double RangeMin
		{
			get
			{
				return (double)base.GetValue(HMIBargraphBase.RangeMinProperty);
			}
			set
			{
				base.SetValue(HMIBargraphBase.RangeMinProperty, value);
			}
		}
		[Category("HMI"), TypeConverter(typeof(DoubleTypeConverter))]
		public double RangeMax
		{
			get
			{
				return (double)base.GetValue(HMIBargraphBase.RangeMaxProperty);
			}
			set
			{
				base.SetValue(HMIBargraphBase.RangeMaxProperty, value);
			}
		}
		[Category("HMI")]
		public bool Scale
		{
			get
			{
				return (bool)base.GetValue(HMIBargraphBase.ScaleProperty);
			}
			set
			{
				base.SetValue(HMIBargraphBase.ScaleProperty, value);
			}
		}
		[Category("HMI")]
		public string SecurityAuthorizationName
		{
			get
			{
				return (string)base.GetValue(HMIBargraphBase.SecurityAuthorizationNameProperty);
			}
			set
			{
				base.SetValue(HMIBargraphBase.SecurityAuthorizationNameProperty, value);
			}
		}
		[Category("HMI")]
		public DenyAccessBehaviour SecurityDenyAccessBehaviour
		{
			get
			{
				return (DenyAccessBehaviour)base.GetValue(HMIBargraphBase.SecurityDenyAccessBehaviourProperty);
			}
			set
			{
				base.SetValue(HMIBargraphBase.SecurityDenyAccessBehaviourProperty, value);
			}
		}
		[Category("HMI")]
		protected string ServerName
		{
			get
			{
				return (string)base.GetValue(HMIBargraphBase.ServerNameProperty);
			}
			set
			{
				base.SetValue(HMIBargraphBase.ServerNameProperty, value);
			}
		}
		[Category("HMI")]
		public string SignalName
		{
			get
			{
				return (string)base.GetValue(HMIBargraphBase.SignalNameProperty);
			}
			set
			{
				base.SetValue(HMIBargraphBase.SignalNameProperty, value);
			}
		}
		[Category("HMI")]
		public string SignalPrefix
		{
			get
			{
				return (string)base.GetValue(HMIBargraphBase.SignalPrefixProperty);
			}
			set
			{
				base.SetValue(HMIBargraphBase.SignalPrefixProperty, value);
			}
		}
		[Category("HMI"), TypeConverter(typeof(NullableInt32TypeConverter))]
		public int? VisibilityMask
		{
			get
			{
				return (int?)base.GetValue(HMIBargraphBase.VisibilityMaskProperty);
			}
			set
			{
				base.SetValue(HMIBargraphBase.VisibilityMaskProperty, value);
			}
		}
		[Category("HMI")]
		public string VisibilitySignalName
		{
			get
			{
				return (string)base.GetValue(HMIBargraphBase.VisibilitySignalNameProperty);
			}
			set
			{
				base.SetValue(HMIBargraphBase.VisibilitySignalNameProperty, value);
			}
		}
		private static void OnObjectIDPropertyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnRangeMinPropertyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
		{
			double newMinimum = (double)e.NewValue;
			double oldMinimum = (double)e.OldValue;
			HMIBargraphBase? bargraph = dp as HMIBargraphBase;
			if (bargraph != null && newMinimum != oldMinimum)
			{
				bargraph.UpdateRangeMin(oldMinimum, newMinimum);
			}
		}
        /// <summary>

        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected virtual void UpdateRangeMin(double oldValue, double newValue)
		{
			if (this.IsTemplateApplied && newValue >= this.RangeMax)
			{
				this.RangeMin = oldValue;
			}
			this.oldRangeMinValue = oldValue;
			this.newRangeMinValue = newValue;
			if (this.scale0Element != null)
			{
				this.scale0Element.Text=(this.RangeMin.ToString());
			}
			if (this.scale50Element != null)
			{
				this.scale50Element.Text=(((this.RangeMax + this.RangeMin) / 2.0).ToString());
			}
		}
		private static void OnRangeMaxPropertyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
		{
			HMIBargraphBase? bargraph = dp as HMIBargraphBase;
			double newMaximum = (double)e.NewValue;
			double oldMaximum = (double)e.OldValue;
			if (bargraph != null && newMaximum != oldMaximum)
			{
				bargraph.UpdateRangeMax(oldMaximum, newMaximum);
			}
		}
        /// <summary>

        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected virtual void UpdateRangeMax(double oldValue, double newValue)
		{
			if (this.IsTemplateApplied && newValue <= this.RangeMin)
			{
				this.RangeMax = oldValue;
			}
			this.oldRangeMaxValue = oldValue;
			this.newRangeMaxValue = newValue;
			if (this.scale100Element != null)
			{
				this.scale100Element.Text=(this.RangeMax.ToString());
			}
			if (this.scale50Element != null)
			{
				this.scale50Element.Text=(((this.RangeMax - this.RangeMin) / 2.0 + this.RangeMin).ToString());
			}
		}
		private static void OnScalePropertyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
		{
			HMIBargraphBase? wFBargraphBase = dp as HMIBargraphBase;
			if (wFBargraphBase != null)
			{
				if (wFBargraphBase.scalePathElement != null)
				{
					if ((bool)e.NewValue)
					{
						wFBargraphBase.scalePathElement.Visibility= System.Windows.Visibility.Visible;
					}
					else
					{
						wFBargraphBase.scalePathElement.Visibility= System.Windows.Visibility.Collapsed;
					}
				}
				if (wFBargraphBase.scaleGridElement != null)
				{
					if ((bool)e.NewValue)
					{
						wFBargraphBase.scaleGridElement.Visibility= System.Windows.Visibility.Visible;
						return;
					}
					wFBargraphBase.scaleGridElement.Visibility= System.Windows.Visibility.Collapsed;
				}
			}
		}
		private static void OnSecurityAuthorizationNamePropertyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnSecurityDenyAccessBehaviourPropertyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnServerNamePropertyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnSignalNamePropertyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnSignalPrefixPropertyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnVisibilityMaskPropertyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnVisibilitySignalNamePropertyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
		{
		}
		protected HMIBargraphBase()
		{
			base.DefaultStyleKey=(typeof(HMIBargraphBase));

			this.oldRangeMaxValue = (this.newRangeMaxValue = this.RangeMax);
			this.oldRangeMinValue = (this.newRangeMinValue = this.RangeMin);
		}
		~HMIBargraphBase()
		{
			this.Dispose(false);
		}
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		private void OnSignalValueChanged(string signalName, object signalValue)
		{
			try
			{
				double clampedSignalValue = this.ConvertSignalValueToDouble(signalValue);
				this.signalValue = clampedSignalValue;
				if (clampedSignalValue < this.RangeMin)
				{
					clampedSignalValue = this.RangeMin;
				}
				else
				{
					if (clampedSignalValue > this.RangeMax)
					{
						clampedSignalValue = this.RangeMax;
					}
				}
				this.signalValueProcentage = (clampedSignalValue - this.RangeMin) / (this.RangeMax - this.RangeMin);
			}
			catch
			{
				this.signalValue = double.NaN;
				this.signalValueProcentage = this.RangeMin;
			}
			this.FillBar();
		}
		private void OnVisibilitySignalValueChanged(string signalName, object signalValue)
		{
			double visibilitySignalValue = this.ConvertSignalValueToDouble(signalValue);
			this.canShowControlFromVisibilitySignal =
				!double.IsNaN(visibilitySignalValue) &&
				visibilitySignalValue != 0.0 &&
				(!this.VisibilityMask.HasValue ||
				 (Convert.ToInt64(visibilitySignalValue) & (long)this.VisibilityMask.Value) != (long)this.VisibilityMask.Value);
			this.ShowControl(this.canShowControlFromVisibilitySignal);
		}
		private void HandleAuthorizationContextChanged(string userName)
		{
			if (this.isSecurityAvailable)
			{
				this.isAuthorized = this.connector.CheckProjectAuthorization(this.SecurityAuthorizationName);
				switch (this.SecurityDenyAccessBehaviour)
				{
				case DenyAccessBehaviour.Disable:
					this.EnableControl(this.isAuthorized);
					break;
				case DenyAccessBehaviour.Hide:
					this.ShowControl(this.isAuthorized);
					break;
				default:
					throw new ArgumentOutOfRangeException("DenyAccessBehaviour");
				}
			}
			this.OnUserContextChanged(userName);
		}
		public override void OnApplyTemplate()
		{
			this.IsTemplateApplied = true;
			this.UpdateRangeMin(this.oldRangeMinValue, this.newRangeMinValue);
			this.UpdateRangeMax(this.oldRangeMaxValue, this.newRangeMaxValue);
			this.barElement = null;
			this.scalePathElement = null;
			this.scaleGridElement = null;
			this.scale100Element = null;
			this.scale50Element = null;
			this.scale0Element = null;

			base.OnApplyTemplate();

			this.barElement = (base.GetTemplateChild(BarElementName) as Shape);
			this.scalePathElement = (base.GetTemplateChild(ScalePathElement) as Path);
			if (this.scalePathElement != null)
			{
				if (this.Scale)
				{
					this.scalePathElement.Visibility= System.Windows.Visibility.Visible;
				}
				else
				{
					this.scalePathElement.Visibility= System.Windows.Visibility.Collapsed;
				}
			}

			this.scaleGridElement = (base.GetTemplateChild(ScaleGridElement) as Grid);
			if (this.scaleGridElement != null)
			{
				if (this.Scale)
				{
					this.scaleGridElement.Visibility= System.Windows.Visibility.Visible;
				}
				else
				{
					this.scaleGridElement.Visibility= System.Windows.Visibility.Collapsed;
				}
			}

			this.scale100Element = (base.GetTemplateChild(Scale100Element) as TextBlock);
			if (this.scale100Element != null)
			{
				this.scale100Element.Text=(this.RangeMax.ToString());
			}

			this.scale50Element = (base.GetTemplateChild(Scale50Element) as TextBlock);
			if (this.scale50Element != null)
			{
				this.scale50Element.Text=(((this.RangeMax - this.RangeMin) / 2.0 + this.RangeMin).ToString());
			}

			this.scale0Element = (base.GetTemplateChild(Scale0Element) as TextBlock);
			if (this.scale0Element != null)
			{
				this.scale0Element.Text=(this.RangeMin.ToString());
			}

			base.IsTabStop=(false);
			bool isInDesignMode;
			if (isInDesignMode = DesignerProperties.GetIsInDesignMode(this))
			{
				this.connector = RuntimeServices.GetRequiredService<IHMIConnector>();
			}
			else
			{
				if (this.connector != null)
				{
					return;
				}
				this.connector = RuntimeServices.GetRequiredService<IHMIConnector>();
				if (string.IsNullOrEmpty(this.SecurityAuthorizationName))
				{
					this.isAuthorized = true;
					this.isSecurityAvailable = false;
				}
				else
				{
					this.isAuthorized = false;
					this.isSecurityAvailable = true;
					switch (this.SecurityDenyAccessBehaviour)
					{
					case DenyAccessBehaviour.Disable:
						base.IsEnabled=(false);
						break;
					case DenyAccessBehaviour.Hide:
						base.Visibility= System.Windows.Visibility.Collapsed;
						break;
					default:
						throw new ArgumentOutOfRangeException("DenyAccessBehaviour");
					}
				}
				this.connector.UserContextChanged += new OnUserContextChangedEventHandler(this.HandleAuthorizationContextChanged);
				if (!string.IsNullOrEmpty(this.SignalName))
				{
					this.SignalName = this.SignalPrefix + this.SignalName;
					this.SignalName = SignalNameBuilder.BuildSignalName(this.SignalName, this);
					this.signalChangedHandler = new OnSignalChangedDelegate(this.OnSignalValueChanged);
					this.RegisterSignal(this.ServerName, this.SignalName, this.signalChangedHandler);
				}
				if (!string.IsNullOrEmpty(this.VisibilitySignalName))
				{
					this.VisibilitySignalName = this.SignalPrefix + this.VisibilitySignalName;
					this.VisibilitySignalName = SignalNameBuilder.BuildSignalName(this.VisibilitySignalName, this);
					this.visibilitySignalChangedHandler = new OnSignalChangedDelegate(this.OnVisibilitySignalValueChanged);
					this.RegisterSignal(this.ServerName, this.VisibilitySignalName, this.visibilitySignalChangedHandler);
				}
			}
			this.FillBar();
		}
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.connector != null)
				{
					this.connector.UserContextChanged -= new OnUserContextChangedEventHandler(this.HandleAuthorizationContextChanged);
					if (this.signalChangedHandler != null)
					{
						this.UnregisterSignal(this.ServerName, this.SignalName, this.signalChangedHandler);
					}
					if (this.visibilitySignalChangedHandler != null)
					{
						this.UnregisterSignal(this.ServerName, this.VisibilitySignalName, this.visibilitySignalChangedHandler);
					}
					this.connector.Dispose();
				}
				this.StopStoryboards();
			}
		}
		protected virtual void OnUserContextChanged(string userName)
		{
		}
		private void EnableControl(bool enable)
		{
			if (!enable)
			{
				base.IsEnabled=(false);
				return;
			}
			if (this.isSecurityAvailable && !this.isAuthorized && this.SecurityDenyAccessBehaviour == DenyAccessBehaviour.Disable)
			{
				base.IsEnabled=(false);
				return;
			}
			base.IsEnabled=(true);
		}
		private void ShowControl(bool show)
		{
			if (!show)
			{
				base.Visibility= System.Windows.Visibility.Collapsed;
				return;
			}
			if (this.isSecurityAvailable && !this.isAuthorized && this.SecurityDenyAccessBehaviour == DenyAccessBehaviour.Hide)
			{
				base.Visibility= System.Windows.Visibility.Collapsed;
				return;
			}
			if (string.IsNullOrEmpty(this.VisibilitySignalName))
			{
				base.Visibility= System.Windows.Visibility.Visible;
				return;
			}
			if (this.canShowControlFromVisibilitySignal)
			{
				base.Visibility= System.Windows.Visibility.Visible;
				return;
			}
			base.Visibility= System.Windows.Visibility.Collapsed;
		}
		protected virtual void FillBar()
		{
			if (this.barElement != null)
			{
				try
				{
					if (this.signalValueProcentage != 0.0)
					{
						if (this.signalValueProcentage != 1.0)
						{
							(this.barElement.Fill as LinearGradientBrush)!.GradientStops[0].Offset=(this.signalValueProcentage);
							(this.barElement.Fill as LinearGradientBrush)!.GradientStops[1].Offset=(this.signalValueProcentage - 0.005);
							return;
						}
					}

					(this.barElement.Fill as LinearGradientBrush)!.GradientStops[0].Offset=(this.signalValueProcentage);
					(this.barElement.Fill as LinearGradientBrush)!.GradientStops[1].Offset=(this.signalValueProcentage - 0.0001);
				}
				catch (Exception ex)
				{
					this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
				}
			}
		}
		private double ConvertSignalValueToDouble(object signalValue)
		{
			double result = double.NaN;
			try
			{
				result = double.Parse(signalValue.ToString());
			}
			catch
			{
				result = double.NaN;
			}
			return result;
		}
		protected void RegisterSignal(string serverName, string signalName, OnSignalChangedDelegate signalChangedDelegate)
		{
			try
			{
				this.connector.RegisterSignalChangedHandler(serverName, signalName, signalChangedDelegate);
			}
			catch (Exception ex)
			{
				this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
			}
		}
		protected void UnregisterSignal(string serverName, string signalName, OnSignalChangedDelegate signalChangedDelegate)
		{
			try
			{
				this.connector.UnregisterSignalChangedHandler(serverName, signalName, signalChangedDelegate);
			}
			catch (Exception ex)
			{
				this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
			}
		}
	}
}
