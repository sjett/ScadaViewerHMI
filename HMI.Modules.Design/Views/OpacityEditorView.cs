// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;
using HMI.Common.Presentation.Views;
using HMI.Infrastructure.Controls;
using HMI.Modules.Design.Controls;

namespace HMI.Modules.Design.Views
{
    [TemplatePart(Name = "uxEditor", Type = typeof(SliderBox))]
    public class OpacityEditorView : EditorView
    {
        private const string EditorTemplatePartName = "uxEditor";
        private SliderBox _editorSlider;
        static OpacityEditorView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(OpacityEditorView), new FrameworkPropertyMetadata(typeof(OpacityEditorView)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _editorSlider = this.FindTemplateChild<SliderBox>(EditorTemplatePartName);
            if (_editorSlider != null)
            {
                _editorSlider.SetBinding(SliderBox.ValueProperty, ValueBinding);
            }
        }
    }
}
