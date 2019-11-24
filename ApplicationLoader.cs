using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Warden.Applications;

namespace Warden
{
    class ApplicationLoader
    {
        public static void SaveApplications(List<Application> apps, string path)
        {
            string json = JsonConvert.SerializeObject(apps);
            File.WriteAllText(path, json);
        }

        public static List<Application> GetApplications(string path)
        {
            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<List<Application>>(json);
        }
    }
}
