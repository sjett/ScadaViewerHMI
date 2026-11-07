// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Infrastructure.Presentation.ViewModels
{
    /// <summary>
    /// 参考线设置视图模型接口
    /// </summary>
    public interface IGuideSettingsViewModel
    {
        /// <summary>
        /// 显示标尺参考线
        /// </summary>
        bool ShowRulerGuides
        {
            get;
            set;
        }
        /// <summary>
        /// 显示网格线
        /// </summary>
        bool ShowGridLines
        {
            get;
            set;
        }
        /// <summary>
        /// 显示参考线
        /// </summary>
        bool SnapToGridLines
        {
            get;
            set;
        }
        /// <summary>
        /// 显示对齐参考线
        /// </summary>
        bool SnapToGuideLines
        {
            get;
            set;
        }
    }
}
