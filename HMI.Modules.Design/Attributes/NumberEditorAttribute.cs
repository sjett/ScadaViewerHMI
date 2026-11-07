// SPDX-License-Identifier: GPL-3.0-only
using HMI.Domain.Models;
using HMI.Infrastructure.Converters;
using HMI.Modules.Design.Views;
using System.Windows.Data;

namespace HMI.Modules.Design.Attributes
{
    /// <summary>
    /// 数字编辑器属性
    /// </summary>
    public class NumberEditorAttribute : PropertyEditorAttribute
    {
        private readonly bool _isInteger;

        public int Precision
        {
            get;
            set;
        }
        public NumberEditorAttribute(bool isInteger)
        {
            _isInteger = isInteger;
            Precision = 2;
        }
        protected override IEditorView BuildEditor(IProperty property, IBindingSource editedObject)
        {
            return new NumberEditorView
            {
                IsInteger = _isInteger,
                Precision = Precision
            };
        }
        protected override Binding CreateValueBinding(IBindingSource bindingSource, IProperty property)
        {
            Binding binding = base.CreateValueBinding(bindingSource, property);
            if (_isInteger)
            {
                binding.Converter = new IntToDoubleConverter();
            }
            return binding;
        }
        public override bool IsMultiSelectionCompatibleWith(IProperty property, IProperty otherProperty)
        {
            NumberEditorAttribute numberEditorAttribute = otherProperty.PropertyEditorBuilder as NumberEditorAttribute;
            return numberEditorAttribute != null && numberEditorAttribute._isInteger == _isInteger;
        }
    }
}
