using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Warden.Applications
{
    public class Application
    {
        public string AppPath { get; set; }
        public string ExecutableName { get; set; }
        public List<string> ErrorList { get; set; }
        public int TimeoutSeconds { get; set; }
        public bool ShouldPrint { get; set; }

        public Process Process { get; set; }
        public event EventHandler<DataReceivedEventArgs> NewOutput;
        public string LatestOutput;

        private readonly Timer _tickTimer = new Timer(1000);
        public int SecondsSinceLastTick;

        public void StartApp()
        {
            SecondsSinceLastTick = 0;
            _tickTimer.AutoReset = true;
            _tickTimer.Elapsed += TickTimer;
            _tickTimer.Start();

            var processStartInfo = new ProcessStartInfo
            {
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                FileName = AppPath + "\\" + ExecutableName,
                WorkingDirectory = AppPath
            };

            Process = new Process {StartInfo = processStartInfo, EnableRaisingEvents = true};

            if (ShouldPrint)
            {
                Process.OutputDataReceived += delegate(object sender, DataReceivedEventArgs e)
                {
                    NewOutput?.Invoke(sender,e);
                    OutputEvent(e);
                };

                Process.ErrorDataReceived += delegate(object sender, DataReceivedEventArgs e)
                {
                    NewOutput?.Invoke(sender, e);
                    OutputEvent(e);
                };
            }

            Process.Start();
            Process.BeginOutputReadLine();
            Process.BeginErrorReadLine();
        }

        public void OutputEvent(DataReceivedEventArgs e)
        {
            if (e == null) return;
            SecondsSinceLastTick = 0;
            LatestOutput = e.Data;

            if (ErrorList == null) return;
            if (ErrorList.Count > 0 && ErrorList.Any(error => e.Data.Contains(error)))
            {
                CloseApp();
                StartApp();
            }
        }

        public ApplicationHealth AppHealth()
        {
            if (Process == null || Process.HasExited) return ApplicationHealth.Dead;
            return SecondsSinceLastTick > (TimeoutSeconds / 2) ? ApplicationHealth.Delay : ApplicationHealth.Healthy;
        }

        public void CloseApp()
        {
            Process?.Kill();
            Process?.Close();

            LatestOutput = "";
            SecondsSinceLastTick = 0;
            Process = null;
        }

        private void TickTimer(object sender, ElapsedEventArgs e)
        {
            SecondsSinceLastTick++;

            if (SecondsSinceLastTick >= TimeoutSeconds)
            {
                CloseApp();
                StartApp();
            }
        }

    }
}
