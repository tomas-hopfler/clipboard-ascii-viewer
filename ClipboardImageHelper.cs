using System.Drawing;
using System.Runtime.InteropServices;

namespace CIH
{
	public class ClipboardImageHelper
	{
		[DllImport("user32.dll")]
		static extern bool OpenClipboard(IntPtr hWndNewOwner);

		[DllImport("user32.dll")]
		static extern bool CloseClipboard();

		[DllImport("user32.dll")]
		static extern bool IsClipboardFormatAvailable(uint format);

		[DllImport("user32.dll")]
		static extern IntPtr GetClipboardData(uint uFormat);

		const uint CF_BITMAP = 2;

		public static Image GetImageFromClipboard()
		{
			if (!OpenClipboard(IntPtr.Zero))
			{
				Console.WriteLine("Failed to open clipboard.");
				return null;
			}

			try
			{
				if (!IsClipboardFormatAvailable(CF_BITMAP))
				{
					Console.WriteLine("No bitmap in clipboard.");
					return null;
				}

				IntPtr handle = GetClipboardData(CF_BITMAP);
				if (handle == IntPtr.Zero)
				{
					Console.WriteLine("Failed to get clipboard data.");
					return null;
				}

				return Image.FromHbitmap(handle);
			}
			finally
			{
				CloseClipboard();
			}
		}
	}
}
