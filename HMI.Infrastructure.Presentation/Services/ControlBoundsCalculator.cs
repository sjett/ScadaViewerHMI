// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using HMI.Domain.Models;
using HMI.Infrastructure.Presentation.ViewModels;

namespace HMI.Infrastructure.Presentation.Services
{
    /// <summary>
    /// 控件边界计算器，用于计算控件在组控件缩放或移动时的位置和尺寸变换。
    /// </summary>
    public class ControlBoundsCalculator
    {
        // 当前控件的视图模型
        private readonly ICommonControlViewModel control;
        // 当前控件的中心点
        private readonly Point controlCenter;
        // 组控件的视图模型
        private readonly ICommonControlViewModel groupControl;
        // 组控件的中心点
        private readonly Point groupControlCenter;
        // 控件的高度
        private readonly double controlHeight;
        // 控件的宽度
        private readonly double controlWidth;
        // 控件的原始高度
        private readonly double height;
        // 控件的原始宽度
        private readonly double width;

        /// <summary>
        /// 构造函数，初始化控件和组控件的相关属性。
        /// </summary>
        /// <param name="control">需要计算的控件</param>
        /// <param name="groupControl">所属的组控件</param>
        public ControlBoundsCalculator(ICommonControlViewModel control, ICommonControlViewModel groupControl)
        {
            this.groupControl = groupControl;
            this.control = control;
            width = control.Width;
            height = control.Height;
            controlWidth = control.ControlWidth;
            controlHeight = control.ControlHeight;
            groupControlCenter = GeometryService.GetControlCenter(groupControl);
            controlCenter = GeometryService.GetControlCenter(control);
        }

        /// <summary>
        /// 根据组控件的初始尺寸和新尺寸，按比例调整控件的宽高。
        /// </summary>
        /// <param name="groupInitialSize">组控件的初始尺寸</param>
        /// <param name="groupNewSize">组控件的新尺寸</param>
        public void PerformResize(Size groupInitialSize, Size groupNewSize)
        {
            // 计算宽高缩放比例
            double pi = groupNewSize.Width / groupInitialSize.Width;
            double pi2 = groupNewSize.Height / groupInitialSize.Height;
            // 计算缩放后的控件宽高
            double pi3 = width * pi;
            double pi4 = height * pi2;
            double pi5 = controlWidth * pi;
            double pi6 = controlHeight * pi2;
            // 设置控件的新宽高，最小为1.0
            control.Width = Math.Max(1.0, pi3);
            control.Height = Math.Max(1.0, pi4);
            control.ControlWidth = Math.Max(1.0, pi5);
            control.ControlHeight = Math.Max(1.0, pi6);
        }

        /// <summary>
        /// 根据组控件的初始尺寸和新尺寸，计算控件的平移（位置变换）。
        /// </summary>
        /// <param name="groupInitialSize">组控件的初始尺寸</param>
        /// <param name="groupNewSize">组控件的新尺寸</param>
        /// <param name="groupInitialCoordinates">组控件的初始坐标</param>
        public void PerformTranslation(Size groupInitialSize, Size groupNewSize, Point groupInitialCoordinates)
        {
            // 计算宽高缩放比例
            double pi = groupNewSize.Width / groupInitialSize.Width;
            double pi2 = groupNewSize.Height / groupInitialSize.Height;
            // 计算控件中心点相对于组中心点的向量
            Vector vector = this.controlCenter - groupControlCenter;
            // 按比例缩放向量
            double pi3 = vector.X * pi;
            double pi4 = vector.Y * pi2;
            // 获取组控件的新中心点
            Point controlCenter = GeometryService.GetControlCenter(groupControl);
            // 偏移控件中心点
            controlCenter.Offset(pi3, pi4);
            // 计算控件相对于中心点的平移量
            Point translationRelativeToCenter = GeometryService.GetTranslationRelativeToCenter(controlCenter, control);
            // 应用平移变换
            control.Translate(translationRelativeToCenter.X, translationRelativeToCenter.Y, TransformMode.Absolute);
        }
    }
}
