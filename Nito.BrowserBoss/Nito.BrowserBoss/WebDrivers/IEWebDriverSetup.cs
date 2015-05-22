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
    public sealed class IEWebDriverSetup : WebDriverSetupBase
    {
        public IEWebDriverSetup()
            : base("IE")
        {
        }

        protected override async Task UpdateAsync(string version)
        {
            using (var client = new HttpClient())
            using (var stream = await client.GetStreamAsync("http://selenium-release.storage.googleapis.com/" + version + "/IEDriverServer_Win32_" + version + ".0.zip").ConfigureAwait(false))
            using (var archive = new ZipArchive(stream))
                archive.ExtractToDirectory(Path.Combine(ParentPath, version));
        }

        protected override Task<string> AvailableVersionAsync()
        {
            // TODO: better implementation
            return Task.FromResult("2.45");
        }
    }
}
