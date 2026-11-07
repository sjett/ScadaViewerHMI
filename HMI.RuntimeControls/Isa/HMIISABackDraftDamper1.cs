// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.RuntimeControls.Isa;

public sealed class HMIISABackDraftDamper1 : HMIISABase
{
    static HMIISABackDraftDamper1()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(HMIISABackDraftDamper1),
            new FrameworkPropertyMetadata(typeof(HMIISABackDraftDamper1)));
    }
}
