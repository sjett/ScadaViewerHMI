// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Services;
using HMI.Modules.ControlTree.Services;
namespace HMI.Modules.ControlTree
{
	public class ControlTreeViewModel : IControlTreeViewModel
	{
		private IControlTreeService _controlTreeService;
        private IControlTreeItemMapperService _controlTreeItemMapperService;
        private IErrorHandlingService _errorHandlingService;
        private ControlMappings _controlMappings;
        private List<string> _mappingErrors = new List<string>();
        private ControlTreeItem _root;

		public ControlTreeItem Root
		{
			get
			{
				return _root;
			}
			set
			{
                _root = value;
			}
		}
		public ControlTreeViewModel(IControlTreeService controlTreeService, IControlTreeItemMapperService controlTreeItemMapperService, IErrorHandlingService errorHandlingService)
		{
			if (errorHandlingService == null)
			{
				throw new ArgumentNullException("errorHandlingService", "errorHandlingService is null.");
			}
			if (controlTreeItemMapperService == null)
			{
				throw new ArgumentNullException("controlTreeItemMapperService", "controlTreeItemMapperService is null.");
			}
			if (controlTreeService == null)
			{
				throw new ArgumentNullException("controlTreeService", "controlTreeService is null.");
			}
            _controlTreeService = controlTreeService;
            _controlTreeItemMapperService = controlTreeItemMapperService;
            _errorHandlingService = errorHandlingService;

			try
			{
				LoadControlMappings();
			}
			catch (Exception ex)
			{
				errorHandlingService.HandleError("Error loading control mappings: {0}", new object[]
				{
					ex.Message
				});
			}
            LoadControlTree();
		}
        private void LoadControlMappings()
        {
            _controlMappings = _controlTreeService.GetControlMappings();
        }
        private void LoadControlTree()
		{
			ControlCategory? controlCategory = GetRootCategory();
			if (controlCategory == null)
			{
                _root = null;
				return;
			}
			if (!TryMapRootCategory(controlCategory))
			{
				return;
			}
            _mappingErrors.Clear();
			AddMappedControlsToTree(Root);
			if (_mappingErrors.Any())
			{
                _errorHandlingService.HandleError("Some errors were encountered when loading the control library: {0}", new object[]
				{
					string.Join(Environment.NewLine, _mappingErrors.ToArray())
				});
			}
		}
        private ControlCategory? GetRootCategory()
        {
            ControlCategory? result = null;
            try
            {
                result = _controlTreeService.GetRootCategory();
            }
            catch (Exception ex)
            {
                _errorHandlingService.HandleError("Error loading root category: {0}", new object[]
                {
                    ex.Message
                });
            }
            return result;
        }
        private bool TryMapRootCategory(ControlCategory rootCategory)
        {
            bool result;
            try
            {
                _root = _controlTreeItemMapperService.ConvertFromControlCategory(rootCategory);
                result = true;
            }
            catch (Exception ex)
            {
                _errorHandlingService.HandleError("Error loading categories: {0}", new object[]
                {
                    ex.Message
                });
                result = false;
            }
            return result;
        }
        private void AddMappedControlsToTree(ControlTreeItem categoryTreeItem)
        {
            IEnumerable<ControlCategoryMapping> controlsInCategory = _controlMappings.GetControlsInCategory(categoryTreeItem.Name);
            foreach (ControlCategoryMapping current in controlsInCategory)
            {
                try
                {
                    ControlTreeControlItem item = _controlTreeItemMapperService.ConvertFromControlCategoryMapping(current);
                    categoryTreeItem.Children.Add(item);
                }
                catch (Exception ex)
                {
                    string mappingError = string.Format("{0}/{1} ({2})", current.CategoryName, current.ControlName, ex.Message);
                    _mappingErrors.Add(mappingError);
                }
            }
            IEnumerable<ControlTreeControlItem> mappedControls = categoryTreeItem.Children.OfType<ControlTreeControlItem>();
            IEnumerable<ControlTreeItem> childCategories = categoryTreeItem.Children.Except(mappedControls.Cast<ControlTreeItem>());
            foreach (ControlTreeItem childCategory in childCategories)
            {
                AddMappedControlsToTree(childCategory);
            }
        }
    }
}
