using System.IO;
using System.IO.Compression;
using System.Net;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;

namespace Nito.BrowserBoss.WebDrivers
{
    /// <summary>
    /// Manages the installation/updates for the IE web driver.
    /// </summary>
    public sealed class InternetExplorerWebDriverSetup : WebDriverSetupBase
    {
        /// <summary>
        /// Creates an instance responsible for installing/updating the IE web driver.
        /// </summary>
        public InternetExplorerWebDriverSetup()
            : base("IE")
        {
        }

        /// <summary>
        /// Downloads the specified web driver version.
        /// </summary>
        /// <param name="version">The version to download.</param>
        protected override void Update(string version)
        {
            using (var client = new WebClient())
            using (var stream = client.OpenRead("http://selenium-release.storage.googleapis.com/" + version + "/IEDriverServer_Win32_" + version + ".0.zip"))
            using (var archive = new ZipArchive(stream))
                archive.ExtractToDirectory(Path.Combine(ParentPath, version));
        }

        /// <summary>
        /// Returns the newest version available for download. Currently always returns "2.45".
        /// </summary>
        protected override string AvailableVersion()
        {
            // TODO: better implementation
            return "2.45";
        }

        /// <summary>
        /// Starts a new instance of the web driver, installing or updating it as necessary.
        /// </summary>
        public override IWebDriver Start()
        {
            var path = Install();
            return new InternetExplorerDriver(path);
        }
    }
}
