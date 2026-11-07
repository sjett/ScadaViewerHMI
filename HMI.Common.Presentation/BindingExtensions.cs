// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using System.Windows.Data;
namespace HMI.Common.Presentation
{
	public static class BindingExtensions
	{
		private static readonly DependencyProperty EvaluationValueProperty = DependencyProperty.RegisterAttached("Dummy", typeof(object), typeof(DependencyObject), new UIPropertyMetadata(null));
		public static object Eval(object container, string expression)
		{
			Binding binding = new Binding(expression)
			{
				Source = container
			};
			DependencyObject dependencyObject = new DependencyObject();
			BindingOperations.SetBinding(dependencyObject, EvaluationValueProperty, binding);
			return dependencyObject.GetValue(EvaluationValueProperty);
		}
		public static object Eval(this Binding binding)
		{
			DependencyObject dependencyObject = new DependencyObject();
			BindingOperations.SetBinding(dependencyObject, EvaluationValueProperty, binding);
			return dependencyObject.GetValue(EvaluationValueProperty);
		}
		public static void Set(this Binding binding, object value)
		{
			DependencyObject dependencyObject = new DependencyObject();
			BindingOperations.SetBinding(dependencyObject, EvaluationValueProperty, binding);
			dependencyObject.SetValue(EvaluationValueProperty, value);
		}
		public static void Set(object container, string expression, object value)
		{
			Binding binding = new Binding(expression)
			{
				Source = container
			};
			DependencyObject dependencyObject = new DependencyObject();
			BindingOperations.SetBinding(dependencyObject, EvaluationValueProperty, binding);
			dependencyObject.SetValue(EvaluationValueProperty, value);
		}
	}
}
