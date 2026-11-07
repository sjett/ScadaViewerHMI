// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Collections;
using HMI.Infrastructure.Presentation.Resources;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Common.Instrumentation.ErrorHandling;
namespace HMI.Infrastructure.Presentation.Services
{
    /// <summary>
    /// 管理设计页面及其控件的选择状态。
    /// </summary>
    public class PageSelectionService : IPageSelectionService
	{
		readonly IServiceProvider services;
		private bool isUpdatingSelection;
		ISelectionControlViewModel selectionControlViewModel;
		IProjectPageViewModel page;
		
		public IProjectPageViewModel Page
		{
			get
			{
				if (page == null)
				{
					throw new InvalidOperationException(ErrorTexts.ServiceMustBeAttachedToPage);
				}
				return page;
			}
			private set
			{
                page = value;
			}
		}
		public bool HasMultipleSelection
		{
			get
			{
				return selectionControlViewModel != null;
			}
		}
		public PageSelectionService(IServiceProvider container)
		{
			Contract.Requires<ArgumentNullException>(container != null, "container");
            services = container;
		}
		public void Attach(IProjectPageViewModel page)
		{
			Contract.Requires<ArgumentNullException>(page != null, "page");
            this.page = page;
		}
		public ICommonControlViewModel CreateGroupFromSelection()
		{
			ISelectionControlViewModel selectionViewModel = GetSelectionViewModel();
			return selectionViewModel.CreateGroupFromSelection();
		}
		public void SelectItems(ICollection<ICommonControlViewModel> selectedItems)
		{
			if (selectedItems.Count == 0)
			{
				return;
			}
			selectedItems.First().Select();
			foreach (ICommonControlViewModel current in selectedItems.Skip(1))
			{
                page.AddToSelection(current);
			}
		}
		public void DeselectAllChildrenAndSelectPage()
		{
            SelectSingleChild(null);
            Page.IsSelected = true;
		}
		public void SelectSingleChild(ICommonControlViewModel child)
		{
            isUpdatingSelection = true;
			if (child != null)
			{
				child.IsSelected = true;
			}
			ClearDescendantSelections(Page, child);
            isUpdatingSelection = false;
            UpdateContainerSelectionStatus();
		}
        void ClearDescendantSelections(IExplorerNodeViewModel page, ICommonControlViewModel child)
        {
            if (page.Children == null)
            {
                return;
            }
            foreach (IExplorerNodeViewModel current in page.Children)
            {
                ClearSelectionExcept(current, child);
            }
        }
        void ClearSelectionExcept(IExplorerNodeViewModel child, ICommonControlViewModel selectedChild)
        {
            if (child != selectedChild)
            {
                child.IsSelected = false;
            }
            ClearDescendantSelections(child, selectedChild);
        }
        public virtual void UpdateContainerSelectionStatus()
		{
			if (isUpdatingSelection)
			{
				return;
			}
            UpdateSelectionViewModel();
			if (selectionControlViewModel == null)
			{
				UpdateSelectedControlDecorators();
			}
		}
        void UpdateSelectedControlDecorators()
        {
            ICommonControlViewModel commonControlViewModel = GetSelectedControls().SingleOrDefault();
            if (commonControlViewModel != null)
            {
                commonControlViewModel.UpdateDecorators();
            }
        }
        ICollection<ICommonControlViewModel> GetSelectedControls()
        {
            IEnumerable<IExplorerNodeViewModel> pageChildren = Page.Children.ToList();
            return pageChildren.Where(child => child.IsSelected).OfType<ICommonControlViewModel>().ToList();
        }
        public void RemoveSelectionViewModel()
		{
			if (selectionControlViewModel == null)
			{
				return;
			}
            Page.LayoutChildren.RemoveNonGroupItem(selectionControlViewModel);
            selectionControlViewModel.Dispose();
            selectionControlViewModel = null;
		}
		public void AddControlToSelection(ICommonControlViewModel control)
		{
            isUpdatingSelection = true;
			control.IsSelected = true;
            isUpdatingSelection = false;
            UpdateContainerSelectionStatus();
		}
		public void RemoveControlFromSelection(ICommonControlViewModel node)
		{
            isUpdatingSelection = true;
			node.IsSelected = false;
            isUpdatingSelection = false;
            UpdateContainerSelectionStatus();
		}
		public void ToggleItemSelection(ICommonControlViewModel item, bool isControlPressed)
		{
            isUpdatingSelection = true;
			if (!isControlPressed)
			{
				item.IsSelected = true;
			}
			else
			{
				item.IsSelected = !item.IsSelected;
			}
            isUpdatingSelection = false;
            UpdateContainerSelectionStatus();
		}
		public virtual ISelectionControlViewModel GetSelectionViewModel()
		{
			if (selectionControlViewModel != null)
			{
				return selectionControlViewModel;
			}
            selectionControlViewModel = ResolveSelectionControlViewModel();
            page.LayoutChildren.AddNonGroupItem(selectionControlViewModel);
            selectionControlViewModel.XTranslation = 100.0;
            selectionControlViewModel.YTranslation = 100.0;
            selectionControlViewModel.Width = 200.0;
            selectionControlViewModel.Height = 200.0;
            selectionControlViewModel.ControlWidth = 200.0;
            selectionControlViewModel.ControlHeight = 200.0;
            selectionControlViewModel.ZIndex = 2147483647;
			return selectionControlViewModel;
		}
        ISelectionControlViewModel ResolveSelectionControlViewModel()
        {
            return ActivatorUtilities.CreateInstance<SelectionControlViewModel>(services, Page);
        }
        private void UpdateSelectionViewModel()
		{
			ICollection<ICommonControlViewModel> collection = GetSelectedControls();
			if (collection.Count() > 1)

            {
				UpdateSelectionControl(collection);
				return;
			}
            RemoveSelectionViewModel();
		}

        void UpdateSelectionControl(ICollection<ICommonControlViewModel> selectedControls)
        {
            GetSelectionViewModel();
            AddSelectedControlsToSelectionControl(selectedControls);
            RemoveUnselectedControlsFromSelectionControl(selectedControls);
            selectionControlViewModel.RequestFocus();
        }
        void AddSelectedControlsToSelectionControl(IEnumerable<ICommonControlViewModel> selectedControls)
        {
            foreach (ICommonControlViewModel control in selectedControls)
            {
                AddControlToSelectionControl(control);
            }
        }
        void AddControlToSelectionControl(ICommonControlViewModel control)
        {
            if (!selectionControlViewModel.Children.Contains(control))
            {
                selectionControlViewModel.Children.Add(control);
            }
        }
        void RemoveUnselectedControlsFromSelectionControl(IEnumerable<ICommonControlViewModel> selectedControls)
        {
            IExplorerNodeViewModel[] items = selectionControlViewModel.Children.Except(selectedControls).ToArray<IExplorerNodeViewModel>();
            selectionControlViewModel.Children.RemoveRange(items);
        }

	}
}
