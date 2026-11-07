// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
namespace HMI.Modules.Design.Views
{
	[ExcludeFromCodeCoverage]
	public class HorizontalAlignmentEditorView : EditorView
	{
		public static readonly DependencyProperty AlignmentProperty;
		public HorizontalAlignment Alignment
		{
			get
			{
				return (HorizontalAlignment)base.GetValue(HorizontalAlignmentEditorView.AlignmentProperty);
			}
			set
			{
				base.SetValue(HorizontalAlignmentEditorView.AlignmentProperty, value);
			}
		}
		static HorizontalAlignmentEditorView()
		{
			HorizontalAlignmentEditorView.AlignmentProperty = DependencyProperty.Register("Alignment", typeof(HorizontalAlignment), typeof(HorizontalAlignmentEditorView), new PropertyMetadata(HorizontalAlignment.Left));
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HorizontalAlignmentEditorView), new FrameworkPropertyMetadata(typeof(HorizontalAlignmentEditorView)));
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			base.SetBinding(HorizontalAlignmentEditorView.AlignmentProperty, base.ValueBinding);
		}
	}
}
