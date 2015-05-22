using System;
using System.IO;
using System.Threading.Tasks;

namespace Nito.BrowserBoss.WebDrivers
{
    public abstract class WebDriverSetupBase : IWebDriverSetup
    {
        private readonly string _parentPath;
        private readonly FileInfo _localVersionFile;

        protected WebDriverSetupBase(string webDriverName)
        {
            _parentPath = LocalDirectories.WebDriverPath(webDriverName);
            Directory.CreateDirectory(_parentPath);
            _localVersionFile = new FileInfo(Path.Combine(_parentPath, "version.txt"));
        }

        protected string ParentPath
        {
            get { return _parentPath; }
        }

        public async Task<string> InstallAsync()
        {
            // Only check for driver updates every so often.
            if (LatestLocalVersionUpdate() > DateTimeOffset.UtcNow.Subtract(TimeSpan.FromDays(7)))
                return Path.Combine(_parentPath, LocalVersion());

            // Get our installed version and compare it with the available version.
            var localVersion = LocalVersion();
            var availableVersion = await AvailableVersionAsync().ConfigureAwait(false);
            if (localVersion == availableVersion)
            {
                _localVersionFile.LastWriteTimeUtc = DateTime.UtcNow;
                return Path.Combine(_parentPath, localVersion);
            }

            // Download the newer version and update our installed version to it.
            await UpdateAsync(availableVersion).ConfigureAwait(false);
            File.WriteAllText(_localVersionFile.FullName, availableVersion);
            return Path.Combine(_parentPath, availableVersion);
        }

        protected abstract Task<string> AvailableVersionAsync();
        protected abstract Task UpdateAsync(string availableVersion);

        private string LocalVersion()
        {
            return !_localVersionFile.Exists ? null : File.ReadAllText(_localVersionFile.FullName);
        }

        private DateTimeOffset LatestLocalVersionUpdate()
        {
            return !_localVersionFile.Exists ? DateTimeOffset.MinValue : _localVersionFile.LastWriteTimeUtc;
        }
    }
}