// SPDX-License-Identifier: GPL-3.0-only
//using Microsoft.Win32;
//using System.ComponentModel;
//using System.Runtime.InteropServices;
//using System.Security;
//using System.Security.Permissions;

//namespace HMI.Common.Presentation.Controls
//{
//    internal static class WinApiHelper
//    {
//        public delegate int Delegate0(IntPtr hwnd, int msg, IntPtr lParam, IntPtr lpData);

//        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
//        public class Class5
//        {
//            public IntPtr intptr_0;
//            public IntPtr intptr_1;
//            public IntPtr intptr_2;
//            public string string_0;
//            public int int_0;
//            public WinApiHelper.Delegate0 delegate0_0;
//            public IntPtr intptr_3;
//            public int int_1;
//        }

//        [Guid("00000002-0000-0000-c000-000000000046"),
//			InterfaceType(ComInterfaceType.InterfaceIsIUnknown), 
//			SuppressUnmanagedCodeSecurity]
//        [ComImport]
//        public interface Interface0
//        {
//            [PreserveSig]
//            IntPtr imethod_0(int int_0);
//            [PreserveSig]
//            IntPtr imethod_1(IntPtr intptr_0, int int_0);
//            [PreserveSig]
//            void imethod_2(IntPtr intptr_0);
//            [PreserveSig]
//            int imethod_3(IntPtr intptr_0);
//            [PreserveSig]
//            int imethod_4(IntPtr intptr_0);
//            [PreserveSig]
//            void imethod_5();
//        }
//        [SuppressUnmanagedCodeSecurity]
//        internal static class Class6
//        {
//        }
//        internal static class Class7
//        {
//        }
//    }

//    public class FolderBrowserDialogEx : CommonDialog
//	{
//        [DllImport("shell32.dll")]
//        private static extern int SHGetMalloc([MarshalAs(UnmanagedType.LPArray)][Out] WinApiHelper.Interface0[] interface0_0);
//        [DllImport("shell32.dll")]
//        private static extern int SHGetSpecialFolderLocation(IntPtr intptr_0, int int_0, ref IntPtr intptr_1);
//        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
//        private static extern IntPtr SHBrowseForFolder([In] WinApiHelper.Class5 class5_0);
//        [DllImport("user32.dll", CharSet = CharSet.Auto)]
//        private static extern IntPtr SendMessage(HandleRef handleRef_0, int int_0, int int_1, int int_2);
//        [DllImport("user32.dll", CharSet = CharSet.Auto, EntryPoint = "SendMessage")]
//        private static extern IntPtr SendMessage_1(HandleRef handleRef_0, int int_0, int int_1, string string_0);
//        [DllImport("user32.dll", SetLastError = true)]
//        private static extern IntPtr FindWindowEx(HandleRef handleRef_0, IntPtr intptr_0, string string_0, string string_1);
//        [DllImport("gdi32.dll")]
//        private static extern bool DeleteObject(IntPtr intptr_0);
//        [DllImport("user32.dll", SetLastError = true)]
//        private static extern bool SetWindowText(IntPtr intptr_0, string string_0);
//        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
//        private static extern bool SHGetPathFromIDList(IntPtr intptr_0, IntPtr intptr_1);


//  //      private class Class1
//		//{
//		//	public const int int_0 = 0;
//		//	public const int int_1 = 4096;
//		//	public const int int_2 = 8192;
//		//	public const int int_3 = 16384;
//		//	public const int int_4 = 128;
//		//	public const int int_5 = 2;
//		//	public const int int_6 = 16;
//		//	public const int int_7 = 64;
//		//	public const int int_8 = 512;
//		//	public const int int_9 = 8;
//		//	public const int int_10 = 1;
//		//	public const int int_11 = 32768;
//		//	public const int int_12 = 4;
//		//	public const int int_13 = 256;
//		//	public const int int_14 = 32;
//		//	public const int int_15 = 1024;
//		//}

//		//private static class Class2
//		//{
//		//	public const int int_0 = 1;
//		//	public const int int_1 = 2;
//		//	public const int int_2 = 3;
//		//	public const int int_3 = 4;
//		//	public const int int_4 = 5;
//		//	public const int int_5 = 1124;
//		//	public const int int_6 = 1125;
//		//	public const int int_7 = 1126;
//		//	public const int int_8 = 1127;
//		//}
//		//private class Class3
//		//{
//		//	public const int int_0 = 4;
//		//	public const int int_1 = 18;
//		//}


//		private static readonly int int_0 = 260;
//		private WinApiHelper.Delegate0 delegateMethod;
//		private string _description;
//		private bool bool_0;
//		private IntPtr intptr_0;
//		private bool bool_1 = true;
//		private Environment.SpecialFolder _rootFolder;
//		internal IntPtr intptr_1;
//		private string _selectedPath;
//		private bool bool_2;
//		private bool bool_3;
//		private bool bool_4;
//		private bool bool_5 = true;
//		private bool bool_6;
//		internal int int_1;

//		public string Description
//		{
//			get
//			{
//				return this._description;
//			}
//			set
//			{
//				this._description = (value ?? string.Empty);
//			}
//		}
//		public Environment.SpecialFolder RootFolder
//		{
//			get
//			{
//				return this._rootFolder;
//			}
//			set
//			{
//				if (!Enum.IsDefined(typeof(Environment.SpecialFolder), value))
//				{
//					throw new InvalidEnumArgumentException("value", (int)value, typeof(Environment.SpecialFolder));
//				}
//				this._rootFolder = value;
//			}
//		}
//		public string SelectedPath
//		{
//			get
//			{
//				if (!string.IsNullOrEmpty(this._selectedPath) && this.bool_2)
//				{
//					new FileIOPermission(FileIOPermissionAccess.PathDiscovery, this._selectedPath).Demand();

//					this.bool_2 = false;
//				}
//				return this._selectedPath;
//			}
//			set
//			{
//				this._selectedPath = (value ?? string.Empty);
//				this.bool_2 = true;
//			}
//		}
//		public bool ShowNewFolderButton
//		{
//			get
//			{
//				return this.bool_6;
//			}
//			set
//			{
//				this.bool_6 = value;
//			}
//		}
//		public bool ShowEditBox
//		{
//			get
//			{
//				return this.bool_4;
//			}
//			set
//			{
//				this.bool_4 = value;
//			}
//		}
//		public bool NewStyle
//		{
//			get
//			{
//				return this.bool_1;
//			}
//			set
//			{
//				this.bool_1 = value;
//			}
//		}
//		public bool DontIncludeNetworkFoldersBelowDomainLevel
//		{
//			get
//			{
//				return this.bool_0;
//			}
//			set
//			{
//				this.bool_0 = value;
//			}
//		}
//		public bool ShowFullPathInEditBox
//		{
//			get
//			{
//				return this.bool_5;
//			}
//			set
//			{
//				this.bool_5 = value;
//			}
//		}
//		public bool ShowBothFilesAndFolders
//		{
//			get
//			{
//				return this.bool_3;
//			}
//			set
//			{
//				this.bool_3 = value;
//			}
//		}
//		public FolderBrowserDialogEx()
//		{
//			this.Reset();
//		}
//		public static FolderBrowserDialogEx PrinterBrowser()
//		{
//			FolderBrowserDialogEx folderBrowserDialogEx = new FolderBrowserDialogEx();
//			smethod_17(folderBrowserDialogEx);
//			return folderBrowserDialogEx;
//		}
//        private static void smethod_17(FolderBrowserDialogEx folderBrowserDialogEx_0)
//        {
//            folderBrowserDialogEx_0.int_1 += 8192;
//            folderBrowserDialogEx_0.Description = "Select a printer:";
//            SHGetSpecialFolderLocation(IntPtr.Zero, 4, ref folderBrowserDialogEx_0.intptr_1);
//            folderBrowserDialogEx_0.ShowNewFolderButton = false;
//            folderBrowserDialogEx_0.ShowEditBox = false;
//        }
        

//        public static FolderBrowserDialogEx ComputerBrowser()
//		{
//			FolderBrowserDialogEx folderBrowserDialogEx = new FolderBrowserDialogEx();
//			smethod_25(folderBrowserDialogEx);
//			return folderBrowserDialogEx;
//		}

//        private static void smethod_25(FolderBrowserDialogEx folderBrowserDialogEx_0)
//        {
//            folderBrowserDialogEx_0.int_1 += 4096;
//            folderBrowserDialogEx_0.Description = "Select a computer:";
//            SHGetSpecialFolderLocation(IntPtr.Zero, 18, ref folderBrowserDialogEx_0.intptr_1);
//            folderBrowserDialogEx_0.ShowNewFolderButton = false;
//            folderBrowserDialogEx_0.ShowEditBox = false;
//        }

//        private int callbackMethod(IntPtr intptr_2, int int_2, IntPtr intptr_3, IntPtr intptr_4)
//		{
//			switch (int_2)
//			{
//			case 1:
//				if (this._selectedPath.Length != 0)
//				{
//					SendMessage_1(new HandleRef(null, intptr_2), 1127, 1, this._selectedPath);
//					if (this.bool_4 && this.bool_5)
//					{
//						this.intptr_0 = FindWindowEx(new HandleRef(null, intptr_2), IntPtr.Zero, "Edit", null);
//						SetWindowText(this.intptr_0, this._selectedPath);
//					}
//				}
//				break;

//			case 2:
//				if (intptr_3 != IntPtr.Zero)
//				{
//					if ((this.int_1 & 8192) != 8192)
//					{
//						if ((this.int_1 & 4096) != 4096)
//						{
//							IntPtr intPtr = Marshal.AllocHGlobal(FolderBrowserDialogEx.int_0 * Marshal.SystemDefaultCharSize);
//							bool bool_1234 = SHGetPathFromIDList(intptr_3, intPtr);
//							string value = Marshal.PtrToStringAuto(intPtr);
//							Marshal.FreeHGlobal(intPtr);
//							SendMessage(new HandleRef(null, intptr_2), 1125, 0, bool_1234 ? 1 : 0);
//							if (!bool_1234 || string.IsNullOrEmpty(value))
//							{
//								break;
//							}
//							if (this.bool_4 && this.bool_5 && this.intptr_0 != IntPtr.Zero)
//							{
//								SetWindowText(this.intptr_0, value);
//							}
//							if ((this.int_1 & 4) == 4)
//							{
//								SendMessage_1(new HandleRef(null, intptr_2), 1124, 0, value);
//								break;
//							}
//							break;
//						}
//					}
//					SendMessage(new HandleRef(null, intptr_2), 1125, 0, 1);
//				}
//				break;
//			}
//			return 0;
//		}
//		public override void Reset()
//		{
//			this._rootFolder = Environment.SpecialFolder.Desktop;
//			this._description = string.Empty;
//			this._selectedPath = string.Empty;
//			this.bool_2 = false;
//			this.bool_6 = true;
//			this.bool_4 = true;
//			this.bool_1 = true;
//			this.bool_0 = false;
//			this.intptr_0 = IntPtr.Zero;
//			this.intptr_1 = IntPtr.Zero;
//		}
//		protected override bool RunDialog(IntPtr hWndOwner)
//		{
//			bool result = false;
//			if (this.intptr_1 == IntPtr.Zero)
//			{
//				SHGetSpecialFolderLocation(hWndOwner, (int)this._rootFolder, ref this.intptr_1);
//				if (this.intptr_1 == IntPtr.Zero)
//				{
//					SHGetSpecialFolderLocation(hWndOwner, 0, ref this.intptr_1);
//					if (this.intptr_1 == IntPtr.Zero)
//					{
//						throw new InvalidOperationException("FolderBrowserDialogNoRootFolder");
//					}
//				}
//			}
//			this.intptr_0 = IntPtr.Zero;
//			if (this.bool_0)
//			{
//				this.int_1 += 2;
//			}
//			if (this.bool_1)
//			{
//				this.int_1 += 64;
//			}
//			if (!this.bool_6)
//			{
//				this.int_1 += 512;
//			}
//			if (this.bool_4)
//			{
//				this.int_1 += 16;
//			}
//			if (this.bool_3)
//			{
//				this.int_1 += 16384;
//			}
//			if (Thread.CurrentThread.GetApartmentState() != ApartmentState.STA)
//			{
//				throw new ThreadStateException("DebuggingException: ThreadMustBeSTA");
//			}
//			IntPtr value = IntPtr.Zero;
//			IntPtr intPtr = IntPtr.Zero;
//			IntPtr intPtr2 = IntPtr.Zero;
//			try
//			{
//				WinApiHelper.Class5 @class = new WinApiHelper.Class5();
//				intPtr = Marshal.AllocHGlobal(FolderBrowserDialogEx.int_0 * Marshal.SystemDefaultCharSize);
//				intPtr2 = Marshal.AllocHGlobal(FolderBrowserDialogEx.int_0 * Marshal.SystemDefaultCharSize);
//				this.delegateMethod = new WinApiHelper.Delegate0(this.callbackMethod);
//				@class.intptr_1 = this.intptr_1;
//				@class.intptr_0 = hWndOwner;
//				@class.intptr_2 = intPtr;
//				@class.string_0 = this._description;
//				@class.int_0 = this.int_1;
//				@class.delegate0_0 = this.delegateMethod;
//				@class.intptr_3 = IntPtr.Zero;
//				@class.int_1 = 0;
//				value = SHBrowseForFolder(@class);
//				if ((this.int_1 & 8192) != 8192)
//				{
//					if ((this.int_1 & 4096) != 4096)
//					{
//						if (value != IntPtr.Zero)
//						{
//							SHGetPathFromIDList(value, intPtr2);
//							this.bool_2 = true;
//							this._selectedPath = Marshal.PtrToStringAuto(intPtr2);
//							result = true;
//							goto IL_213;
//						}
//						goto IL_213;
//					}
//				}
//				this._selectedPath = Marshal.PtrToStringAuto(@class.intptr_2);
//				result = true;
//				IL_213:;
//			}
//			finally
//			{
//				WinApiHelper.Interface0 @interface = smethod_31();
//				@interface.imethod_2(this.intptr_1);
//				this.intptr_1 = IntPtr.Zero;
//				if (value != IntPtr.Zero)
//				{
//					@interface.imethod_2(value);
//				}
//				if (intPtr2 != IntPtr.Zero)
//				{
//					Marshal.FreeHGlobal(intPtr2);
//				}
//				if (intPtr != IntPtr.Zero)
//				{
//					Marshal.FreeHGlobal(intPtr);
//				}
//				this.delegateMethod = null;
//			}
//			return result;
//		}
//        private static WinApiHelper.Interface0 smethod_31()
//        {
//            WinApiHelper.Interface0[] array = new WinApiHelper.Interface0[1];
//            SHGetMalloc(array);
//            return array[0];
//        }
//    }
//}
