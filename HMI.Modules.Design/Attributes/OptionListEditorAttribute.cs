// SPDX-License-Identifier: GPL-3.0-only
using HMI.Domain.Models;
using HMI.Modules.Design.Resources;
using HMI.Modules.Design.Views;

namespace HMI.Modules.Design.Attributes
{
	public class OptionListEditorAttribute : CollectionBoundEditorAttribute
	{
		public bool IsEditable
		{
			get;
			set;
		}
		public bool IsUnselectable
		{
			get;
			set;
		}
		public string DisplayMemberPath
		{
			get;
			set;
		}
		public OptionListEditorAttribute(string dataSourcePropertyName) : base(dataSourcePropertyName)
		{
		}
		protected override IEditorView CreateEditor()
		{
			OptionListEditorView optionListEditorView = new OptionListEditorView();
			optionListEditorView.IsEditable = IsEditable;
			optionListEditorView.IsUnselectable = IsUnselectable;
			if (!string.IsNullOrEmpty(DisplayMemberPath))
			{
				optionListEditorView.DisplayMemberPath = DisplayMemberPath;
			}
			return optionListEditorView;
		}
		public override bool IsMultiSelectionCompatibleWith(IProperty property, IProperty otherProperty)
		{
			OptionListEditorAttribute optionListEditorAttribute = otherProperty.PropertyEditorBuilder as OptionListEditorAttribute;
			if (optionListEditorAttribute != null && IsEditable == optionListEditorAttribute.IsEditable)
			{
				if (IsUnselectable == optionListEditorAttribute.IsUnselectable)
				{
					if (property.SourceInstance != null && otherProperty.SourceInstance != null)
					{
						return IsDataSourceCompatibileWith(property, otherProperty);
					}
					throw new InvalidOperationException(ErrorTexts.InitializeMustBeCalledBeforeIsMultiSelectionCompatibileWith);
				}
			}
			return false;
		}
	}
}
