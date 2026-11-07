// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.ViewModels;
using Newtonsoft.Json.Linq;

namespace HMI.Infrastructure.Presentation.Serialization
{
	public static class NavigationParameterSerializationExtensions
	{
		public static void DeserializeNavigationParameter(this BaseSerializer serializer, JObject propertyCollectionsElement, IExplorerNodeViewModel node, System.Collections.Generic.ICollection<NavigationParameter> navigationParameters, System.Version version)
		{
			var xElement = propertyCollectionsElement["Parameters"]?.ToArray();
            if (xElement == null)
                return;

            navigationParameters.Clear();

			ISerializer serializer2 = serializer.SerializerFactory.GetSerializer<NavigationParameter>();

			foreach (var current in xElement)
			{
                NavigationParameter? navigationParameter = serializer2.Deserialize(node, (JObject)current, version) as NavigationParameter;
				if (navigationParameter != null)
				{
					navigationParameters.Add(navigationParameter);
				}
			}
		}
		public static JProperty GetSerializedNavigationParameters(this BaseSerializer serializer, System.Collections.Generic.ICollection<NavigationParameter> navigationParameters)
		{
            var xElement = new List<JObject>();
            foreach (NavigationParameter current in navigationParameters)
			{
				ISerializer serializer2 = serializer.SerializerFactory.GetSerializer(current.GetType());
				xElement.Add(serializer2.Serialize(current));
			}
			return new JProperty("Parameters", xElement);
		}
	}
}
