// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.ViewModels;

namespace HMI.Infrastructure.Presentation.Services
{
    /// <summary>
    /// ZOrder服务
    /// </summary>
	public class ZOrderService : IZOrderService
	{
		public void BringForward(IProjectPageViewModel page, IEnumerable<ICommonControlViewModel> selectedControls)
		{
			List<ICommonControlViewModel> list_ = GetControlsByDescendingZIndex(page);

			List<ICommonControlViewModel> selectedControlsByZIndex = selectedControls.OrderByDescending(t=> t.ZIndex).ToList();
			MoveSelectedControlsForward(list_, selectedControlsByZIndex);
            ResetZIndexDescending(list_);
		}
        List<ICommonControlViewModel> GetControlsByDescendingZIndex(IProjectPageViewModel page)
        {
            IEnumerable<ICommonControlViewModel> pageControls = page.Children.Controls();

            List<ICommonControlViewModel> list = pageControls.OrderByDescending(t => t.ZIndex).ToList<ICommonControlViewModel>();
            list.Insert(0, null);
            return list;
        }
        void MoveSelectedControlsForward(List<ICommonControlViewModel> allControls, List<ICommonControlViewModel> bringForwardControls)
        {
            int num = 1;
            while (num < allControls.Count && bringForwardControls.Count > 0)
            {
                ICommonControlViewModel commonControlViewModel = allControls[num];
                if (commonControlViewModel == bringForwardControls[0])
                {
                    bringForwardControls.RemoveAt(0);
                    allControls[num] = allControls[num - 1];
                    allControls[num - 1] = commonControlViewModel;
                }
                num++;
            }
        }

        public void SendBackward(IProjectPageViewModel page, IEnumerable<ICommonControlViewModel> selectedControls)
		{
			List<ICommonControlViewModel> list = GetControlsByAscendingZIndex(page);

			List<ICommonControlViewModel> list_ = selectedControls.OrderBy(t=> t.ZIndex).ToList();
			MoveSelectedControlsBackward(list_, list);
            ResetZIndexAscending(list);
		}
       
        List<ICommonControlViewModel> GetControlsByAscendingZIndex(IProjectPageViewModel page)
        {
            IEnumerable<ICommonControlViewModel> pageControls = page.Children.Controls();

            List<ICommonControlViewModel> list = pageControls.OrderBy(t=> t.ZIndex).ToList<ICommonControlViewModel>();
            list.Insert(0, null);
            return list;
        }
  
        void MoveSelectedControlsBackward(List<ICommonControlViewModel> controls, List<ICommonControlViewModel> orderedControls)
        {
            int num = 1;
            while (num < orderedControls.Count && controls.Count > 0)
            {
                ICommonControlViewModel commonControlViewModel = orderedControls[num];
                if (commonControlViewModel == controls[0])
                {
                    controls.RemoveAt(0);
                    orderedControls[num] = orderedControls[num - 1];
                    orderedControls[num - 1] = commonControlViewModel;
                }
                num++;
            }
        }
        public void SendToBack(IProjectPageViewModel page, IEnumerable<ICommonControlViewModel> selectedControls)
		{
			List<ICommonControlViewModel> list = GetControlsByAscendingZIndex(page);
			list.RemoveAt(0);

			List<ICommonControlViewModel> selectedControlsByZIndex = selectedControls.OrderBy(t=> t.ZIndex).ToList();
			List<ICommonControlViewModel> list_ = selectedControlsByZIndex.Concat(list.Except(selectedControlsByZIndex)).ToList();
            ResetZIndexAscending(list_);
		}
		public void BringToFront(IProjectPageViewModel page, IEnumerable<ICommonControlViewModel> selectedControls)
		{
			List<ICommonControlViewModel> list = GetControlsByDescendingZIndex(page);
			list.RemoveAt(0);

			List<ICommonControlViewModel> selectedControlsByZIndex = selectedControls.OrderByDescending(t=> t.ZIndex).ToList();
			List<ICommonControlViewModel> list_ = selectedControlsByZIndex.Concat(list.Except(selectedControlsByZIndex)).ToList();
            ResetZIndexDescending(list_);
		}

		private void ResetZIndexAscending(List<ICommonControlViewModel> controls)
		{
			int num = 0;
			for (int i = 0; i < controls.Count; i++)
			{
				if (controls[i] != null)
				{
                    IEnumerable<ICommonControlViewModel> childControls = EnumerateDescendantControls(controls[i].Children.Controls());

					List<ICommonControlViewModel> list = childControls.OrderBy(t=> t.ZIndex).ToList();
					foreach (ICommonControlViewModel current in list)
					{
						current.ZIndex = num++;
					}
					controls[i].ZIndex = num++;
				}
			}
		}
		private void ResetZIndexDescending(List<ICommonControlViewModel> controls)
		{
			int num = 0;
			for (int i = controls.Count - 1; i >= 0; i--)
			{
				if (controls[i] != null)
				{
                    IEnumerable<ICommonControlViewModel> childControls = EnumerateDescendantControls( controls[i].Children.Controls());

					List<ICommonControlViewModel> list = childControls.OrderBy(t=> t.ZIndex).ToList();
					foreach (ICommonControlViewModel current in list)
					{
						current.ZIndex = num++;
					}
					controls[i].ZIndex = num++;
				}
			}
		}
        // 递归获取所有子控件
        IEnumerable<ICommonControlViewModel> EnumerateDescendantControls( IEnumerable<ICommonControlViewModel> controls)
        {
            foreach (ICommonControlViewModel control in controls)
            {
                yield return control;

                foreach (ICommonControlViewModel child in EnumerateDescendantControls(control.Children.OfType<ICommonControlViewModel>()))
                {
                    yield return child;
                }
            }
        }

	}
}
