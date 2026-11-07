// SPDX-License-Identifier: GPL-3.0-only
using System;
using HMI.Domain.Models;
using HMI.Modules.Design.Views;
namespace HMI.Modules.Design.Attributes
{
	public class HorizontalAlignmentEditorAttribute : PropertyEditorAttribute
	{
		protected  override IEditorView BuildEditor(IProperty property, IBindingSource editedObject)
		{
			return new HorizontalAlignmentEditorView();
		}
		public override bool IsMultiSelectionCompatibleWith(IProperty property, IProperty otherProperty)
		{
			var horizontalAlignmentEditorAttribute = otherProperty.PropertyEditorBuilder as HorizontalAlignmentEditorAttribute;
			return horizontalAlignmentEditorAttribute != null;
		}
	}
}
