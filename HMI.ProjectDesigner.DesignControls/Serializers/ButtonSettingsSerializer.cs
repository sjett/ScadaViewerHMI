// SPDX-License-Identifier: GPL-3.0-only
using System;
using HMI.Infrastructure.Presentation.Factories;
namespace HMI.Infrastructure.Serialization
{
	public abstract class ButtonSettingsSerializer : ItemSecuritySettingSerializer
	{
		public ButtonSettingsSerializer(IServiceProvider container, ISerializerFactory serializerFactory) : base(container, serializerFactory)
		{
		}
	}
}
