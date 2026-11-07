// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.ProjectDesigner.DesignControls.Navigation.ViewModels;
using Newtonsoft.Json.Linq;
using System.Windows.Controls;
using System.Windows.Media;

namespace HMI.ProjectDesigner.DesignControls.Navigation.Serializers
{
	public class HMINavigationTargetSerializer : CommonControlSerializer
	{
		private string defaultPageText;
		private HMINavigationTargetViewModel node;
		public HMINavigationTargetSerializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService) : base(serializerFactory, container, projectManagerService)
		{
		}
		protected override ICommonControlViewModel CreateNode(System.Type controlType, IExplorerNodeViewModel parentNode)
		{
			return new HMINavigationTargetViewModel(base.Container, parentNode, controlType);
		}
		protected override void DeserializeProperties(JObject? properties, ICommonControlViewModel target, System.Version version)
		{
			this.node = (target as HMINavigationTargetViewModel);
			base.DeserializeProperties(properties, this.node, version);

			this.defaultPageText = ((string?)properties!["DefaultPage"]) ?? this.defaultPageText;

            this.node.IsSelected = ((bool?)properties!["IsSelected"]) ?? this.node.IsSelected;

			this.node.StretchContent = properties!["StretchContent"]?.ToObject<Stretch?>() ?? this.node.StretchContent;

            this.node.HorizontalScrollBarVisibility = properties!["HorizontalScrollBarVisibility"]?.ToObject<ScrollBarVisibility?>() ?? this.node.HorizontalScrollBarVisibility;

            this.node.VerticalScrollBarVisibility = properties!["VerticalScrollBarVisibility"]?.ToObject<ScrollBarVisibility?>() ?? this.node.VerticalScrollBarVisibility;

		}
		public override void OnDeserializationFinished(IExplorerNodeViewModel viewModel)
		{
			if (this.node != null)
			{
				this.node.DefaultPage = this.GetDefaultPage();
				base.OnDeserializationFinished(viewModel);
			}
		}
		private IExplorerNodeViewModel? GetDefaultPage()
		{
			if (this.node.Children != null)
			{
				return (
					from c in this.node.Children
					where c.ID == this.defaultPageText
					select c).FirstOrDefault<IExplorerNodeViewModel>();
			}
			return null;
		}
		protected override System.Collections.Generic.IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
		{
			HMINavigationTargetViewModel wFNavigationTargetViewModel = source as HMINavigationTargetViewModel;
			System.Collections.Generic.IEnumerable<JProperty> first = base.SerializeProperties(wFNavigationTargetViewModel);

			return first.Concat(new System.Collections.Generic.List<JProperty>
			{
				new JProperty("DefaultPage", (wFNavigationTargetViewModel.DefaultPage == null) ? string.Empty : wFNavigationTargetViewModel.DefaultPage.ID),
				new JProperty("IsSelected", wFNavigationTargetViewModel.IsSelected),
				new JProperty("StretchContent", wFNavigationTargetViewModel.StretchContent),
				new JProperty("HorizontalScrollBarVisibility", wFNavigationTargetViewModel.HorizontalScrollBarVisibility),
				new JProperty("VerticalScrollBarVisibility", wFNavigationTargetViewModel.VerticalScrollBarVisibility)
			});
		}
		public override JObject? Serialize(object source)
		{
			INavigationTargetViewModel? navigationTargetViewModel = source as INavigationTargetViewModel;
			if (navigationTargetViewModel.IsInvalidTarget)
			{
				return null;
			}

			return base.Serialize(source);
		}
	}
}
