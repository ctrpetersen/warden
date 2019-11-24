using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Warden.Applications;

namespace Warden
{
    class Warden
    {
        private static readonly List<Application> TrackedApps = new List<Application>();

        public void Start()
        {
            var mantis = new Mantis();
            mantis.NewOutput += RefreshScreen;

            TrackedApps.Add(mantis);

            StartAll();

            Console.ReadLine();
        }

        private void RefreshScreen(object sender, DataReceivedEventArgs e)
        {
            Console.Clear();
            foreach (var app in TrackedApps)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"{app.Process.ProcessName} HEALTH: {app.AppHealth()} {app.SecondsSinceLastTick} seconds since tick");
                Console.ResetColor();
                Console.WriteLine(app.LatestOutput);
                Console.WriteLine();
            }
        }

        private void StartAll()
        {
            foreach (var app in TrackedApps)
            {
                app.StartApp();
            }
        }

        private void CloseAll()
        {
            foreach (var app in TrackedApps)
            {
                app.CloseApp();
            }
        }
    }
}
