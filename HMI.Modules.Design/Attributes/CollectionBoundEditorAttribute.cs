// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Metadata;
using HMI.Domain.Models;
using HMI.Modules.Design.Views;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace HMI.Modules.Design.Attributes
{
	public abstract class CollectionBoundEditorAttribute : PropertyEditorAttribute
	{
		
		protected string DataSourcePropertyName
		{
			get;
			private set;
		}
		protected CollectionBoundEditorAttribute(string dataSourcePropertyName)
		{
            DataSourcePropertyName = dataSourcePropertyName;
		}
		
        protected virtual void SetDataSourceBinding(IProperty property, FrameworkElement view)
		{
			Binding binding = new Binding(DataSourcePropertyName)
			{
				Mode = BindingMode.OneWay,
				Source = property.SourceInstance
			};
			view.SetBinding(ItemsControl.ItemsSourceProperty, binding);
		}
		protected abstract IEditorView CreateEditor();
		protected override IEditorView BuildEditor(IProperty property, IBindingSource editedObject)
		{
			IEditorView editorView = CreateEditor();
            SetDataSourceBinding(property, (FrameworkElement)editorView);
			return editorView;
		}
		protected bool IsDataSourceCompatibileWith(IProperty property, IProperty otherProperty)
		{
			CollectionBoundEditorAttribute collectionBoundEditorAttribute = (CollectionBoundEditorAttribute)property.PropertyEditorBuilder;
			object propertyValue = Reflection.GetPropertyValue(property.SourceInstance, DataSourcePropertyName);
			object propertyValue2 = Reflection.GetPropertyValue(otherProperty.SourceInstance, collectionBoundEditorAttribute.DataSourcePropertyName);
			return AreDataSourcesEquivalent(propertyValue2, propertyValue);
		}
        private bool AreDataSourcesEquivalent(object firstValue, object secondValue)
        {
            return secondValue == null == (firstValue == null) &&
                (secondValue == null || ReferenceEquals(secondValue, firstValue) ||
                 HaveEquivalentItems(firstValue as IEnumerable, secondValue as IEnumerable));
        }
        private bool HaveEquivalentItems(IEnumerable firstItems, IEnumerable secondItems)
        {
            if (firstItems != null && secondItems != null)
            {
                ICollection firstCollection = firstItems.Cast<object>().ToArray();
                ICollection secondCollection = secondItems.Cast<object>().ToArray();
                return firstCollection.Count == secondCollection.Count &&
                    (secondCollection.Count == 0 || HaveEquivalentItemCounts(firstCollection, secondCollection));
            }
            return false;
        }
        private bool HaveEquivalentItemCounts(ICollection firstCollection, ICollection secondCollection)
        {
            Dictionary<object, int> firstItemCounts = CountItems(firstCollection, out int firstNullCount);
            Dictionary<object, int> secondItemCounts = CountItems(secondCollection, out int secondNullCount);
            if (secondNullCount != firstNullCount)
            {
                return false;
            }
            foreach (object item in firstItemCounts.Keys)
            {
                firstItemCounts.TryGetValue(item, out int firstCount);
                secondItemCounts.TryGetValue(item, out int secondCount);
                if (firstCount != secondCount)
                {
                    return false;
                }
            }
            return true;
        }
        private Dictionary<object, int> CountItems(IEnumerable items, out int nullItemCount)
        {
            Dictionary<object, int> itemCounts = new Dictionary<object, int>();
            nullItemCount = 0;
            foreach (object item in items)
            {
                if (item == null)
                {
                    nullItemCount++;
                }
                else
                {
                    itemCounts.TryGetValue(item, out int count);
                    count++;
                    itemCounts[item] = count;
                }
            }
            return itemCounts;
        }
    }
}
