// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Windows.Input;
namespace HMI.Runtime.Shared.KeyboardControl
{
	public static class KeyHandler
	{
		public static Key GetKeyFromString(string keyString)
		{
			int no = -1;
			Key result = 0;
			KeyHandler.GetKeyNumber(keyString, ref no);
			KeyHandler.GetKeyAdderKey(keyString, ref no);
			KeyHandler.GetKeyFromNumber(no, ref result);
			return result;
		}
        private static void GetKeyFromNumber(int no, ref Key key)
        {
            switch (no)
            {
                case 0:
                    key = Key.NumPad0;
                    return;
                case 1:
                    key = Key.NumPad1;
                    return;
                case 2:
                    key = Key.NumPad2;
                    return;
                case 3:
                    key = Key.NumPad3;
                    return;
                case 4:
                    key = Key.NumPad4;
                    return;
                case 5:
                    key = Key.NumPad5;
                    return;
                case 6:
                    key = Key.NumPad6;
                    return;
                case 7:
                    key = Key.NumPad7;
                    return;
                case 8:
                    key = Key.NumPad8;
                    return;
                case 9:
                    key = Key.NumPad9;
                    return;
                case 10:
                    key = Key.Subtract;
                    return;
                case 11:
                    key = Key.Enter;
                    return;
                case 12:
                    key = Key.Delete;
                    return;
                case 13:
                    key = Key.Decimal;
                    return;
                case 14:
                    key = Key.Delete;
                    return;
                case 15:
                    key = Key.LeftAlt;
                    return;
                case 16:
                    key = Key.LeftShift;
                    return;
                default:
                    key = Key.None;
                    return;
            }
        }
		private static void GetKeyAdderKey(string keyString, ref int no)
		{
            switch (keyString)
            {
                case "-":
                    no = 10;
                    return;

                case "Enter":
                    no = 11;
                    return;

                case "Delete":
                    no = 12;
                    return;

                case ".":
                    no = 13;
                    return;

                case ",":
                    no = 13;
                    return;

                case "Backspace":
                    no = 14;
                    return;

                case "Alt":
                    no = 15;
                    return;

                case "Shift":
                    no = 0x10;
                    return;
            }

		}
		private static void GetKeyNumber(string keyString, ref int no)
		{
			for (int i = 0; i < 10; i++)
			{
				if (keyString.Equals(i.ToString()))
				{
					no = i;
					return;
				}
			}
		}
	}
}
