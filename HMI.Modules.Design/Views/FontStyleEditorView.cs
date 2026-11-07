// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls.Primitives;
using Telerik.Windows.Controls;
using HMI.Common.Presentation.Views;
namespace HMI.Modules.Design.Views
{
	[ExcludeFromCodeCoverage, TemplatePart(Name = "uxEditor", Type = typeof(RadComboBox))]
	public class FontStyleEditorView : EditorView
	{
		private const string EditorTemplatePartName = "uxEditor";
		private RadComboBox _editorComboBox;
		public IEnumerable<string> Styles
		{
			get;
			private set;
		}
		static FontStyleEditorView()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(FontStyleEditorView), new FrameworkPropertyMetadata(typeof(FontStyleEditorView)));
		}
		public FontStyleEditorView()
		{
			this.Styles = new string[]
			{
				"Normal",
				"Italic"
			};
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
