// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.ErrorHandling;
using HMI.Domain.Models;
using HMI.Infrastructure.Design;
using HMI.Infrastructure.Presentation.ViewModels;
using System.Linq.Expressions;
using System.Reflection;

namespace HMI.Infrastructure.Presentation.Services
{
    /// <summary>
    /// 信号解析服务实现
    /// </summary>
    public class SignalResolverService : ISignalResolverService
    {
        /// <summary>
        /// 获取解析后的信号前缀
        /// </summary>
        /// <param name="control">要解析信号前缀的控件对象。</param>
        /// <param name="page">控件所属页面。</param>
        /// <returns>解析后的信号前缀；控件不包含信号相关属性时返回 null。</returns>
        /// <exception cref="NotSupportedException">控件声明了 ObjectID 但未声明 SignalPrefix。</exception>
        public string? GetResolvedSignalPrefix(object control, IPage page)
        {
            if (control == null)
            {
                return null;
            }

            Type controlType = control.GetType();
            PropertyInfo? objectIdProperty = controlType.GetProperty("ObjectID");
            PropertyInfo? signalPrefixProperty = controlType.GetProperty("SignalPrefix");
            if (objectIdProperty != null && signalPrefixProperty == null)
            {
                throw new NotSupportedException();
            }

            if (objectIdProperty != null)
            {
                return GetSignalName(page, objectIdProperty, signalPrefixProperty!, control);
            }

            return signalPrefixProperty != null
                ? GetSignalPrefixValue(control, signalPrefixProperty)
                : null;
        }

        /// <summary>
        /// 获取信号监视器
        /// </summary>
        /// <param name="controlViewModel">控件视图模型。</param>
        /// <param name="signalExpression">信号名称表达式。</param>
        /// <param name="resolvedSignalExpression">解析后的信号名称表达式。</param>
        /// <returns>用于观察信号变化的监视器。</returns>
        public ISignalWatcher GetSignalWatcher(
            ICommonControlViewModel controlViewModel,
            Expression<Func<string>> signalExpression,
            Expression<Func<string>> resolvedSignalExpression)
        {
            Contract.Requires<ArgumentNullException>(controlViewModel != null, nameof(controlViewModel));
            Contract.Requires<ArgumentNullException>(signalExpression != null, nameof(signalExpression));
            Contract.Requires<ArgumentNullException>(resolvedSignalExpression != null, nameof(resolvedSignalExpression));
            return new SignalWatcher(controlViewModel, signalExpression, resolvedSignalExpression);
        }

        private static string? GetSignalPrefixValue(object control, PropertyInfo signalPrefixProperty)
        {
            return (string?)signalPrefixProperty.GetValue(control);
        }

        private string GetSignalName(
            IPage page,
            PropertyInfo objectIdProperty,
            PropertyInfo signalPrefixProperty,
            object control)
        {
            string objectId = (objectIdProperty.GetValue(control) as string) ?? string.Empty;
            SignalPrefix signalPrefix = (SignalPrefix)signalPrefixProperty.GetValue(control)!;
            return GetSignalName(signalPrefix, page, objectId);
        }

        private string GetSignalName(SignalPrefix signalPrefix, IPage page, string objectId)
        {
            return signalPrefix switch
            {
                SignalPrefix.None => null,
                SignalPrefix.ObjectName => objectId,
                SignalPrefix.ObjectName_PageSignalPrefix => GetObjectNameAndPagePrefix(objectId, page),
                SignalPrefix.PageSignalPrefix => GetPageSignalPrefix(page),
                SignalPrefix.PageSignalPrefix_ObjectName => GetPagePrefixAndObjectName(objectId, page),
                _ => throw new NotSupportedException()
            };
        }

        private static string? GetObjectNameAndPagePrefix(string objectId, IPage page)
        {
            if (page == null)
            {
                return null;
            }

            return string.Format("{0}.{1}", objectId, GetPageSignalPrefix(page));
        }

        private static string? GetPagePrefixAndObjectName(string objectId, IPage page)
        {
            if (page == null)
            {
                return null;
            }

            return string.Format("{0}.{1}", GetPageSignalPrefix(page), objectId);
        }

        private static string? GetPageSignalPrefix(IPage page)
        {
            if (page == null)
            {
                return null;
            }

            return page.UsePageNameAsSignalPrefix ? page.ID : page.SignalPrefix;
        }
    }
}
