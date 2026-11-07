// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Resources;
using System.Linq.Expressions;

namespace HMI.Common.Instrumentation.Metadata
{
    /// <summary>
    /// 反射辅助类
    /// </summary>
    public static class Reflection
    {
        /// <summary>
        /// 获取属性名称
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        public static string GetPropertyName<TResult>(Expression<Func<TResult>> action)
        {
            return Reflection.GetPropertyName((LambdaExpression)action);
        }
        /// <summary>
        /// 获取属性名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        public static string GetPropertyName<T, TResult>(Expression<Func<T, TResult>> action)
        {
            return Reflection.GetPropertyName((LambdaExpression)action);
        }
        /// <summary>
        /// 获取属性名称
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static string GetPropertyName(LambdaExpression action)
        {
            MemberExpression memberExpression = (MemberExpression)action.Body;
            return memberExpression.Member.Name;
        }
        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="target"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        /// <exception cref="ReflectionException"></exception>
        public static object GetPropertyValue(object target, string propertyName)
        {
            object result;
            try
            {
                result = target.GetType().GetProperty(propertyName).GetValue(target);
                //result = Impromptu.InvokeGet(target, propertyName);
            }
            catch (Exception inner)
            {
                throw new ReflectionException(string.Format(Errors.ErrorGettingPropertyValueFormat, propertyName), inner);
            }
            return result;
        }
        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="target"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <exception cref="ReflectionException"></exception>
        public static void SetPropertyValue(object target, string propertyName, object value)
        {
            try
            {
                target.GetType().GetProperty(propertyName).SetValue(target, value);
                //Impromptu.InvokeSet(target, propertyName, value);
            }
            catch (Exception inner)
            {
                throw new ReflectionException(string.Format(Errors.ErrorSettingPropertyValueFormat, propertyName), inner);
            }
        }
    }
}
