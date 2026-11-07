// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Runtime.Serialization;
namespace HMI.Modules.ControlTree
{
	[DataContract]
	public class ControlCategoryMapping
	{
		[DataMember(Order = 1, EmitDefaultValue = false)]
		public string ControlName
		{
			get;
			set;
		}
		[DataMember(Order = 2, EmitDefaultValue = false)]
		public string CategoryName
		{
			get;
			set;
		}
		[DataMember(Order = 3, EmitDefaultValue = false)]
		public string AssemblyName
		{
			get;
			set;
		}
		[DataMember(Order = 4, EmitDefaultValue = false)]
		public string ClassName
		{
			get;
			set;
		}
		[DataMember(Order = 5, EmitDefaultValue = false, IsRequired = false)]
		public string XamlPath
		{
			get;
			set;
		}
		public ControlCategoryMapping()
		{
		}
		public ControlCategoryMapping(string controlName, string categoryName, string assemblyName, string className) : this(controlName, categoryName, assemblyName, className, null)
		{
		}
		public ControlCategoryMapping(string controlName, string categoryName, string assemblyName, string className, string xamlPath)
		{
            ControlName = controlName;
            CategoryName = categoryName;
            AssemblyName = assemblyName;
            ClassName = className;
            XamlPath = xamlPath;
		}
	}
}
