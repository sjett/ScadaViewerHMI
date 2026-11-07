// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Runtime.Shared.VirtualKeyboard.KeyboardAware
{
    using System;
    using System.Windows.Controls;

    internal abstract class KeyboardAwarePasswordBoxBase : TextualKeyboardAwareBase
    {
        protected KeyboardAwarePasswordBoxBase(PasswordBox element) : base(element)
        {
        }

        protected override void FocusElement()
        {
            ((PasswordBox) base.element).Focus();
        }

        protected override int GetSelectedLength()
        {
            return 0;
        }

        protected override void SetCarretPosition(int carretPosition)
        {
        }

        protected override string Text
        {
            get
            {
                return ((PasswordBox) base.element).Password;
            }
            set
            {
                ((PasswordBox) base.element).Password=(value);
            }
        }
    }
}

