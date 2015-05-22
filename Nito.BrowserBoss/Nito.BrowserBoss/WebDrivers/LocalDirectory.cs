using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nito.BrowserBoss.WebDrivers
{
    public static class LocalDirectories
    {
        public static string WebDriverPath(string webDriverName)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Nito", "BrowserBoss", "WebDrivers", webDriverName);
        }
    }
}
