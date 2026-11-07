// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.ErrorHandling;
using System.Linq.Expressions;

namespace HMI.Modules.Design.Attributes
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public class EnumTranslationAttribute : Attribute
	{

		public Func<string> GetTranslation { get; private set; }

		public EnumTranslationAttribute(Type resourceManagerType, string keyName)
		{
			Contract.Requires<ArgumentNullException>(resourceManagerType != null, "resourceManagerType");
			Contract.Requires<ArgumentNullException>(keyName != null, "key");
			Contract.Requires<ArgumentException>(keyName.Length > 0, "key");

			MemberExpression body = Expression.Property(null, resourceManagerType, keyName);

			this.GetTranslation = Expression.Lambda<Func<string>>(body, new ParameterExpression[0]).Compile();
		}
		public EnumTranslationAttribute(string translation)
		{
			this.GetTranslation = () => translation;
		}
	}
}
