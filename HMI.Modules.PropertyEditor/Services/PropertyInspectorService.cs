// SPDX-License-Identifier: GPL-3.0-only
using HMI.Domain.Models;
using HMI.Domain.Services;
using HMI.Infrastructure.Extensions;
using HMI.Infrastructure.Services;
using HMI.Modules.Design.Attributes;
using HMI.Modules.PropertyEditor.Models;
using System.ComponentModel;
using HMI.Common.Instrumentation.ErrorHandling;
using System.Runtime.Caching;

namespace HMI.Modules.PropertyEditor.Services
{
	public class PropertyInspectorService : IPropertyInspectorService
	{
		private readonly HashSet<string> globalPropertyVisibilityBlacklist = new HashSet<string>
		{
			"Id",
			"IsSealed",
			"DependencyObjectType",
			"Dispatcher",
			"Services",
			"ContentReplacementService"
		};
		private readonly IActionMonitor actionMonitor;
		private readonly IServiceProvider serviceLocator;
		public PropertyInspectorService(IServiceProvider serviceLocator, IActionMonitor actionMonitor)
		{
			Contract.Requires<ArgumentNullException>(serviceLocator != null);
			Contract.Requires<ArgumentNullException>(actionMonitor != null, "actionMonitor");

			this.serviceLocator = serviceLocator;
			this.actionMonitor = actionMonitor;
		}
		public ICollection<IProperty> GetProperties(params object[] items)
		{
			items = items ?? Array.Empty<object>();
			IEnumerable<IProperty> allProperties = GetAllProperties(items);
			bool areMultiplePropertiesSelected = items.Length > 1;
			return (
				from x in GetFilteredProperties(items, areMultiplePropertiesSelected, allProperties)
				orderby x.Category, x.SortOrder, x.DisplayName
				select x).ToList();
		}
		public IEnumerable<PropertyDescriptor> GetBrowsablePropertyDescriptors(object item)
		{
			IEnumerable<PropertyDescriptor> result;
			lock (MemoryCache.Default)
			{
				result = UnsynchronizedGetAllBrowsableProperties(item);
			}
			return result;
		}
		private IEnumerable<IProperty> GetFilteredProperties(object[] items, bool areMultiplePropertiesSelected, IEnumerable<IProperty> allProperties)
		{
            IFilteringStrategy filteringStrategy = GetFilteringStrategy(areMultiplePropertiesSelected);
			return filteringStrategy.Filter(items, allProperties);
		}
		private IFilteringStrategy GetFilteringStrategy(bool areMultiplePropertiesSelected)
		{
			if (areMultiplePropertiesSelected)
			{
				return new MultipleInstancesFilteringStrategy(actionMonitor);
			}
			return new SingleInstanceFilteringStrategy();
		}
		private IEnumerable<IProperty> GetAllProperties(IEnumerable<object> items)
		{
			return (
				from i in items
				where i != null
				select i).SelectMany(new Func<object, IEnumerable<IProperty>>(GetAllBrowsableItemProperties));
		}
		private IEnumerable<IProperty> GetAllBrowsableItemProperties(object item)
		{
			return 
				from property in GetPropertyDescriptors(item)
				select CreateProperty(item, property);
		}
		private IEnumerable<PropertyDescriptor> GetPropertyDescriptors(object item)
		{
			item.GetType();
			return GetBrowsablePropertyDescriptors(item);
		}
		private IEnumerable<PropertyDescriptor> UnsynchronizedGetAllBrowsableProperties(object item)
		{
			if (!ArePropertiesCached(item.GetType().FullName))
			{
                AddBrowsablePropertiesToCache(item);
			}
			return GetCachedProperties(item.GetType().FullName) ?? Enumerable.Empty<PropertyDescriptor>();
		}
		private IProperty CreateProperty(object item, PropertyDescriptor descriptor)
		{
			IExplicitPropertyMetadataProvider metadataProvider = GetMetadataProvider(item, descriptor);
			return new Property(item, descriptor.Name, descriptor.PropertyType, metadataProvider, serviceLocator);
		}
		private IExplicitPropertyMetadataProvider GetMetadataProvider(object item, PropertyDescriptor descriptor)
		{
			IExplicitPropertyMetadataProvider? explicitPropertyMetadataProvider = item as IExplicitPropertyMetadataProvider;
			CompositeMetadataProvider compositeMetadataProvider = new CompositeMetadataProvider();
			if (explicitPropertyMetadataProvider != null)
			{
				compositeMetadataProvider.AddProvider(explicitPropertyMetadataProvider);
			}
			compositeMetadataProvider.AddProvider(new ReflectionBasedPropertyMetadataProvider(descriptor));
			compositeMetadataProvider.AddProvider(new DefaultValuesPropertyDescriptorMetadataProvider(descriptor));
			return compositeMetadataProvider;
		}
		private bool ArePropertiesCached(string itemTypeName)
		{
			return false;
		}
		private IEnumerable<PropertyDescriptor> GetCachedProperties(string itemTypeName)
		{
			return MemoryCache.Default[itemTypeName] as IEnumerable<PropertyDescriptor>;
		}
		private void AddBrowsablePropertiesToCache(object item)
		{
			CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
			cacheItemPolicy.SlidingExpiration = TimeSpan.FromMinutes(5.0);
			List<PropertyDescriptor> reflectedBrowsableProperties = GetReflectedBrowsableProperties(item);
			MemoryCache.Default.Set(item.GetType().FullName, reflectedBrowsableProperties, cacheItemPolicy, null);
		}
		private List<PropertyDescriptor> GetReflectedBrowsableProperties(object item)
		{
			return TypeDescriptor.GetProperties(item).Cast<PropertyDescriptor>().Where(new Func<PropertyDescriptor, bool>(IsBrowsable)).ToList();
		}
		private bool IsBrowsable(PropertyDescriptor propertyDescriptor)
		{
			if (globalPropertyVisibilityBlacklist.Contains(propertyDescriptor.Name))
			{
				return false;
			}
			bool flag = !propertyDescriptor.IsCollectionProperty() || propertyDescriptor.Attributes.OfType<CollectionEditorAttribute>().Any() || propertyDescriptor.Attributes.OfType<PropertyEditorAttribute>().Any();
			return propertyDescriptor.IsBrowsable && flag && propertyDescriptor.Name.Contains('.') == false;
		}
	}
}
