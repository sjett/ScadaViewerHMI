// SPDX-License-Identifier: GPL-3.0-only
using HMI.Runtime.Core;
using HMI.Runtime.Core.Helper.Extensions;
using HMI.Runtime.Shared;
using HMI.RuntimeControls.Indicators.Localization;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
namespace HMI.RuntimeControls.Indicators
{
	[
		TemplateVisualState(GroupName = "CommonStates", Name = "State4"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "State8"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "State3"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "Normal"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "State2"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "State5"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "State7"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "State6"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "State1")]
	public class HMICompass1 : HMIIndicatorBase, IDisposable
	{
		private Storyboard? UxRotatePointer;
		private IHMIConnector connector;
		private ILanguageTranslator languageTranslator;

		private TextBlock? UxNorth;
		private TextBlock? UxNorthEast;
		private TextBlock? UxEast;
		private TextBlock? UxSouthEast;
		private TextBlock? UxSouth;
		private TextBlock? UxSouthWest;
		private TextBlock? UxWest;
		private TextBlock? UxNorthWest;

		private readonly string[] symbolicNames = new string[]
		{
			"HMISN_Ctrl_HMICompass1_North",
			"HMISN_Ctrl_HMICompass1_NorthEast",
			"HMISN_Ctrl_HMICompass1_East",
			"HMISN_Ctrl_HMICompass1_SouthEast",
			"HMISN_Ctrl_HMICompass1_South",
			"HMISN_Ctrl_HMICompass1_SouthWest",
			"HMISN_Ctrl_HMICompass1_West",
			"HMISN_Ctrl_HMICompass1_NorthWest"
		};

		public static readonly DependencyProperty CompassAngleProperty = DependencyProperty.Register("CompassAngle", typeof(double), typeof(HMICompass1), null);
		public static readonly DependencyProperty ChromeFrontBrushProperty = DependencyProperty.Register("ChromeFrontBrush", typeof(Brush), typeof(HMICompass1), null);
		public static readonly DependencyProperty MajorLabelsFontSizeProperty = DependencyProperty.Register("MajorLabelsFontSize", typeof(int), typeof(HMICompass1), new PropertyMetadata(36));
		public static readonly DependencyProperty MajorLabelsForegroundProperty = DependencyProperty.Register("MajorLabelsForeground", typeof(Brush), typeof(HMICompass1), null);
		public static readonly DependencyProperty MajorLabelsVisibleProperty = DependencyProperty.Register("MajorLabelsVisible", typeof(bool), typeof(HMICompass1), new PropertyMetadata(true));
		public static readonly DependencyProperty MajorScaleFillBrushProperty = DependencyProperty.Register("MajorScaleFillBrush", typeof(Brush), typeof(HMICompass1), null);
		public static readonly DependencyProperty MinorLabelsFontSizeProperty = DependencyProperty.Register("MinorLabelsFontSize", typeof(int), typeof(HMICompass1), new PropertyMetadata(18));
		public static readonly DependencyProperty MinorLabelsForegroundProperty = DependencyProperty.Register("MinorLabelsForeground", typeof(Brush), typeof(HMICompass1), null);
		public static readonly DependencyProperty MinorLabelsVisibleProperty = DependencyProperty.Register("MinorLabelsVisible", typeof(bool), typeof(HMICompass1), new PropertyMetadata(true));
		public static readonly DependencyProperty MinorScaleFillBrushProperty = DependencyProperty.Register("MinorScaleFillBrush", typeof(Brush), typeof(HMICompass1), null);
		public static readonly DependencyProperty PointerFillBrushProperty = DependencyProperty.Register("PointerFillBrush", typeof(Brush), typeof(HMICompass1), null);
		public static readonly DependencyProperty PointerHeadFillBrushProperty = DependencyProperty.Register("PointerHeadFillBrush", typeof(Brush), typeof(HMICompass1), null);
		public static readonly DependencyProperty PointerIsInvertedProperty = DependencyProperty.Register("PointerIsInverted", typeof(bool), typeof(HMICompass1), null);
		public static readonly DependencyProperty PointerStrokeBrushProperty = DependencyProperty.Register("PointerStrokeBrush", typeof(Brush), typeof(HMICompass1), null);
		public static readonly DependencyProperty SignalNameProperty = DependencyProperty.Register("SignalName", typeof(string), typeof(HMICompass1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMICompass1.OnSignalNamePropertyChanged)));
		

		[Browsable(false)]
		public double CompassAngle
		{
			get
			{
				return (double)base.GetValue(HMICompass1.CompassAngleProperty);
			}
			set
			{
				base.SetValue(HMICompass1.CompassAngleProperty, value);
			}
		}
		public Brush ChromeFrontBrush
		{
			get
			{
				return (Brush)base.GetValue(HMICompass1.ChromeFrontBrushProperty);
			}
			set
			{
				base.SetValue(HMICompass1.ChromeFrontBrushProperty, value);
			}
		}
		[Category("HMI")]
		public int MajorLabelsFontSize
		{
			get
			{
				return (int)base.GetValue(HMICompass1.MajorLabelsFontSizeProperty);
			}
			set
			{
				base.SetValue(HMICompass1.MajorLabelsFontSizeProperty, value);
			}
		}
		public Brush MajorLabelsForeground
		{
			get
			{
				return (Brush)base.GetValue(HMICompass1.MajorLabelsForegroundProperty);
			}
			set
			{
				base.SetValue(HMICompass1.MajorLabelsForegroundProperty, value);
			}
		}
		[Category("HMI")]
		public bool MajorLabelsVisible
		{
			get
			{
				return (bool)base.GetValue(HMICompass1.MajorLabelsVisibleProperty);
			}
			set
			{
				base.SetValue(HMICompass1.MajorLabelsVisibleProperty, value);
			}
		}
		public Brush MajorScaleFillBrush
		{
			get
			{
				return (Brush)base.GetValue(HMICompass1.MajorScaleFillBrushProperty);
			}
			set
			{
				base.SetValue(HMICompass1.MajorScaleFillBrushProperty, value);
			}
		}
		[Category("HMI")]
		public int MinorLabelsFontSize
		{
			get
			{
				return (int)base.GetValue(HMICompass1.MinorLabelsFontSizeProperty);
			}
			set
			{
				base.SetValue(HMICompass1.MinorLabelsFontSizeProperty, value);
			}
		}
		public Brush MinorLabelsForeground
		{
			get
			{
				return (Brush)base.GetValue(HMICompass1.MinorLabelsForegroundProperty);
			}
			set
			{
				base.SetValue(HMICompass1.MinorLabelsForegroundProperty, value);
			}
		}
		[Category("HMI")]
		public bool MinorLabelsVisible
		{
			get
			{
				return (bool)base.GetValue(HMICompass1.MinorLabelsVisibleProperty);
			}
			set
			{
				base.SetValue(HMICompass1.MinorLabelsVisibleProperty, value);
			}
		}
		public Brush MinorScaleFillBrush
		{
			get
			{
				return (Brush)base.GetValue(HMICompass1.MinorScaleFillBrushProperty);
			}
			set
			{
				base.SetValue(HMICompass1.MinorScaleFillBrushProperty, value);
			}
		}
		public Brush PointerFillBrush
		{
			get
			{
				return (Brush)base.GetValue(HMICompass1.PointerFillBrushProperty);
			}
			set
			{
				base.SetValue(HMICompass1.PointerFillBrushProperty, value);
			}
		}
		public Brush PointerHeadFillBrush
		{
			get
			{
				return (Brush)base.GetValue(HMICompass1.PointerHeadFillBrushProperty);
			}
			set
			{
				base.SetValue(HMICompass1.PointerHeadFillBrushProperty, value);
			}
		}
		[Category("HMI")]
		public bool PointerIsInverted
		{
			get
			{
				return (bool)base.GetValue(HMICompass1.PointerIsInvertedProperty);
			}
			set
			{
				base.SetValue(HMICompass1.PointerIsInvertedProperty, value);
			}
		}
		public Brush PointerStrokeBrush
		{
			get
			{
				return (Brush)base.GetValue(HMICompass1.PointerStrokeBrushProperty);
			}
			set
			{
				base.SetValue(HMICompass1.PointerStrokeBrushProperty, value);
			}
		}
		[Category("HMI")]
		public string SignalName
		{
			get
			{
				return (string)base.GetValue(HMICompass1.SignalNameProperty);
			}
			set
			{
				base.SetValue(HMICompass1.SignalNameProperty, value);
			}
		}
		private IHMIConnector getConnector()
		{
			return this.connector;
		}
		private static void OnSignalNamePropertyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
		{
			HMICompass1? wFCompass = dp as HMICompass1;
			if (wFCompass != null && DesignerProperties.GetIsInDesignMode(wFCompass))
			{
				wFCompass.resolveSignalNamePlaceholder();
				return;
			}
		}
		public HMICompass1()
		{
			base.DefaultStyleKey=(typeof(HMICompass1));
		}
		private void OnSignalChanged(string signalName, object signalValue)
		{
			if (signalValue == null)
			{
				return;
			}
			this.CompassAngle = Convert.ToDouble(signalValue);
			this.UxRotatePointer?.Begin();
		}
		private void languageTranslator_LanguageChanged(object? sender, EventArgs e)
		{
			this.setDesingLanguageText();
		}
		private void setDesingLanguageText()
		{
			this.UxNorth.Text = (this.languageTranslator.North);
			this.UxNorthEast.Text=(this.languageTranslator.NorthEast);
			this.UxEast.Text=(this.languageTranslator.East);
			this.UxSouthEast.Text=(this.languageTranslator.SouthEast);
			this.UxSouth.Text=(this.languageTranslator.South);
			this.UxSouthWest.Text=(this.languageTranslator.SouthWest);
			this.UxWest.Text=(this.languageTranslator.West);
			this.UxNorthWest.Text=(this.languageTranslator.NorthWest);
		}
		public override void OnApplyTemplate()
		{
			this.resetControl();

			base.OnApplyTemplate();

			this.initControl();

			bool isInDesignTool = DesignerPropertiesExt.IsInDesignTool;
			this.connector = RuntimeServices.GetRequiredService<IHMIConnector>();

			this.resolveSignalNamePlaceholder();

			if (isInDesignTool)
			{
				this.languageTranslator = new DesignLanguageTranslator();
				this.setDesingLanguageText();
				return;
			}
			this.languageTranslator = new LanguageTranslator(this.getConnector());
			this.registerConnector();
		}
		private void resetControl()
		{
			this.UxRotatePointer = null;
			this.UxNorth = null;
			this.UxNorthEast = null;
			this.UxEast = null;
			this.UxSouthEast = null;
			this.UxSouth = null;
			this.UxSouthWest = null;
			this.UxWest = null;
			this.UxNorthWest = null;
		}
		private void initControl()
		{
			this.UxRotatePointer = (base.GetTemplateChild("UxRotatePointer") as Storyboard);
			this.UxNorth = (base.GetTemplateChild("UxNorth") as TextBlock);
			this.UxNorthEast = (base.GetTemplateChild("UxNorthEast") as TextBlock);
			this.UxEast = (base.GetTemplateChild("UxEast") as TextBlock);
			this.UxSouthEast = (base.GetTemplateChild("UxSouthEast") as TextBlock);
			this.UxSouth = (base.GetTemplateChild("UxSouth") as TextBlock);
			this.UxSouthWest = (base.GetTemplateChild("UxSouthWest") as TextBlock);
			this.UxWest = (base.GetTemplateChild("UxWest") as TextBlock);
			this.UxNorthWest = (base.GetTemplateChild("UxNorthWest") as TextBlock);
		}
		private void resolveSignalNamePlaceholder()
		{
			if (!string.IsNullOrEmpty(this.SignalName))
			{
				this.SignalName = StringPlaceholdersResolver.Resolve(base.SignalPrefix + this.SignalName, this);
			}
		}
		private void registerConnector()
		{
			this.registerSignalName();
			this.registerLanguageTranslator();
		}
		private void registerSignalName()
		{
			if (string.IsNullOrEmpty(this.SignalName))
			{
				return;
			}
			this.getConnector().RegisterSignalChangedHandler("", this.SignalName, new OnSignalChangedDelegate(this.OnSignalChanged));
		}
		private void registerLanguageTranslator()
		{
			this.languageTranslator.LanguageChanged += new EventHandler(this.languageTranslator_LanguageChanged);
		}
		private void OnLanguageChanged(string[] symbolicNames, string[] translationTexts)
        {
			var symbolicNamesWithIndexes = symbolicNames.Select((string symbolicName, int index) => (symbolicName, index));

			var message = 
				string.Join("\r\n", 
					symbolicNamesWithIndexes
						.Where(item => item.symbolicName.Contains("Compass"))
						.Select(item => item.symbolicName + "=>" + translationTexts[item.index]));

            MessageBox.Show(message);
		}
		public override void Dispose()
		{
			base.Dispose();
			this.DisposeConnector();
		}
		private void DisposeConnector()
		{
			this.getConnector().UnregisterSignalChangedHandler("", this.SignalName, new OnSignalChangedDelegate(this.OnSignalChanged));
			this.languageTranslator.LanguageChanged -= new EventHandler(this.languageTranslator_LanguageChanged);
			this.getConnector().UnregisterLanguageChangedHandler(this.symbolicNames, new OnLanguageChangedDelegate(this.OnLanguageChanged));
		}

	}
}
