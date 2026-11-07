// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.Modules.Explorer.ViewModels
{
	public enum RenameStatus
	{
		Success,
		NoChange,
		NullNode,
		InvalidName,
		NameExists
	}
}
