// SPDX-License-Identifier: GPL-3.0-only
using HMI.Domain.Models;
using HMI.Modules.Design.Resources;
using HMI.Modules.Design.Views;
using HMI.Common.Instrumentation.ErrorHandling;

namespace HMI.Modules.Design.Attributes
{
	public class EnumEditorAttribute : PropertyEditorAttribute
	{
		private readonly Type enumType;
		public EnumEditorAttribute(Type enumType)
		{
			Contract.Requires<ArgumentException>(enumType.IsEnum, ErrorTexts.TypeMustBeEnum);
			this.enumType = enumType;
		}
		protected override IEditorView BuildEditor(IProperty property, IBindingSource editedObject)
		{
			return new EnumEditorView(this.enumType);
		}
		public override bool IsMultiSelectionCompatibleWith(IProperty property, IProperty otherProperty)
		{
			EnumEditorAttribute? enumEditorAttribute = otherProperty.PropertyEditorBuilder as EnumEditorAttribute;
			return enumEditorAttribute != null && enumEditorAttribute.enumType == this.enumType;
		}
	}
}
