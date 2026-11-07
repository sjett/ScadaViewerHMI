// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace HMI.Modules.ControlTree
{
	[DataContract]
	public class ControlCategory
	{
		[DataMember(Name = "Children", IsRequired = false, Order = 1)]
		private List<ControlCategory> children = new List<ControlCategory>();

		[DataMember(IsRequired = false, Order = 0)]
		public string Name
		{
			get;
			set;
		}
		public ICollection<ControlCategory> Children
		{
			get
			{
				return children;
			}
		}
		public ControlCategory()
		{
		}
		public ControlCategory(string categoryName)
		{
			if (string.IsNullOrEmpty(categoryName))
			{
				throw new ArgumentException("categoryName is null or empty.", "categoryName");
			}
            Name = categoryName;
		}
		[OnDeserialized]
		private void InitializeChildren(StreamingContext context)
		{
			if (children == null)
			{
                children = new List<ControlCategory>();
			}
		}
	}
}
