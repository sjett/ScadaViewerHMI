// SPDX-License-Identifier: GPL-3.0-only

using HMI.Infrastructure.Models;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.ViewModels;
using Newtonsoft.Json.Linq;

namespace HMI.Infrastructure.Serialization
{
	public class NavigationItemSerializer : BaseSerializer
	{
		private string targetText;
		private string sourceText;
		private NavigationItem navigationItem;
		public NavigationItemSerializer(IServiceProvider container, ISerializerFactory serializerFactory) : base(container, serializerFactory)
		{
		}
		public override JObject Serialize(object source)
		{
			NavigationItem navigationItem = source as NavigationItem;
			return new JObject(
					new JProperty("Properties", new JObject(
						new JProperty("Attributes", new JObject(
							new JProperty("Target", (navigationItem.Target == null) ? string.Empty : navigationItem.Target.ID),
							new JProperty("Source", (navigationItem.Source == null) ? string.Empty : navigationItem.Source.ID)
						)),
						new JProperty("Collections",new JObject(
							this.GetSerializedNavigationParameters(navigationItem.ParameterValues)
						))
					)));
		}
		protected override object GetDeserializedObjectFromUpgraded(IExplorerNodeViewModel ancestorNode, JObject root, System.Version version)
		{
			var xElement = root["Properties"]!["Attributes"];
			this.navigationItem = new NavigationItem(base.Container, ancestorNode);

			this.targetText = ((string?)xElement!["Target"]) ?? this.targetText;

            this.sourceText = ((string?)xElement!["Source"]) ?? this.sourceText;

			var propertyCollectionsElement = root["Properties"]!["Collections"];
			this.DeserializeNavigationParameter((JObject)propertyCollectionsElement, ancestorNode, this.navigationItem.ParameterValues, version);
			return this.navigationItem;
		}
		public override void OnDeserializationFinished(IExplorerNodeViewModel viewModel)
		{
			if (this.navigationItem != null)
			{
				this.navigationItem.Target = this.GetNavigationTarget();
				this.navigationItem.Source = this.GetNavigationSource();
				base.OnDeserializationFinished(viewModel);
			}
		}
		private IExplorerNodeViewModel? GetNavigationSource()
		{
			if (this.navigationItem.Target != null && this.navigationItem.Target.Children != null)
			{
				return (
					from c in this.navigationItem.Target.Children
					where c.ID == this.sourceText
					select c).FirstOrDefault<IExplorerNodeViewModel>();
			}
			return null;
		}
		private INavigationTargetViewModel GetNavigationTarget()
		{
			return ((INavigationViewModel)this.navigationItem.Parent).NavigationTargets.ToList<INavigationTargetViewModel>().FirstOrDefault((INavigationTargetViewModel n) => n.ID == this.targetText);
		}
	}
}
