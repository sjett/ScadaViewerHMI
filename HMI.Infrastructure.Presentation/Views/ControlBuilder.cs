// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.ViewModels;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HMI.Infrastructure.Presentation.Views
{
    /// <summary>
    /// 控件构建器
    /// </summary>
    public class ControlBuilder : IValueConverter
    {
        /// <summary>
        /// 控件工厂
        /// </summary>
        public IControlFactory ControlFactory
        {
            get;
            set;
        }

        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ICommonControlViewModel commonControlViewModel = value as ICommonControlViewModel;
            if (commonControlViewModel == null)
            {
                return null;
            }

            IDesignerControl designerControl = this.ControlFactory.CreateControl(commonControlViewModel.ControlType);

            if (string.Equals(parameter as string, "true", StringComparison.InvariantCultureIgnoreCase))
            {
                designerControl.Loaded += new RoutedEventHandler(designerControl_Loaded);
            }
            return designerControl;
        }
        /// <summary>
        /// 设计器控件加载完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void designerControl_Loaded(object sender, RoutedEventArgs e)
        {
            IDesignerControl? designerControl = sender as IDesignerControl;
            if (!string.IsNullOrEmpty(designerControl!.DesignerTemplateKey))
            {
                designerControl.SetResourceReference(FrameworkElement.StyleProperty, designerControl.DesignerTemplateKey);
            }

        }
        /// <summary>
        /// 反向转换
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

    }
}
