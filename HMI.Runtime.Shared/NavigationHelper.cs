// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Resources;
using HMI.Runtime.Shared.Common;
using Microsoft.Xaml.Behaviors;

namespace HMI.Runtime.Shared
{
	public static class NavigationHelper
	{
		public static UserControl CreateUserControl(Control currentControl, string source)
		{
			string userControlName;
			string text;
			NavigationHelper.ParseSource(source, out userControlName, out text);
			if (string.IsNullOrEmpty(text))
			{
				return NavigationHelper.FindUserControl(userControlName, currentControl);
			}
			Assembly assembly = NavigationHelper.LoadAssemblyFromResources(text);
			return NavigationHelper.FindUserControl(userControlName, assembly);
		}
		private static void ParseSource(string source, out string userControlName, out string assemblyName)
		{
			if (source.Contains(":"))
			{
				int num = source.IndexOf(':');
				assemblyName = source.Substring(0, num);
				userControlName = source.Substring(num + 1);
                if (!assemblyName.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase))
				{
					assemblyName += ".dll";
					return;
				}
			}
			else
			{
				assemblyName = string.Empty;
				userControlName = source;
			}
		}
		private static UserControl FindUserControl(string userControlName, Control relativeControl)
		{
			UserControl userControl = NavigationHelper.FindFirstUserControlParent(relativeControl);
			if (userControl == null)
			{
				return null;
			}
			UserControl userControl2 = NavigationHelper.FindUserControl(userControlName, userControl.GetType().Assembly);
			if (userControl2 != null)
			{
				return userControl2;
			}
			return NavigationHelper.FindUserControl(userControlName, userControl);
		}
		public static UserControl FindFirstUserControlParent(DependencyObject source)
		{
			UserControl userControl = null;
			for (DependencyObject parent = VisualTreeHelperWrapper.GetParent(source); parent != null; parent = VisualTreeHelperWrapper.GetParent(parent))
			{
				userControl = (parent as UserControl);
				if (userControl != null)
				{
					break;
				}
			}
			return userControl;
		}
		private static UserControl FindUserControl(string userControlName, Assembly assembly)
		{
			if (assembly == null)
			{
				return null;
			}
			string fullControlName = NavigationHelper.GetFullControlName(userControlName, assembly);
			if (fullControlName == null)
			{
				return null;
			}
			return assembly.CreateInstance(fullControlName) as UserControl;
		}
		private static string GetFullControlName(string controlName, Assembly assembly)
		{
			if (controlName.Contains("."))
			{
				return controlName;
			}
			Type[] types = assembly.GetTypes();
			for (int i = 0; i < types.Length; i++)
			{
				Type type = types[i];
				if (type.Name.Equals(controlName, StringComparison.InvariantCultureIgnoreCase))
				{
					return type.FullName;
				}
			}
			return null;
		}
		private static Assembly LoadAssemblyFromResources(string assemblyName)
		{
			StreamResourceInfo resourceStream = Application.GetResourceStream(new Uri(assemblyName, UriKind.Relative));
			if (resourceStream != null && resourceStream.Stream != null)
			{
                byte[] assemblyRawBytes = new byte[resourceStream.Stream.Length];
                resourceStream.Stream.Read(assemblyRawBytes, 0, assemblyRawBytes.Length);
                return Assembly.Load(assemblyRawBytes);

				//return new AssemblyPart().Load(resourceStream.Stream);
			}
			return null;
		}
		public static ContentControl FindControl(DependencyObject source, string controlName)
		{
			ContentControl contentControl = source as ContentControl;
			if (contentControl != null && contentControl.Name == controlName)
			{
				return contentControl;
			}
			int childrenCount = VisualTreeHelperWrapper.GetChildrenCount(source);
			if (childrenCount == 0)
			{
				return null;
			}
			contentControl = null;
			for (int i = 0; i < childrenCount; i++)
			{
				DependencyObject child = VisualTreeHelperWrapper.GetChild(source, i);
				contentControl = NavigationHelper.FindControl(child, controlName);
				if (contentControl != null)
				{
					break;
				}
			}
			return contentControl;
		}
		public static IHMINavigationParameter FindParameterControl(DependencyObject source)
		{
			IHMINavigationParameter? iHMINavigationParameter = source as IHMINavigationParameter;
			if (iHMINavigationParameter != null)
			{
				return iHMINavigationParameter;
			}
			int childrenCount = VisualTreeHelperWrapper.GetChildrenCount(source);
			if (childrenCount == 0)
			{
				return null;
			}
			iHMINavigationParameter = null;
			for (int i = 0; i < childrenCount; i++)
			{
				DependencyObject child = VisualTreeHelperWrapper.GetChild(source, i);
				iHMINavigationParameter = NavigationHelper.FindParameterControl(child);
				if (iHMINavigationParameter != null)
				{
					break;
				}
			}
			return iHMINavigationParameter;
		}
		public static void DisposeControls(DependencyObject source)
		{
			if (source == null)
			{
				return;
			}
			int childrenCount = VisualTreeHelperWrapper.GetChildrenCount(source);
			for (int i = childrenCount - 1; i >= 0; i--)
			{
				DependencyObject child = VisualTreeHelperWrapper.GetChild(source, i);
				NavigationHelper.DisposeControls(child);
			}
			ContentControl contentControl = source as ContentControl;
			if (contentControl != null)
			{
				NavigationHelper.DisposeControls(contentControl.Content as DependencyObject);
			}
			if (source is IDisposable)
			{
				((IDisposable)source).Dispose();
			}
			NavigationHelper.RemoveBehaviors(source);
			NavigationHelper.RemoveTriggers(source);
		}
        public static void RemoveTriggers(DependencyObject target)
        {

            var triggers = Interaction.GetTriggers(target);


            var list = triggers.ToList();

            foreach (var trigger in list)
            {
                trigger.Detach();
                triggers.Remove(trigger);
            }


            if (target is FrameworkElement fe)
            {
                fe.Triggers.Clear();
            }
        }


		private static void RemoveBehaviors(DependencyObject target)
		{
			BehaviorCollection behaviors = Interaction.GetBehaviors(target);
			using (IEnumerator<Behavior> enumerator = behaviors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Behavior current = enumerator.Current;
					current.Detach();
				}
			}
		}
	}
}
