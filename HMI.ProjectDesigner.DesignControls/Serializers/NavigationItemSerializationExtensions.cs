// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Models;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.ViewModels;
using Newtonsoft.Json.Linq;

namespace HMI.Infrastructure.Serialization
{
	public static class NavigationItemSerializationExtensions
	{
		public static JProperty GetSerializedNavigationItems(this BaseSerializer serializer, System.Collections.Generic.ICollection<NavigationItem> navigationItems)
		{
			var xElement = new List<JObject>();
			foreach (NavigationItem current in navigationItems)
			{
				ISerializer serializer2 = serializer.SerializerFactory.GetSerializer(current.GetType());
				xElement.Add(serializer2.Serialize(current));
			}
			return new JProperty("Navigations", xElement);
		}
		public static void DeserializeNavigationItem(this BaseSerializer serializer, JObject propertyCollectionsElement, IExplorerNodeViewModel node, System.Collections.Generic.ICollection<NavigationItem> navigationItems, System.Version version)
		{
			var xElement = propertyCollectionsElement["Navigations"]?.ToArray();
            if (xElement == null)
                return;

            navigationItems.Clear();

			foreach (var current in xElement)
			{
				ISerializer serializer2 = serializer.SerializerFactory.GetSerializer<NavigationItem>();
				NavigationItem? navigationItem = serializer2.Deserialize(node, (JObject)current, version) as NavigationItem;
				if (navigationItem != null)
				{
					navigationItems.Add(navigationItem);
				}
			}
		}
	}
}
