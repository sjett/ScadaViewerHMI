// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Common.Presentation.Events.Interactions
{
    /// <summary>
    /// 通知详情
    /// </summary>
    public class NotificationDetails : InteractionDetails
	{
        /// <summary>
        /// 通知严重性
        /// </summary>
        public NotificationSeverity Severity
		{
			get;
			set;
		}
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
		{
			get;
			set;
		}
        /// <summary>
        /// 内容
        /// </summary>
        public string Content
		{
			get;
			set;
		}
        /// <summary>
        /// 异常
        /// </summary>
        /// </summary>
        public Exception Exception
		{
			get;
			set;
		}
        /// <summary>
        /// 通知详情构造函数
        /// </summary>
        /// <param name="severity">通知严重性</param>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="exception">异常</param>
        public NotificationDetails(NotificationSeverity severity, string title, string content, Exception exception)
		{
			this.Severity = severity;
			this.Title = title;
			this.Content = content;
			this.Exception = exception;
		}
        /// <summary>
        /// 通知详情默认构造函数
        /// </summary>
		public NotificationDetails()
		{
		}
	}
}
