using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Warden.Applications;


namespace Warden
{
    class Program
    {
        static void Main(string[] args)
        {
            var warden = new Warden();
            warden.Start();
        }
    }
}
