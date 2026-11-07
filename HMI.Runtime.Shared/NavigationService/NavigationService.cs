// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
//using System.Windows.Browser;
using System.Windows.Controls;
using Telerik.Windows.Controls;
using HMI.Runtime.Core.Helper;
using HMI.Runtime.Shared.Dialogs;

namespace HMI.Runtime.Shared.NavigationService
{
    public class NavigationService : INavigationService
    {
        public void Navigate(string url, DialogModality modality = DialogModality.Modeless, int width = 800, int height = 600)
        {
            //if (Application.Current.IsRunningOutOfBrowser)
            if(true)
            {
                this.OutOfBrowserNavigate(url, modality, width, height);
                return;
            }
            //this.InBrowserNavigate(url, modality, width, height);
        }
        //private void InBrowserNavigate(string url, DialogModality modality, int width, int height)
        //{
        //    if (modality == DialogModality.Modal)
        //    {
        //        this.InBrowserNavigateModal(url, width, height);
        //        return;
        //    }
        //    this.InBrowserNavigateModeless(url);
        //}
        //private void InBrowserNavigateModeless(string url)
        //{
        //    string text = string.Format("window.open('{0}', '')", url);
        //    HtmlPage.Window.Eval(text);
        //}
        //private void InBrowserNavigateModal(string url, int width, int height)
        //{
        //    string text = string.Format("window.showModalDialog('{0}', '', 'dialogHeight:{1}px; dialogWidth:{2}px; resizable=yes')", url, height, width);
        //    HtmlPage.Window.Eval(text);
        //}
        private void OutOfBrowserNavigate(string url, DialogModality modality, int width, int height)
        {
            if (modality == DialogModality.Modal)
            {
                this.OutOfBrowserNavigateModal(url, width, height);
                return;
            }
            this.OutOfBrowserNavigateModeless(url);
        }
        private void OutOfBrowserNavigateModal(string url, int width, int height)
        {
            WebBrowser webBrowser = new WebBrowser();
            webBrowser.HorizontalAlignment= HorizontalAlignment.Stretch;
            webBrowser.VerticalAlignment= VerticalAlignment.Stretch;
            WebBrowser webBrowser2 = webBrowser;
            RadWindow radWindow = new RadWindow();
            radWindow.Header=(url);
            radWindow.Width=((double)width);
            radWindow.Height=((double)height);
            radWindow.Content=(webBrowser2);
            RadWindow radWindow2 = radWindow;
            radWindow2.ShowDialog();
            webBrowser2.Navigate(new Uri(url));
        }
        private void OutOfBrowserNavigateModeless(string url)
        {
            //WFWindowsScriptHostBridge wFWindowsScriptHostBridge = new WFWindowsScriptHostBridge();
            //wFWindowsScriptHostBridge.Run(url);

            //System.Diagnostics.ProcessStartInfo
            System.Diagnostics.Process.Start(url);
        }
    }
}
