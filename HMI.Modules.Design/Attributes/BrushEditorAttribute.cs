// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows.Data;
using HMI.Domain.Models;
using HMI.Infrastructure.Converters;
using HMI.Modules.Design.Views;

namespace HMI.Modules.Design.Attributes
{
	public class BrushEditorAttribute : PropertyEditorAttribute
	{
		protected override IEditorView BuildEditor(IProperty property, IBindingSource editedObject)
		{
			return new ColorEditorView();
		}
		public override bool IsMultiSelectionCompatibleWith(IProperty property, IProperty otherProperty)
		{
			return otherProperty.PropertyEditorBuilder is BrushEditorAttribute;
		}
		protected override Binding CreateValueBinding(IBindingSource bindingSource, IProperty property)
		{
			Binding binding = base.CreateValueBinding(bindingSource, property);
			binding.Converter = new SolidColorBrushToColorConverter();
			return binding;
		}
	}
}
