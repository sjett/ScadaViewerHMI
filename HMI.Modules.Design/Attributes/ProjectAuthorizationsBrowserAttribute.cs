// SPDX-License-Identifier: GPL-3.0-only
using HMI.Domain.Models;
using HMI.Modules.Design.Views;
namespace HMI.Modules.Design.Attributes
{
	public class ProjectAuthorizationsBrowserAttribute : PropertyEditorAttribute, ITemplatableEditor
	{
		protected override IEditorView BuildEditor(IProperty property, IBindingSource editedObject)
		{
			return new ProjectAuthorizationEditorView();
		}
		public override bool IsMultiSelectionCompatibleWith(IProperty property, IProperty otherProperty)
		{
			return otherProperty.PropertyEditorBuilder is ProjectAuthorizationsBrowserAttribute;
		}
	}
}
