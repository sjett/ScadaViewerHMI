// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Runtime.CompilerServices;
using HMI.Common.Browsers.ViewModels;
using HMI.Common.Presentation.ViewModels;
using HMI.Infrastructure.Presentation.ViewModels;
namespace HMI.Modules.Design.ViewModels.Browsers
{
	public class TemplateParameterDefinitionViewModel : ViewModel, IBrowserItemViewModel, ITemplateParameterViewModel
	{
		
		public string Name
		{
			get;
			private set;
		}
		public string DefaultValue
		{
			get;
			private set;
		}
		public string ItemText
		{
			get
			{
				return "[PC:" + this.Name + "]";
			}
		}
		public TemplateParameterDefinitionViewModel(IParameterDefinition parameterDefinition)
		{
			this.Name = parameterDefinition.Name;
			this.DefaultValue = parameterDefinition.DefaultValue;
		}
	}
}
