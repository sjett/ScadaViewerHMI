// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Telerik.Windows.Controls;
using HMI.Runtime.Shared.VirtualKeyboard.KeyboardAware;
using HMI.Runtime.Shared.VirtualKeyboard;
using Xceed.Wpf.Toolkit;


namespace HMI.Runtime.Shared.KeyboardControl
{
	public static class KeyboardService
	{
		private static object currentFocusedElement;
		private static Dictionary<WeakReference, IVirtualKeyboardAware> controlVirtualKeyboardAwareMap = new Dictionary<WeakReference, IVirtualKeyboardAware>();
		public static readonly DependencyProperty VirtualKeyboardType = DependencyProperty.RegisterAttached("VirtualKeyboardType", typeof(VirtualKeyboardType), typeof(KeyboardService), new PropertyMetadata(new PropertyChangedCallback(KeyboardService.OnVirtualKeyboardType)));
		private static Dictionary<WeakReference, VirtualKeyboardType> loadQueue = new Dictionary<WeakReference, VirtualKeyboardType>();
		internal static bool IsKeyboardActive
		{
			get;
			set;
		}
		public static VirtualKeyboardType GetVirtualKeyboardType(DependencyObject d)
		{
			return (VirtualKeyboardType)d.GetValue(KeyboardService.VirtualKeyboardType);
		}
		public static void SetVirtualKeyboardType(DependencyObject d, VirtualKeyboardType value)
		{
			d.SetValue(KeyboardService.VirtualKeyboardType, value);
		}
		private static void OnVirtualKeyboardType(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			UIElement uIElement = d as UIElement;
			if (uIElement != null)
			{
				KeyboardService.RemoveFocusEvents(uIElement);
				VirtualKeyboardType virtualKeyboardType = (VirtualKeyboardType)e.NewValue;
				KeyboardService.SetKeyboard(uIElement, virtualKeyboardType);
			}
		}
		public static void DetachKeyboardFrom(UIElement element)
		{
			KeyboardService.RemoveFocusEvents(element);
			WeakReference weakReference = Enumerable.FirstOrDefault<WeakReference>(KeyboardService.controlVirtualKeyboardAwareMap.Keys, (WeakReference key) => key.Target == element);
			if (weakReference != null)
			{
				KeyboardService.controlVirtualKeyboardAwareMap.Remove(weakReference);
			}
		}
		private static void SetKeyboard(UIElement element, VirtualKeyboardType virtualKeyboardType)
		{
			KeyboardService.RemoveUnused();
			if (!(element is IVirtualKeyboardAware) && KeyboardService.TryKnownTypes(element, virtualKeyboardType))
			{
				return;
			}
			FrameworkElement frameworkElement = element as FrameworkElement;
			if (frameworkElement != null)
			{
				KeyboardService.loadQueue[new WeakReference(frameworkElement, false)] = virtualKeyboardType;
				frameworkElement.Loaded+=(new RoutedEventHandler(KeyboardService.frameworkElement_Loaded));
				frameworkElement.Unloaded+=(new RoutedEventHandler(KeyboardService.frameworkElement_Unloaded));
			}
		}
		private static void frameworkElement_Unloaded(object sender, RoutedEventArgs e)
		{
			KeyboardService.UnLoadElement(sender);
		}
		private static void frameworkElement_Loaded(object sender, RoutedEventArgs e)
		{
            FrameworkElement frameworkElement = sender as FrameworkElement;
            var type = (from kvp in loadQueue
                        where kvp.Key.Target == frameworkElement
                        select new { KVP = kvp }).FirstOrDefault();
            if (type != null)
            {
                Control control = frameworkElement as Control;
                if (control != null)
                {
                    control.ApplyTemplate();
                }
                SetChildrenKeyboard(frameworkElement, type.KVP.Value);
                loadQueue.Remove(type.KVP.Key);
                frameworkElement.Loaded -= (new RoutedEventHandler(frameworkElement_Loaded));
            }

		}
		private static void UnLoadElement(object sender)
		{
			UIElement uIElement = sender as UIElement;
			if (uIElement == null)
			{
				return;
			}
			if (uIElement is FrameworkElement)
			{
				(uIElement as FrameworkElement).Unloaded-=(new RoutedEventHandler(KeyboardService.frameworkElement_Unloaded));
			}
			for (int i = 0; i < VisualTreeHelper.GetChildrenCount(uIElement); i++)
			{
				UIElement sender2 = VisualTreeHelper.GetChild(uIElement, i) as UIElement;
				KeyboardService.UnLoadElement(sender2);
			}
			KeyboardService.DetachKeyboardFrom(uIElement);
			KeyboardService.RemoveUnused();
		}
		private static bool TryKnownTypes(UIElement element, VirtualKeyboardType virtualKeyboardType)
		{
			return element == null || KeyboardService.TryVirtualKeyboardAwareAttach(element, virtualKeyboardType) || KeyboardService.TryTextBoxAttach(element, virtualKeyboardType) || KeyboardService.TryPasswordBoxAttach(element, virtualKeyboardType) || KeyboardService.TryNumericUpDownAttach(element, virtualKeyboardType) || KeyboardService.TryRadNumericUpDownAttach(element, virtualKeyboardType);
		}
		private static void SetChildrenKeyboard(UIElement element, VirtualKeyboardType virtualKeyboardType)
		{
			if (KeyboardService.TryKnownTypes(element, virtualKeyboardType))
			{
				return;
			}
			int childrenCount = VisualTreeHelper.GetChildrenCount(element);
			if (childrenCount != 0)
			{
				for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
				{
					UIElement element2 = VisualTreeHelper.GetChild(element, i) as UIElement;
					KeyboardService.SetChildrenKeyboard(element2, virtualKeyboardType);
				}
				return;
			}
			ContentControl contentControl = element as ContentControl;
			if (contentControl != null)
			{
				KeyboardService.SetChildrenKeyboard(contentControl.Content as UIElement, virtualKeyboardType);
				return;
			}
			ButtonSpinner buttonSpinner = element as ButtonSpinner;
			if (buttonSpinner != null)
			{
				KeyboardService.SetChildrenKeyboard(buttonSpinner.Content as UIElement, virtualKeyboardType);
				return;
			}
			ContentPresenter contentPresenter = element as ContentPresenter;
			if (contentPresenter != null)
			{
				KeyboardService.SetChildrenKeyboard(contentPresenter.Content as UIElement, virtualKeyboardType);
			}
		}
		private static bool TryVirtualKeyboardAwareAttach(UIElement element, VirtualKeyboardType virtualKeyboardType)
		{
            IVirtualKeyboardAware aware = element as IVirtualKeyboardAware;
            if (aware == null)
            {
                return false;
            }
            if ((from kvp in controlVirtualKeyboardAwareMap
                 where kvp.Key.Target == element
                 select new { Element = kvp.Key.Target, VirtualKeyboardAware = kvp.Value }).FirstOrDefault() == null)
            {
                if (virtualKeyboardType != VirtualKeyboard.VirtualKeyboardType.None)
                {
                    controlVirtualKeyboardAwareMap[new WeakReference(element, false)] = aware;
                    if (aware.VirtualKeyboardSettings.Activation == VirtualKeyboardControlActivation.OnFocus)
                    {
                        AddFocusEvents(element);
                    }
                }
                else
                {
                    controlVirtualKeyboardAwareMap[new WeakReference(element, false)] = null;
                }
            }
            return true;

		}
		private static bool TryTextBoxAttach(UIElement element, VirtualKeyboardType virtualKeyboardType)
		{
            TextBox box = element as TextBox;
            if (box == null)
            {
                return false;
            }
            if ((from kvp in controlVirtualKeyboardAwareMap
                 where kvp.Key.Target == element
                 select new { Element = kvp.Key.Target, VirtualKeyboardAware = kvp.Value }).FirstOrDefault() == null)
            {
                IVirtualKeyboardAware aware = null;
                switch (virtualKeyboardType)
                {
                    case VirtualKeyboard.VirtualKeyboardType.Numeric:
                        aware = new NumericKeyboardAwareTextBox(box);
                        break;

                    case VirtualKeyboard.VirtualKeyboardType.Alphanumeric:
                        aware = new AlphanumericKeyboardAwareTextBox(box);
                        break;
                }
                if (aware != null)
                {
                    AddFocusEvents(element);
                }
                controlVirtualKeyboardAwareMap[new WeakReference(element, false)] = aware;
            }
            return true;

		}
		private static bool TryPasswordBoxAttach(UIElement element, VirtualKeyboardType virtualKeyboardType)
		{
            PasswordBox box = element as PasswordBox;
            if (box == null)
            {
                return false;
            }
            if ((from kvp in controlVirtualKeyboardAwareMap
                 where kvp.Key.Target == element
                 select new { Element = kvp.Key.Target, VirtualKeyboardAware = kvp.Value }).FirstOrDefault() == null)
            {
                IVirtualKeyboardAware aware = null;
                switch (virtualKeyboardType)
                {
                    case VirtualKeyboard.VirtualKeyboardType.Numeric:
                        aware = new NumericKeyboardAwarePasswordBox(box);
                        break;

                    case VirtualKeyboard.VirtualKeyboardType.Alphanumeric:
                        aware = new AlphanumericKeyboardAwarePasswordBox(box);
                        break;
                }
                if (aware != null)
                {
                    AddFocusEvents(element);
                }
                controlVirtualKeyboardAwareMap[new WeakReference(element, false)] = aware;
            }
            return true;

		}
		private static bool TryNumericUpDownAttach(UIElement element, VirtualKeyboardType virtualKeyboardType)
		{
            //NumericUpDown down = element as NumericUpDown;
            DoubleUpDown down = element as DoubleUpDown;
            if (down == null)
            {
                return false;
            }
            if ((from kvp in controlVirtualKeyboardAwareMap
                 where kvp.Key.Target == element
                 select new { Element = kvp.Key.Target, VirtualKeyboardAware = kvp.Value }).FirstOrDefault() == null)
            {
                IVirtualKeyboardAware aware = null;
                switch (virtualKeyboardType)
                {
                    case VirtualKeyboard.VirtualKeyboardType.Numeric:
                        aware = new NumericKeyboardAwareNumericUpDown(down);
                        break;

                    case VirtualKeyboard.VirtualKeyboardType.Alphanumeric:
                        aware = new AlphanumericKeyboardAwareNumericUpDown(down);
                        break;
                }
                if (aware != null)
                {
                    AddFocusEvents(element);
                }
                controlVirtualKeyboardAwareMap[new WeakReference(element, false)] = aware;
            }
            return true;

		}
		private static bool TryRadNumericUpDownAttach(UIElement element, VirtualKeyboardType virtualKeyboardType)
		{
            RadNumericUpDown down = element as RadNumericUpDown;
            if (down == null)
            {
                return false;
            }
            if ((from kvp in controlVirtualKeyboardAwareMap
                 where kvp.Key.Target == element
                 select new { Element = kvp.Key.Target, VirtualKeyboardAware = kvp.Value }).FirstOrDefault() == null)
            {
                IVirtualKeyboardAware aware = null;
                switch (virtualKeyboardType)
                {
                    case VirtualKeyboard.VirtualKeyboardType.Numeric:
                        aware = new NumericKeyboardAwareRadNumericUpDown(down);
                        break;

                    case VirtualKeyboard.VirtualKeyboardType.Alphanumeric:
                        aware = new AlphanumericKeyboardAwareRadNumericUpDown(down);
                        break;
                }
                if (aware != null)
                {
                    AddFocusEvents(element);
                }
                controlVirtualKeyboardAwareMap[new WeakReference(element, false)] = aware;
            }
            return true;

		}
		private static void AddFocusEvents(UIElement uiElement)
		{
			uiElement.GotFocus+=(new RoutedEventHandler(KeyboardService.uiElement_GotFocus));
			uiElement.LostFocus+=(new RoutedEventHandler(KeyboardService.uiElement_LostFocus));
		}
		private static void RemoveFocusEvents(UIElement uiElement)
		{
			uiElement.GotFocus-=(new RoutedEventHandler(KeyboardService.uiElement_GotFocus));
			uiElement.LostFocus-=(new RoutedEventHandler(KeyboardService.uiElement_LostFocus));
		}
		private static void uiElement_LostFocus(object sender, RoutedEventArgs e)
		{
			KeyboardService.currentFocusedElement = null;
			KeyboardController.Instance.DettachKeyboard();
		}
		private static void uiElement_GotFocus(object sender, RoutedEventArgs e)
		{
            currentFocusedElement = sender;
            var type = (from kvp in controlVirtualKeyboardAwareMap
                        where kvp.Key.Target == sender
                        select new { Element = kvp.Key.Target, VirtualKeyboardAware = kvp.Value }).FirstOrDefault();
            if (type != null)
            {
                KeyboardController.Instance.OpenKeyboardFor(type.VirtualKeyboardAware);
            }

		}
		private static void RemoveUnused()
		{
			List<WeakReference> list = new List<WeakReference>();
			IEnumerable<WeakReference> enumerable = Enumerable.Where<WeakReference>(Enumerable.Select<KeyValuePair<WeakReference, IVirtualKeyboardAware>, WeakReference>(KeyboardService.controlVirtualKeyboardAwareMap, (KeyValuePair<WeakReference, IVirtualKeyboardAware> kvp) => kvp.Key), (WeakReference k) => !(k.Target is UIElement) || !k.IsAlive);
			using (IEnumerator<WeakReference> enumerator = enumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					WeakReference current = enumerator.Current;
					list.Add(current);
				}
			}
			using (List<WeakReference>.Enumerator unusedReferenceEnumerator = list.GetEnumerator())
			{
				while (unusedReferenceEnumerator.MoveNext())
				{
					WeakReference staleReference = unusedReferenceEnumerator.Current;
					KeyboardService.controlVirtualKeyboardAwareMap.Remove(staleReference);
				}
			}
		}
	}
}
