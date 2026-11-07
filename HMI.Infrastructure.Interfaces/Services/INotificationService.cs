// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.Infrastructure.Services
{
    /// <summary>
    /// 通知服务接口
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// 显示警告信息
        /// </summary>
        /// <param name="text"></param>
        /// <param name="exception"></param>
        void ShowWarning(string text, Exception exception = null);
        /// <summary>
        /// 显示错误对话框
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        void ShowErrorDialog(string message, Exception exception = null);
        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        void ShowInfo(string message, Exception exception = null);
        /// <summary>
        /// 显示忙指示器
        /// </summary>
        /// <param name="message"></param>
        void ShowBusyIndicator(string message);
        /// <summary>
        /// 隐藏忙指示器
        /// </summary>
        void HideBusyIndicator();
        /// <summary>
        /// 显示消息框
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        MessageBoxResult ShowOKMessageBox(string title, string text);
        /// <summary>
        /// 显示消息框
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        MessageBoxResult ShowOKCancelMessageBox(string title, string text);
        /// <summary>
        /// 显示消息框
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        MessageBoxResult ShowYesNoMessageBox(string title, string text);
        /// <summary>
        /// 显示消息框
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        MessageBoxResult ShowYesNoCancelMessageBox(string title, string text);
        /// <summary>
        /// 显示新建项目对话框
        /// </summary>
        /// <param name="projectFolder"></param>
        /// <param name="projectName"></param>
        /// <returns></returns>
        bool ShowNewProjectDialog(out string projectFolder, out string projectName);
        /// <summary>
        /// 显示打开项目对话框
        /// </summary>
        /// <param name="projectFullPath"></param>
        /// <returns></returns>
        bool ShowOpenProjectDialog(out string projectFullPath);
        /// <summary>
        /// 显示另存为项目对话框
        /// </summary>
        /// <param name="projectFolder"></param>
        /// <param name="projectName"></param>
        /// <returns></returns>
        bool ShowSaveProjectAsDialog(out string projectFolder, out string projectName);
        /// <summary>
        /// 获取新语言名称
        /// </summary>
        /// <param name="title"></param>
        /// <param name="existingLanguage"></param>
        /// <returns></returns>
        string GetNewLanguage(string title, string existingLanguage);
    }

}
