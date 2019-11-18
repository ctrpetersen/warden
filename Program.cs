using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Terminal.Gui;
using Warden.Applications;
using Application = Terminal.Gui.Application;
using Attribute = Terminal.Gui.Attribute;

namespace Warden
{
    class Program
    {
        static void Main(string[] args)
        {
            var mantis = new Mantis();
            mantis.NewOutput += a_Output;
            mantis.StartApp();



/*            Application.Init();
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

            Application.Run();*/



            Console.ReadLine();
        }

        private static void a_Output(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }
    }
}
