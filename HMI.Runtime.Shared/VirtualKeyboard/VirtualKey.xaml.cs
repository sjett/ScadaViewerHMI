// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace HMI.Runtime.Shared.VirtualKeyboard
{
    public partial class VirtualKey : VirtualKeyBase
    {
        protected override string KeyString
        {
            get
            {
                return this.uxKeyButton.Content.ToString();
            }
        }
        public VirtualKey()
        {
            this.InitializeComponent();
            base.KeyButton = this.uxKeyButton;
        }
    }
}
