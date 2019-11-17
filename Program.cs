using System;
using Terminal.Gui;
using Attribute = Terminal.Gui.Attribute;

namespace Warden
{
    class Program
    {
        static void Main(string[] args)
        {
            Application.Init();
            var top = Application.Top;

            var win = new Window("Warden")
            {
                X = 0,
                Y = 0,

                Width = Dim.Fill(),
                Height = Dim.Fill()
            };

            win.ColorScheme.Normal = Attribute.Make(Color.BrightGreen, Color.Black);

            top.Add(win);

            var debugText = new Label("test"){X = 0, Y = 0};
            win.Add(debugText);

            Application.Run();
        }
    }
}
