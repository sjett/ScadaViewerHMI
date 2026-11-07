// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Runtime.Shared.VirtualKeyboard.KeyboardAware
{
    using System;
    using Telerik.Windows.Controls;

    internal abstract class KeyboardAwareRadNumericUpDownBase : NumericKeyboardAwareBase
    {
        protected KeyboardAwareRadNumericUpDownBase(RadNumericUpDown element) : base(element)
        {
        }

        protected override void FocusElement()
        {
            ((RadNumericUpDown) base.element).Focus();
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
                double? nullable = ((RadNumericUpDown) base.element).Value;
                if (!nullable.HasValue)
                {
                    return 0.0;
                }
                return nullable.Value;
            }
            set
            {
                ((RadNumericUpDown) base.element).Value=(new double?(value));
                base.ForceValidationAndValueChanged();
            }
        }
    }
}

