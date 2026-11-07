// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.Common.Browsers.ViewModels
{
	internal class SignalGroupViewModel : IHierarchicalFilterViewModel, ISignalGroupViewModel
	{
		private readonly IHierarchicalFilterViewModel parent;
		public string Name
		{
			get;
			private set;
		}
		public string Description
		{
			get;
			private set;
		}
		public SignalGroupViewModel(IHierarchicalFilterViewModel parent, string name, string description)
		{
            this.parent = parent;
            Name = name;
            Description = description;
		}
		public bool IsMatchingFilter(ISignalDefinitionViewModel item)
		{
			return parent.IsMatchingFilter(item) && item.Group == Name;
		}
		public override string ToString()
		{
			return string.Format("{0}/{1}", parent, Name);
		}
		public override bool Equals(object obj)
		{
			return AreEqual(obj as SignalGroupViewModel);
		}
		private bool AreEqual(SignalGroupViewModel other)
		{
			return other != null && other.Name == Name && parent.Equals(other.parent);
		}
		public override int GetHashCode()
		{
			return Name.GetHashCode() ^ parent.GetHashCode();
		}
	}
}
