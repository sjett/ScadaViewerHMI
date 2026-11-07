// SPDX-License-Identifier: GPL-3.0-only
using HMI.Modules.PropertyEditor.Resources;
using System.ComponentModel;
using HMI.Common.Instrumentation.ErrorHandling;
using System.Globalization;

namespace HMI.Modules.PropertyEditor.Services
{
	public abstract class PropertyDescriptorMetadataProvider
	{
		protected PropertyDescriptor Descriptor
		{
			get;
			private set;
		}
		protected PropertyDescriptorMetadataProvider(PropertyDescriptor descriptor)
		{
			Contract.Requires<ArgumentNullException>(descriptor != null, "descriptor");
            Descriptor = descriptor;
		}
		protected void ValidatePropertyName(string propertyName)
		{
			if (Descriptor.Name != propertyName)
			{
				string message = string.Format(CultureInfo.CurrentCulture, ErrorTexts.PropertyNamesDoNotMatchFormat, new object[]
				{
					propertyName,
                    Descriptor.Name
				});
				throw new InvalidOperationException(message);
			}
		}
	}
}
