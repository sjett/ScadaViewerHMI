// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Runtime.Shared.VirtualKeyboard.KeyboardAware
{
    using System;
    using System.Windows;

    public abstract class TextualKeyboardAwareBase : KeyboardAwareBase
    {
        protected TextualKeyboardAwareBase(FrameworkElement element) : base(element)
        {
        }

        protected override void AddOrRemoveSign()
        {
            int index = this.Text.IndexOf("-", StringComparison.InvariantCulture);
            this.Text = (index == -1) ? this.Text.Insert(0, "-") : this.Text.Remove(index, 1);
        }

        protected override void AddOrReplaceOnKey(string key)
        {
            if (this.GetSelectedLength() == this.Text.Length)
            {
                this.Text = key;
            }
            else
            {
                this.Text = this.Text + key;
            }
        }

        protected override void DeleteLastCharacter()
        {
            if (!string.IsNullOrEmpty(this.Text))
            {
                this.Text = this.Text.Remove(this.Text.Length - 1, 1);
            }
        }

        protected override void SetCarretPositionAtEnd()
        {
            this.SetCarretPosition(this.Text.Length);
        }

        protected abstract string Text { get; set; }
    }
}

