// SPDX-License-Identifier: GPL-3.0-only
using System;
using HMI.Domain.Models;
using HMI.Modules.Design.Views;
namespace HMI.Modules.Design.Attributes
{
	public class TextAlignmentEditorAttribute : PropertyEditorAttribute
	{
		protected  override IEditorView BuildEditor(IProperty property, IBindingSource editedObject)
		{
			return new TextAlignmentEditorView();
		}
		public override bool IsMultiSelectionCompatibleWith(IProperty property, IProperty otherProperty)
		{
			return otherProperty.PropertyEditorBuilder is TextAlignmentEditorAttribute;
		}
	}
}
