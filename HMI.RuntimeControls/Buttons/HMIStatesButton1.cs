// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
namespace HMI.RuntimeControls.Buttons
{
    public class HMIStatesButton1 : HMIStatesButtonBase
    {
        private Image uxButtonImage;

        public static readonly DependencyProperty IconImageProperty = DependencyProperty.Register("IconImage", typeof(ImageSource), typeof(HMIStatesButton1), null);
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(HMIStatesButton1), null);
        
        public ImageSource IconImage
        {
            get
            {
                return (ImageSource)base.GetValue(HMIStatesButton1.IconImageProperty);
            }
            set
            {
                base.SetValue(HMIStatesButton1.IconImageProperty, value);
            }
        }
        public double Value
        {
            get
            {
                return (double)base.GetValue(HMIStatesButton1.ValueProperty);
            }
            set
            {
                base.SetValue(HMIStatesButton1.ValueProperty, value);
            }
        }
        public HMIStatesButton1()
        {
            base.DefaultStyleKey=(typeof(HMIStatesButton1));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.uxButtonImage = (base.GetTemplateChild("uxButtonImage") as Image);
            this.UpdateIconPlacement();

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }
            this.Value = Math.Round(this.Value, 5);
        }
        protected override double GetWriteValue()
        {
            return this.Value;
        }
        protected override void UpdateIconPlacement()
        {
            if (this.uxButtonImage == null)
            {
                return;
            }
            Grid.SetRow(this.uxButtonImage, base.GetIconGridRow());
            Grid.SetColumn(this.uxButtonImage, base.GetIconGridColumn());
        }
    }
}
