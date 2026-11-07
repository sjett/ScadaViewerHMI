// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Runtime.Shared.VirtualKeyboard.KeyboardAware
{
    using System;
    using System.Globalization;
    using System.Windows;

    public abstract class NumericKeyboardAwareBase : KeyboardAwareBase
    {
        protected NumericKeyboardAwareBase(FrameworkElement element) : base(element)
        {
        }

        protected override void AddOrRemoveSign()
        {
            this.Value = 0.0 - this.Value;
        }

        protected override void AddOrReplaceOnKey(string key)
        {
            int? intValue = this.GetIntValue(key);
            if (intValue.HasValue)
            {
                this.Value = (this.Value * 10.0) + ((double) intValue.Value);
            }
        }

        protected override void DeleteLastCharacter()
        {
            this.Value /= 10.0;
        }

        private int? GetIntValue(string key)
        {
            int num;
            if (!int.TryParse(key, out num))
            {
                return null;
            }
            return new int?(num);
        }

        protected override void SetCarretPositionAtEnd()
        {
            this.SetCarretPosition(this.Value.ToString(CultureInfo.InvariantCulture).Length);
        }

        protected abstract double Value { get; set; }
    }
}

