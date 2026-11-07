// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Reflection;
using System.Security;
using System.Windows.Input;

namespace HMI.Infrastructure
{
    public static class EditorCommands
    {
        public static RoutedCommand Build = new RoutedCommand();
        public static RoutedCommand AddTemplatePage = new RoutedCommand();
        public static RoutedCommand AddNavigationPage = new RoutedCommand();
        public static RoutedCommand RemovePage = new RoutedCommand();
        public static RoutedCommand RemovePageExplorer = new RoutedCommand();
        public static RoutedCommand SetStartPage = new RoutedCommand();
        public static RoutedCommand Rename = new RoutedCommand();
        public static RoutedCommand ShowToolbox = new RoutedCommand();
        public static RoutedCommand ShowProperties = new RoutedCommand();
        public static RoutedCommand ShowExplorer = new RoutedCommand();
        public static RoutedCommand ShowLog = new RoutedCommand();
        public static RoutedCommand OpenMruItem = new RoutedCommand();
        public static RoutedCommand SelectLanguage = new RoutedCommand();

    }
}
