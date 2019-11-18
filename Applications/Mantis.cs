using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Warden.Applications
{
    public class Mantis : Application
    {
        public override string AppPath { get; } =
            "C:\\Users\\zirr_\\source\\repos\\Mantis\\bin\\Release\\netcoreapp3.0";
        public override string ExecutableName { get; } = "Mantis.exe";
        public override List<string> ErrorList { get; set;  } = new List<string>();
        public override int Timeout { get; set; } = 1000;
        public override bool ShouldPrint { get; set; } = true;

        public Mantis()
        {
            ErrorList.Add("Serving");
        }

    }
}
