using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace Rhiannon.Extensions
{
	public static class BitmapExtensions
	{
		[DllImport("gdi32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool DeleteObject(IntPtr handle);

		public static BitmapSource ToBitmapSource(this Bitmap bitmap)
		{
			if (bitmap == null)
			{
				return null;
			}
			BitmapSource bitmapSource;
			IntPtr hBitmap = bitmap.GetHbitmap();
			try
			{
				bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
			}
			catch (Win32Exception)
			{
				bitmapSource = null;
			}
			finally
			{
				DeleteObject(hBitmap);
			}
			return bitmapSource;
		}

		//public static BitmapSource Test(this Bitmap icon)
		//{
		//    if (icon == null)
		//    {
		//        return null;
		//    }
		//    BitmapSource bitmapSource;
		//    try
		//    {
		//        //BitmapImage image = new BitmapImage(new Uri("pack://application:,,,/Chronos.Shell.Win.Resources;component/images/Profiler_MainView_Icon.ico", UriKind.RelativeOrAbsolute));
		//        bitmapSource = icon.ToBitmapSource();
		//        BitmapSource image = new WriteableBitmap(bitmapSource);
		//        bitmapSource = BitmapFrame.Create(image);
		//        //using (MemoryStream stream = new MemoryStream())
		//        //{
		//        //    icon.Save(stream, ImageFormat.Png);
		//        //    stream.Seek(0, SeekOrigin.Begin);
		//        //    IconBitmapDecoder decoder = new IconBitmapDecoder(stream, BitmapCreateOptions.None, BitmapCacheOption.Default);
		//        //    bitmapSource = decoder.Frames[0];
		//        //    //bitmapSource = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

		//        //    //Image image = Image.FromStream(stream);
		//        //    //bitmapSource = BitmapFrame.Create(stream, BitmapCreateOptions.IgnoreColorProfile, BitmapCacheOption.Default);
		//        //    bitmapSource = new CachedBitmap(bitmapSource, BitmapCreateOptions.None, BitmapCacheOption.Default);
		//        //}
		//        //using (var iconStream = new MemoryStream())
		//        //{
		//        //    Icon icon = Icon.FromHandle(hBitmap);
		//        //    icon.Save(iconStream);
		//        //    iconStream.Seek(0, SeekOrigin.Begin);
		//        //    bitmapSource = BitmapFrame.Create(iconStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
		//        //}
		//        //bitmapSource = Imaging.CreateBitmapSourceFromHIcon(bitmap.GetHicon(), Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
		//    }
		//    catch (Win32Exception)
		//    {
		//        bitmapSource = null;
		//    }
		//    return bitmapSource;
		//}

		//public static BitmapSource ToBitmapSource(this Icon icon)
		//{
		//    if (icon == null)
		//    {
		//        return null;
		//    }
		//    BitmapSource bitmapSource;
		//    try
		//    {
		//        using (MemoryStream stream = new MemoryStream())
		//        {
		//            icon.Save(stream);
		//            stream.Seek(0, SeekOrigin.Begin);
		//            IconBitmapDecoder decoder = new IconBitmapDecoder(stream, BitmapCreateOptions.None, BitmapCacheOption.Default);
		//            bitmapSource = decoder.Frames[0];
		//            //bitmapSource = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

		//            //Image image = Image.FromStream(stream);
		//            //bitmapSource = BitmapFrame.Create(stream, BitmapCreateOptions.IgnoreColorProfile, BitmapCacheOption.Default);
		//            bitmapSource = new CachedBitmap(bitmapSource, BitmapCreateOptions.None, BitmapCacheOption.Default);
		//        }
		//        //using (var iconStream = new MemoryStream())
		//        //{
		//        //    Icon icon = Icon.FromHandle(hBitmap);
		//        //    icon.Save(iconStream);
		//        //    iconStream.Seek(0, SeekOrigin.Begin);
		//        //    bitmapSource = BitmapFrame.Create(iconStream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
		//        //}
		//        //bitmapSource = Imaging.CreateBitmapSourceFromHIcon(bitmap.GetHicon(), Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
		//    }
		//    catch (Win32Exception)
		//    {
		//        bitmapSource = null;
		//    }
		//    return bitmapSource;
		//}
	}
}
