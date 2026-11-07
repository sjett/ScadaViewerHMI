// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Collections;
using System.ComponentModel;

namespace HMI.Infrastructure.Presentation.ViewModels
{
    /// <summary>
    /// HTTP协议枚举
    /// </summary>
    [Obsolete]
    public enum HttpProtocol
    {
        /// <summary>
        /// HTTP协议
        /// </summary>
        HTTP,
        /// <summary>
        /// HTTPS协议
        /// </summary>
        HTTPS
    }

    public interface IAlarmGroupParameterDefinition : IParameterDefinition, ICollectionItem, IHierarchicalNode, INotifyPropertyChanged
    {
    }

    public interface IAlarmTypeParameterDefinition : IParameterDefinition, ICollectionItem, IHierarchicalNode, INotifyPropertyChanged
    {
    }

    

    

    public interface INavigationViewModel
    {
        IObservableCollection<INavigationTargetViewModel> NavigationTargets
        {
            get;
        }
    }

    

    /// <summary>
    /// 参数值接口
    /// </summary>
    public interface IParameterValue : ICollectionItem
    {
        /// <summary>
        /// 名称
        /// </summary>
        string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 值
        /// </summary>
        string Value
        {
            get;
            set;
        }
        /// <summary>
        /// 参数定义
        /// </summary>
        IParameterDefinition Definition
        {
            get;
        }
    }

    

    public interface IProjectSerializer : ISerializer
    {
    }

    

    public interface ISignalParameterDefinition : IParameterDefinition, ICollectionItem, IHierarchicalNode, INotifyPropertyChanged
    {
    }

    

    public interface IStaticSymbolViewModel : INotifyPropertyChanged
    {
        string ContentString
        {
            get;
            set;
        }
    }

    public interface ISymbolicTextParameterDefinition : IParameterDefinition, ICollectionItem, IHierarchicalNode, INotifyPropertyChanged
    {
    }

}
