// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Runtime.Shared.VirtualKeyboard.KeyboardAware
{
    using System;
    using System.Windows.Controls;
    using Xceed.Wpf.Toolkit;

    internal abstract class KeyboardAwareNumericUpDownBase : NumericKeyboardAwareBase
    {
        protected KeyboardAwareNumericUpDownBase(DoubleUpDown element) : base(element)
        {
        }

        protected override void FocusElement()
        {
            ((DoubleUpDown)base.element).Focus();
        }

        protected override int GetSelectedLength()
        {
            return 0;
        }

        protected override void SetCarretPosition(int carretPosition)
        {
        }

        protected override double Value
        {
            get
            {
                return ((DoubleUpDown)base.element).Value.Value;
            }
            set
            {
                ((DoubleUpDown)base.element).Value = (value);
            }
        }
    }
}

