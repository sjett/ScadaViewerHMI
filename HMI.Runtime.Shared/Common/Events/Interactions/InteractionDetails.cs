// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Runtime.Shared.Common.Events.Interactions
{
    using System;
    using System.Runtime.CompilerServices;

    public class InteractionDetails
    {
        public InteractionDetails()
        {
            this.IsActiveWindowChild = true;
        }

        public bool IsActiveWindowChild { get; set; }
    }
}

