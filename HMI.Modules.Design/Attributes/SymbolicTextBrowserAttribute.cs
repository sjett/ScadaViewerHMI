// SPDX-License-Identifier: GPL-3.0-only
using System;
using HMI.Domain.Models;
using HMI.Modules.Design.Views;

namespace HMI.Modules.Design.Attributes
{
	public class SymbolicTextBrowserAttribute : PropertyEditorAttribute, ITemplatableEditor
	{
		protected override IEditorView BuildEditor(IProperty property, IBindingSource editedObject)
		{
			return new SymbolicTextEditorView();
		}
		public override bool IsMultiSelectionCompatibleWith(IProperty property, IProperty otherProperty)
		{
			return otherProperty.PropertyEditorBuilder is SymbolicTextBrowserAttribute;
		}
	}
}
