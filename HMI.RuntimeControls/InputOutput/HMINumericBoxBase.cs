// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HMI.Runtime.Core;
using HMI.Runtime.Core.Helper.Extensions;
using HMI.Runtime.Shared;
using HMI.Runtime.Shared.VirtualKeyboard;
namespace HMI.RuntimeControls.InputOutput
{
	[
		TemplatePart(Name = "FocusVisualElement", Type = typeof(FrameworkElement)), 
		TemplatePart(Name = "ContentElement", Type = typeof(FrameworkElement)), 
		TemplatePart(Name = "RootElement", Type = typeof(FrameworkElement)), 
		TemplatePart(Name = "DisabledVisualElement", Type = typeof(FrameworkElement)), 
		TemplatePart(Name = "ReadOnlyVisualElement", Type = typeof(FrameworkElement)), 
		TemplateVisualState(Name = "Disabled", GroupName = "CommonStates"), 
		TemplateVisualState(Name = "MouseOver", GroupName = "CommonStates"), 
		TemplateVisualState(Name = "Unfocused", GroupName = "FocusStates"), 
		TemplateVisualState(Name = "Normal", GroupName = "CommonStates"), 
		TemplateVisualState(Name = "Focused", GroupName = "FocusStates"), 
		TemplateVisualState(Name = "ReadOnly", GroupName = "CommonStates")]
	public abstract class HMINumericBoxBase : HMITextBoxBase
	{
		private const string ElementUnit = "UnitElement";

		private IHMIConnector connector;
		private TextBlock tbUnitElement;

        private VirtualKeyboardSettings virtualKeyboardSettings;

        public static readonly DependencyProperty PrecisionProperty = DependencyProperty.Register("Precision", typeof(int), typeof(HMINumericBoxBase), new PropertyMetadata(2, new PropertyChangedCallback(HMINumericBoxBase.OnPrecisionPropertyChanged)));
		public static readonly DependencyProperty UnitProperty = DependencyProperty.Register("Unit", typeof(string), typeof(HMINumericBoxBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMINumericBoxBase.OnUnitPropertyChanged)));

		[Category("HMI")]
		public int Precision
		{
			get
			{
				return (int)base.GetValue(HMINumericBoxBase.PrecisionProperty);
			}
			set
			{
				base.SetValue(HMINumericBoxBase.PrecisionProperty, value);
			}
		}
		[Category("HMI")]
		public string Unit
		{
			get
			{
				return (string)base.GetValue(HMINumericBoxBase.UnitProperty);
			}
			set
			{
				base.SetValue(HMINumericBoxBase.UnitProperty, value);
			}
		}
		public override VirtualKeyboardSettings VirtualKeyboardSettings
		{
			get
			{
				VirtualKeyboardSettings arg_23_0;
				if ((arg_23_0 = this.virtualKeyboardSettings) == null)
				{
					arg_23_0 = (this.virtualKeyboardSettings = new VirtualKeyboardSettings(this, VirtualKeyboardControlActivation.Manual)
					{
						VirtualKeyboardType = VirtualKeyboardType.Numeric
					});
				}
				return arg_23_0;
			}
		}
		private static void OnPrecisionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			HMINumericBoxBase wFNumericBoxBase = d as HMINumericBoxBase;
			if (wFNumericBoxBase == null)
			{
				return;
			}
			if ((int)e.NewValue < 0)
			{
				wFNumericBoxBase.SetValue(e.Property, e.OldValue);
			}
		}
		private static void OnUnitPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		public HMINumericBoxBase()
		{
			base.DefaultStyleKey=(typeof(HMINumericBoxBase));
		}
		private void OnUnitTranslationsChangedHandler(string[] symbolicTexts, string[] translationTexts)
		{
			if (translationTexts.Length <= 0)
			{
				return;
			}
			this.OnUpdateOutput();
			this.UpdateUnitElementText(translationTexts[0]);
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			bool isInDesignTool = DesignerPropertiesExt.IsInDesignTool;
			this.connector = RuntimeServices.GetRequiredService<IHMIConnector>();
			if (isInDesignTool)
			{
				return;
			}
			this.InitializeUnitElement();
		}
		private void InitializeUnitElement()
		{
			this.tbUnitElement = (base.GetTemplateChild(ElementUnit) as TextBlock);
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
		private void UpdateUnitElementText(string unitTranslation)
		{
			if (this.tbUnitElement == null)
			{
				return;
			}
			try
			{
				this.tbUnitElement.Text=(unitTranslation);
			}
			catch (Exception ex)
			{
				base.Connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
			}
		}
		public override void OnUpdateOutput()
		{
			try
			{
				base.Text=(base.ConvertSignalValueToDouble(base.SignalValue).ToString("F" + this.Precision, base.CurrentCultureInfo.NumberFormat));
			}
			catch (Exception ex)
			{
				base.Connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
			}
		}
		protected override bool ShouldHandlePressedKey(KeyEventArgs e)
		{
			PressedKeyInformation pressedKeyInformation = this.CreatePressedKeyInformation(e);
			return this.ShouldHandlePressedKey(pressedKeyInformation);
		}
		protected override bool ShouldHandlePressedKey(PressedKeyInformation pressedKeyInformation)
		{
            if (pressedKeyInformation.Key == Key.Subtract)
			{
				return true;
			}
			IKeyPressedHandleChecker keyPressedHandleChecker = this.CreateKeyPressedHandleChecker();
			return keyPressedHandleChecker.ShouldHandlePressedKey(pressedKeyInformation);
		}
		private PressedKeyInformation CreatePressedKeyInformation(KeyEventArgs e)
		{
			return new PressedKeyInformation
			{
				Key = e.Key,
				KeyboardModifiers = Keyboard.Modifiers//,
			};
		}
		private IKeyPressedHandleChecker CreateKeyPressedHandleChecker()
		{
			return new KeyPressedHandleChecker
			{
				TextBoxInformation = this.CreateTextBoxInformation(),
				NumberFormat = this.CreateNumberFormat()
			};
		}
		private TextBoxInformation CreateTextBoxInformation()
		{
			return new TextBoxInformation
			{
				IsInEditMode = base.IsInEditMode,
				Precision = this.Precision,
				SelectionLength = base.SelectionLength,
				SelectionStart = base.SelectionStart,
				Text = base.Text
			};
		}
		private NumberFormat CreateNumberFormat()
		{
			return new NumberFormat
			{
				DecimalSeparator = base.CurrentCultureInfo.NumberFormat.NumberDecimalSeparator[0],
				NegativeSign = base.CurrentCultureInfo.NumberFormat.NegativeSign[0]
			};
		}
		protected override void HandlePressedKey(string key, PressedKeyInformation pressedKeyInformation)
		{
			if (pressedKeyInformation.Key == Key.Escape)
			{
				this.HandlePressedSubtractKey();
				return;
			}
			base.HandlePressedKey(key, pressedKeyInformation);
		}
		private void HandlePressedSubtractKey()
		{
			if (this.HasNegativeSign())
			{
				this.RemoveNegativeSignFromText();
				return;
			}
			this.AddNegativeSignToText();
		}
		private bool HasNegativeSign()
		{
			return this.GetNegativeSignIndex() != -1;
		}
		private int GetNegativeSignIndex()
		{
			return base.Text.IndexOf("-");
		}
		private void AddNegativeSignToText()
		{
			base.Text=(base.Text.Insert(0, "-"));
		}
		private void RemoveNegativeSignFromText()
		{
			base.Text=(base.Text.Remove(0, 1));
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
