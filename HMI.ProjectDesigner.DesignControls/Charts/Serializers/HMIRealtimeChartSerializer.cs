// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Domain.Models;
using HMI.ProjectDesigner.DesignControls.Charts.ViewModels;
using Newtonsoft.Json.Linq;

namespace HMI.ProjectDesigner.DesignControls.Charts.Serializers;

public sealed class HMIRealtimeChartSerializer : CommonControlSerializer
{
    public HMIRealtimeChartSerializer(
        ISerializerFactory serializerFactory,
        IServiceProvider container,
        IProjectManagerService projectManagerService)
        : base(serializerFactory, container, projectManagerService)
    {
    }

    protected override ICommonControlViewModel CreateNode(Type controlType, IExplorerNodeViewModel parentNode) =>
        new HMIRealtimeChartViewModel(Container, parentNode, controlType);

    protected override void DeserializeProperties(
        JObject? properties,
        ICommonControlViewModel node,
        Version version)
    {
        base.DeserializeProperties(properties, node, version);
        if (node is not HMIRealtimeChartViewModel viewModel || properties is null)
        {
            return;
        }

        viewModel.ChartType = properties["ChartType"]?.ToObject<HMIRealtimeChartType>() ?? viewModel.ChartType;
        viewModel.Interval = (double?)properties["Interval"] ?? viewModel.Interval;
        viewModel.Minimum = (double?)properties["Minimum"] ?? viewModel.Minimum;
        viewModel.Maximum = (double?)properties["Maximum"] ?? viewModel.Maximum;
        viewModel.ServerName = (string?)properties["ServerName"] ?? viewModel.ServerName;
        viewModel.SignalsNames = (string?)properties["SignalsNames"] ?? viewModel.SignalsNames;
        viewModel.SignalsDescriptions = (string?)properties["SignalsDescriptions"] ?? viewModel.SignalsDescriptions;
        viewModel.SignalPrefix = (string?)properties["SignalPrefix"] ?? viewModel.SignalPrefix;
        viewModel.SecurityAuthorizationName = (string?)properties["SecurityAuthorizationName"] ?? viewModel.SecurityAuthorizationName;
        viewModel.SecurityDenyAccessBehaviour =
            properties["SecurityDenyAccessBehaviour"]?.ToObject<HMIRealtimeChartDenyAccessBehaviour>() ??
            viewModel.SecurityDenyAccessBehaviour;
    }

    protected override IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
    {
        IEnumerable<JProperty> baseProperties = base.SerializeProperties(source);
        HMIRealtimeChartViewModel viewModel = (HMIRealtimeChartViewModel)source;
        return baseProperties.Concat(
        [
            new JProperty("ChartType", viewModel.ChartType),
            new JProperty("Interval", viewModel.Interval),
            new JProperty("Minimum", viewModel.Minimum),
            new JProperty("Maximum", viewModel.Maximum),
            new JProperty("ServerName", viewModel.ServerName),
            new JProperty("SignalsNames", viewModel.SignalsNames),
            new JProperty("SignalsDescriptions", viewModel.SignalsDescriptions),
            new JProperty("SignalPrefix", viewModel.SignalPrefix),
            new JProperty("SecurityAuthorizationName", viewModel.SecurityAuthorizationName),
            new JProperty("SecurityDenyAccessBehaviour", viewModel.SecurityDenyAccessBehaviour)
        ]);
    }
}
