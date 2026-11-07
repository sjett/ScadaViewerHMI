// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using HMI.Runtime.Shared;
namespace HMI.RuntimeControls.Navigation
{
	public partial class HMINavigationParameter : UserControl, IHMINavigationParameter
	{
		private ObservableCollection<ParameterItem> parameters = new ObservableCollection<ParameterItem>();

		[Category("HMI")]
		public ObservableCollection<ParameterItem> Parameters
		{
			get
			{
				return this.parameters;
			}
		}
		public HMINavigationParameter()
		{
			this.InitializeComponent();
		}
		
	}
}
