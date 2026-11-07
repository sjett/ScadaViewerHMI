// SPDX-License-Identifier: GPL-3.0-only
using HMI.Modules.Design.Attributes;
using System.ComponentModel;

namespace HMI.Infrastructure.Presentation.ViewModels
{
    public class SignalParameterDefinition : ParameterDefinition, ICollectionItem, IHierarchicalNode, INotifyPropertyChanged, IParameterDefinition, ISignalParameterDefinition
	{
		public SignalParameterDefinition(IServiceProvider container, IExplorerNodeViewModel parentNode) : base(container, parentNode)
		{
		}
		public override ICollectionItem CreateClone()
		{
			return new SignalParameterDefinition(Services, Parent);
		}
		public override ICollection<Attribute> GetAttributes(string propertyName)
		{
			return GetPropertyAttributes(propertyName).ToList();
		}
        private IEnumerable<Attribute> GetPropertyAttributes(string propertyName)
        {
            if(propertyName != "DefaultValue")
                yield break;

            yield return new SignalBrowserAttribute();
        }

	}
}
