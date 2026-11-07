// SPDX-License-Identifier: GPL-3.0-only
using HMI.Domain.Models;
using HMI.Modules.Design.Views;
using Telerik.Windows.Controls;

namespace HMI.Modules.Design.Attributes
{
	public class DateTimeEditorAttribute : PropertyEditorAttribute
	{
		public DatePickerMode DatePickerMode
		{
			get;
			set;
		}
		protected override IEditorView BuildEditor(IProperty property, IBindingSource editedObject)
		{
			return new DateTimeEditorView
			{
				DatePickerMode = GetInputMode()
            };
		}
		public override bool IsMultiSelectionCompatibleWith(IProperty property, IProperty otherProperty)
		{
			DateTimeEditorAttribute? dateTimeEditorAttribute = otherProperty.PropertyEditorBuilder as DateTimeEditorAttribute;
			return dateTimeEditorAttribute != null && dateTimeEditorAttribute.DatePickerMode == this.DatePickerMode;
		}
        private InputMode GetInputMode()
        {
            switch (DatePickerMode)
            {
                case DatePickerMode.Date:
                    return InputMode.DatePicker;
                case DatePickerMode.Time:
                    return InputMode.TimePicker;
                default:
                    return InputMode.DateTimePicker;
            }
        }
    }
}
