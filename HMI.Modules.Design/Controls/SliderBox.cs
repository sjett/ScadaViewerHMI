// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;
using System.Windows.Controls;

namespace HMI.Modules.Design.Controls
{
    [TemplatePart(Name = "uxTextValue", Type = typeof(TextBox)), TemplatePart(Name = "uxSlider", Type = typeof(Slider))]
    public class SliderBox : Control
    {
        public static readonly DependencyProperty ValueProperty = 
            DependencyProperty.Register("Value", 
                typeof(double), 
                typeof(SliderBox), 
                new PropertyMetadata(0.0, 
                    new PropertyChangedCallback(OnValueChanged),
                    new CoerceValueCallback(CoerceValue)));
        public double Value
        {
            get
            {
                return (double)base.GetValue(SliderBox.ValueProperty);
            }
            set
            {
                base.SetValue(SliderBox.ValueProperty, value);
            }
        }

        public static readonly DependencyProperty MinValueProperty = 
            DependencyProperty.Register("MinValue", 
                typeof(double), 
                typeof(SliderBox), 
                new PropertyMetadata(0.0, 
                    new PropertyChangedCallback(OnMinValueChanged),
                    new CoerceValueCallback(CoerceMinValue)));
        public double MinValue
        {
            get
            {
                return (double)base.GetValue(SliderBox.MinValueProperty);
            }
            set
            {
                base.SetValue(SliderBox.MinValueProperty, value);
            }
        }

        public static readonly DependencyProperty MaxValueProperty = 
            DependencyProperty.Register("MaxValue", 
                typeof(double), 
                typeof(SliderBox), 
                new PropertyMetadata(100.0, 
                    new PropertyChangedCallback(OnMaxValueChanged),
                    new CoerceValueCallback(CoerceMaxValue)));
        public double MaxValue
        {
            get
            {
                return (double)base.GetValue(SliderBox.MaxValueProperty);
            }
            set
            {
                base.SetValue(SliderBox.MaxValueProperty, value);
            }
        }

        public static readonly DependencyProperty SmallIncrementProperty = 
            DependencyProperty.Register("SmallIncrement", typeof(double), typeof(SliderBox), new PropertyMetadata(0.1));
        public double SmallIncrement
        {
            get
            {
                return (double)base.GetValue(SliderBox.SmallIncrementProperty);
            }
            set
            {
                base.SetValue(SliderBox.SmallIncrementProperty, value);
            }
        }

        public static readonly DependencyProperty LargeIncrementProperty = 
            DependencyProperty.Register("LargeIncrement", typeof(double), typeof(SliderBox), new PropertyMetadata(1.0));
        public double LargeIncrement
        {
            get
            {
                return (double)base.GetValue(SliderBox.LargeIncrementProperty);
            }
            set
            {
                base.SetValue(SliderBox.LargeIncrementProperty, value);
            }
        }

        public static readonly DependencyProperty IsIntegerProperty = 
            DependencyProperty.Register("IsInteger", typeof(bool), typeof(SliderBox), new PropertyMetadata(false));
        public bool IsInteger
        {
            get
            {
                return (bool)base.GetValue(SliderBox.IsIntegerProperty);
            }
            set
            {
                base.SetValue(SliderBox.IsIntegerProperty, value);
            }
        }
        static SliderBox()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(SliderBox), new FrameworkPropertyMetadata(typeof(SliderBox)));
        }
        /// <summary>
        /// 值改变回调
        /// </summary>
        private static void OnValueChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            dependencyObject.CoerceValue(SliderBox.MaxValueProperty);// 确保MaxValue不小于Value
            dependencyObject.CoerceValue(SliderBox.MinValueProperty);// 确保MinValue不大于Value
        }
        /// <summary>
        /// 值强制回调
        /// </summary>
        private static object CoerceValue(DependencyObject dependencyObject, object baseValue)
        {
            SliderBox sliderBox = (SliderBox)dependencyObject;
            double coercedValue = (double)baseValue;
            if (coercedValue > sliderBox.MaxValue)
            {
                coercedValue = sliderBox.MaxValue;
            }
            if (coercedValue < sliderBox.MinValue)
            {
                coercedValue = sliderBox.MinValue;
            }
            return coercedValue;
        }
        private static void OnMinValueChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            dependencyObject.CoerceValue(SliderBox.ValueProperty);
            dependencyObject.CoerceValue(SliderBox.MaxValueProperty);
        }
        private static object CoerceMinValue(DependencyObject dependencyObject, object baseValue)
        {
            SliderBox sliderBox = (SliderBox)dependencyObject;
            double coercedValue = (double)baseValue;
            if (coercedValue > sliderBox.MaxValue)
            {
                coercedValue = sliderBox.MaxValue;
            }
            return coercedValue;
        }
        private static void OnMaxValueChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
            dependencyObject.CoerceValue(SliderBox.ValueProperty);
            dependencyObject.CoerceValue(SliderBox.MinValueProperty);
        }
        private static object CoerceMaxValue(DependencyObject dependencyObject, object baseValue)
        {
            SliderBox sliderBox = (SliderBox)dependencyObject;
            double coercedValue = (double)baseValue;
            if (coercedValue < sliderBox.MinValue)
            {
                coercedValue = sliderBox.MinValue;
            }
            return coercedValue;
        }
    }
}
