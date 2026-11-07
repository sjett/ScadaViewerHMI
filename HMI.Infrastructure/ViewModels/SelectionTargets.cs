// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Infrastructure.ViewModels
{
    [Flags]
    public enum SelectionTargets
    {
        None = 0,
        Project = 1,
        Pages = 2,
        TemplatePages = 4,
        NavigationTargets = 8,
        Controls = 16,
        All = 31
    }
}
