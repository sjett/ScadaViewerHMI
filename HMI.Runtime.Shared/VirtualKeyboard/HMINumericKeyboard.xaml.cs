// SPDX-License-Identifier: GPL-3.0-only
using HMI.Runtime.Core;
using HMI.Runtime.Core.Helper;
using HMI.Runtime.Shared.KeyboardControl;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace HMI.Runtime.Shared.VirtualKeyboard
{
    public partial class HMINumericKeyboard : UserControl, IVirtualKeyboard
    {
        private static Panel parnetGrid;
        private static HMINumericKeyboard wfNumericKeyboardInstance;

        private IVirtualKeyboardHelper helper;
        private bool hasNCHitTest;
        private Point initialNCHitTest;
        private bool isFirstDisplay = true;
        private bool dragStarted;
        private IHMIConnector connector;
        private CultureInfo currentCultureInfo = CultureInfo.CurrentUICulture;

        public static readonly DependencyProperty KeyboardSizeProperty = DependencyProperty.Register("KeyboardSize", typeof(double), typeof(HMINumericKeyboard), new PropertyMetadata(170.0, new PropertyChangedCallback(HMINumericKeyboard.OnKeyboardSizePropertyChanged)));

        public bool IsInDesignMode
        {
            get
            {
                return DesignerProperties.GetIsInDesignMode(this);
            }
        }
        public static Thickness ThicknessPossition
        {
            get;
            set;
        }
        [Category("HMI")]
        public double KeyboardSize
        {
            get
            {
                return (double)base.GetValue(HMINumericKeyboard.KeyboardSizeProperty);
            }
            set
            {
                base.SetValue(HMINumericKeyboard.KeyboardSizeProperty, value);
            }
        }
        public static HMINumericKeyboard HMINumericKeyboardInstance
        {
            get
            {
                if (HMINumericKeyboard.wfNumericKeyboardInstance == null)
                {
                    HMINumericKeyboard.InitializeInstance();
                }
                return HMINumericKeyboard.wfNumericKeyboardInstance;
            }
            set
            {
                HMINumericKeyboard.wfNumericKeyboardInstance = value;
            }
        }
        public Control KeyboardControl
        {
            get
            {
                return this;
            }
        }
        public IVirtualKeyboardAware Owner
        {
            get;
            set;
        }
        private static void OnKeyboardSizePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }
        public HMINumericKeyboard()
        {
            this.InitializeComponent();
            this.helper = new VirtualKeyboardHelper();
            this.uxPopupElement.Opened+=(new EventHandler(this.uxPopupElement_Opened));
            this.uxGridElement.MouseLeftButtonDown+=(new MouseButtonEventHandler(this.uxGridElement_MouseLeftButtonDown));
            this.uxGridElement.MouseLeftButtonUp+=(new MouseButtonEventHandler(this.uxGridElement_MouseLeftButtonUp));
            this.uxGridElement.MouseMove+=(new MouseEventHandler(this.uxGridElement_MouseMove));
        }
        private static void InitializeInstance()
        {
            HMINumericKeyboard.wfNumericKeyboardInstance = new HMINumericKeyboard();
            HMINumericKeyboard.wfNumericKeyboardInstance.SetValue(Canvas.ZIndexProperty, 2);
            HMINumericKeyboard.wfNumericKeyboardInstance.Closed.Storyboard.Completed+=(new EventHandler(HMINumericKeyboard.StoryboardClosed_Completed));
        }
        private static bool ExistsAsChildren()
        {
            return HMIKeyboardWindow.Container.Children.Contains(HMINumericKeyboard.HMINumericKeyboardInstance);
        }
        private void Content_Resized(object sender, SizeChangedEventArgs e)
        {
            this.uxCanvasElement.Height=(Application.Current.MainWindow.ActualHeight);
            this.uxCanvasElement.Width = (Application.Current.MainWindow.ActualWidth);
        }
        private void Key_VirtualKeyboardKeyPressed(object sender, VirtualKeyPressedEventArgs e)
        {
            if (this.Owner != null)
            {
                this.Owner.SendKey(e.Key);
            }
        }
        public void KeyboardClose(object sender, MouseButtonEventArgs e)
        {
            if (HMINumericKeyboard.ExistsAsChildren())
            {
                this.HideKeyboard();
            }
            e.Handled=(true);
        }
        private static void StoryboardClosed_Completed(object sender, EventArgs e)
        {
            KeyboardController.Instance.DettachKeyboard();
        }
        private void uxGridElement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Handled)
            {
                return;
            }
            this.hasNCHitTest = true;
            this.initialNCHitTest = e.GetPosition(this.uxGridElement);
            this.uxGridElement.CaptureMouse();
            e.Handled=(true);
        }
        private void uxGridElement_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.Handled)
            {
                return;
            }
            if (this.hasNCHitTest)
            {
                this.hasNCHitTest = false;
                this.dragStarted = false;
                this.uxGridElement.ReleaseMouseCapture();
                e.Handled=(true);
            }
        }
        private void uxGridElement_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.hasNCHitTest)
            {
                Point position = e.GetPosition(this.uxGridElement);
                Point point = new Point(position.X - this.initialNCHitTest.X, position.Y - this.initialNCHitTest.Y);
                if (this.dragStarted || System.Math.Abs(point.X) > 3.0 || System.Math.Abs(point.Y) > 3.0)
                {
                    this.dragStarted = true;
                    GeneralTransform generalTransform = this.uxGridElement.TransformToVisual(this.uxCanvasElement);
                    Point point2 = generalTransform.Transform(point);
                    if (point2.X < 0.0 || point2.X + this.uxGridElement.ActualWidth > this.uxCanvasElement.ActualWidth)
                    {
                        point.X=(0.0);
                    }
                    if (point2.Y < 0.0 || point2.Y + this.uxGridElement.ActualHeight > this.uxCanvasElement.ActualHeight)
                    {
                        point.Y=(0.0);
                    }
                    this.uxGridElement.SetValue(Canvas.TopProperty, (double)this.uxGridElement.GetValue(Canvas.TopProperty) + point.Y);
                    this.uxGridElement.SetValue(Canvas.LeftProperty, (double)this.uxGridElement.GetValue(Canvas.LeftProperty) + point.X);
                }
            }
        }
        private void uxPopupElement_Opened(object sender, EventArgs e)
        {
            VisualStateManager.GoToState(this, "Open", true);
        }
        internal static void PrepareForRezise()
        {
            HMINumericKeyboard.wfNumericKeyboardInstance.ToggleResizeControl();
        }
        public void StartWarningAnimation()
        {
            var WarningAnimation = this.Resources["WarningAnimation"] as Storyboard;
            WarningAnimation.Begin();
            (this.uxKeyboardSizeControl.Resources["WarningAnimation"] as Storyboard).Begin();
        }
        public void ShowKeyboard()
        {
            if (this.connector == null && !this.IsInDesignMode)
            {
                this.connector = RuntimeServices.GetRequiredService<IHMIConnector>();
                this.connector.RegisterLanguageChangedHandler(new string[]
				{
					"!null!"
				}, new OnLanguageChangedDelegate(this.OnSymbolicTextTranslationsChangedHandler));
            }
            Application.Current.MainWindow.SizeChanged += (new SizeChangedEventHandler(this.Content_Resized));
            this.uxCanvasElement.Background=(null);
            this.uxCanvasElement.Height=(Application.Current.MainWindow.ActualHeight);
            this.uxCanvasElement.Width = (Application.Current.MainWindow.ActualWidth);
            Transform transform = this.helper.CreateRenderTransform(base.TransformToVisual(null));
            if (transform != null)
            {
                base.RenderTransform=(transform);
            }
            this.uxPopupElement.IsOpen=(true);
            if (this.isFirstDisplay)
            {
                this.isFirstDisplay = false;
                this.helper.CenterBottomControl(this.uxGridElement, this.uxCanvasElement, 1);//Application.Current.MainWindow.ZoomFactor
            }
        }
        public void HideKeyboard()
        {
            Application.Current.MainWindow.SizeChanged -= (new SizeChangedEventHandler(this.Content_Resized));
            VisualStateManager.GoToState(this, "Closed", true);
            this.uxKeyboardSizeControl.Hide();
        }
        public void ToggleResizeControl()
        {
            if (!this.uxKeyboardSizeControl.IsOpened)
            {
                if (this.uxKeyboardSizeControl.ActualHeight == 0.0 || double.IsNaN(this.uxKeyboardSizeControl.ActualHeight))
                {
                    this.uxKeyboardSizeControl.Measure(new Size(this.uxCanvasElement.Width, this.uxCanvasElement.Height));
                }
                this.uxKeyboardSizeControl.SetValue(Canvas.TopProperty, 0.0);
                this.uxKeyboardSizeControl.SetValue(Canvas.LeftProperty, this.uxCanvasElement.Width - this.uxKeyboardSizeControl.Width);
                this.uxKeyboardSizeControl.Visibility= System.Windows.Visibility.Visible;
                this.uxKeyboardSizeControl.Owner = this;
                this.uxKeyboardSizeControl.KeyboardSize = this.KeyboardViewBox.ActualWidth;
                this.uxKeyboardSizeControl.Show();
                return;
            }
            this.uxKeyboardSizeControl.Hide();
        }
        private void OnSymbolicTextTranslationsChangedHandler(string[] symbolicTexts, string[] translationTexts)
        {
            this.currentCultureInfo = CultureInfoCache.GetCultureInfo(this.connector.GetLanguageId());
            this.comma_Key.Text = this.currentCultureInfo.NumberFormat.CurrencyDecimalSeparator;
        }
    }
}
