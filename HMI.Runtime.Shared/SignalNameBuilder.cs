// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
namespace HMI.Runtime.Shared
{
	public class SignalNameBuilder
	{
		private const string PlaceholderN = "[N]";
		private const string PlaceholderName = "[Name]";

		private const string PlaceholderOID = "[OID]";
		private const string PlaceholderObjectID = "[ObjectID]";

		private const string PlaceholderPN = "[PN]";
		private const string PlaceholderParentName = "[ParentName]";

		private const string PlaceholderPCN = "[PCN]";
		private const string PlaceholderParentControlName = "[ParentControlName]";

		private const string PlaceholderRegexPC = "(\\[PC\\:(([^\\]])*)\\])";
		private const string PlaceholderRegexParamControl = "(\\[ParamControl\\:(([^\\]])*)\\])";

		private const string PropertyObjectID = "ObjectID";
		private SignalNameBuilder()
		{
		}
		public static string BuildSignalName(string signalNameToBuild, Control ctrl)
		{
            if (signalNameToBuild == null)
            {
                throw new ArgumentNullException("signalNameToBuild");
            }
            if (ctrl == null)
            {
                throw new ArgumentNullException("ctrl");
            }
            string input = signalNameToBuild;
            input = Regex.Replace(Regex.Replace(input, Regex.Escape(PlaceholderN), ctrl.Name, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase), Regex.Escape(PlaceholderName), ctrl.Name, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
            PropertyInfo property = ctrl.GetType().GetProperty(PropertyObjectID, BindingFlags.FlattenHierarchy | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (property != null)
            {
                string replacement = property.GetValue(ctrl, null) as string;
                if (replacement != null)
                {
                    input = Regex.Replace(Regex.Replace(input, Regex.Escape(PlaceholderOID), replacement, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase), Regex.Escape(PlaceholderObjectID), replacement, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
                }
            }
            FrameworkElement element = ctrl.Parent as FrameworkElement;
            if (element != null)
            {
                input = Regex.Replace(Regex.Replace(input, Regex.Escape(PlaceholderPN), element.Name, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase), Regex.Escape(PlaceholderParentName), element.Name, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
            }
            UserControl source = null;
            if ((input.IndexOf(PlaceholderPCN, StringComparison.InvariantCultureIgnoreCase) >= 0) || (input.IndexOf(PlaceholderParentControlName, StringComparison.InvariantCultureIgnoreCase) >= 0))
            {
                source = FindParentUserControl(ctrl);
                if (source != null)
                {
                    input = Regex.Replace(Regex.Replace(input, Regex.Escape(PlaceholderPCN), source.Name, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase), Regex.Escape(PlaceholderParentControlName), source.Name, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
                }
            }
            if (!Regex.IsMatch(input, PlaceholderRegexPC, RegexOptions.IgnoreCase) && !Regex.IsMatch(input, PlaceholderRegexParamControl, RegexOptions.IgnoreCase))
            {
                return input;
            }
            MatchEvaluator evaluator = null;
            MatchEvaluator evaluator2 = null;
            IHMINavigationParameter wfNavigationParameter = null;
            if (source == null)
            {
                source = FindParentUserControl(ctrl);
            }
            if (source != null)
            {
                wfNavigationParameter = NavigationHelper.FindParameterControl(source);
            }
            if (wfNavigationParameter == null)
            {
                wfNavigationParameter = NavigationHelper.FindParameterControl(Application.Current.MainWindow);
            }
            if (wfNavigationParameter == null)
            {
                return input;
            }
            if (evaluator == null)
            {
                evaluator = delegate(Match m)
                {
                    string str = string.Empty;
                    using (IEnumerator<ParameterItem> enumerator = wfNavigationParameter.Parameters.GetEnumerator())
                    {
                        ParameterItem current;
                        while (enumerator.MoveNext())
                        {
                            current = enumerator.Current;
                            if (string.Equals(current.ParameterName, m.Groups[2].Value, StringComparison.InvariantCultureIgnoreCase))
                            {
                                goto Label_004A;
                            }
                        }
                        return str;
                    Label_004A:
                        str = current.ParameterValue;
                    }
                    return str;
                };
            }
            if (evaluator2 == null)
            {
                evaluator2 = delegate(Match m)
                {
                    string str = string.Empty;
                    using (IEnumerator<ParameterItem> enumerator = wfNavigationParameter.Parameters.GetEnumerator())
                    {
                        ParameterItem current;
                        while (enumerator.MoveNext())
                        {
                            current = enumerator.Current;
                            if (string.Equals(current.ParameterName, m.Groups[2].Value, StringComparison.InvariantCultureIgnoreCase))
                            {
                                goto Label_004A;
                            }
                        }
                        return str;
                    Label_004A:
                        str = current.ParameterValue;
                    }
                    return str;
                };
            }
            return Regex.Replace(Regex.Replace(input, PlaceholderRegexPC, evaluator, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase), PlaceholderRegexParamControl, evaluator2, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
 
		}
		private static UserControl FindParentUserControl(Control ctrl)
		{
			UserControl userControl = null;
			for (DependencyObject parent = ctrl.Parent; parent != null; parent = VisualTreeHelper.GetParent(parent))
			{
				userControl = (parent as UserControl);
				if (userControl != null)
				{
					break;
				}
			}
			return userControl;
		}
	}
}
