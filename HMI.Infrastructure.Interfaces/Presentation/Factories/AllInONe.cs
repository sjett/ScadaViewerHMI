// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Services;

namespace HMI.Infrastructure.Presentation.Factories
{
    /// <summary>
    /// 控件工厂接口
    /// </summary>
    public interface IControlFactory
    {
        /// <summary>
        /// 创建控件
        /// </summary>
        /// <param name="controlTypeFullName"></param>
        /// <returns></returns>
        IDesignerControl CreateControl(string controlTypeFullName);
        /// <summary>
        /// 创建控件
        /// </summary>
        /// <param name="controlType"></param>
        /// <returns></returns>
        IDesignerControl CreateControl(Type controlType);
    }
    /// <summary>
    /// 参数引用监视器工厂接口
    /// </summary>
    public interface IParameterReferenceMonitorFactory
    {
        /// <summary>
        /// 创建监视器
        /// </summary>
        /// <param name="node">层级节点</param>
        /// <returns></returns>
        IParameterReferenceMonitor CreateMonitor(IHierarchicalNode node);
    }

    /// <summary>
    /// 序列化器工厂接口
    /// </summary>
    public interface ISerializerFactory
    {
        /// <summary>
        /// 注册序列化器
        /// </summary>
        /// <typeparam name="TFor"></typeparam>
        /// <typeparam name="TSerializer"></typeparam>
        void RegisterSerializer<TFor, TSerializer>() where TSerializer : ISerializer;

        /// <summary>
        /// 注册序列化器
        /// </summary>
        /// <typeparam name="TFor"></typeparam>
        /// <param name="instance"></param>
        void RegisterSerializer<TFor>(ISerializer instance);

        /// <summary>
        /// 获取序列化器
        /// </summary>
        /// <typeparam name="TFor">TFor 继承 ISerializer</typeparam>
        /// <returns></returns>
        ISerializer GetSerializer<TFor>();
        /// <summary>
        /// 获取序列化器
        /// </summary>
        /// <param name="forType"></param>
        /// <returns></returns>
        ISerializer GetSerializer(Type forType);
    }
}
