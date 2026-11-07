// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.ProjectDesigner.DesignControls.StaticSymbols.ViewModels;
using Newtonsoft.Json.Linq;

namespace HMI.ProjectDesigner.DesignControls.StaticSymbols.Serializers
{
	public class StaticSymbolSerializer : CommonControlSerializer
	{
		public StaticSymbolSerializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new StaticSymbolViewModel(base.Container, parentNode, controlType);
		}
		protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel target, System.Version version)
		{
			IStaticSymbolViewModel? node = target as IStaticSymbolViewModel;
			base.DeserializeProperties(properties, target, version);

			node.ContentString = ((string?)properties["ContentString"]) ?? node.ContentString;

		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			IStaticSymbolViewModel staticSymbolViewModel = source as IStaticSymbolViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(source);
			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("ContentString", staticSymbolViewModel.ContentString)
			});
		}
	}
}
