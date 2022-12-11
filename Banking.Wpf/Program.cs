using System;

using Banking.UI;
using Eto.Forms;

namespace Banking.Wpf
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            new Application(Eto.Platforms.Wpf).Run(new MainForm(args));
        }
    }
}