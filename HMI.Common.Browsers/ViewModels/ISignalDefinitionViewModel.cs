// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.Common.Browsers.ViewModels
{
	public interface ISignalDefinitionViewModel : IBrowserItemViewModel
	{
		string Name
		{
			get;
			set;
		}
		string AliasName
		{
			get;
			set;
		}
		string Description
		{
			get;
			set;
		}
		bool IsActive
		{
			get;
			set;
		}
		string Unit
		{
			get;
			set;
		}
		bool IsOpcEnabled
		{
			get;
			set;
		}
		double FactorX1
		{
			get;
			set;
		}
		double FactorX2
		{
			get;
			set;
		}
		double FactorY1
		{
			get;
			set;
		}
		double FactorY2
		{
			get;
			set;
		}
		bool LogUserActivity
		{
			get;
			set;
		}
		double Hysterese
		{
			get;
			set;
		}
		double Maximum
		{
			get;
			set;
		}
		double Minimum
		{
			get;
			set;
		}
		bool IsVChannel
		{
			get;
			set;
		}
		string Server
		{
			get;
			set;
		}
		string ServerDescription
		{
			get;
			set;
		}
		string Connector
		{
			get;
			set;
		}
		string ConnectorDescription
		{
			get;
			set;
		}
		string Group
		{
			get;
			set;
		}
		string GroupDescription
		{
			get;
			set;
		}
		string WriteGroup
		{
			get;
			set;
		}
		//string LogTag
		//{
		//	get;
		//	set;
		//}
		//bool IsLogActive
		//{
		//	get;
		//	set;
		//}
		//string LogDescription
		//{
		//	get;
		//	set;
		//}
		//Guid? LogId
		//{
		//	get;
		//	set;
		//}
	}
}
