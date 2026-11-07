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
	public class FontStretchEditorView : EditorView
	{
		private const string EditorTemplatePartName = "uxEditor";
		private RadComboBox _editorComboBox;
		public IEnumerable<string> Stretches
		{
			get;
			private set;
		}
		static FontStretchEditorView()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(FontStretchEditorView), new FrameworkPropertyMetadata(typeof(FontStretchEditorView)));
		}
		public FontStretchEditorView()
		{
			this.Stretches = new string[]
			{
				"UltraCondensed",
				"ExtraCondensed",
				"Condensed",
				"SemiCondensed",
				"Normal",
				"SemiExpanded",
				"Expanded",
				"ExtraExpanded",
				"UltraExpanded"
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
