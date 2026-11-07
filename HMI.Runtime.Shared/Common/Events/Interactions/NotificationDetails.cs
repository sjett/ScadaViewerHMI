// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Runtime.Shared.Common.Events.Interactions
{
    using System;
    using System.Runtime.CompilerServices;

    public class NotificationDetails : InteractionDetails
    {
        public NotificationDetails()
        {
        }

        public NotificationDetails(NotificationSeverity severity, string title, string content)
        {
            this.Severity = severity;
            this.Title = title;
            this.Content = content;
        }

        public string Content { get; set; }

        public NotificationSeverity Severity { get; set; }

        public string Title { get; set; }
    }
}

