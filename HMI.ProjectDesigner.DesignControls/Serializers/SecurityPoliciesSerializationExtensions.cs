// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.ProjectDesigner.DesignControls.Models;
using Newtonsoft.Json.Linq;
namespace HMI.ProjectDesigner.DesignControls.Serializers
{
	public static class SecurityPoliciesSerializationExtensions
	{
		public static void DeserializeSecurityPolicies(this BaseSerializer serializer, JObject propertyCollectionsElement, IExplorerNodeViewModel node, System.Collections.Generic.ICollection<SecurityPolicy> dataTableButtonSettings, System.Version version)
		{
			var xElement = propertyCollectionsElement["SecurityPolicies"];
			if (xElement == null)
				return;

			System.Collections.Generic.Dictionary<AlarmViewerSecuredAction, SecurityPolicy> dictionary = new System.Collections.Generic.Dictionary<AlarmViewerSecuredAction, SecurityPolicy>();
			ISerializer serializer2 = serializer.SerializerFactory.GetSerializer<SecurityPolicy>();
			foreach (var current in xElement)
			{
				if (((string?)current["ItemElementName"]) != "SecurityPolicy")
					continue;

				SecurityPolicy? securityPolicy = serializer2.Deserialize(node, (JObject)current, version) as SecurityPolicy;
				if (securityPolicy != null)
				{
					dictionary[securityPolicy.Action] = securityPolicy;
				}
			}
			foreach (SecurityPolicy existingPolicy in dataTableButtonSettings)
			{
				SecurityPolicy deserializedPolicy;
				if (dictionary.TryGetValue(existingPolicy.Action, out deserializedPolicy))
				{
					deserializedPolicy.DeepCopyFields(existingPolicy);
				}
			}
		}
		public static JProperty GetSerializedSecurityPolicies(this BaseSerializer serializer, System.Collections.Generic.ICollection<SecurityPolicy> settings)
		{
			var xElement = new List<JObject>();
			foreach (SecurityPolicy current in settings)
			{
				ISerializer serializer2 = serializer.SerializerFactory.GetSerializer(current.GetType());
				xElement.Add(serializer2.Serialize(current));
			}
			return new JProperty("SecurityPolicies", xElement);
		}
	}
}
