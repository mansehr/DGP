using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Forms;

namespace DGP_Messenger
{
	/// <summary>
	/// Summary description for clsUser32Dll.
	/// </summary>
	public class clsUser32Dll
	{
		public enum FLASHWINFOFLAGS
		{
			FLASHW_STOP         =0,
			FLASHW_CAPTION      =0x00000001,
			FLASHW_TRAY         =0x00000002,
			FLASHW_ALL          =(FLASHW_CAPTION | FLASHW_TRAY),
			FLASHW_TIMER        =0x00000004,
			FLASHW_TIMERNOFG    =0x0000000C
		}

		public enum ShowWindowConstants
		{
			SW_HIDE = 0,
			SW_SHOWNORMAL = 1,
			SW_NORMAL = 1,
			SW_SHOWMINIMIZED = 2,
			SW_SHOWMAXIMIZED = 3,
			SW_MAXIMIZE = 3,
			SW_SHOWNOACTIVATE = 4,
			SW_SHOW = 5,
			SW_MINIMIZE = 6,
			SW_SHOWMINNOACTIVE = 7,
			SW_SHOWNA = 8,
			SW_RESTORE = 9,
			SW_SHOWDEFAULT = 10,
			SW_FORCEMINIMIZE = 11,
			SW_MAX = 11
		}


		[StructLayout(LayoutKind.Sequential)]
		public struct FLASHWINFO
		{
			public UInt32 cbSize;  
			public IntPtr hwnd;  
			public Int32 dwFlags;  
			public UInt32 uCount;  
			public Int32 dwTimeout;
		}

		[DllImport("user32.dll")]
		public static extern int FlashWindowEx(ref FLASHWINFO pfwi);

		[DllImport("User32.dll")]
		public static extern int ShowWindowAsync(IntPtr hWnd , int swCommand);




		public static int FlashWindowOnce(System.Windows.Forms.Form hWnd)
		{
			FLASHWINFO fw = new FLASHWINFO();
			fw.cbSize = Convert.ToUInt32(Marshal.SizeOf(typeof(FLASHWINFO)));
			fw.hwnd = hWnd.Handle;
			fw.dwFlags = (Int32)(FLASHWINFOFLAGS.FLASHW_ALL | FLASHWINFOFLAGS.FLASHW_TIMER);
			fw.dwTimeout = 500;
 
			return FlashWindowEx(ref fw);
		}

		public static int FlashWindowStop(System.Windows.Forms.Form hWnd)
		{
			FLASHWINFO fw = new FLASHWINFO();
			fw.cbSize = Convert.ToUInt32(Marshal.SizeOf(typeof(FLASHWINFO)));
			fw.hwnd = hWnd.Handle;
			fw.dwFlags = (Int32)( FLASHWINFOFLAGS.FLASHW_STOP);
			fw.dwTimeout = 0;
 
			return FlashWindowEx(ref fw);
		}

	}

	#region Class ExtractIcon
	/// <summary>
	/// Summary description for ExtractIcon.
	/// </summary>
	public class ExtractIcon
	{
		[DllImport("Shell32.dll")]
		private static extern IntPtr SHGetFileInfo
			(
			string pszPath,
			uint dwFileAttributes,
			out SHFILEINFO psfi,
			uint cbfileInfo,
			SHGFI uFlags
			);

		[DllImport("comctl32.dll")]
		private static extern int ImageList_GetImageCount(
			IntPtr himl
			);

		[DllImport("comctl32.dll")]
		private static extern IntPtr ImageList_GetIcon(
			IntPtr himl,
			int i,  
			uint flags 
			);

		[StructLayout(LayoutKind.Sequential)]
			private struct SHFILEINFO
		{
			public SHFILEINFO(bool b)
			{
				hIcon=IntPtr.Zero;iIcon=0;dwAttributes=0;szDisplayName="";szTypeName="";
			}
			public IntPtr hIcon;
			public int iIcon;
			public uint dwAttributes;
			[MarshalAs(UnmanagedType.LPStr, SizeConst=260)]
			public string szDisplayName;
			[MarshalAs(UnmanagedType.LPStr, SizeConst=80)]
			public string szTypeName;
		};

		private ExtractIcon()
		{
		}

		private enum SHGFI
		{
			SHGFI_ICON =             0x000000100,     // get icon
			SHGFI_DISPLAYNAME =      0x000000200,     // get display name
			SHGFI_TYPENAME =         0x000000400,     // get type name
			SHGFI_ATTRIBUTES =       0x000000800,     // get attributes
			SHGFI_ICONLOCATION =     0x000001000,     // get icon location
			SHGFI_EXETYPE =          0x000002000,     // return exe type
			SHGFI_SYSICONINDEX =     0x000004000,     // get system icon index
			SHGFI_LINKOVERLAY =      0x000008000,     // put a link overlay on icon
			SHGFI_SELECTED =         0x000010000,     // show icon in selected state
			SHGFI_ATTR_SPECIFIED =   0x000020000,     // get only specified attributes
			SHGFI_LARGEICON =        0x000000000,     // get large icon
			SHGFI_SMALLICON =        0x000000001,     // get small icon
			SHGFI_OPENICON =         0x000000002,     // get open icon
			SHGFI_SHELLICONSIZE =    0x000000004,     // get shell size icon
			SHGFI_PIDL =             0x000000008,     // pszPath is a pidl
			SHGFI_USEFILEATTRIBUTES = 0x000000010     // use passed dwFileAttribute
		}

		private enum SHIL
		{
			SHIL_LARGE =          0,   // normally 32x32
			SHIL_SMALL =          1,  // normally 16x16
			SHIL_EXTRALARGE =     2,
			SHIL_SYSSMALL =       3   // like SHIL_SMALL, but tracks system small icon metric correctly
		}

		/// <summary>
		/// Get the associated Icon for a file or application, this method always returns
		/// an icon.  If the strPath is invalid or there is no idonc the default icon is returned
		/// </summary>
		/// <param name="strPath">full path to the file</param>
		/// <param name="bSmall">if true, the 16x16 icon is returned otherwise the 32x32</param>
		/// <returns></returns>
		public static Icon GetIcon(string strPath, bool bSmall)
		{
			SHFILEINFO info = new SHFILEINFO(true);
			int cbFileInfo = Marshal.SizeOf(info);
			SHGFI flags;
			if (bSmall)
				flags = SHGFI.SHGFI_ICON|SHGFI.SHGFI_SMALLICON|SHGFI.SHGFI_USEFILEATTRIBUTES;
			else
				flags = SHGFI.SHGFI_ICON|SHGFI.SHGFI_LARGEICON|SHGFI.SHGFI_USEFILEATTRIBUTES;

			SHGetFileInfo(strPath, 256, out info,(uint)cbFileInfo, flags);
			return Icon.FromHandle(info.hIcon);
		}

		public static int GetIconIndex(string strPath, ImageList imgList)
		{
			SHFILEINFO info = new SHFILEINFO(true);
			int cbFileInfo = Marshal.SizeOf(info);
			SHGFI flags;
			IntPtr hIcon;

			flags = SHGFI.SHGFI_SYSICONINDEX|SHGFI.SHGFI_SMALLICON;

			IntPtr ret = SHGetFileInfo("c:\\", 256, out info, (uint)cbFileInfo, flags);
			int nbIcon = ImageList_GetImageCount(ret);

			for (int i = 0; i < nbIcon; i++)
			{
				hIcon = ImageList_GetIcon(ret, i, 0);
				imgList.Images.Add (Icon.FromHandle(hIcon));
			}

			return info.iIcon;
		}
	}
	#endregion
}
