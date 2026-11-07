// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows.Input;
using HMI.Runtime.Shared;
namespace HMI.RuntimeControls.InputOutput
{
	public class KeyPressedHandleChecker : IKeyPressedHandleChecker
	{
		public TextBoxInformation TextBoxInformation
		{
			get;
			set;
		}
		public NumberFormat NumberFormat
		{
			get;
			set;
		}
		public bool ShouldHandlePressedKey(PressedKeyInformation keyInformation)
		{
			if (this.IsNonAlphaNumeric(keyInformation.Key))
			{
				return true;
			}
			if (this.IsNumericKey(keyInformation.Key))
			{
				return (!this.IsNumberNegative() || this.TextBoxInformation.SelectionStart != 0) && (!this.HasDecimalSeparator() || this.SelectionStartsBeforeDecimalSeparator() || this.TextBoxInformation.SelectionLength > 0 || this.GetNumberOfDigitsAfterDecimalSeparator() < this.TextBoxInformation.Precision);
			}
			if (this.IsCommaOrDot(keyInformation) && this.TextBoxInformation.Precision > 0)
			{
				if (!this.HasDecimalSeparator() && this.GetNumberOfCharsAfterSelection() <= this.TextBoxInformation.Precision)
				{
					return true;
				}
				if (this.HasSelection() && this.SelectionStartsBeforeDecimalSeparator() && this.SelectionIncludesDecimalSeparator())
				{
					return true;
				}
			}
			if (KeyPressedHandleChecker.IsNegativeSign(keyInformation))
			{
				if (this.GetNegativeSignIndex() == -1)
				{
					return this.TextBoxInformation.SelectionStart == 0;
				}
				if (this.SelectionIncludesNegativeSign())
				{
					return true;
				}
			}
			return false;
		}
		private static bool IsNegativeSign(PressedKeyInformation keyInformation)
		{
            return keyInformation.Key == Key.Subtract;
		}
		private bool SelectionIncludesDecimalSeparator()
		{
			return this.SelectionEndposition() > this.GetDecimalSeparatorIndex();
		}
		private bool SelectionIncludesNegativeSign()
		{
			int negativeSignIndex = this.GetNegativeSignIndex();
			return negativeSignIndex >= this.TextBoxInformation.SelectionStart && negativeSignIndex <= this.SelectionEndposition();
		}
		private int SelectionEndposition()
		{
			return this.TextBoxInformation.SelectionStart + this.TextBoxInformation.SelectionLength;
		}
		private bool HasSelection()
		{
			return this.TextBoxInformation.SelectionLength > 0;
		}
		private int GetNumberOfCharsAfterSelection()
		{
			return this.TextBoxInformation.Text.Length - this.TextBoxInformation.SelectionStart;
		}
		private bool IsCommaOrDot(PressedKeyInformation keyInformation)
		{
			return this.IsComma(keyInformation) || this.IsDot(keyInformation);
		}
		private bool IsDot(PressedKeyInformation keyInformation)
		{
            return this.NumberFormat.DecimalSeparator.Equals('.')
                && (keyInformation.Key == Key.Decimal);
		}
		private bool IsComma(PressedKeyInformation keyInformation)
		{
            return this.NumberFormat.DecimalSeparator.Equals(',')
                && (keyInformation.Key == Key.Decimal);
		}
		private int GetNumberOfDigitsAfterDecimalSeparator()
		{
			return this.TextBoxInformation.Text.Substring(this.GetDecimalSeparatorIndex()).Length - 1;
		}
		private bool IsNumberNegative()
		{
			int num = this.TextBoxInformation.Text.IndexOf(this.NumberFormat.NegativeSign);
			return num != -1;
		}
		private bool HasDecimalSeparator()
		{
			return this.GetDecimalSeparatorIndex() != -1;
		}
		private bool SelectionStartsBeforeDecimalSeparator()
		{
			return this.TextBoxInformation.SelectionStart <= this.GetDecimalSeparatorIndex();
		}
		private int GetDecimalSeparatorIndex()
		{
			return this.TextBoxInformation.Text.IndexOf(this.NumberFormat.DecimalSeparator);
		}
		private int GetNegativeSignIndex()
		{
			return this.TextBoxInformation.Text.IndexOf(this.NumberFormat.NegativeSign);
		}
		private bool IsNumericKey(Key key)
		{
            return key == Key.D0 || key == Key.D1 || key == Key.D2 || key == Key.D3 || key == Key.D4 || key == Key.D5 || key == Key.D6 || key == Key.D7 || key == Key.D8 || key == Key.D9 || key == Key.NumPad0 || key == Key.NumPad1 || key == Key.NumPad2 || key == Key.NumPad3 || key == Key.NumPad4 || key == Key.NumPad5 || key == Key.NumPad6 || key == Key.NumPad7 || key == Key.NumPad8 || key == Key.NumPad9;
		}
		private bool IsNonAlphaNumeric(Key key)
		{
			return key == Key.Back || key == Key.Down || key == Key.End || key == Key.Home || key == Key.Left || key == Key.Right || key == Key.Tab || key == Key.Up || key == Key.Delete || key == Key.Escape || key == Key.Enter;
		}
	}
}
