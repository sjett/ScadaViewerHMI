// SPDX-License-Identifier: GPL-3.0-only
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Telerik.Windows.Controls;
using HMI.Common.Presentation.Views;
namespace HMI.Modules.Design.Views
{
	[ExcludeFromCodeCoverage, TemplatePart(Name = "uxEditor", Type = typeof(RadComboBox))]
	public class OptionListEditorView : ItemControlEditorView
	{
		private const string EditorTemplatePartName = "uxEditor";
		private RadComboBox _editorComboBox;
		public static readonly DependencyProperty IsEditableProperty;
		public static readonly DependencyProperty IsUnselectableProperty;
		public bool IsEditable
		{
			get
			{
				return (bool)base.GetValue(IsEditableProperty);
			}
			set
			{
				base.SetValue(IsEditableProperty, value);
			}
		}
		public bool IsUnselectable
		{
			get
			{
				return (bool)base.GetValue(IsUnselectableProperty);
			}
			set
			{
				base.SetValue(IsUnselectableProperty, value);
			}
		}
		static OptionListEditorView()
		{
			IsEditableProperty = DependencyProperty.Register("IsEditable", typeof(bool), typeof(OptionListEditorView), null);
			IsUnselectableProperty = DependencyProperty.Register("IsUnselectable", typeof(bool), typeof(OptionListEditorView), null);
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(OptionListEditorView), new FrameworkPropertyMetadata(typeof(OptionListEditorView)));
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			_editorComboBox = this.FindTemplateChild<RadComboBox>(EditorTemplatePartName);
			if (_editorComboBox != null)
			{
				_editorComboBox.SetBinding(IsEditable ? RadComboBox.TextProperty : Selector.SelectedValueProperty, base.ValueBinding);
			}
		}
		public override void SetErrorTemplate(object template)
		{
			if (_editorComboBox != null)
			{
				_editorComboBox.SetValue(Validation.ErrorTemplateProperty, template);
			}
		}
	}
}
