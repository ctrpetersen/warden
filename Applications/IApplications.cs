using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Warden.Applications
{
    interface IApplication
    {
        string AppPath { get; }
        List<string> ErrorList { get; }
        int Timeout { get; }
        bool ShouldPrint { get; }
        Process Process { get; set; }

        void StartApp();
        void CloseApp();
    }
}
