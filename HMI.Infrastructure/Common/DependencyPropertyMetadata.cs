// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Reflection;
using System.Security;
using System.Windows;

namespace HMI.Infrastructure.Common
{
    public static class DependencyPropertyMetadata
	{
		public static void OverridePropertyCallback<T>(DependencyProperty property, PropertyChangedCallback callback) where T : DependencyObject
		{
			FrameworkPropertyMetadata typeMetadata = new FrameworkPropertyMetadata(callback);
			property.OverrideMetadata(typeof(T), typeMetadata);
		}
	}
}
