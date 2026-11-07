// SPDX-License-Identifier: GPL-3.0-only
using HMI.Runtime.Shared.Dialogs;
using System;
namespace HMI.Runtime.Shared.NavigationService
{
    public interface INavigationService
    {
        void Navigate(string url, DialogModality modality = DialogModality.Modeless, int width = 800, int height = 600);
    }
}
