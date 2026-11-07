// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;
using Telerik.Windows.Controls;
using HMI.Common.Presentation.Views;

namespace HMI.Modules.Design.Views
{
    /// <summary>
    /// 数字编辑器视图
    /// </summary>
    public class NumberEditorView : EditorView
    {
        private const string EditorTemplatePartName = "uxEditor";
        private RadNumericUpDown _numericEditor;

        public static readonly DependencyProperty IsIntegerProperty = DependencyProperty.Register("IsInteger", typeof(bool), typeof(NumberEditorView), null);
        public static readonly DependencyProperty PrecisionProperty = DependencyProperty.Register("Precision", typeof(int), typeof(NumberEditorView), new PropertyMetadata(0));
        static NumberEditorView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumberEditorView), new FrameworkPropertyMetadata(typeof(NumberEditorView)));
        }
        public bool IsInteger
        {
            get
            {
                return (bool)GetValue(IsIntegerProperty);
            }
            set
            {
                SetValue(IsIntegerProperty, value);
            }
        }
        public int Precision
        {
            get
            {
                return (int)GetValue(PrecisionProperty);
            }
            set
            {
                SetValue(PrecisionProperty, value);
            }
        }
        
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _numericEditor = this.FindTemplateChild<RadNumericUpDown>(EditorTemplatePartName);
            if (_numericEditor != null)
            {
                _numericEditor.SetBinding(RadRangeBase.ValueProperty, ValueBinding);
            }
        }
    }
}
