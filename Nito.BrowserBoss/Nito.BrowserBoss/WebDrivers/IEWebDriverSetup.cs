using System.IO;
using System.IO.Compression;
using System.Net;

namespace Nito.BrowserBoss.WebDrivers
{
    public sealed class IEWebDriverSetup : WebDriverSetupBase
    {
        public IEWebDriverSetup()
            : base("IE")
        {
        }

        protected override void Update(string version)
        {
            using (var client = new WebClient())
            using (var stream = client.OpenRead("http://selenium-release.storage.googleapis.com/" + version + "/IEDriverServer_Win32_" + version + ".0.zip"))
            using (var archive = new ZipArchive(stream))
                archive.ExtractToDirectory(Path.Combine(ParentPath, version));
        }

        protected override string AvailableVersion()
        {
            // TODO: better implementation
            return "2.45";
        }
    }
}
