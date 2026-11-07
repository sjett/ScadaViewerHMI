// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;
namespace HMI.Runtime.Shared
{
	public static class UIElementExtensions
	{
		public static List<T> GetChildrenByType<T>(this UIElement element) where T : UIElement
		{
			if (element != null)
			{
                return element.GetChildrenByType((Func<T, bool>)null);
			}
			return new List<T>();
		}
		public static List<T> GetChildrenByType<T>(this UIElement element, Func<T, bool> condition) where T : UIElement
		{
			List<T> list = new List<T>();
			UIElementExtensions.GetChildrenByType<T>(element, condition, list);
			return list;
		}
		private static void GetChildrenByType<T>(UIElement element, Func<T, bool> condition, List<T> results) where T : UIElement
		{
			if (element == null)
			{
				return;
			}
			condition = UIElementExtensions.GetNonNullCondition<T>(condition);
			T t = element as T;
			if (t != null)
			{
				UIElementExtensions.AddItemToResults<T>(condition, results, t);
			}
			UIElementExtensions.AddChildrenOfType<T>(element, condition, results);
			if (UIElementExtensions.GetContentControlChildren<T>(element, condition, results))
			{
				return;
			}
			UIElementExtensions.GetButtonSpinnerChildren<T>(element, condition, results);
		}
		private static bool GetButtonSpinnerChildren<T>(UIElement element, Func<T, bool> condition, List<T> results) where T : UIElement
		{
			ButtonSpinner buttonSpinner = element as ButtonSpinner;
			if (buttonSpinner == null)
			{
				return false;
			}
			UIElementExtensions.GetChildrenByType<T>(buttonSpinner.Content as UIElement, condition, results);
			return true;
		}
		private static bool GetContentControlChildren<T>(UIElement element, Func<T, bool> condition, List<T> results) where T : UIElement
		{
			ContentControl contentControl = element as ContentControl;
			if (contentControl == null)
			{
				return false;
			}
			UIElementExtensions.GetChildrenByType<T>(contentControl.Content as UIElement, condition, results);
			return true;
		}
		private static Func<T, bool> GetNonNullCondition<T>(Func<T, bool> condition) where T : UIElement
		{
			condition = (condition ?? ((T x) => true));
			return condition;
		}
		private static void AddChildrenOfType<T>(UIElement element, Func<T, bool> condition, List<T> results) where T : UIElement
		{
			for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
			{
				UIElement element2 = VisualTreeHelper.GetChild(element, i) as UIElement;
				UIElementExtensions.GetChildrenByType<T>(element2, condition, results);
			}
		}
		private static void AddItemToResults<T>(Func<T, bool> condition, List<T> results, T t) where T : UIElement
		{
			if (condition.Invoke(t))
			{
				results.Add(t);
			}
		}
		public static bool HasChildrenByType<T>(this UIElement element, Func<T, bool> condition) where T : UIElement
		{
			return element.GetChildrenByType(condition).Count != 0;
		}
		public static IEnumerable<FrameworkElement> FindControls(Type controlType, FrameworkElement parent)
		{
			List<FrameworkElement> list = new List<FrameworkElement>();
			if (parent == null)
			{
				return list;
			}
			UIElementExtensions.FindControlsInContentControl(controlType, parent, list);
			UIElementExtensions.FindControlsInTabControl(controlType, parent, list);
			UIElementExtensions.FindChildControls(controlType, parent, list);
			return list;
		}
		private static void FindChildControls(Type controlType, FrameworkElement parent, List<FrameworkElement> controls)
		{
			try
			{
				for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
				{
					FrameworkElement child = VisualTreeHelper.GetChild(parent, i) as FrameworkElement;
					UIElementExtensions.FindChildControlOrDescendants(controlType, controls, child);
				}
			}
			catch (InvalidOperationException)
			{
			}
		}
		private static void FindChildControlOrDescendants(Type controlType, List<FrameworkElement> controls, FrameworkElement child)
		{
			if (child == null)
			{
				return;
			}
			if (child.GetType() == controlType)
			{
				controls.Add(child);
			}
			controls.AddRange(UIElementExtensions.FindControls(controlType, child));
		}
		private static void FindControlsInTabControl(Type controlType, FrameworkElement parent, List<FrameworkElement> controls)
		{
			TabControl tabControl = parent as TabControl;
			if (tabControl == null)
			{
				return;
			}
			//using (IEnumerator<object> enumerator = tabControl.Items.GetEnumerator())
			//{
				//while (enumerator.MoveNext())
            foreach(TabItem parent2 in tabControl.Items)
				{
					//TabItem parent2 = (TabItem)enumerator.Current;
					controls.AddRange(UIElementExtensions.FindControls(controlType, parent2));
				}
			//}
		}
		private static void FindControlsInContentControl(Type controlType, FrameworkElement parent, List<FrameworkElement> controls)
		{
			ContentControl contentControl = parent as ContentControl;
			if (contentControl == null)
			{
				return;
			}
			FrameworkElement child = contentControl.Content as FrameworkElement;
			UIElementExtensions.FindChildControlOrDescendants(controlType, controls, child);
		}
		public static IEnumerable<DependencyObject> FindControls(string name, DependencyObject parent)
		{
			List<DependencyObject> list = new List<DependencyObject>();
			UIElementExtensions.FindControlsInContentControl(name, parent, list);
			UIElementExtensions.FindControlsInTabControl(name, parent, list);
			UIElementExtensions.FindChildControls(name, parent, list);
			return list;
		}
		private static void FindChildControls(string name, DependencyObject parent, List<DependencyObject> controls)
		{
			for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
			{
				DependencyObject child = VisualTreeHelper.GetChild(parent, i);
				UIElementExtensions.FindChildControlOrDescendants(name, controls, child);
			}
		}
		private static void FindControlsInTabControl(string name, DependencyObject parent, List<DependencyObject> controls)
		{
			TabControl tabControl = parent as TabControl;
			if (tabControl == null)
			{
				return;
			}
			//using (IEnumerator<object> enumerator = tabControl.Items.GetEnumerator())
			//{
				//while (enumerator.MoveNext())
            foreach(TabItem parent2 in tabControl.Items)
				{
					//TabItem parent2 = (TabItem)enumerator.Current;
					controls.AddRange(UIElementExtensions.FindControls(name, parent2));
				}
			//}
		}
		private static void FindControlsInContentControl(string name, DependencyObject parent, List<DependencyObject> controls)
		{
			ContentControl contentControl = parent as ContentControl;
			if (contentControl == null)
			{
				return;
			}
			DependencyObject child = contentControl.Content as DependencyObject;
			UIElementExtensions.FindChildControlOrDescendants(name, controls, child);
		}
		private static void FindChildControlOrDescendants(string name, List<DependencyObject> controls, DependencyObject child)
		{
			if (child == null)
			{
				return;
			}
			FrameworkElement frameworkElement = child as FrameworkElement;
			if (frameworkElement != null && frameworkElement.Name == name)
			{
				controls.Add(child);
			}
			controls.AddRange(UIElementExtensions.FindControls(name, child));
		}
		public static DependencyObject FindControl(string name, DependencyObject parent)
		{
			DependencyObject arg_1D_0;
			if ((arg_1D_0 = UIElementExtensions.FindControlInContentControl(name, parent)) == null)
			{
				arg_1D_0 = (UIElementExtensions.FindControlInTabControl(name, parent) ?? UIElementExtensions.FindControlInVisualChildren(name, parent));
			}
			return arg_1D_0;
		}
		private static DependencyObject FindControlInVisualChildren(string name, DependencyObject parent)
		{
			if (parent == null)
			{
				return null;
			}
			for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
			{
				DependencyObject child = VisualTreeHelper.GetChild(parent, i);
				if (UIElementExtensions.IsFrameworkElementWithName(child, name))
				{
					return child;
				}
				DependencyObject dependencyObject = UIElementExtensions.FindControl(name, child);
				if (dependencyObject != null)
				{
					return dependencyObject;
				}
			}
			return null;
		}
		private static bool IsFrameworkElementWithName(DependencyObject child, string name)
		{
			FrameworkElement frameworkElement = child as FrameworkElement;
			return frameworkElement != null && frameworkElement.Name == name;
		}
		private static DependencyObject FindControlInTabControl(string name, DependencyObject parent)
		{
			TabControl tabControl = parent as TabControl;
			if (tabControl == null)
			{
				return null;
			}
			return Enumerable.FirstOrDefault<DependencyObject>(Enumerable.Select<TabItem, DependencyObject>(Enumerable.Cast<TabItem>(tabControl.Items), (TabItem item) => UIElementExtensions.FindControl(name, item)), (DependencyObject foundObject) => foundObject != null);
		}
		private static DependencyObject FindControlInContentControl(string name, DependencyObject parent)
		{
			ContentControl contentControl = parent as ContentControl;
			if (contentControl == null)
			{
				return null;
			}
			FrameworkElement frameworkElement = contentControl.Content as FrameworkElement;
			if (frameworkElement != null && frameworkElement.Name == name)
			{
				return frameworkElement;
			}
			return UIElementExtensions.FindControl(name, contentControl.Content as DependencyObject);
		}
	}
}
