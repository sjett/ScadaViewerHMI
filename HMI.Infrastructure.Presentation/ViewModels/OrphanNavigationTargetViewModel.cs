// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Infrastructure.Presentation.ViewModels
{
    public class OrphanNavigationTargetViewModel : CommonControlViewModel,  IExplorerNodeViewModel, INavigationTargetViewModel
	{
        public override bool IsEditable
		{
			get
			{
				return false;
			}
		}
        public bool IsInvalidTarget
		{
			get
			{
				return true;
			}
		}
        /// <param name="container"></param>
        /// <param name="parentValue"></param>
        /// <param name="controlType"></param>
        public OrphanNavigationTargetViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, Type controlType) : base(container, parentValue, controlType)
		{
            ID = "OrphanSubPages";
		}
		public override string ToString()
		{
			return ID ?? string.Empty;
		}
	}
}
