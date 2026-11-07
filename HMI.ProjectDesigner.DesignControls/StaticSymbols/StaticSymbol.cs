// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using HMI.Infrastructure.Controls;
using HMI.Infrastructure.Presentation.Views;
namespace HMI.ProjectDesigner.DesignControls.StaticSymbols
{
	public class StaticSymbol : CommonControl, IStaticSymbol
	{
		public static readonly System.Windows.DependencyProperty ContentStringProperty;
		public string ContentString
		{
			get
			{
				return (string)base.GetValue(StaticSymbol.ContentStringProperty);
			}
			set
			{
				base.SetValue(StaticSymbol.ContentStringProperty, value);
			}
		}
		static StaticSymbol()
		{
			StaticSymbol.ContentStringProperty = System.Windows.DependencyProperty.Register("ContentString", typeof(string), typeof(StaticSymbol), null);
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(StaticSymbol), new System.Windows.FrameworkPropertyMetadata(typeof(StaticSymbol)));
		}
	}
}
