using System.IO;
using System.IO.Compression;
using System.Net;

namespace Nito.BrowserBoss.WebDrivers
{
    public sealed class ChromeWebDriverSetup : WebDriverSetupBase
    {
        public ChromeWebDriverSetup()
            : base("Chrome")
        {
        }

        protected override void Update(string version)
        {
            using (var client = new WebClient())
            using (var stream = client.OpenRead("http://chromedriver.storage.googleapis.com/" + version + "/chromedriver_win32.zip"))
            using (var archive = new ZipArchive(stream))
                archive.ExtractToDirectory(Path.Combine(ParentPath, version));
        }

        protected override string AvailableVersion()
        {
            using (var client = new WebClient())
                return client.DownloadString("http://chromedriver.storage.googleapis.com/LATEST_RELEASE");
        }
    }
}
