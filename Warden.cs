using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;
using Warden.Applications;

namespace Warden
{
    class Warden
    {
        private List<Application> _trackedApps = new List<Application>();
        private readonly Timer _refreshTimer = new Timer(TimeSpan.FromSeconds(5).TotalMilliseconds);

        public void Start()
        {
            _refreshTimer.AutoReset = true;
            _refreshTimer.Elapsed += AutoRefreshScreen;
            _refreshTimer.Start();

            _trackedApps = ApplicationLoader.GetApplications("json.txt");

            foreach (var app in _trackedApps)
            {
                app.NewOutput += RefreshScreen;
            }

            StartAll();

            Console.ReadLine();
        }

        private void RefreshScreen(object sender, DataReceivedEventArgs e)
        {
            Console.Clear();
            foreach (var app in _trackedApps)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"{app.Process.ProcessName, 8}  |  {app.AppHealth(), 8}  |  {app.SecondsSinceLastTick} seconds since last tick  |  {MemInMb(app.Process.WorkingSet64)} mb");

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(app.LatestOutput + "\n");
            }
        }

        private void AutoRefreshScreen(object sender, ElapsedEventArgs e)
        {
            RefreshScreen(sender, null);
        }

        private void StartAll()
        {
            foreach (var app in _trackedApps)
            {
                app.StartApp();
            }
        }

        private void CloseAll()
        {
            foreach (var app in _trackedApps)
            {
                app.CloseApp();
            }
        }

        public static string MemInMb(long mem)
        {
            var mb = mem / 1e+6;
            return mb.ToString("#.#");
        }
    }
}
