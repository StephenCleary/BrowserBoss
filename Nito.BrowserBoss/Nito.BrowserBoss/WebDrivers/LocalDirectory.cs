using System;
using System.IO;

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
