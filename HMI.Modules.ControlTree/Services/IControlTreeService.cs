// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.Modules.ControlTree.Services
{
	public interface IControlTreeService
	{
		ControlMappings GetControlMappings();
		ControlCategory GetRootCategory();
	}
}
