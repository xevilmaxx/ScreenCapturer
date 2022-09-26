using ScreenCapturer.Common;
using ScreenCapturer.Gtk;
using ScreenCapturer.Gdi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScreenCapturer
{
    public class Factory
    {

        public ICommon GetPlatform()
        {
            try
            {

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    return new GtkScreenshot();
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return new GdiScreenshot();
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
