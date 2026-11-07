// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.Common.Browsers.Models
{
	public class ExtendedAlarmProperty
	{
		public int Index
		{
			get;
			set;
		}
		public bool IsActive
		{
			get;
			set;
		}
		public string SymbolicTextName
		{
			get;
			set;
		}
		public ExtendedAlarmProperty()
		{
		}
		//internal ExtendedAlarmProperty(ExtendedAlarmPropertyDTO dto)
		//{
		//	if (dto == null)
		//	{
		//		return;
		//	}
		//	this.Index = dto.Index;
		//	this.IsActive = dto.IsActive;
		//	this.SymbolicTextName = dto.SymbolicTextName;
		//}
	}
}
