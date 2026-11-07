// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using System.Windows.Controls;
namespace HMI.Editor.Surface.Transformations
{
	public class SelectionDecorator : Control
	{
		static SelectionDecorator()
		{
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SelectionDecorator), new FrameworkPropertyMetadata(typeof(SelectionDecorator)));
		}
	}
}
