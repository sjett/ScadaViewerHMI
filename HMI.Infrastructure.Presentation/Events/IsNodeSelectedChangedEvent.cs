// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.ViewModels;

namespace HMI.Infrastructure.Presentation.Events
{
    public class IsNodeSelectedChangedEvent : PubSubEvent<IExplorerNodeViewModel>
    {
    }
}
