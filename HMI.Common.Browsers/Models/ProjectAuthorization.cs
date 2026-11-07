// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.Common.Browsers.Models
{
	public class ProjectAuthorization : Model
	{
		public string Name
		{
			get;
			set;
		}
		public string Description
		{
			get;
			set;
		}
		public ProjectAuthorization()
		{
		}
		//internal ProjectAuthorization(ProjectAuthorizationDTO dto) : base(dto)
		//{
		//	if (dto == null)
		//	{
		//		return;
		//	}
		//	this.Name = dto.Name;
		//	this.Description = dto.Description;
		//}
	}
}
