// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace HMI.RuntimeControls.Dialogs
{
    [
        TemplatePart(Name = "uxOK", Type = typeof(Button)), 
        TemplatePart(Name = "uxNo", Type = typeof(Button)), 
        TemplatePart(Name = "uxYes", Type = typeof(Button)), 
        TemplatePart(Name = "uxCancel", Type = typeof(Button)), 
        TemplatePart(Name = "uxIcon", Type = typeof(Image))]
    public class HMIMessageBoxControl : HMIDialogControl
    {
        private const string TemplatePartOKButton = "uxOK";
        private const string TemplatePartCancelButton = "uxCancel";
        private const string TemplatePartYesButton = "uxYes";
        private const string TemplatePartNoButton = "uxNo";
        private const string TemplatePartIcon = "uxIcon";

        private Button uxOK;
        private Button uxCancel;
        private Button uxYes;
        private Button uxNo;
        private Image uxIcon;

        public static readonly DependencyProperty ButtonsStyleProperty = DependencyProperty.Register("ButtonsStyle", typeof(Style), typeof(HMIMessageBoxControl), new PropertyMetadata(null));
        public static readonly DependencyProperty OKTextProperty = DependencyProperty.Register("OKText", typeof(string), typeof(HMIMessageBoxControl), new PropertyMetadata("OK"));
        public static readonly DependencyProperty CancelTextProperty = DependencyProperty.Register("CancelText", typeof(string), typeof(HMIMessageBoxControl), new PropertyMetadata("Cancel"));
        public static readonly DependencyProperty YesTextProperty = DependencyProperty.Register("YesText", typeof(string), typeof(HMIMessageBoxControl), new PropertyMetadata("Yes"));
        public static readonly DependencyProperty NoTextProperty = DependencyProperty.Register("NoText", typeof(string), typeof(HMIMessageBoxControl), new PropertyMetadata("No"));
        public static readonly DependencyProperty ButtonsWidthProperty = DependencyProperty.Register("ButtonsWidth", typeof(double), typeof(HMIMessageBoxControl), new PropertyMetadata(75.0));
        public static readonly DependencyProperty ButtonsProperty = DependencyProperty.Register("Buttons", typeof(HMIMessageBoxControlButtons), typeof(HMIMessageBoxControl), new PropertyMetadata(HMIMessageBoxControlButtons.OK, new PropertyChangedCallback(HMIMessageBoxControl.OnButtonsChanged)));
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(HMIMessageBoxControlIcon), typeof(HMIMessageBoxControl), new PropertyMetadata(HMIMessageBoxControlIcon.Info, new PropertyChangedCallback(HMIMessageBoxControl.OnIconChanged)));
        
        public Style ButtonsStyle
        {
            get
            {
                return (Style)base.GetValue(HMIMessageBoxControl.ButtonsStyleProperty);
            }
            set
            {
                base.SetValue(HMIMessageBoxControl.ButtonsStyleProperty, value);
            }
        }
        public string OKText
        {
            get
            {
                return (string)base.GetValue(HMIMessageBoxControl.OKTextProperty);
            }
            set
            {
                base.SetValue(HMIMessageBoxControl.OKTextProperty, value);
            }
        }
        public string CancelText
        {
            get
            {
                return (string)base.GetValue(HMIMessageBoxControl.CancelTextProperty);
            }
            set
            {
                base.SetValue(HMIMessageBoxControl.CancelTextProperty, value);
            }
        }
        public string YesText
        {
            get
            {
                return (string)base.GetValue(HMIMessageBoxControl.YesTextProperty);
            }
            set
            {
                base.SetValue(HMIMessageBoxControl.YesTextProperty, value);
            }
        }
        public string NoText
        {
            get
            {
                return (string)base.GetValue(HMIMessageBoxControl.NoTextProperty);
            }
            set
            {
                base.SetValue(HMIMessageBoxControl.NoTextProperty, value);
            }
        }
        public double ButtonsWidth
        {
            get
            {
                return (double)base.GetValue(HMIMessageBoxControl.ButtonsWidthProperty);
            }
            set
            {
                base.SetValue(HMIMessageBoxControl.ButtonsWidthProperty, value);
            }
        }
        public HMIMessageBoxControlButtons Buttons
        {
            get
            {
                return (HMIMessageBoxControlButtons)base.GetValue(HMIMessageBoxControl.ButtonsProperty);
            }
            set
            {
                base.SetValue(HMIMessageBoxControl.ButtonsProperty, value);
            }
        }
        public HMIMessageBoxControlIcon Icon
        {
            get
            {
                return (HMIMessageBoxControlIcon)base.GetValue(HMIMessageBoxControl.IconProperty);
            }
            set
            {
                base.SetValue(HMIMessageBoxControl.IconProperty, value);
            }
        }
        private static void OnButtonsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            HMIMessageBoxControl? wFMessageBoxControl = d as HMIMessageBoxControl;
            if (wFMessageBoxControl != null)
            {
                wFMessageBoxControl.ResetAllButtons();
                wFMessageBoxControl.ConfigureButtons();
            }
        }
        private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            HMIMessageBoxControl? wFMessageBoxControl = d as HMIMessageBoxControl;
            if (wFMessageBoxControl != null)
            {
                wFMessageBoxControl.SetIcon();
            }
        }
        public HMIMessageBoxControl()
        {
            base.DefaultStyleKey = (typeof(HMIMessageBoxControl));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.uxOK = (base.GetTemplateChild(TemplatePartOKButton) as Button);
            this.uxCancel = (base.GetTemplateChild(TemplatePartCancelButton) as Button);
            this.uxYes = (base.GetTemplateChild(TemplatePartYesButton) as Button);
            this.uxNo = (base.GetTemplateChild(TemplatePartNoButton) as Button);
            this.uxIcon = (base.GetTemplateChild(TemplatePartIcon) as Image);

            this.ConfigureButtons();
            this.SetIcon();
        }
        private void SetIcon()
        {
            if (this.uxIcon == null)
            {
                return;
            }
            switch (this.Icon)
            {
                case HMIMessageBoxControlIcon.Info:
                    this.ShowIcon("/HMI.RuntimeControls;component/images/info.png");
                    return;
                case HMIMessageBoxControlIcon.Warning:
                    this.ShowIcon("/HMI.RuntimeControls;component/images/warning.png");
                    return;
                case HMIMessageBoxControlIcon.Question:
                    this.ShowIcon("/HMI.RuntimeControls;component/images/question.png");
                    return;
                case HMIMessageBoxControlIcon.Error:
                    this.ShowIcon("/HMI.RuntimeControls;component/images/error.png");
                    return;
                default:
                    this.HideIcon();
                    return;
            }
        }
        protected virtual void ConfigureButtons()
        {
            switch (this.Buttons)
            {
                case HMIMessageBoxControlButtons.OKCancel:
                    this.IncludeButton(this.uxOK, true);
                    this.IncludeButton(this.uxCancel, false);
                    return;
                case HMIMessageBoxControlButtons.YesNo:
                    this.IncludeButton(this.uxYes, true);
                    this.IncludeButton(this.uxNo, false);
                    return;
                default:
                    this.IncludeButton(this.uxOK, true);
                    return;
            }
        }
        private void ShowIcon(string uriPath)
        {
            this.uxIcon.Visibility = System.Windows.Visibility.Visible;
            this.uxIcon.Source = (new BitmapImage(new Uri(uriPath, 0)));
        }
        private void HideIcon()
        {
            this.uxIcon.Visibility = System.Windows.Visibility.Collapsed;
        }
        private void IncludeButton(Button button, bool clickResult)
        {
            if (button != null)
            {
                button.Visibility = System.Windows.Visibility.Visible;
                button.Tag = (clickResult);
                button.Click += (new RoutedEventHandler(this.Button_Click));
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button.Tag != null && button.Tag is bool)
            {
                base.DialogResult = (new bool?((bool)button.Tag));
            }
        }
        private void ResetButton(Button button)
        {
            if (button != null)
            {
                button.Visibility = System.Windows.Visibility.Collapsed;
                button.Click -= (new RoutedEventHandler(this.Button_Click));
            }
        }
        private void ResetAllButtons()
        {
            this.ResetButton(this.uxOK);
            this.ResetButton(this.uxCancel);
            this.ResetButton(this.uxYes);
            this.ResetButton(this.uxNo);
        }
    }
}
