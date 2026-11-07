// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.Common.Browsers.Models
{
	public class TranslatedEntity : Model
	{
		public string Name
		{
			get;
			set;
		}
		public string Translation
		{
			get;
			set;
		}
		public TranslatedEntity()
		{
		}
		//internal TranslatedEntity(AlarmGroupDTO dto) : base(dto)
		//{
		//	if (dto == null)
		//	{
		//		return;
		//	}
		//	this.Name = dto.SymbolicTextName;
		//	this.Translation = dto.SymbolicTextTranslation;
		//}
		//internal TranslatedEntity(AlarmTypeDTO dto) : base(dto)
		//{
		//	if (dto == null)
		//	{
		//		return;
		//	}
		//	this.Name = dto.SymbolicTextName;
		//	this.Translation = dto.SymbolicTextTranslation;
		//}
	}
}
