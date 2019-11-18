using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Warden.Applications
{
    public abstract class Application
    {
        public abstract string AppPath { get; }
        public abstract string ExecutableName { get; }
        public abstract List<string> ErrorList { get; set; }
        public abstract int Timeout { get; set; }
        public abstract bool ShouldPrint { get; set; }

        public virtual Process Process { get; set; }
        public virtual List<string> OutputList { get; set; }
        public event EventHandler<DataReceivedEventArgs> NewOutput;

        public virtual void StartApp()
        {
            OutputList = new List<string>();

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
                    OutputList.Add(e.Data);
                    NewOutput?.Invoke(sender,e);
                    OutputEvent(e);
                };

                Process.ErrorDataReceived += delegate(object sender, DataReceivedEventArgs e)
                {
                    OutputList.Add(e.Data);
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
            if (ErrorList.Count > 0 && ErrorList.Any(error => e.Data.Contains(error)))
            {
                CloseApp();
                StartApp();
            }
        }

        public bool IsHealthy()
        {
            if (Process != null && !Process.HasExited)
            {
                return true;
            }

            return false;
        }

        public void CloseApp()
        {
            Process?.Kill();
            Process?.Close();
            

            if (ShouldPrint)
            {
                OutputList = new List<string>();
            }

            Process = null;
        }

    }
}
