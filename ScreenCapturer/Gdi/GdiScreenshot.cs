using GLib;
using NetCoreEx.Geometry;
using ScreenCapturer.Common;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows;
using WinApi.Gdi32;
using WinApi.User32;
using WinApi.User32.Experimental;
using DateTime = System.DateTime;
using Rectangle = NetCoreEx.Geometry.Rectangle;

//V1
//https://github.com/eladkarako/kaptureScreen/blob/master/kaptureScreen/ScreenCapture.cs
//V2
//https://gist.github.com/holmesconan/23fdbd3ce16ee411f885
namespace ScreenCapturer.Gdi
{
    public class GdiScreenshot : ICommon
    {
        
        public byte[] TakeScreenshot()
        {

            Console.WriteLine("Windows ScreenShot invoked!");

            // capture entire screen, and save it to a file
            string sFileName = string.Format("screenshot-{0:hh-mm-ss_dd-MM-yyyy}.jpg", DateTime.Now);
            CaptureScreenToFile(@".\" + sFileName, ImageFormat.Jpeg);

            var result = CaptureScreenToArray(ImageFormat.Jpeg);

            Console.WriteLine($"Grabbed bytes: {result.Length}");

            return result;

        }

        public Image CaptureScreen()
        {
            //no real difference between 2 methods found for now
            //return CaptureWindow();
            return CaptureScreenV2();
        }

        /// <summary>
        /// Works only when you are not logged in with Remote Desktop
        /// </summary>
        /// <returns></returns>
        public Image CaptureWindow()
        {

            IntPtr handle = User32Methods.GetDesktopWindow();

            // get te hDC of the target window
            IntPtr hdcSrc = User32Methods.GetWindowDC(handle);
            // get the size
            Rectangle windowRect = new Rectangle();
            User32Methods.GetWindowRect(handle, out windowRect);

            // create a device context we can copy to
            IntPtr hdcDest = Gdi32Methods.CreateCompatibleDC(hdcSrc);
            // create a bitmap we can copy it to,
            // using GetDeviceCaps to get the width/height
            IntPtr hBitmap = Gdi32Methods.CreateCompatibleBitmap(hdcSrc, windowRect.Width, windowRect.Height);
            // select the bitmap object
            IntPtr hOld = Gdi32Methods.SelectObject(hdcDest, hBitmap);
            // bitblt over
            Gdi32Methods.BitBlt(hdcDest, 0, 0, windowRect.Width, windowRect.Height, hdcSrc, 0, 0, BitBltFlags.SRCCOPY);
            // restore selection
            Gdi32Methods.SelectObject(hdcDest, hOld);
            // clean up 
            Gdi32Methods.DeleteDC(hdcDest);
            User32Methods.ReleaseDC(handle, hdcSrc);

            // get a .NET image object for it
            Image img = Image.FromHbitmap(hBitmap);
            // free up the Bitmap object
            Gdi32Methods.DeleteObject(hBitmap);

            return img;

        }

        /// <summary>
        /// Works only when you are not logged in with Remote Desktop
        /// </summary>
        /// <returns></returns>
        private Image CaptureScreenV2()
        {

            var hdc = User32Methods.GetDC((IntPtr)null); // get the desktop device context
            var hDest = Gdi32Methods.CreateCompatibleDC(hdc); // create a device context to use yourself

            // get the height and width of the screen
            int height = User32Methods.GetSystemMetrics(SystemMetrics.SM_CYVIRTUALSCREEN);
            int width = User32Methods.GetSystemMetrics(SystemMetrics.SM_CXVIRTUALSCREEN);

            // create a bitmap
            var hbDesktop = Gdi32Methods.CreateCompatibleBitmap(hdc, width, height);

            // use the previously created device context with the bitmap
            Gdi32Methods.SelectObject(hDest, hbDesktop);

            // copy from the desktop device context to the bitmap device context
            // call this once per 'frame'
            Gdi32Methods.BitBlt(hDest, 0, 0, width, height, hdc, 0, 0, BitBltFlags.SRCCOPY);

            // after the recording is done, release the desktop context you got..
            User32Methods.ReleaseDC((IntPtr)null, hdc);

            // ..and delete the context you created
            Gdi32Methods.DeleteDC(hDest);

            // get a .NET image object for it
            Image img = Image.FromHbitmap(hbDesktop);
            // free up the Bitmap object
            Gdi32Methods.DeleteObject(hbDesktop);

            return img;

        }

        public void CaptureWindowToFile(string filename, ImageFormat format)
        {
            Image img = CaptureWindow();
            img.Save(filename, format);
        }

        public void CaptureScreenToFile(string filename, ImageFormat format)
        {
            Image img = CaptureScreen();
            img.Save(filename, format);
        }

        public byte[] CaptureScreenToArray(ImageFormat format)
        {
            Image img = CaptureScreen();
            using (MemoryStream mStream = new MemoryStream())
            {
                img.Save(mStream, format);
                return mStream.ToArray();
            }
        }



        //private class GDI32
        //{

        //    public const int SRCCOPY = 0x00CC0020; // BitBlt dwRop parameter

        //    [DllImport("gdi32.dll")]
        //    public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest,
        //        int nWidth, int nHeight, IntPtr hObjectSource,
        //        int nXSrc, int nYSrc, int dwRop);
        //    [DllImport("gdi32.dll")]
        //    public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth,
        //        int nHeight);
        //    [DllImport("gdi32.dll")]
        //    public static extern IntPtr CreateCompatibleDC(IntPtr hDC);
        //    [DllImport("gdi32.dll")]
        //    public static extern bool DeleteDC(IntPtr hDC);
        //    [DllImport("gdi32.dll")]
        //    public static extern bool DeleteObject(IntPtr hObject);
        //    [DllImport("gdi32.dll")]
        //    public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
        //}

        //private class User32
        //{
        //    [StructLayout(LayoutKind.Sequential)]
        //    public struct RECT
        //    {
        //        public int left;
        //        public int top;
        //        public int right;
        //        public int bottom;
        //    }

        //    [DllImport("user32.dll")]
        //    public static extern IntPtr GetDesktopWindow();
        //    [DllImport("user32.dll")]
        //    public static extern IntPtr GetWindowDC(IntPtr hWnd);
        //    [DllImport("user32.dll")]
        //    public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);
        //    [DllImport("user32.dll")]
        //    public static extern IntPtr GetWindowRect(IntPtr hWnd, ref RECT rect);

        //}

    }
}
