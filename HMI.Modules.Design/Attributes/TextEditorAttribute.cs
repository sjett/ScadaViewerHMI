// SPDX-License-Identifier: GPL-3.0-only
using HMI.Domain.Models;
using HMI.Infrastructure.Converters;
using HMI.Modules.Design.Views;
using System.Windows.Data;
namespace HMI.Modules.Design.Attributes
{
	public class TextEditorAttribute : PropertyEditorAttribute
	{
		public bool IsNullable
		{
			get;
			set;
		}
		protected override IEditorView BuildEditor(IProperty property, IBindingSource editedObject)
		{
			return new TextEditorView
			{
				IsReadOnly = property.IsReadOnly
			};
		}
		public override bool IsMultiSelectionCompatibleWith(IProperty property, IProperty otherProperty)
		{
			return otherProperty.PropertyEditorBuilder is TextEditorAttribute;
		}
		protected override Binding CreateValueBinding(IBindingSource bindingSource, IProperty property)
		{
			Binding binding = base.CreateValueBinding(bindingSource, property);
			if (this.IsNullable)
			{
				binding.Converter = new NullableValueConverter();
			}
			return binding;
		}
	}
}
