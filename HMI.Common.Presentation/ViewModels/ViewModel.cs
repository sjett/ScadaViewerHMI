// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Presentation.Events;
using System.ComponentModel;
using System.Linq.Expressions;

namespace HMI.Common.Presentation.ViewModels
{
    /// <summary>
    /// 视图模型接口
    /// </summary>
    public interface IViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 引发属性更改事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyExpression"></param>
        void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression);
        /// <summary>
        /// 引发属性更改中事件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyExpression"></param>
        void RaisePropertyChanging<T>(Expression<Func<T>> propertyExpression);
    }

    public class ViewModel : PropertyChangedNotifier, IViewModel
    {
    }
}
