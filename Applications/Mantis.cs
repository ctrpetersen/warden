using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Warden.Applications
{
    class Mantis : IApplication
    {
        public string AppPath => "C:\\Users\\zirr_\\source\\repos\\Mantis\\bin\\Release\\netcoreapp3.0\\Mantis.exe";

        public List<string> ErrorList => null;

        public int Timeout => 1000;

        public bool ShouldPrint => true;

        public Process Process { get; set; }


        public void StartApp()
        {
            Process = new Process();
        }

        public void CloseApp()
        {
            throw new NotImplementedException();
        }
    }
}
