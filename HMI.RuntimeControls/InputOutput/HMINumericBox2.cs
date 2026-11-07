// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using HMI.Runtime.Core;
namespace HMI.RuntimeControls.InputOutput
{
	[
		TemplatePart(Name = "DisabledVisualElement", Type = typeof(FrameworkElement)), 
		TemplatePart(Name = "EditElement", Type = typeof(ButtonBase)), 
		TemplatePart(Name = "CancelElement", Type = typeof(ButtonBase)), 
		TemplatePart(Name = "ReadOnlyVisualElement", Type = typeof(FrameworkElement)), 
		TemplatePart(Name = "FocusVisualElement", Type = typeof(FrameworkElement)), 
		TemplatePart(Name = "ApplyElement", Type = typeof(ButtonBase)), 
		TemplatePart(Name = "ContentElement", Type = typeof(FrameworkElement)), 
		TemplatePart(Name = "RootElement", Type = typeof(FrameworkElement)), 
		TemplateVisualState(Name = "Normal", GroupName = "CommonStates"), 
		TemplateVisualState(Name = "NormalMode", GroupName = "EditStates"), 
		TemplateVisualState(Name = "Focused", GroupName = "FocusStates"), 
		TemplateVisualState(Name = "Disabled", GroupName = "CommonStates"), 
		TemplateVisualState(Name = "Unfocused", GroupName = "FocusStates"), 
		TemplateVisualState(Name = "MouseOver", GroupName = "CommonStates"), 
		TemplateVisualState(Name = "EditingMode", GroupName = "EditStates"), 
		TemplateVisualState(Name = "ReadOnly", GroupName = "CommonStates")]
	public class HMINumericBox2 : HMINumericBoxBase
	{
		private const string ApplyElement = "ApplyElement";
		private const string EditElement = "EditElement";
		private const string CancelElement = "CancelElement";
		private const string VisualStateNormalMode = "NormalMode";
		private const string VisualStateEditingMode = "EditingMode";
		private const string VisualStateGroupEditStates = "EditStates";

		private char negativeSign;
		private char numberDecimalSeparator;

		private ButtonBase? applyElement;
		private ButtonBase? cancelElement;
		private ButtonBase? editElement;
		private CultureInfo cultureInfo;

		protected override bool VKActiveOnEditModeOnly
		{
			get
			{
				return true;
			}
		}
		public HMINumericBox2()
		{
			base.DefaultStyleKey=(typeof(HMINumericBox2));
		}
		private void applyElement_Click(object sender, RoutedEventArgs e)
		{
			base.Focus();
			if (base.IsAuthorized && !string.IsNullOrEmpty(base.SignalName))
			{
				try
				{
					double num = base.ConvertSignalValueToDouble(base.Text);
					base.WriteSignal(base.ServerName, base.SignalName, num);
					this.OnUpdateOutput();
				}
				catch (Exception ex)
				{
					base.Connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
				}
			}
			base.IsInEditMode = false;
		}
		private void cancelElement_Click(object sender, RoutedEventArgs e)
		{
			base.Focus();
			base.IsInEditMode = false;
			this.OnUpdateOutput();
		}
		private void editElement_Click(object sender, RoutedEventArgs e)
		{
			base.Focus();
			base.IsInEditMode = true;
		}
		public override void OnApplyTemplate()
		{
			this.applyElement = null;
			this.cancelElement = null;
			this.editElement = null;
			this.cultureInfo = CultureInfo.CurrentCulture;
			this.numberDecimalSeparator = this.cultureInfo.NumberFormat.NumberDecimalSeparator.ToCharArray()[0];
			this.negativeSign = this.cultureInfo.NumberFormat.NegativeSign.ToCharArray()[0];
			base.OnApplyTemplate();
			this.applyElement = (base.GetTemplateChild("ApplyElement") as ButtonBase);
			if (this.applyElement != null)
			{
				this.applyElement.IsTabStop=(false);
				this.applyElement.Click+=(new RoutedEventHandler(this.applyElement_Click));
			}
			this.editElement = (base.GetTemplateChild("EditElement") as ButtonBase);
			if (this.editElement != null)
			{
				this.editElement.IsTabStop=(false);
				this.editElement.Click+=(new RoutedEventHandler(this.editElement_Click));
			}
			this.cancelElement = (base.GetTemplateChild("CancelElement") as ButtonBase);
			if (this.cancelElement != null)
			{
				this.cancelElement.IsTabStop=(false);
				this.cancelElement.Click+=(new RoutedEventHandler(this.cancelElement_Click));
			}
		}
		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);
            if (base.IsInEditMode && e.Key == Key.Escape)
			{
				base.IsInEditMode = false;
				this.OnUpdateOutput();
				e.Handled=(true);
			}
		}
		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (!base.IsInEditMode)
			{
				e.Handled=(!base.IsNavigationKey(e.Key));
			}
			base.OnKeyDown(e);
		}
		private void ChangeVisualState()
		{
			if (base.IsInEditMode)
			{
				VisualStateManager.GoToState(this, "EditingMode", true);
				return;
			}
			VisualStateManager.GoToState(this, "NormalMode", true);
			if (base.SelectionLength > 0)
			{
				base.SelectionLength=(0);
			}
		}
		protected override void OnIsInEditModeChanged()
		{
			base.OnIsInEditModeChanged();
			this.ChangeVisualState();
		}
	}
}
