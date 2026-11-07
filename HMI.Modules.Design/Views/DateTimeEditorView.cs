// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls.Primitives;
using Telerik.Windows.Controls;
using HMI.Common.Presentation.Views;
namespace HMI.Modules.Design.Views
{
	[ExcludeFromCodeCoverage, TemplatePart(Name = "uxEditor", Type = typeof(RadDatePicker))]
	public class DateTimeEditorView : EditorView
	{
		private const string EditorTemplatePartName = "uxEditor";
		private RadDatePicker _editorDatePicker;
		public static readonly DependencyProperty DatePickerModeProperty;
		public InputMode DatePickerMode
		{
			get
			{
				return (InputMode)base.GetValue(DateTimeEditorView.DatePickerModeProperty);
			}
			set
			{
				base.SetValue(DateTimeEditorView.DatePickerModeProperty, value);
			}
		}
		static DateTimeEditorView()
		{
			DateTimeEditorView.DatePickerModeProperty = DependencyProperty.Register("DatePickerMode", typeof(InputMode), typeof(DateTimeEditorView), null);
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(DateTimeEditorView), new FrameworkPropertyMetadata(typeof(DateTimeEditorView)));
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
            this._editorDatePicker = this.FindTemplateChild<RadDatePicker>(EditorTemplatePartName);
			if (this._editorDatePicker != null)
			{
				this._editorDatePicker.SetBinding(ToggleButton.IsCheckedProperty, base.ValueBinding);
			}
		}
	}
}
