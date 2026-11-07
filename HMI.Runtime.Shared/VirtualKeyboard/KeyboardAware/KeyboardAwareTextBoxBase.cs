// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Runtime.Shared.VirtualKeyboard.KeyboardAware
{
    using System;
    using System.Windows.Controls;

    internal abstract class KeyboardAwareTextBoxBase : TextualKeyboardAwareBase
    {
        protected KeyboardAwareTextBoxBase(TextBox element) : base(element)
        {
        }

        protected override void FocusElement()
        {
            ((TextBox) base.element).Focus();
        }

        protected override int GetSelectedLength()
        {
            return ((TextBox) base.element).SelectionLength;
        }

        protected override void SetCarretPosition(int carretPosition)
        {
            ((TextBox) base.element).SelectionStart=(carretPosition);
        }

        protected override string Text
        {
            get
            {
                return ((TextBox) base.element).Text;
            }
            set
            {
                ((TextBox) base.element).Text=(value);
            }
        }
    }
}

