// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Collections.Generic;
using System.Linq;
using HMI.ProjectDesigner.DesignControls.Isa.ViewModels;
using HMI.ProjectDesigner.DesignControls.Serializers;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using Newtonsoft.Json.Linq;
namespace HMI.ProjectDesigner.DesignControls.Isa.Serializers
{
	public abstract class HMIISABaseSerializer : HMIStaticStatesControlBaseSerializer
	{
		public HMIISABaseSerializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel target, System.Version version)
		{
			HMIISABaseViewModel node = target as HMIISABaseViewModel;
			base.DeserializeProperties(properties, node, version);

			node.LineThickness = ((double?)properties["LineThickness"]) ?? node.LineThickness;
            
		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMIISABaseViewModel wFISABaseViewModel = source as HMIISABaseViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(wFISABaseViewModel);
			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("LineThickness", wFISABaseViewModel.LineThickness)
			});
		}
	}
}
