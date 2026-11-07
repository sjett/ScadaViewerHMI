// SPDX-License-Identifier: GPL-3.0-only
using System;
using HMI.Infrastructure.Commanding;
using HMI.Infrastructure.Presentation;
using HMI.Modules.Artboard.Actions;
using HMI.Modules.Artboard.Commandhandlers;
using HMI.Modules.Artboard.Services;
namespace HMI.Modules.Artboard
{
	public class ArtboardModule : PluginModule
	{
		protected override void RegisterServices(IServiceCollection containerRegistry)
		{
			base.RegisterServices(containerRegistry);

			containerRegistry.AddTransient<IControlLayoutService, ControlLayoutService>();
			containerRegistry.AddKeyedTransient<ICommandHandler, AlignLeftHandler>("AlignSelectionLeft");
			containerRegistry.AddKeyedTransient<ICommandHandler, AlignCenterHandler>("AlignSelectionCenterHandler");
			containerRegistry.AddKeyedTransient<ICommandHandler, AlignRightHandler>("AlignSelectionRightHandler");
			containerRegistry.AddKeyedTransient<ICommandHandler, AlignTopHandler>("AlignSelectionTopHandler");
			containerRegistry.AddKeyedTransient<ICommandHandler, AlignMiddleHandler>("AlignSelectionMiddleHandler");
			containerRegistry.AddKeyedTransient<ICommandHandler, AlignBottomHandler>("AlignSelectionBottomHandler");
			containerRegistry.AddKeyedTransient<ICommandHandler, BringToFrontHandler>("BringToFrontHandler");
			containerRegistry.AddKeyedTransient<ICommandHandler, BringForwardHandler>("BringForwardHandler");
			containerRegistry.AddKeyedTransient<ICommandHandler, SendToBackHandler>("SendToBackHandler");
			containerRegistry.AddKeyedTransient<ICommandHandler, SendBackwardHandler>("SendBackwardHandler");
			containerRegistry.AddKeyedTransient<ICommandHandler, DistributeHorizontallyHandler>("DistributeHorizontallyHandler");
			containerRegistry.AddKeyedTransient<ICommandHandler, DistributeVerticallyHandler>("DistributeVerticallyHandler");
			containerRegistry.AddKeyedTransient<ICommandHandler, MakeSameWidthHandler>("MakeSameWidthHandler");
			containerRegistry.AddKeyedTransient<ICommandHandler, MakeSameHeightHandler>("MakeSameHeightHandler");
			containerRegistry.AddKeyedTransient<ICommandHandler, GroupHandler>("GroupHandler");
			containerRegistry.AddKeyedTransient<ICommandHandler, UngroupHandler>("UngroupHandler");
			containerRegistry.AddKeyedTransient<ICommandHandler, CutObjectsHandler>("CutObjectsHandler");
			containerRegistry.AddKeyedTransient<ICommandHandler, CopyObjectsHandler>("CopyObjectsHandler");
			containerRegistry.AddKeyedTransient<ICommandHandler, PasteObjectsHandler>("PasteObjectsHandler");
			containerRegistry.AddKeyedTransient<ICommandHandler, DeleteObjectsHandler>("DeleteObjectsHandler");
			
			containerRegistry.AddKeyedTransient<IWorkflowAction, AlignLeftAction>("AlignLeft");
			containerRegistry.AddKeyedTransient<IWorkflowAction, AlignCenterAction>("AlignCenter");
			containerRegistry.AddKeyedTransient<IWorkflowAction, AlignRightAction>("AlignRight");
			containerRegistry.AddKeyedTransient<IWorkflowAction, AlignTopAction>("AlignTop");
			containerRegistry.AddKeyedTransient<IWorkflowAction, AlignMiddleAction>("AlignMiddle");
			containerRegistry.AddKeyedTransient<IWorkflowAction, AlignBottomAction>("AlignBottom");
			containerRegistry.AddKeyedTransient<IWorkflowAction, DistributeHorizontallyAction>("DistributeHorizontally");
			containerRegistry.AddKeyedTransient<IWorkflowAction, DistributeVerticallyAction>("DistributeVertically");
			containerRegistry.AddKeyedTransient<IWorkflowAction, BringToFrontAction>("BringToFront");
			containerRegistry.AddKeyedTransient<IWorkflowAction, BringForwardAction>("BringForward");
			containerRegistry.AddKeyedTransient<IWorkflowAction, SendToBackAction>("SendToBack");
			containerRegistry.AddKeyedTransient<IWorkflowAction, SendBackwardAction>("SendBackward");
			containerRegistry.AddKeyedTransient<IWorkflowAction, MakeSameWidthAction>("MakeSameWidth");
			containerRegistry.AddKeyedTransient<IWorkflowAction, MakeSameHeightAction>("MakeSameHeight");
			containerRegistry.AddKeyedTransient<IWorkflowAction, GroupAction>("Group");
			containerRegistry.AddKeyedTransient<IWorkflowAction, UngroupAction>("Ungroup");
			containerRegistry.AddKeyedTransient<IWorkflowAction, RecalculateSelectionBoundsAction>("RecalculateSelectionBounds");
			containerRegistry.AddKeyedTransient<IWorkflowAction, CopyAction>("Copy");
			containerRegistry.AddKeyedTransient<IWorkflowAction, PasteAction>("Paste");
			containerRegistry.AddKeyedTransient<IWorkflowAction, DeleteAction>("Delete");
		}
	}
}
