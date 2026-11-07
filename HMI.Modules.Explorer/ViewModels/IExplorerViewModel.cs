// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using HMI.Infrastructure.Presentation.ViewModels;

namespace HMI.Modules.Explorer.ViewModels
{
	public interface IExplorerViewModel : INotifyPropertyChanged
	{
		IProjectManagerViewModel ProjectManager
		{
			get;
		}
		RenameStatus ValidateRename(IExplorerNodeViewModel node, string oldName, string newName);
	}
}
