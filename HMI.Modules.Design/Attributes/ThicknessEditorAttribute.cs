// SPDX-License-Identifier: GPL-3.0-only
using System;
using HMI.Domain.Models;
using HMI.Modules.Design.Views;
namespace HMI.Modules.Design.Attributes
{
	public class ThicknessEditorAttribute : PropertyEditorAttribute
	{
		protected override IEditorView BuildEditor(IProperty property, IBindingSource editedObject)
		{
			return new ThicknessEditorView();
		}
		public override bool IsMultiSelectionCompatibleWith(IProperty property, IProperty otherProperty)
		{
			ThicknessEditorAttribute thicknessEditorAttribute = otherProperty.PropertyEditorBuilder as ThicknessEditorAttribute;
			return thicknessEditorAttribute != null;
		}
	}
}
