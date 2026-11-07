// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.ViewModels;

namespace HMI.ProjectDesigner.DesignControls.Charts.ViewModels;

public class HMITrending3ViewModel : HMITrendingBaseViewModel
{
    public HMITrending3ViewModel(
        IServiceProvider container,
        IExplorerNodeViewModel parentValue,
        Type controlType)
        : base(container, parentValue, controlType)
    {
    }

    public override string GetXamlControlName() => "HMITrending3";
}
