// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Metadata;
using HMI.Infrastructure.Services;
using System.Diagnostics;
using HMI.Common.Instrumentation.ErrorHandling;
using System.Globalization;
namespace HMI.Infrastructure.Presentation.Services.UndoRedo
{
    /// <summary>
    /// 属性更改操作
    /// </summary>
    [DebuggerDisplay("PropertyChanged {propertyName}")]
	internal class PropertyChangedAction : IAction
	{
		private readonly object newValue;
		private readonly object oldValue;
        private readonly string propertyName;
        private readonly object target;
        /// <summary>
        /// 属性更改操作构造函数
        /// </summary>
        /// <param name="target"></param>
        /// <param name="propertyName"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        public PropertyChangedAction(object target, string propertyName, object oldValue, object newValue)
		{
			Contract.Requires<ArgumentNullException>(target != null, "target");
			Contract.Requires<ArgumentNullException>(propertyName != null, "propertyName");
			Contract.Requires<ArgumentException>(propertyName!.Length > 0, "propertyName");
			this.propertyName = propertyName;
			this.target = target;
			this.newValue = newValue;
			this.oldValue = oldValue;
		}
        /// <summary>
        /// 将此操作与下一个操作组合
        /// </summary>
        /// <param name="nextAction"></param>
        /// <returns></returns>
        public IAction GroupWith(IAction nextAction)
		{
			return new HomogenousGroupAction(new IAction[]
			{
				this,
				nextAction
			});
		}
        /// <summary>
        /// 确定此操作是否可以与另一个操作组合
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool CanGroup(IAction action)
		{
			PropertyChangedAction propertyChangedAction = action as PropertyChangedAction;
			return propertyChangedAction != null && ReferenceEquals(propertyChangedAction.target, this.target) && this.propertyName == propertyChangedAction.propertyName;
		}
        /// <summary>
        /// 撤销操作
        /// </summary>
        public void Undo()
		{
			Reflection.SetPropertyValue(this.target, this.propertyName, this.oldValue);
		}
        /// <summary>
        /// 重做操作
        /// </summary>
        public void Redo()
		{
			Reflection.SetPropertyValue(this.target, this.propertyName, this.newValue);
		}
        /// <summary>
        /// 返回操作的字符串表示形式
        /// </summary>
        /// <returns></returns>
        public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "PropertyChanged from {0} to {1} ({2})", new object[]
			{
				this.oldValue,
				this.newValue,
				this.target
			});
		}
	}
}
