// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
namespace HMI.Modules.Design.Views
{
	[ExcludeFromCodeCoverage]
	public class TextAlignmentEditorView : EditorView
	{
		public static readonly DependencyProperty AlignmentProperty;
		public TextAlignment Alignment
		{
			get
			{
				return (TextAlignment)base.GetValue(TextAlignmentEditorView.AlignmentProperty);
			}
			set
			{
				base.SetValue(TextAlignmentEditorView.AlignmentProperty, value);
			}
		}
		static TextAlignmentEditorView()
		{
			TextAlignmentEditorView.AlignmentProperty = DependencyProperty.Register("Alignment", typeof(TextAlignment), typeof(TextAlignmentEditorView), new PropertyMetadata(TextAlignment.Left));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(TextAlignmentEditorView), new FrameworkPropertyMetadata(typeof(TextAlignmentEditorView)));
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			base.SetBinding(TextAlignmentEditorView.AlignmentProperty, base.ValueBinding);
		}
	}
}
