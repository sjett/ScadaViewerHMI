// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Events;
using HMI.Infrastructure.Presentation.Resources;
namespace HMI.Infrastructure.Presentation.Services
{
    /// <summary>
    /// 页面撤销重做服务
    /// </summary>
    public class PageUndoRedoService : IPageUndoRedoService
	{
		private bool isInUndoRedo;
		private IPageSelectionService pageSelectionService;
		public IEventAggregator EventAggregator
		{
			get;
			private set;
		}
		public IPageSelectionService PageSelectionService
		{
			get
			{
				if (pageSelectionService == null)
				{
					throw new InvalidOperationException(ErrorTexts.ServiceMustBeAttachedToPageSelectionService);
				}
				return pageSelectionService;
			}
			set
			{
                pageSelectionService = value;
			}
		}
		public bool AutoSelectPageAfterUndoRedo
		{
			get;
			set;
		}
		public PageUndoRedoService(IEventAggregator eventAggregator)
		{
            EventAggregator = eventAggregator;
		}
		public void AttachToPageSelectionService(IPageSelectionService pageSelectionService)
		{
            PageSelectionService = pageSelectionService;
            SubscribeToUndoingEvent();
            SubscribeToRedoingEvent();
            SubscribeToUndoEvent();
            SubscribeToRedoEvent();
		}
		private void SubscribeToRedoEvent()
		{
			RedoEvent @event = EventAggregator.GetEvent<RedoEvent>();
			if (@event != null)
			{
				@event.Subscribe(new Action<object>(OnUndoRedo), ThreadOption.UIThread);
			}
		}
		private void SubscribeToUndoEvent()
		{
			UndoEvent @event = EventAggregator.GetEvent<UndoEvent>();
			if (@event != null)
			{
				@event.Subscribe(new Action<object>(OnUndoRedo), ThreadOption.UIThread);
			}
		}
		private void SubscribeToRedoingEvent()
		{
			RedoingEvent @event = EventAggregator.GetEvent<RedoingEvent>();
			if (@event != null)
			{
				@event.Subscribe(new Action<object>(OnRedoing), ThreadOption.UIThread);
			}
		}
		private void SubscribeToUndoingEvent()
		{
			UndoingEvent @event = EventAggregator.GetEvent<UndoingEvent>();
			if (@event != null)
			{
				@event.Subscribe(new Action<object>(OnUndoing), ThreadOption.UIThread);
			}
		}
		private void OnUndoing(object obj)
		{
            PrepareUndoRedoSelection();
		}
		private void PrepareUndoRedoSelection()
		{
            isInUndoRedo = true;
            AutoSelectPageAfterUndoRedo = false;
		}
		private void OnRedoing(object obj)
		{
            PrepareUndoRedoSelection();
		}
		private void OnUndoRedo(object obj)
		{
			if (isInUndoRedo && AutoSelectPageAfterUndoRedo)
			{
                PageSelectionService.DeselectAllChildrenAndSelectPage();
			}
            ResetUndoRedoAutoSelection();
		}
		private void ResetUndoRedoAutoSelection()
		{
            isInUndoRedo = false;
            AutoSelectPageAfterUndoRedo = false;
		}
	}
}
