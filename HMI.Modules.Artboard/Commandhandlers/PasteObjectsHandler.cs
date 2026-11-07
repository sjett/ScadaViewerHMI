// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Collections.Generic;
using System.Windows.Input;
using HMI.Infrastructure.Commanding;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
namespace HMI.Modules.Artboard.Commandhandlers
{
	public class PasteObjectsHandler : WorkflowCommandHandler
	{
		private readonly Lazy<IProjectManagerViewModel> _projectManager;
		private readonly Lazy<IClipboardManagerService> _clipboardManager;
		protected override ICommand Command
		{
			get
			{
				return ApplicationCommands.Paste;
			}
		}
		public PasteObjectsHandler(IServiceProvider serviceLocator) : base(serviceLocator)
		{
            _projectManager = new Lazy<IProjectManagerViewModel>(serviceLocator.Resolve<Func<IProjectManagerViewModel>>());
            _clipboardManager = new Lazy<IClipboardManagerService>(serviceLocator.Resolve<Func<IClipboardManagerService>>());
		}
		protected override bool CanExecute(object state)
		{
			return _projectManager.Value.ActivePage != null && _clipboardManager.Value.HasData();
		}
		protected override void SetActions(ICollection<string> actionNames)
		{
			actionNames.Add("BeginUndoGroup");
			actionNames.Add("Paste");
			actionNames.Add("EndUndoGroup");
		}
	}
}
