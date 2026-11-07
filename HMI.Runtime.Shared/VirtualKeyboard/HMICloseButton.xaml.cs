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
    public partial class CloseButton : UserControl
    {
        public CloseButton()
        {
            InitializeComponent();
        }

        private void HMIButton_MouseOver(object sender, MouseEventArgs e)
        {
            this.GoToState("MouseOver");
        }
        private void HMIButton_MouseLeave(object sender, MouseEventArgs e)
        {
            this.GoToState("Normal");
            base.Cursor=(Cursors.Arrow);
        }
        private void HMIButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.GoToState("Pressed");
        }
        private void HMIButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.GoToState("MouseOver");
        }
        private void GoToState(string stateName)
        {
            VisualStateManager.GoToState(this.Button, stateName, true);
        }
    }
}
