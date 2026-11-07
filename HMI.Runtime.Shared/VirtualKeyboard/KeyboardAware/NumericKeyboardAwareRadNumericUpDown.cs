// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Runtime.Shared.VirtualKeyboard.KeyboardAware
{
    using System;
    using Telerik.Windows.Controls;
    using HMI.Runtime.Shared;

    internal class NumericKeyboardAwareRadNumericUpDown : KeyboardAwareRadNumericUpDownBase
    {
        public NumericKeyboardAwareRadNumericUpDown(RadNumericUpDown element) : base(element)
        {
        }

        protected override VirtualKeyboardType GetKeyboardType()
        {
            return VirtualKeyboardType.Numeric;
        }
    }
}

