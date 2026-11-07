// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using HMI.Runtime.Shared;
namespace HMI.RuntimeControls.Navigation
{
	public class NavigationItem : INotifyPropertyChanged
	{
		private ObservableCollection<ParameterItem> parameters = new ObservableCollection<ParameterItem>();
		private string target;
		private string source;

		public event PropertyChangedEventHandler? PropertyChanged;

		public string Target
		{
			get
			{
				return this.target;
			}
			set
			{
				this.target = value;
				this.OnPropertyChanged("Target");
			}
		}
		public string Source
		{
			get
			{
				return this.source;
			}
			set
			{
				this.source = value;
				this.OnPropertyChanged("Source");
			}
		}
		public ObservableCollection<ParameterItem> Parameters
		{
			get
			{
				return this.parameters;
			}
		}
		protected virtual void OnPropertyChanged(string propertyName)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		public override string ToString()
		{
			return string.Format("Source:{0} Target:{1}", this.source, this.target);
		}
	}
}
