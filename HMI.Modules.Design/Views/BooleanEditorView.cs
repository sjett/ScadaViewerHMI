// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using HMI.Common.Presentation.Views;
namespace HMI.Modules.Design.Views
{
	[ExcludeFromCodeCoverage, TemplatePart(Name = "uxEditor", Type = typeof(CheckBox))]
	public class BooleanEditorView : EditorView
	{
		private const string EditorTemplatePartName = "uxEditor";
		private CheckBox _editorCheckBox;
		static BooleanEditorView()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(BooleanEditorView), new FrameworkPropertyMetadata(typeof(BooleanEditorView)));
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this._editorCheckBox = this.FindTemplateChild<CheckBox>(EditorTemplatePartName);
			if (this._editorCheckBox != null)
			{
				this._editorCheckBox.SetBinding(ToggleButton.IsCheckedProperty, base.ValueBinding);
			}
		}
	}
}
