// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using System.Windows;


namespace HMI.Runtime.Core.Helper.Extensions
{
    public static class DesignerPropertiesExt
    {
        /// <summary>
        /// wpf 通用判断设计时
        /// </summary>
        public static bool IsInDesignTool
        {
            get
            {
                return (bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue;
            }
        }
    }
}
