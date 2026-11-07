// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using HMI.Runtime.Core;
using HMI.Runtime.Core.Helper.Extensions;
using HMI.Runtime.Shared;
namespace HMI.RuntimeControls.InputOutput
{
	[
		TemplatePart(Name = "OutputElement", Type = typeof(TextBlock)), 
		TemplateVisualState(GroupName = "CommonStates", Name = "Limit4"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "Normal"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "Limit1"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "Limit2"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "Limit3")]
	public class HMINumericDisplay1 : HMIDisplayBase
	{
		private string unitTranslation;

		public static readonly DependencyProperty PrecisionProperty = DependencyProperty.Register("Precision", typeof(int), typeof(HMINumericDisplay1), new PropertyMetadata(2, new PropertyChangedCallback(HMINumericDisplay1.OnPrecisionPropertyChanged)));
		public static readonly DependencyProperty UnitProperty = DependencyProperty.Register("Unit", typeof(string), typeof(HMINumericDisplay1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMINumericDisplay1.OnUnitPropertyChanged)));
		
		[Category("HMI")]
		public int Precision
		{
			get
			{
				return (int)base.GetValue(HMINumericDisplay1.PrecisionProperty);
			}
			set
			{
				base.SetValue(HMINumericDisplay1.PrecisionProperty, value);
			}
		}
		[Category("HMI")]
		public string Unit
		{
			get
			{
				return (string)base.GetValue(HMINumericDisplay1.UnitProperty);
			}
			set
			{
				base.SetValue(HMINumericDisplay1.UnitProperty, value);
			}
		}
		private static void OnPrecisionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			HMINumericDisplay1? wFNumericDisplay = d as HMINumericDisplay1;
			if (wFNumericDisplay == null)
			{
				return;
			}
			if ((int)e.NewValue < 0)
			{
				wFNumericDisplay.SetValue(e.Property, e.OldValue);
			}
		}
		private static void OnUnitPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		public HMINumericDisplay1()
		{
			base.DefaultStyleKey=(typeof(HMINumericDisplay1));
		}
		private void OnUnitTranslationsChangedHandler(string[] symbolicTexts, string[] translationTexts)
		{
			if (translationTexts.Length <= 0)
			{
				return;
			}
			this.unitTranslation = translationTexts[0];
			this.OnUpdateOutput();
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			if (DesignerPropertiesExt.IsInDesignTool)
			{
				return;
			}
			this.InitializeUnit();
		}
		private void InitializeUnit()
		{
			if (string.IsNullOrEmpty(this.Unit))
			{
				return;
			}
			this.Unit = StringPlaceholdersResolver.Resolve(this.Unit, this);
			base.Connector.RegisterLanguageChangedHandler(new string[]
			{
				this.Unit
			}, new OnLanguageChangedDelegate(this.OnUnitTranslationsChangedHandler));
		}
		protected override void OnUpdateOutput()
		{
			if (base.OutputElement == null)
			{
				return;
			}
			try
			{
				string text = base.GetSignalValue(base.SignalValue).ToString("F" + this.Precision);
				if (!string.IsNullOrEmpty(this.unitTranslation))
				{
					text = text + " " + this.unitTranslation;
				}
				base.OutputElement.Text=(text);
			}
			catch (Exception ex)
			{
				base.Connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
			}
		}
		protected override void Dispose(bool disposing)
		{
			this.DisposeConnector();
			base.Dispose(disposing);
		}
		private void DisposeConnector()
		{
			if (base.Connector == null)
			{
				return;
			}
			base.Connector.UnregisterLanguageChangedHandler(null, new OnLanguageChangedDelegate(this.OnUnitTranslationsChangedHandler));
		}
	}
}
