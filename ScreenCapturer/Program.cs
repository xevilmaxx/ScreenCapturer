using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace ScreenCapturer
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Hello, World!");

            var platform = new Factory().GetPlatform();

            var screenshot = platform.TakeScreenshot();

            Console.ReadLine();

        }

    }
}