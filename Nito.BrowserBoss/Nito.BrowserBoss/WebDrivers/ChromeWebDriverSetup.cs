using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Nito.BrowserBoss.WebDrivers
{
    public sealed class ChromeWebDriverSetup : WebDriverSetupBase
    {
        public ChromeWebDriverSetup()
            : base("Chrome")
        {
        }

        protected override async Task UpdateAsync(string version)
        {
            using (var client = new HttpClient())
            using (var stream = await client.GetStreamAsync("http://chromedriver.storage.googleapis.com/" + version + "/chromedriver_win32.zip").ConfigureAwait(false))
            using (var archive = new ZipArchive(stream))
                archive.ExtractToDirectory(Path.Combine(ParentPath, version));
        }

        protected override async Task<string> AvailableVersionAsync()
        {
            using (var client = new HttpClient())
                return await client.GetStringAsync("http://chromedriver.storage.googleapis.com/LATEST_RELEASE").ConfigureAwait(false);
        }
    }
}
