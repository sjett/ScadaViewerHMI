// SPDX-License-Identifier: GPL-3.0-only
using HMI.Domain.Services;
using System.ComponentModel;
namespace HMI.Modules.PropertyEditor.Services
{
	public class ReflectionBasedPropertyMetadataProvider : PropertyDescriptorMetadataProvider, IExplicitPropertyMetadataProvider
	{
		public ReflectionBasedPropertyMetadataProvider(PropertyDescriptor descriptor) : base(descriptor)
		{
		}
		public ICollection<Attribute> GetAttributes(string propertyName)
		{
            ValidatePropertyName(propertyName);
			return Descriptor.Attributes.OfType<Attribute>().ToList();
		}
	}
}
