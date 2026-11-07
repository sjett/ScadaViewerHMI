// SPDX-License-Identifier: GPL-3.0-only
using System.Collections;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace HMI.Infrastructure.Behaviors
{
    public static class ItemsSourceChangedBehavior
    {
        
        private sealed class ItemsSourceCollectionChangedListener
        {
            private readonly ItemsControl itemsControl;

            public ItemsSourceCollectionChangedListener(ItemsControl itemsControl)
            {
                this.itemsControl = itemsControl;
            }

            public void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                itemsControl.RaiseEvent(new RoutedEventArgs(ItemsSourceChangedEvent));
            }
        }
        public static readonly DependencyProperty ItemsSourceChangedProperty = DependencyProperty.RegisterAttached("ItemsSourceChanged", typeof(bool), typeof(ItemsSourceChangedBehavior), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnItemsSourceChangedPropertyChanged)));
        public static readonly DependencyProperty ItemsSourceListenerProperty = DependencyProperty.RegisterAttached("ItemsSourceListener", typeof(object), typeof(ItemsSourceChangedBehavior), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnItemsSourceListenerChanged)));
        public static readonly RoutedEvent ItemsSourceChangedEvent = EventManager.RegisterRoutedEvent("ItemsSourceChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ItemsControl));
        public static readonly DependencyProperty PreventSelectedItemResetOnItemsSourceChangesProperty = DependencyProperty.RegisterAttached("PreventSelectedItemResetOnItemsSourceChanges", typeof(bool), typeof(ItemsSourceChangedBehavior), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnPreventSelectionResetChanged)));
        public static readonly DependencyProperty PreviousSelectedValueProperty = DependencyProperty.RegisterAttached("PreviousSelectedValue", typeof(object), typeof(ItemsSourceChangedBehavior));
        public static readonly DependencyProperty PreviousItemsSourceProperty = DependencyProperty.RegisterAttached("PreviousItemsSource", typeof(object), typeof(ItemsSourceChangedBehavior));
        public static readonly DependencyProperty IgnoreSelectionChangedProperty = DependencyProperty.RegisterAttached("IgnoreSelectionChanged", typeof(bool), typeof(ItemsSourceChangedBehavior), new UIPropertyMetadata(false));
        public static bool GetItemsSourceChanged(DependencyObject obj)
        {
            return (bool)obj.GetValue(ItemsSourceChangedProperty);
        }
        public static void SetItemsSourceChanged(DependencyObject obj, bool value)
        {
            obj.SetValue(ItemsSourceChangedProperty, value);
        }
        public static bool GetPreventSelectedItemResetOnItemsSourceChanges(DependencyObject obj)
        {
            return (bool)obj.GetValue(PreventSelectedItemResetOnItemsSourceChangesProperty);
        }
        public static void SetPreventSelectedItemResetOnItemsSourceChanges(DependencyObject obj, bool value)
        {
            obj.SetValue(PreventSelectedItemResetOnItemsSourceChangesProperty, value);
        }
        public static bool GetIgnoreSelectionChanged(DependencyObject obj)
        {
            return (bool)obj.GetValue(IgnoreSelectionChangedProperty);
        }
        public static void SetIgnoreSelectionChanged(DependencyObject obj, bool value)
        {
            obj.SetValue(IgnoreSelectionChangedProperty, value);
        }

        private static void OnItemsSourceChangedPropertyChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs eventArgs)
        {
            ItemsControl itemsControl = dependencyObject as ItemsControl;
            if (itemsControl == null)
            {
                return;
            }
            if (!(bool)eventArgs.OldValue && (bool)eventArgs.NewValue)
            {
                Binding binding = new Binding
                {
                    Source = itemsControl,
                    Path = new PropertyPath(ItemsControl.ItemsSourceProperty)
                };
                itemsControl.SetBinding(ItemsSourceListenerProperty, binding);
                return;
            }
            if ((bool)eventArgs.OldValue && !(bool)eventArgs.NewValue)
            {
                BindingOperations.ClearBinding(itemsControl, ItemsSourceListenerProperty);
            }
        }

        private static void OnItemsSourceListenerChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs eventArgs)
        {
            ItemsControl itemsControl = dependencyObject as ItemsControl;
            if (itemsControl == null)
            {
                return;
            }
            if (eventArgs.NewValue is INotifyCollectionChanged collection)
            {
                var listener = new ItemsSourceCollectionChangedListener(itemsControl);
                collection.CollectionChanged += listener.OnCollectionChanged;
            }
            if (GetItemsSourceChanged(itemsControl))
            {
                itemsControl.RaiseEvent(new RoutedEventArgs(ItemsSourceChangedEvent));
            }
        }

        private static void OnPreventSelectionResetChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs eventArgs)
        {
            Selector selector = dependencyObject as Selector;
            if (selector == null)
            {
                return;
            }
            bool isEnabled = (bool)eventArgs.NewValue;
            SetItemsSourceChanged(dependencyObject, isEnabled);
            if (isEnabled)
            {
                selector.SelectionChanged += OnSelectionChanged;
                selector.AddHandler(ItemsSourceChangedEvent, new RoutedEventHandler(OnItemsSourceChanged));
                return;
            }
            if (eventArgs.OldValue != DependencyProperty.UnsetValue)
            {
                selector.SelectionChanged -= OnSelectionChanged;
                selector.RemoveHandler(ItemsSourceChangedEvent, new RoutedEventHandler(OnItemsSourceChanged));
            }
        }
        private static void OnSelectionChanged(object sender, SelectionChangedEventArgs eventArgs)
        {
            DependencyObject dependencyObject = sender as DependencyObject;
            if (GetIgnoreSelectionChanged(dependencyObject))
            {
                return;
            }
            if (GetPreviousSelectedValue(dependencyObject) != null)
            {
                SaveFirstChangedItem(eventArgs.RemovedItems, dependencyObject);
                return;
            }
            SaveFirstChangedItem(eventArgs.AddedItems, dependencyObject);
        }
        private static object GetPreviousSelectedValue(DependencyObject dependencyObject)
        {
            return dependencyObject.GetValue(PreviousSelectedValueProperty);
        }
        private static void SaveFirstChangedItem(IList changedItems, DependencyObject dependencyObject)
        {
            if (changedItems != null && changedItems.Count != 0 && changedItems[0] != null)
            {
                SetPreviousSelectedValue(dependencyObject, changedItems[0]);
                return;
            }
        }
        private static void SetPreviousSelectedValue(DependencyObject dependencyObject, object value)
        {
            dependencyObject.SetValue(PreviousSelectedValueProperty, value);
        }
        private static void OnItemsSourceChanged(object sender, RoutedEventArgs eventArgs)
        {
            Selector selector = sender as Selector;
            if (selector == null)
            {
                return;
            }
            IEnumerable itemsSource = selector.ItemsSource;
            if (itemsSource != null && itemsSource == GetPreviousItemsSource(selector))
            {
                int selectedIndex = -1;
                object selectedValue = GetPreviousSelectedValue(selector);
                if (selectedValue == null)
                {
                    SetSelectedIndex(selector, -1);
                    return;
                }
                IEnumerator enumerator = itemsSource.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        object currentItem = enumerator.Current;
                        selectedIndex++;
                        if (object.Equals(currentItem, selectedValue))
                        {
                            SetSelectedIndex(selector, selectedIndex);
                        }
                    }
                    return;
                }
                finally
                {
                    IDisposable disposable = enumerator as IDisposable;
                    if (disposable != null)
                    {
                        disposable.Dispose();
                    }
                }
            }
            SetPreviousSelectedValue(selector, null);
            SetPreviousItemsSource(selector, itemsSource);
            SetSelectedIndex(selector, -1);
        }
        private static object GetPreviousItemsSource(DependencyObject dependencyObject)
        {
            return dependencyObject.GetValue(PreviousItemsSourceProperty);
        }
        private static void SetSelectedIndex(Selector selector, int selectedIndex)
        {
            SetIgnoreSelectionChanged(selector, true);
            selector.SelectedIndex = selectedIndex;
            SetIgnoreSelectionChanged(selector, false);
        }
        private static void SetPreviousItemsSource(DependencyObject dependencyObject, object itemsSource)
        {
            dependencyObject.SetValue(PreviousItemsSourceProperty, itemsSource);
        }
    }

}
