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
	public class FontFamilyEditorView : EditorView
	{
		private const string EditorTemplatePartName = "uxEditor";
		private RadComboBox _editorComboBox;
		public IEnumerable<string> Fonts
		{
			get;
			private set;
		}
		static FontFamilyEditorView()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(FontFamilyEditorView), new FrameworkPropertyMetadata(typeof(FontFamilyEditorView)));
		}
		public FontFamilyEditorView()
		{
			this.Fonts = new string[]
			{
				"Arial",
				"Comic Sans MS",
				"Courier New",
				"Georgia",
				"Lucida Sans Unicode",
				"Times New Roman",
				"Trebuchet MS",
				"Verdana",
				"Webdings"
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
