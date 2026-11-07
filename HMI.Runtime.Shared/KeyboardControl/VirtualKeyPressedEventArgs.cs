// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Runtime.Shared.KeyboardControl
{
    using System;
    using System.Runtime.CompilerServices;

    public class VirtualKeyPressedEventArgs : EventArgs
    {
        public VirtualKeyPressedEventArgs(string pressedKey)
        {
            this.Key = pressedKey;
        }

        public string Key { get; private set; }
    }
}

