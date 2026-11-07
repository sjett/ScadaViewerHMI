// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Presentation.ViewModels;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using System.ComponentModel;
using HMI.Common.Instrumentation.ErrorHandling;
namespace HMI.Modules.Explorer.ViewModels
{
	public class ExplorerViewModel : ViewModel, IExplorerViewModel, INotifyPropertyChanged
	{
		private readonly IProjectNameService _projectNameService;
		
		public IProjectManagerViewModel ProjectManager
		{
			get;
			private set;
		}
		public ExplorerViewModel(IProjectManagerViewModel projectManagerViewModel, IProjectNameService projectNameService)
		{
			Contract.Requires<ArgumentNullException>(projectManagerViewModel != null);
			Contract.Requires<ArgumentNullException>(projectNameService != null);

            ProjectManager = projectManagerViewModel;
            _projectNameService = projectNameService;
		}
		public RenameStatus ValidateRename(IExplorerNodeViewModel node, string oldName, string newName)
		{
			if (node == null)
			{
				return RenameStatus.NullNode;
			}
			if (oldName == newName)
			{
				return RenameStatus.NoChange;
			}
			if (!_projectNameService.IsValidName(newName))
			{
				return RenameStatus.InvalidName;
			}
			if (_projectNameService.IsNameInUse(newName, node))
			{
				return RenameStatus.NameExists;
			}
			return RenameStatus.Success;
		}
	}
}
