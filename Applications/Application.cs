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
        public abstract List<string> ErrorList { get; set; }
        public abstract int Timeout { get; set; }
        public abstract bool ShouldPrint { get; set; }

        public virtual Process Process { get; set; }
        public virtual StringBuilder OutputBuilder { get; set; }

        public virtual void StartApp()
        {
            var processStartInfo = new ProcessStartInfo
            {
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                FileName = AppPath
            };

            Process = new Process {StartInfo = processStartInfo, EnableRaisingEvents = true};
            OutputBuilder = new StringBuilder();

            if (ShouldPrint)
            {
                Process.OutputDataReceived += delegate(object sender, DataReceivedEventArgs e)
                {
                    OutputBuilder.Append(e.Data);
                };
                Process.ErrorDataReceived += delegate(object sender, DataReceivedEventArgs e)
                {
                    OutputBuilder.Append(e.Data);
                };
            }

            Process.OutputDataReceived += delegate (object sender, DataReceivedEventArgs e) { OutputEvent(e); };
            Process.ErrorDataReceived += delegate (object sender, DataReceivedEventArgs e) { OutputEvent(e); };

            Process.Start();
            Process.BeginOutputReadLine();
            Process.BeginErrorReadLine();
        }

        public virtual void OutputEvent(DataReceivedEventArgs e)
        {
            foreach (var error in ErrorList.Where(error => e.Data == error))
            {
                CloseApp();
                StartApp();
            }
        }

        public virtual void CloseApp()
        {
            Process?.Kill();
            Process?.Close();

            if (ShouldPrint)
            {
                OutputBuilder = new StringBuilder();
            }

            Process = null;
        }
    }
}
