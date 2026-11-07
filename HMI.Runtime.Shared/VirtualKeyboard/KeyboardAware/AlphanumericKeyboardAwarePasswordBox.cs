// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Runtime.Shared.VirtualKeyboard.KeyboardAware
{
    using System;
    using System.Windows.Controls;
    using HMI.Runtime.Shared;

    internal class AlphanumericKeyboardAwarePasswordBox : KeyboardAwarePasswordBoxBase
    {
        public AlphanumericKeyboardAwarePasswordBox(PasswordBox element) : base(element)
        {
        }

        protected override VirtualKeyboardType GetKeyboardType()
        {
            return VirtualKeyboardType.Alphanumeric;
        }
    }
}

