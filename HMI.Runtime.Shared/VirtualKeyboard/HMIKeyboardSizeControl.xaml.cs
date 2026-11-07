// SPDX-License-Identifier: GPL-3.0-only
using HMI.Runtime.Shared.VirtualKeyboard;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace HMI.Runtime.Shared.VirtualKeyboard
{
    public partial class HMIKeyboardSizeControl : UserControl
    {
        private object owner;
        public static readonly DependencyProperty KeyboardSizeProperty = DependencyProperty.Register("KeyboardSize", typeof(double), typeof(HMIKeyboardSizeControl), new PropertyMetadata(600.0, new PropertyChangedCallback(HMIKeyboardSizeControl.OnKeyboardSizePropertyChanged)));


        public object Owner
        {
            get
            {
                return this.owner;
            }
            set
            {
                this.owner = value;
            }
        }
        [Category("HMI")]
        public double KeyboardSize
        {
            get
            {
                return (double)base.GetValue(HMIKeyboardSizeControl.KeyboardSizeProperty);
            }
            set
            {
                base.SetValue(HMIKeyboardSizeControl.KeyboardSizeProperty, value);
            }
        }
        public bool IsOpened
        {
            get;
            private set;
        }
        public HMIKeyboardSizeControl()
        {
            this.InitializeComponent();
            this.Closing.Storyboard.Completed += (new EventHandler(this.Storyboard_Completed));
            Canvas.SetZIndex(this, 3);
        }
        private void Storyboard_Completed(object sender, EventArgs e)
        {
            base.Visibility = System.Windows.Visibility.Collapsed;
        }
        private static void OnKeyboardSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            HMIKeyboardSizeControl wFKeyboardSizeControl = d as HMIKeyboardSizeControl;
            if (wFKeyboardSizeControl != null)
            {
                if (HMIKeyboardSizeControl.HasVirtualkeyboard())
                {
                    HMIKeyboardSizeControl.HandleVirtualKeyboardSize(e, wFKeyboardSizeControl);
                    return;
                }
                if (HMIKeyboardSizeControl.HasNumericKeyboard())
                {
                    HMIKeyboardSizeControl.HandleNumericKeyboardSize(e, wFKeyboardSizeControl);
                }
            }
        }
        private static void HandleVirtualKeyboardSize(DependencyPropertyChangedEventArgs e, HMIKeyboardSizeControl handler)
        {
            HMIVirtualKeyboard handlerOwner = (HMIVirtualKeyboard)handler.Owner;
            HMIKeyboardSizeControl.SetKeyboardSize(e, handler, handlerOwner);
        }
        private static void SetKeyboardSize(DependencyPropertyChangedEventArgs e, HMIKeyboardSizeControl handler, HMIVirtualKeyboard handlerOwner)
        {
            double num = (double)e.NewValue;
            if (handlerOwner != null)
            {
                double maxWidth = handlerOwner.KeyboardViewBox.MaxWidth;
                double minWidth = handlerOwner.KeyboardViewBox.MinWidth;
                if (num > maxWidth)
                {
                    handlerOwner.KeyboardSize = maxWidth;
                    handler.KeyboardSize = maxWidth;
                    return;
                }
                if (num < minWidth)
                {
                    handlerOwner.KeyboardSize = minWidth;
                    handler.KeyboardSize = minWidth;
                    return;
                }
                handlerOwner.KeyboardSize = num;
            }
        }
        private static void HandleNumericKeyboardSize(DependencyPropertyChangedEventArgs e, HMIKeyboardSizeControl handler)
        {
            HMINumericKeyboard wFNumericKeyboard = (HMINumericKeyboard)handler.Owner;
            double num = (double)e.NewValue;
            if (wFNumericKeyboard != null)
            {
                double maxWidth = wFNumericKeyboard.KeyboardViewBox.MaxWidth;
                double minWidth = wFNumericKeyboard.KeyboardViewBox.MinWidth;
                if (num > maxWidth)
                {
                    wFNumericKeyboard.KeyboardSize = maxWidth;
                    handler.KeyboardSize = maxWidth;
                    return;
                }
                if (num < minWidth)
                {
                    wFNumericKeyboard.KeyboardSize = minWidth;
                    handler.KeyboardSize = minWidth;
                    return;
                }
                wFNumericKeyboard.KeyboardSize = num;
            }
        }
        private static bool HasNumericKeyboard()
        {
            return HMIKeyboardWindow.Container.Children.Contains(HMINumericKeyboard.HMINumericKeyboardInstance);
        }
        private static bool HasVirtualkeyboard()
        {
            return HMIKeyboardWindow.Container.Children.Contains(HMIVirtualKeyboard.HMIKeyboardInstance);
        }
        public void Show()
        {
            this.GoToState("Opened", this);
            this.IsOpened = true;
        }
        public void Hide()
        {
            this.GoToState("Closing", this);
            this.IsOpened = false;
        }
        private void GoToState(string stateName, Control controlName)
        {
            try
            {
                VisualStateManager.GoToState(controlName, stateName, true);
            }
            catch
            {
            }
        }
        private void PlusButtonGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.KeyboardSize += 10.0;
        }
        private void MinusButtonGrid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.KeyboardSize -= 10.0;
        }
    }
}
