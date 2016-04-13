using System.IO;
using System.IO.Compression;
using System.Net;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Nito.BrowserBoss.WebDrivers
{
    /// <summary>
    /// Installs/updates the Chrome web driver.
    /// </summary>
    public sealed class ChromeWebDriverSetup : WebDriverSetupBase
    {
        /// <summary>
        /// Creates an instance responsible for installing/updating the Chrome web driver.
        /// </summary>
        public ChromeWebDriverSetup()
            : base("Chrome")
        {
        }

        /// <summary>
        /// Downloads the specified web driver version.
        /// </summary>
        /// <param name="version">The version to download.</param>
        protected override void Update(string version)
        {
            using (var client = new WebClient())
            using (var stream = client.OpenRead("http://chromedriver.storage.googleapis.com/" + version + "/chromedriver_win32.zip"))
            using (var archive = new ZipArchive(stream))
                archive.ExtractToDirectory(Path.Combine(ParentPath, version));
        }

        /// <summary>
        /// Returns the newest version available for download.
        /// </summary>
        protected override string AvailableVersion()
        {
            using (var client = new WebClient())
            {
                var versionString = client.DownloadString("http://chromedriver.storage.googleapis.com/LATEST_RELEASE");
                return System.Text.RegularExpressions.Regex.Replace(versionString, @"\s", "");
            }
        }

        /// <summary>
        /// Starts a new instance of the web driver, installing or updating it as necessary.
        /// </summary>
        /// <param name="hideCommandWindow">Whether to hide the Selenium command window.</param>
        public override IWebDriver Start(bool hideCommandWindow = true)
        {
            var path = Install();
            var driverService = ChromeDriverService.CreateDefaultService(path);
            driverService.HideCommandPromptWindow = hideCommandWindow;
            return new ChromeDriver(driverService);
        }
    }
}
