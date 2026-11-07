// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.ProjectDesigner.DesignControls.Charts.ViewModels;

namespace HMI.ProjectDesigner.DesignControls.Charts.Serializers;

public class HMITrending3Serializer : HMITrendingBaseSerializer
{
    public HMITrending3Serializer(
        ISerializerFactory serializerFactory,
        IServiceProvider container,
        IProjectManagerService projectManagerService)
        : base(serializerFactory, container, projectManagerService)
    {
    }

    protected override ICommonControlViewModel CreateNode(
        Type controlType,
        IExplorerNodeViewModel parentNode) =>
        new HMITrending3ViewModel(Container, parentNode, controlType);
}
