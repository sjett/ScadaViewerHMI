// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.Editor.Surface
{
    /// <summary>
    /// 吸附模式
    /// </summary>
    public enum SnapMode
    {
        /// <summary>
        /// 忽略吸附
        /// </summary>
        Ignore,
        /// <summary>
        /// 常规吸附
        /// </summary>
		Regular,
        /// <summary>
        /// 强制保持宽高比吸附
        /// </summary>
		ForceAspect,
        /// <summary>
        /// 轴对齐吸附
        /// </summary>
		AxisAligned
    }
}
