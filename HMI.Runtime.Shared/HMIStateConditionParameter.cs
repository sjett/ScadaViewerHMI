// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using System.Windows;
namespace HMI.Runtime.Shared
{
    /// <summary>

    /// </summary>
    public class HMIStateConditionParameter : DependencyObject
	{
		public static readonly DependencyProperty ParameterNameProperty = DependencyProperty.Register("ParameterName", typeof(string), typeof(HMIStateConditionParameter), new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnParameterNamePropertyChanged)));
		public static readonly DependencyProperty SignalNameProperty = DependencyProperty.Register("SignalName", typeof(string), typeof(HMIStateConditionParameter), new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnSignalNamePropertyChanged)));
		[Category("HMI")]
		public string ParameterName
		{
			get
			{
				return (string)GetValue(ParameterNameProperty);
			}
			set
			{
                SetValue(ParameterNameProperty, value);
			}
		}
		[Category("HMI")]
		public string SignalName
		{
			get
			{
				return (string)GetValue(SignalNameProperty);
			}
			set
			{
                SetValue(SignalNameProperty, value);
			}
		}
		[Browsable(false)]
		public string ResolvedSignalName
		{
			get;
			set;
		}
		private static void OnParameterNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnSignalNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		public override string ToString()
		{
			return string.Format("{0}: {1}", ParameterName, ResolvedSignalName ?? SignalName);
		}
		public void ResolvePlaceholders(HMIStatesControlBase owner)
		{
            MakeTrims(owner);
			if (ShouldResolveSignalName())
			{
                ResolvedSignalName = StringPlaceholdersResolver.Resolve(ResolvedSignalName, owner);
			}
		}
		private void MakeTrims(HMIStatesControlBase owner)
		{
            ParameterName = ParameterName.Trim();
            ResolvedSignalName = (owner.SignalPrefix + SignalName).Trim();
		}
		private bool ShouldResolveSignalName()
		{
			return !string.IsNullOrEmpty(ParameterName) && !string.IsNullOrEmpty(SignalName);
		}
	}
}
