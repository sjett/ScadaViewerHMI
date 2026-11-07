// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.Common.Browsers.Services
{
	public interface IServiceConfiguration
	{
		//Uri BaseUri
		//{
		//	get;
		//}
		string Host { get; }
		int Port { get; }
		int Timeout { get; }
		string Username { get; }
		string Password { get; }
		string SecretKey { get; }
		//string Instance { get; }
		string ServerName
		{
			get;
		}
	}
}
