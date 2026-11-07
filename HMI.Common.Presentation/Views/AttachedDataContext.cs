// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using System.Windows.Data;
namespace HMI.Common.Presentation.Views
{
	public class AttachedDataContext : Freezable
	{
		public static readonly DependencyProperty DataContextProperty = FrameworkElement.DataContextProperty.AddOwner(typeof(AttachedDataContext));
		public object DataContext
		{
			get
			{
				return base.GetValue(AttachedDataContext.DataContextProperty);
			}
			set
			{
				base.SetValue(AttachedDataContext.DataContextProperty, value);
			}
		}
		public AttachedDataContext()
		{
			BindingOperations.SetBinding(this, AttachedDataContext.DataContextProperty, new Binding());
		}
		protected override Freezable CreateInstanceCore()
		{
			throw new NotImplementedException();
		}
	}
}
