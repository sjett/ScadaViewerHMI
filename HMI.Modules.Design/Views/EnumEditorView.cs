// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls.Primitives;
using Telerik.Windows.Controls;
using HMI.Common.Presentation.Views;
namespace HMI.Modules.Design.Views
{
	[ExcludeFromCodeCoverage, TemplatePart(Name = "uxEditor", Type = typeof(RadComboBox))]
	public class EnumEditorView : EditorView
	{
		private const string EditorTemplatePartName = "uxEditor";
		private RadComboBox _editorComboBox;
		public ICollection Values
		{
			get;
			private set;
		}
		static EnumEditorView()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(EnumEditorView), new FrameworkPropertyMetadata(typeof(EnumEditorView)));
		}
		public EnumEditorView()
		{
		}
		public EnumEditorView(Type enumType)
		{
			this.Values = Enum.GetValues(enumType);
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
            this._editorComboBox = this.FindTemplateChild<RadComboBox>(EditorTemplatePartName);
			if (this._editorComboBox != null)
			{
				this._editorComboBox.SetBinding(Selector.SelectedValueProperty, base.ValueBinding);
			}
		}
	}
}
