// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using HMI.Common.Instrumentation.Events;
using HMI.Domain.Models;
using HMI.Infrastructure;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Modules.Design.Attributes;
namespace HMI.ProjectDesigner.DesignControls.General.ViewModels
{
	public class HMIConnectorStatisticsViewModel : CommonControlViewModel
	{
		private bool useLocalColors;
		public event EventHandler<ValueChangedEventArgs<bool>> UseLocalColorsChanged;
		[Browsable(true), EditorOrder(-253.0), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public bool UseLocalColors
		{
			get
			{
				return useLocalColors;
			}
			set
			{
				if (useLocalColors != value)
				{
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(useLocalColors, value);
					RaisePropertyChanging(() => UseLocalColors);
					useLocalColors = value;
					UseLocalColorsChanged.Raise(this, e);
					RaisePropertyChanged(() => UseLocalColors);
				}
			}
		}
		public override Size DesignerDesiredSize
		{
			get
			{
				return new Size(300.0, 50.0);
			}
		}
		public HMIConnectorStatisticsViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, Type controlType) : base(container, parentValue, controlType)
		{
		}
		public override string GetXamlControlName()
		{
			return "HMIConnectorStatistics";
		}
		public override System.Collections.Generic.IEnumerable<string> GetNamespaces()
		{
			return new string[]
			{
                "HMIGeneral=\"clr-namespace:HMI.RuntimeControls.General;assembly=HMI.RuntimeControls\""
            };
		}
		//public override System.Collections.Generic.IEnumerable<AssemblyReference> GetRequiredAssemblies()
		//{
		//	return AssemblyGroups.HMIGeneral;
		//}
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
		}
		protected override void ShowHideProperties()
		{
			base.ShowHideProperties();
			IsBackgroundVisible = true;
			IsForegroundVisible = false;
		}
	}
}
