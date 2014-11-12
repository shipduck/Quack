using System;
using Gtk;
using Quack;

public partial class MainWindow : Gtk.Window, IPinAuthHandler
{
    public MainWindow()
        : base(Gtk.WindowType.Toplevel)
    {
        Build();
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }
}
