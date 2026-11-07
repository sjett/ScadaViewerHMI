// SPDX-License-Identifier: GPL-3.0-only
using HMI.Modules.Design.Views;
using System;
using System.Windows;
using System.Windows.Controls;
namespace HMI.Modules.PropertyEditor.Views
{
	public class PropertyEditor
	{
		public static readonly DependencyProperty ValidationErrorTemplateProperty = 
			DependencyProperty.RegisterAttached("ValidationErrorTemplate", typeof(ControlTemplate), typeof(PropertyEditor),
				new FrameworkPropertyMetadata(
					null, 
					FrameworkPropertyMetadataOptions.Inherits | FrameworkPropertyMetadataOptions.NotDataBindable, 
					new PropertyChangedCallback(OnValidationErrorTemplateChanged)));
		public static void SetValidationErrorTemplate(UIElement element, ControlTemplate value)
		{
			element.SetValue(ValidationErrorTemplateProperty, value);
		}
		public static ControlTemplate GetValidationErrorTemplate(UIElement element)
		{
			return (ControlTemplate)element.GetValue(ValidationErrorTemplateProperty);
		}
        private static void OnValidationErrorTemplateChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            if (!(dependencyObject is IEditorView))
            {
                return;
            }
            dependencyObject.SetValue(Validation.ErrorTemplateProperty, e.NewValue);
        }
    }
}
