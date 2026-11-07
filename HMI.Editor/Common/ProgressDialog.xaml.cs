// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;

namespace HMI.Editor.Common
{
    /// <summary>
    /// ProgressDialog.xaml 的交互逻辑
    /// </summary>
    public partial class ProgressDialog : Window
    {
        private readonly BackgroundWorker worker;
        private System.Globalization.CultureInfo uiCulture;
        private bool cancelled;
        private System.Exception error;
        private object result;
        private Action action;
        public static readonly DependencyProperty WindowColor1Property = DependencyProperty.Register("WindowColor1", typeof(Color), typeof(ProgressDialog), new PropertyMetadata(ColorConverter.ConvertFromString("#FF030404"), new PropertyChangedCallback(ProgressDialog.OnWindowColor1PropertyChanged)));
        public static readonly DependencyProperty WindowColor2Property = DependencyProperty.Register("WindowColor2", typeof(Color), typeof(ProgressDialog), new PropertyMetadata(ColorConverter.ConvertFromString("#FF809090"), new PropertyChangedCallback(ProgressDialog.OnWindowColor2PropertyChanged)));
        public static readonly DependencyProperty WindowColor3Property = DependencyProperty.Register("WindowColor3", typeof(Color), typeof(ProgressDialog), new PropertyMetadata(ColorConverter.ConvertFromString("#FF000000"), new PropertyChangedCallback(ProgressDialog.OnWindowColor3PropertyChanged)));
        public static readonly DependencyProperty AnimationColor1Property = DependencyProperty.Register("AnimationColor1", typeof(Color), typeof(ProgressDialog), new PropertyMetadata(Colors.Red, new PropertyChangedCallback(ProgressDialog.OnAnimationColor1PropertyChanged)));
        public static readonly DependencyProperty AnimationColor2Property = DependencyProperty.Register("AnimationColor2", typeof(Color), typeof(ProgressDialog), new PropertyMetadata(Colors.White, new PropertyChangedCallback(ProgressDialog.OnAnimationColor2PropertyChanged)));
        public new static readonly DependencyProperty BorderBrushProperty = DependencyProperty.Register("BorderBrush", typeof(Brush), typeof(ProgressDialog), new PropertyMetadata(Brushes.Transparent, new PropertyChangedCallback(ProgressDialog.OnBorderBrushPropertyChanged)));

        public string Message
        {
            get
            {
                return this.uxMessage.Text;
            }
            set
            {
                this.uxMessage.Text = value;
            }
        }
        public bool Cancelled
        {
            get
            {
                return this.cancelled;
            }
        }
        public System.Exception Error
        {
            get
            {
                return this.error;
            }
        }
        public object Result
        {
            get
            {
                return this.result;
            }
        }
        public bool IsSTAThread
        {
            get;
            set;
        }
        public Color WindowColor1
        {
            get
            {
                return (Color)base.GetValue(ProgressDialog.WindowColor1Property);
            }
            set
            {
                base.SetValue(ProgressDialog.WindowColor1Property, value);
                this.SetColors();
            }
        }
        public Color WindowColor2
        {
            get
            {
                return (Color)base.GetValue(ProgressDialog.WindowColor2Property);
            }
            set
            {
                base.SetValue(ProgressDialog.WindowColor2Property, value);
                this.SetColors();
            }
        }
        public Color WindowColor3
        {
            get
            {
                return (Color)base.GetValue(ProgressDialog.WindowColor3Property);
            }
            set
            {
                base.SetValue(ProgressDialog.WindowColor3Property, value);
                this.SetColors();
            }
        }
        public Color AnimationColor1
        {
            get
            {
                return (Color)base.GetValue(ProgressDialog.AnimationColor1Property);
            }
            set
            {
                base.SetValue(ProgressDialog.AnimationColor1Property, value);
                this.SetColors();
            }
        }
        public Color AnimationColor2
        {
            get
            {
                return (Color)base.GetValue(ProgressDialog.AnimationColor2Property);
            }
            set
            {
                base.SetValue(ProgressDialog.AnimationColor2Property, value);
                this.SetColors();
            }
        }
        public new Brush BorderBrush
        {
            get
            {
                return (Brush)base.GetValue(ProgressDialog.BorderBrushProperty);
            }
            set
            {
                base.SetValue(ProgressDialog.BorderBrushProperty, value);
                this.SetColors();
            }
        }

        public ProgressDialog(string message) : this()
		{
			this.Message = message;
		}

        public ProgressDialog()
        {
            InitializeComponent();

            this.IsSTAThread = false;

            this.worker = new BackgroundWorker();
            this.worker.WorkerReportsProgress = true;
            this.worker.WorkerSupportsCancellation = true;
            this.worker.DoWork += new DoWorkEventHandler(this.worker_DoWork);
            this.worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.worker_RunWorkerCompleted);
        }

        public bool RunWorkerThread(Action actionValue)
        {
            this.uiCulture = System.Globalization.CultureInfo.CurrentUICulture;
            this.uxAnimationControl.IsAnimated = true;
            this.action = actionValue;
            this.worker.RunWorkerAsync();
            return base.ShowDialog() ?? false;
        }
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = this.uiCulture;
            if (this.IsSTAThread)
            {
                System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(this.th_worker));
                thread.TrySetApartmentState(System.Threading.ApartmentState.STA);
                thread.IsBackground = true;
                thread.Start();
                thread.Join();
                return;
            }
            this.action();
        }
        private void th_worker()
        {
            this.action();
        }
        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!base.Dispatcher.CheckAccess())
            {
                RunWorkerCompletedEventHandler method = new RunWorkerCompletedEventHandler(this.worker_RunWorkerCompleted);
                base.Dispatcher.Invoke(DispatcherPriority.SystemIdle, method, new object[]
				{
					sender,
					e
				}, null);
                return;
            }
            this.uxAnimationControl.IsAnimated = false;
            if (e.Error != null)
            {
                this.error = e.Error;
            }
            else
            {
                if (!e.Cancelled)
                {
                    this.result = e.Result;
                }
            }
            base.DialogResult = new bool?(this.error == null && !e.Cancelled);
        }
        private static void OnWindowColor1PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }
        private static void OnWindowColor2PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }
        private static void OnWindowColor3PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }
        private static void OnAnimationColor1PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }
        private static void OnAnimationColor2PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }
        private static void OnBorderBrushPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }
        private void SetColors()
        {
            this.WindowGradient1.Color = this.WindowColor1;
            this.WindowGradient2.Color = this.WindowColor2;
            this.WindowGradient3.Color = this.WindowColor3;
            this.uxAnimationControl.Color1 = this.AnimationColor1;
            this.uxAnimationControl.Color2 = this.AnimationColor2;
            this.ProgressBorder.BorderBrush = this.BorderBrush;
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.SetColors();
        }
    }
}
