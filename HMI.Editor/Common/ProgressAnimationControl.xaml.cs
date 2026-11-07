// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace HMI.Editor.Common
{
    /// <summary>
    /// ProgressAnimationControl.xaml 的交互逻辑
    /// </summary>
    public partial class ProgressAnimationControl : UserControl
    {
        private Storyboard storyboard;
        private bool isAnimated;
        public static readonly DependencyProperty Color1Property = DependencyProperty.Register("Color1", typeof(Color), typeof(ProgressAnimationControl), new PropertyMetadata(ColorConverter.ConvertFromString("#FF030404"), new PropertyChangedCallback(ProgressAnimationControl.OnColor1PropertyChanged)));
        public static readonly DependencyProperty Color2Property = DependencyProperty.Register("Color2", typeof(Color), typeof(ProgressAnimationControl), new PropertyMetadata(ColorConverter.ConvertFromString("#FFABB8B9"), new PropertyChangedCallback(ProgressAnimationControl.OnColor2PropertyChanged)));

        public bool IsAnimated
        {
            get
            {
                return this.isAnimated;
            }
            set
            {
                if (this.isAnimated == value || this.storyboard == null)
                {
                    return;
                }
                this.isAnimated = value;
                if (value)
                {
                    this.storyboard.Begin();
                    return;
                }
                this.storyboard.Stop();
            }
        }
        public Color Color1
        {
            get
            {
                return (Color)base.GetValue(ProgressAnimationControl.Color1Property);
            }
            set
            {
                base.SetValue(ProgressAnimationControl.Color1Property, value);
                this.SetColors();
            }
        }
        public Color Color2
        {
            get
            {
                return (Color)base.GetValue(ProgressAnimationControl.Color2Property);
            }
            set
            {
                base.SetValue(ProgressAnimationControl.Color2Property, value);
                this.SetColors();
            }
        }

        public ProgressAnimationControl()
        {
            InitializeComponent();
            this.storyboard = (base.Resources["RotateAngle"] as Storyboard);
        }

        private static void OnColor1PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }
        private static void OnColor2PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.SetColors();
        }
        private void SetColors()
        {
            this.Gradient1.Color = this.Color1;
            this.Gradient2.Color = this.Color2;
        }
    }
}
