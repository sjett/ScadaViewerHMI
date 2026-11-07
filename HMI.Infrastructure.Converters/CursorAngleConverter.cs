// SPDX-License-Identifier: GPL-3.0-only
using System.Globalization;
using System.Windows.Data;
using System.Windows.Input;
namespace HMI.Infrastructure.Converters
{
    /// <summary>
    /// 光标角度转换器，根据旋转角度返回对应的光标
    /// </summary>
	public class CursorAngleConverter : IValueConverter
	{
		private const double CursorSectorAngle = 22.5;
		private const int FullRotationDegrees = 360;
        private readonly Cursor[] _resizeCursorsByAngle = new Cursor[]
		{
			Cursors.SizeWE,
			Cursors.SizeNWSE,
			Cursors.SizeNWSE,
			Cursors.SizeNS,
			Cursors.SizeNS,
			Cursors.SizeNESW,
			Cursors.SizeNESW,
			Cursors.SizeWE,
			Cursors.SizeWE,
			Cursors.SizeNWSE,
			Cursors.SizeNWSE,
			Cursors.SizeNS,
			Cursors.SizeNS,
			Cursors.SizeNESW,
			Cursors.SizeNESW,
			Cursors.SizeWE
		};
		public object Convert(object angle, Type targetType, object unrotatedCursorName, CultureInfo culture)
		{
			if (!(angle is double))
			{
				return Cursors.Arrow;
			}
			object result;
			try
			{
				int cursorIndex = GetCursorIndex(GetUnrotatedCursorAngle(unrotatedCursorName), (double)angle);
				result = _resizeCursorsByAngle[cursorIndex];
            }
			catch (ArgumentNullException)
			{
				result = Cursors.Arrow;
			}
			catch (ArgumentException)
			{
				result = Cursors.Arrow;
			}
			return result;
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
        /// <summary>
        /// 计算光标角度
        /// </summary>
        /// <param name="cursorAngle"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        private int GetCursorIndex(double cursorAngle, double angle)
        {
            angle += cursorAngle;
            if (angle >= 0.0)
            {
                angle = (double)((int)angle % FullRotationDegrees);
            }
            else
            {
                angle = (double)((int)angle % -FullRotationDegrees + FullRotationDegrees - 1);
            }
            return (int)(angle / CursorSectorAngle);
        }
        /// <summary>
		/// 获取光标默认角度
		/// </summary>
		/// <param name="unrotatedCursorName"></param>
		/// <returns></returns>
        private double GetUnrotatedCursorAngle(object unrotatedCursorName)
        {
            string cursorName = unrotatedCursorName as string;
            if (cursorName == null)
            {
                return double.NaN;
            }
            string normalizedCursorName = cursorName.ToLowerInvariant();
            if (normalizedCursorName != null)
            {
                if (normalizedCursorName == "sizene")
                {
                    return -45.0;
                }
                if (normalizedCursorName == "sizenv")
                {
                    return -135.0;
                }
                if (normalizedCursorName == "sizese")
                {
                    return 45.0;
                }
                if (normalizedCursorName == "sizesv")
                {
                    return 135.0;
                }
            }
            return double.NaN;
        }
    }
}
