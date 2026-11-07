// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
namespace HMI.Runtime.Core
{

    public delegate void OnUserContextChangedEventHandler(string userName);

    public interface IHMISecurity
	{
		event OnUserContextChangedEventHandler UserContextChanged;
		List<string> GetProjectAuthorizations();
		List<string> GetSystemAuthorizations();
		List<string> GetAuthorizationGroups();
		bool CheckProjectAuthorization(string AuthorizationName);
		bool CheckSystemAuthorization(string AuthorizationName);
		bool CheckAuthorizationGroup(string AuthorizationGroupName);
		HMISecurityStatus GetSecurtityStatus();
		string GetUserName();
		bool GetIsDomainUser();
		int GetUserLevel();
		void LoginUser(string user, string password);
		void LoginUser(string user, string password, bool isDomainUser);
		void LoginUser(string user, string password, OnResultDelegate resultDelegate);
		void LoginUser(string user, string password, bool isDomainUser, OnResultDelegate resultDelegate);
		void LoginCurrentWindowsUser(OnResultDelegate resultDelegate);
		void LogoutUser();
		void ChangePasswordAsAdmin(string user, string newPassword);
		void ChangePasswordAsAdmin(string user, string newPassword, bool isDomainUser);
		void ChangePasswordAsAdmin(string user, string newPassword, OnResultDelegate resultDelegate);
		void ChangePasswordAsAdmin(string user, string newPassword, bool isDomainUser, OnResultDelegate resultDelegate);
		void ChangePasswordAsUser(string oldPassword, string newPassword);
		void ChangePasswordAsUser(string oldPassword, string newPassword, bool isDomainUser);
		void ChangePasswordAsUser(string oldPassword, string newPassword, OnResultDelegate resultDelegate);
		void ChangePasswordAsUser(string oldPassword, string newPassword, bool isDomainUser, OnResultDelegate resultDelegate);
		void ChangePasswordAsUser(string username, string oldPassword, string newPassword);
		void ChangePasswordAsUser(string username, string oldPassword, string newPassword, bool isDomainUser);
		void ChangePasswordAsUser(string username, string oldPassword, string newPassword, OnResultDelegate resultDelegate);
		void ChangePasswordAsUser(string username, string oldPassword, string newPassword, bool isDomainUser, OnResultDelegate resultDelegate);
	}
}
