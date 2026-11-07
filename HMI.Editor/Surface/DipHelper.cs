// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using System.Windows.Media;
namespace HMI.Editor.Surface;

/// <summary>
/// DIP帮助类
/// </summary>
public static class DipHelper
{
    private const double DpiBase = 96.0;
    /// <summary>
    /// 毫米转换为DIP
    /// </summary>
    /// <param name="mm"></param>
    /// <returns></returns>
    public static double MmToDip(double mm)
    {
        return DipHelper.CmToDip(mm / 10.0);
    }
    /// <summary>
    /// 厘米转换为DIP
    /// </summary>
    /// <param name="cm"></param>
    /// <returns></returns>
    public static double CmToDip(double cm)
    {
        return cm * DpiBase / 2.54;
    }
    /// <summary>
    /// 英寸转换为DIP
    /// </summary>
    /// <param name="inch"></param>
    /// <returns></returns>
    public static double InchToDip(double inch)
    {
        return inch * DpiBase;
    }
    /// <summary>
    /// DIP转换为英寸
    /// </summary>
    /// <param name="dip"></param>
    /// <returns></returns>
    public static double DipToInch(double dip)
    {
        return dip / DpiBase;
    }
    /// <summary>
    /// 磅转换为DIP
    /// </summary>
    /// <param name="pt"></param>
    /// <returns></returns>
    public static double PtToDip(double pt)
    {
        return pt * DpiBase / 72.0;
    }
    /// <summary>
    /// DIP转换为厘米
    /// </summary>
    /// <param name="dip"></param>
    /// <returns></returns>
    public static double DipToCm(double dip)
    {
        return dip * 2.54 / DpiBase;
    }
    /// <summary>
    /// DIP转换为毫米
    /// </summary>
    /// <param name="dip"></param>
    /// <returns></returns>
    public static double DipToMm(double dip)
    {
        return DipHelper.DipToCm(dip) * 10.0;
    }
    /// <summary>
    /// 获取系统DPI缩放因子
    /// </summary>
    /// <returns></returns>
    private static Point GetSystemDpiFactor()
    {
        PresentationSource presentationSource = PresentationSource.FromVisual(Application.Current.MainWindow);
        Matrix transformToDevice = presentationSource.CompositionTarget.TransformToDevice;
        return new Point(transformToDevice.M11, transformToDevice.M22);
    }
    /// <summary>
    /// 获取系统DPI
    /// </summary>
    /// <returns></returns>
    public static Point GetSystemDpi()
    {
        Point systemDpiFactor = DipHelper.GetSystemDpiFactor();
        return new Point(systemDpiFactor.X * DpiBase, systemDpiFactor.Y * DpiBase);
    }
    /// <summary>
    /// 获取物理DPI
    /// </summary>
    /// <param name="diagonalScreenSize"></param>
    /// <returns></returns>
    public static Point GetPhysicalDpi(double diagonalScreenSize)
    {
        Point systemDpiFactor = DipHelper.GetSystemDpiFactor();
        double physicalScreenWidth = SystemParameters.PrimaryScreenWidth * systemDpiFactor.X;
        double physicalScreenHeight = SystemParameters.PrimaryScreenHeight * systemDpiFactor.Y;
        double aspectRatio = physicalScreenWidth / physicalScreenHeight;
        double physicalScreenHeightInches = diagonalScreenSize / System.Math.Sqrt(aspectRatio * aspectRatio + 1.0);
        double physicalScreenWidthInches = aspectRatio * physicalScreenHeightInches;
        double horizontalDpi = System.Math.Round(physicalScreenWidth / physicalScreenWidthInches);
        double verticalDpi = System.Math.Round(physicalScreenHeight / physicalScreenHeightInches);
        return new Point(horizontalDpi, verticalDpi);
    }
    /// <summary>
    /// DPI转换为缩放因子
    /// </summary>
    /// <param name="dpi"></param>
    /// <returns></returns>
    public static Point DpiToScaleFactor(Point dpi)
    {
        Point systemDpi = DipHelper.GetSystemDpi();
        return new Point(dpi.X / systemDpi.X, dpi.Y / systemDpi.Y);
    }
    /// <summary>
    /// 获取屏幕独立缩放因子
    /// </summary>
    /// <param name="diagonalScreenSize"></param>
    /// <returns></returns>
    public static Point GetScreenIndependentScaleFactor(double diagonalScreenSize)
    {
        return DipHelper.DpiToScaleFactor(DipHelper.GetPhysicalDpi(diagonalScreenSize));
    }
}
