using System;
using System.IO;

namespace Nito.BrowserBoss.WebDrivers
{
    /// <summary>
    /// The local directories used by BrowserBoss.
    /// </summary>
    public static class LocalDirectories
    {
        /// <summary>
        /// Gets the private local directory that web drivers should use for their installations.
        /// </summary>
        /// <param name="webDriverName">The name of the web driver.</param>
        public static string WebDriverPath(string webDriverName)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Nito", "BrowserBoss", "WebDrivers", webDriverName);
        }
    }
}
