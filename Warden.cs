﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;
using Warden.Applications;

namespace Warden
{
    class Warden
    {
        private List<Application> _trackedApps = new List<Application>();
        private readonly Timer _refreshTimer = new Timer(TimeSpan.FromSeconds(1).TotalMilliseconds);

        //should be ran after pre init
        private void Start()
        {
            _refreshTimer.AutoReset = true;
            _refreshTimer.Elapsed += AutoRefreshScreen;
            _refreshTimer.Start();

            
            StartAll();

            Console.ReadLine();
        }

        //check for leftover processes that were not closed correctly
        public void PreInit()
        {
            _trackedApps = ApplicationLoader.GetApplications("json.txt");

            Console.WriteLine($"Checking {_trackedApps.Count} apps for leftover processes...");

            foreach (var trackedApp in _trackedApps)
            {
                var leftoverProcs = Process.GetProcessesByName(trackedApp.ExecutableName);

                if (leftoverProcs.Length > 0)
                {
                    Console.WriteLine($"Found {leftoverProcs.Length} leftover process(es) of {trackedApp.ExecutableName}. Closing...");

                    foreach (var leftoverProc in leftoverProcs)
                    {
                        leftoverProc.Kill();
                        leftoverProc.WaitForExit();
                        leftoverProc.Dispose();
                    }
                }
                else
                {
                    Console.WriteLine($"Found no leftover processes of {trackedApp.ExecutableName}.");
                }
            }

            Console.WriteLine("Pre-init finished. Press enter to start.");

            Console.ReadLine();
            Start();
        }

        private void RefreshScreen(object sender, DataReceivedEventArgs e)
        {
            Console.Clear();
            foreach (var app in _trackedApps)
            {
                if (app.Process.HasExited) //TODO -- this should never be valid
                {
                    Console.WriteLine("!!!APP WITH CLOSED PROCESS!!!");
                }

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
