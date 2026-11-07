// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.IO;
using HMI.Infrastructure.Services;

namespace HMI.Modules.ControlTree.Services
{
	public class ControlTreeService : IControlTreeService
	{
		private readonly IResourceProviderService _resourceProviderService;
		private readonly ISerializationService _serializationService;
		public ControlTreeService(IResourceProviderService resourceProviderService, ISerializationService serializationService)
		{
			if (resourceProviderService == null)
			{
				throw new ArgumentNullException("resourceProviderService", "resourceProviderService is null.");
			}
			if (serializationService == null)
			{
				throw new ArgumentNullException("serializationService", "serializationService is null.");
			}
            _resourceProviderService = resourceProviderService;
            _serializationService = serializationService;
		}
		public ControlMappings GetControlMappings()
		{
			ControlMappings result;
			using (TextReader resourceReader = _resourceProviderService.GetResourceReader(Path.Combine("Mappings", "ControlMappings.xml")))
			{
				string xml = resourceReader.ReadToEnd();
				ControlMappings controlMappings = _serializationService.Deserialize<ControlMappings>(xml);
				result = controlMappings;
			}
			return result;
		}
		public ControlCategory GetRootCategory()
		{
			ControlCategory result;
			using (TextReader resourceReader = _resourceProviderService.GetResourceReader(Path.Combine("Mappings", "ControlCategories.xml")))
			{
				string xml = resourceReader.ReadToEnd();
				ControlCategory controlCategory = _serializationService.Deserialize<ControlCategory>(xml);
				result = controlCategory;
			}
			return result;
		}
	}
}
