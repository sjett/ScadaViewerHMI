// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
namespace HMI.Modules.Design.Views
{
	[ExcludeFromCodeCoverage]
	public class VerticalAlignmentEditorView : EditorView
	{
		public static readonly DependencyProperty AlignmentProperty;
		public VerticalAlignment Alignment
		{
			get
			{
				return (VerticalAlignment)base.GetValue(VerticalAlignmentEditorView.AlignmentProperty);
			}
			set
			{
				base.SetValue(VerticalAlignmentEditorView.AlignmentProperty, value);
			}
		}
		static VerticalAlignmentEditorView()
		{
			VerticalAlignmentEditorView.AlignmentProperty = DependencyProperty.Register("Alignment", typeof(VerticalAlignment), typeof(VerticalAlignmentEditorView), new PropertyMetadata(VerticalAlignment.Top));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VerticalAlignmentEditorView), new FrameworkPropertyMetadata(typeof(VerticalAlignmentEditorView)));
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			base.SetBinding(VerticalAlignmentEditorView.AlignmentProperty, base.ValueBinding);
		}
	}
}
