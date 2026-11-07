// SPDX-License-Identifier: GPL-3.0-only
using HMI.Modules.Design.Attributes;
using System.ComponentModel;
namespace HMI.Infrastructure.Presentation.ViewModels
{
	public class AlarmGroupParameterDefinition : ParameterDefinition, ICollectionItem, IHierarchicalNode, INotifyPropertyChanged, IParameterDefinition, IAlarmGroupParameterDefinition
	{
		public AlarmGroupParameterDefinition(IServiceProvider container, IExplorerNodeViewModel parentNode) : base(container, parentNode)
		{
		}
		public override ICollectionItem CreateClone()
		{
			return new AlarmGroupParameterDefinition(Services, Parent);
		}
		public override ICollection<Attribute> GetAttributes(string propertyName)
		{
		    return GetPropertyAttributes(propertyName).ToList();
		}
        private IEnumerable<Attribute> GetPropertyAttributes(string propertyName)
        {
            if(!(propertyName == "DefaultValue"))
            {
                yield break;
            }
            yield return new AlarmGroupBrowserAttribute();


        }
	}
}
