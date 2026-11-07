// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Runtime.Shared.Dialogs
{
    using System;
    using System.Runtime.CompilerServices;

    public class CustomUserAction : UserAction
    {
        public CustomUserAction(object customActionContent, UserInteractionHandler actionCallback, object state) : base(null, actionCallback, state)
        {
            CustomActionContent = customActionContent;
        }

        public override object GetActionContent()
        {
            return CustomActionContent;
        }

        public object CustomActionContent { get; set; }
    }
}

