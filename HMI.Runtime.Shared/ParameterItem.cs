// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
namespace HMI.Runtime.Shared
{
	public class ParameterItem : INotifyPropertyChanged
	{
		private string parameterName;
		private string parameterValue;

		public event PropertyChangedEventHandler PropertyChanged;

		public string ParameterName
		{
			get
			{
				return this.parameterName;
			}
			set
			{
				this.parameterName = value;
				this.OnPropertyChanged("ParameterName");
			}
		}
		public string ParameterValue
		{
			get
			{
				return this.parameterValue;
			}
			set
			{
				this.parameterValue = value;
				this.OnPropertyChanged("ParameterValue");
			}
		}
		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		public override string ToString()
		{
			return string.Format("{0}: {1}", this.parameterName, this.parameterValue);
		}
	}
}
