// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.Runtime.Shared;

public sealed class SecurityPolicy
{
    public string SecurityAuthorizationName { get; set; } = string.Empty;
    public Visibility Visibility { get; set; } = Visibility.Visible;
    public DenyAccessBehaviour SecurityDenyAccessBehaviour { get; set; }
}
