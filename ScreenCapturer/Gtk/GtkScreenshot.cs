using Gdk;
using ScreenCapturer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenCapturer.Gtk
{
    internal class GtkScreenshot : ICommon
    {

        public byte[] TakeScreenshot()
        {
            var args = new string[] { };
            var isInited = Global.InitCheck(ref args);

            Window root = Global.DefaultRootWindow;

            // create a new pixbuf from the root window
            Pixbuf screenshot = new Pixbuf(root, 0, 0, root.Width, root.Height);

            //https://docs.gtk.org/gdk-pixbuf/method.Pixbuf.save_to_buffer.html
            //“jpeg”, “png”, “tiff”, “ico” or “bmp”
            var result = screenshot.SaveToBuffer("jpeg");

            //test only
            screenshot.Save("./Test0.jpg", "jpeg");

            return result;

        }

    }
}
