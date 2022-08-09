using System;
using Banking.UI;
using Eto.Forms;

namespace Banking.Gtk
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            new Application(Eto.Platforms.Gtk).Run(new MainForm());
        }
    }
}