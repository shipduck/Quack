using System;
using Gtk;

namespace Quack.GTK
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Application.Init();
            MainWindow win = new MainWindow();
            win.Show();
            win.Title = "Quaks";
            Application.Run();
        }
    }
}
