// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Runtime.Shared.VirtualKeyboard.KeyboardAware
{
    using System;
    using System.Windows.Controls;
    using HMI.Runtime.Shared;

    internal class NumericKeyboardAwareTextBox : KeyboardAwareTextBoxBase
    {
        public NumericKeyboardAwareTextBox(TextBox element) : base(element)
        {
        }

        protected override VirtualKeyboardType GetKeyboardType()
        {
            return VirtualKeyboardType.Numeric;
        }
    }
}

