// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.Editor.Common
{
    /// <summary>
    /// 定义状态条件的类型，状态条件可以是一个表达式，也可以是一个信号名称。
    /// </summary>
    public enum StateConditionType
	{
        /// <summary>
        /// 状态条件是一个表达式，表达式可以包含信号名称、常量和运算符，用于计算状态的值。
        /// </summary>
		Equation,
        /// <summary>
        /// 状态条件是一个信号名称，状态的值由该信号的值决定。
        /// </summary>
		SignalName
    }
}
