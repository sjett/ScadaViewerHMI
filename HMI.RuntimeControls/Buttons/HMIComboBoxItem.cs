// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using System.Windows.Media;
using HMI.Runtime.Shared.Common;
namespace HMI.RuntimeControls.Buttons
{
    public class HMIComboBoxItem : PropertyChangedNotifier
    {
        private string symbolicText = "Auto";
        private string translation;
        private double value;
        private ImageSource imageSource;
        private int height = 35;
        public int Height
        {
            get
            {
                return this.height;
            }
            set
            {
                this.height = value;
                base.RaisePropertyChanged<int>(() => this.Height);
            }
        }
        public ImageSource ImageSource
        {
            get
            {
                return this.imageSource;
            }
            set
            {
                this.imageSource = value;
                base.RaisePropertyChanged<ImageSource>(() => this.ImageSource);
            }
        }
        public string SymbolicText
        {
            get
            {
                return this.symbolicText;
            }
            set
            {
                this.symbolicText = value;
                base.RaisePropertyChanged<string>(() => this.SymbolicText);
            }
        }
        [Browsable(false)]
        public string Translation
        {
            get
            {
                return this.translation;
            }
            set
            {
                this.translation = value;
                base.RaisePropertyChanged<string>(() => this.Translation);
            }
        }
        public double Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
                base.RaisePropertyChanged<double>(() => this.Value);
            }
        }
    }
}
