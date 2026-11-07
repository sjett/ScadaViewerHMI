// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using HMI.Common.Presentation.Views;
namespace HMI.Modules.Design.Views
{
	[ExcludeFromCodeCoverage, TemplatePart(Name = "uxEditor", Type = typeof(TextBox))]
	public class TextEditorView : EditorView
	{
		private const string EditorTemplatePartName = "uxEditor";
		private TextBox _editorTextBox;
		public static readonly DependencyProperty IsReadOnlyProperty;
		public bool IsReadOnly
		{
			get
			{
				return (bool)base.GetValue(TextEditorView.IsReadOnlyProperty);
			}
			set
			{
				base.SetValue(TextEditorView.IsReadOnlyProperty, value);
			}
		}
		static TextEditorView()
		{
			TextEditorView.IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(TextEditorView));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(TextEditorView), new FrameworkPropertyMetadata(typeof(TextEditorView)));
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this._editorTextBox = this.FindTemplateChild<TextBox>(EditorTemplatePartName);
			if (this._editorTextBox != null)
			{
				this._editorTextBox.SetBinding(TextBox.TextProperty, base.ValueBinding);
			}
		}
	}
}
