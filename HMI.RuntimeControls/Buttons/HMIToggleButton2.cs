// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.RuntimeControls.Buttons
{
    /// <summary>
    /// HMIToggleButton2是HMIToggleButtonBase的一个子类，提供了一个新的样式和功能。
    /// 它可以用于创建具有切换功能的按钮，用户可以通过点击按钮来切换其状态。
    /// HMIToggleButton2可以自定义外观和行为，以满足不同应用程序的需求。
    /// </summary>
    public class HMIToggleButton2 : HMIToggleButtonBase
    {
        public HMIToggleButton2()
        {
            base.DefaultStyleKey=(typeof(HMIToggleButton2));
        }
    }
}
