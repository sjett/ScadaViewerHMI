// SPDX-License-Identifier: GPL-3.0-only
using HMI.Domain.Services;
using HMI.Infrastructure.Resources;
using HMI.Modules.Design.Attributes;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace HMI.Modules.PropertyEditor.Services
{
	public class DefaultValuesPropertyDescriptorMetadataProvider : PropertyDescriptorMetadataProvider, IExplicitPropertyMetadataProvider
	{
		public DefaultValuesPropertyDescriptorMetadataProvider(PropertyDescriptor descriptor) : base(descriptor)
		{
		}
		public ICollection<Attribute> GetAttributes(string propertyName)
		{
            ValidatePropertyName(propertyName);

            return [
                CreateDefaultEditorAttribute(),
                new PropertyCategoryAttribute("Misc", typeof(StandardPropertyCategories)),
                new DisplayNameAttribute(propertyName),
                EditorOrderAttribute.Default
            ];
            //return this.Descriptor.Attributes.Cast<Attribute>().ToList();
		}
        private Attribute CreateDefaultEditorAttribute()
        {
            Type propertyType = this.Descriptor.PropertyType;
            return CreateEditorAttribute(propertyType);
        }
        private PropertyEditorAttribute CreateEditorAttribute(Type propertyType)
        {
            if (propertyType == typeof(double) || propertyType == typeof(float))
            {
                return new NumberEditorAttribute(false);
            }
            if (propertyType == typeof(int))
            {
                return new NumberEditorAttribute(true);
            }
            if (propertyType == typeof(HorizontalAlignment))
            {
                return new HorizontalAlignmentEditorAttribute();
            }
            if (propertyType == typeof(VerticalAlignment))
            {
                return new VerticalAlignmentEditorAttribute();
            }
            if (propertyType == typeof(Thickness))
            {
                return new ThicknessEditorAttribute();
            }
            if (propertyType == typeof(CornerRadius))
            {
                return new CornerRadiusEditorAttribute();
            }
            if (propertyType == typeof(Enum) || propertyType.IsEnum)
            {
                return new EnumEditorAttribute(propertyType);
            }
            if (propertyType == typeof(Brush) || propertyType.IsSubclassOf(typeof(Brush)))
            {
                return new BrushEditorAttribute();
            }
            if (propertyType == typeof(Color))
            {
                return new ColorEditorAttribute();
            }
            if (propertyType == typeof(bool))
            {
                return new BooleanEditorAttribute();
            }
            if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
            {
                return new DateTimeEditorAttribute();
            }
            if (!(propertyType == typeof(int?)) && !(propertyType == typeof(double?)) && !(propertyType == typeof(float?)))
            {
                return new TextEditorAttribute();
            }
            return new TextEditorAttribute
            {
                IsNullable = true
            };
        }
    }
}
