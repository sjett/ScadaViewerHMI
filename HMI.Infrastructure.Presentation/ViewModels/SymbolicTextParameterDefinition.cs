// SPDX-License-Identifier: GPL-3.0-only
using HMI.Modules.Design.Attributes;
using System.ComponentModel;

namespace HMI.Infrastructure.Presentation.ViewModels
{
	public class SymbolicTextParameterDefinition : ParameterDefinition, ICollectionItem, IHierarchicalNode, INotifyPropertyChanged, IParameterDefinition, ISymbolicTextParameterDefinition
	{
		public SymbolicTextParameterDefinition(IServiceProvider container, IExplorerNodeViewModel parentNode) : base(container, parentNode)
		{
		}
		public override ICollection<Attribute> GetAttributes(string propertyName)
		{
			return GetPropertyAttributes(propertyName).ToList();
		}
        private IEnumerable<Attribute> GetPropertyAttributes(string propertyName)
        {
            if (!(propertyName == "DefaultValue"))
            {
                yield break;
            }

            yield return new SymbolicTextBrowserAttribute();
        }
        public override ICollectionItem CreateClone()
		{
			return new SymbolicTextParameterDefinition(Services, Parent);
		}

	}
}
