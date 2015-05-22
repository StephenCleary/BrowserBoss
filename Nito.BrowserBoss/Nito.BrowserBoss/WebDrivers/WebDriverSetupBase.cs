using System;
using System.IO;

// TODO: FirefoxWebDriverSetup

namespace Nito.BrowserBoss.WebDrivers
{
    /// <summary>
    /// Provides basic functionality for installing/updating web drivers.
    /// </summary>
    public abstract class WebDriverSetupBase : IWebDriverSetup
    {
        private readonly string _parentPath;
        private readonly FileInfo _localVersionFile;

        /// <summary>
        /// Creates the local directory that contains all installations for this web driver.
        /// </summary>
        /// <param name="webDriverName">The name of the web driver.</param>
        protected WebDriverSetupBase(string webDriverName)
        {
            _parentPath = LocalDirectories.WebDriverPath(webDriverName);
            Directory.CreateDirectory(_parentPath);
            _localVersionFile = new FileInfo(Path.Combine(_parentPath, "version.txt"));
        }

        /// <summary>
        /// The path containing all installations for this web driver.
        /// </summary>
        protected string ParentPath
        {
            get { return _parentPath; }
        }

        /// <summary>
        /// Installs/updates the web driver. Checks for updates weekly.
        /// </summary>
        public string Install()
        {
            // Only check for driver updates every so often.
            if (LatestLocalVersionUpdate() > DateTimeOffset.UtcNow.Subtract(TimeSpan.FromDays(7)))
                return Path.Combine(_parentPath, LocalVersion());

            // Get our installed version and compare it with the available version.
            var localVersion = LocalVersion();
            string availableVersion;
            try
            {
                availableVersion = AvailableVersion();
            }
            catch
            {
                if (localVersion == null)
                    throw;
                return Path.Combine(_parentPath, localVersion);
            }
            if (localVersion == availableVersion)
            {
                _localVersionFile.LastWriteTimeUtc = DateTime.UtcNow;
                return Path.Combine(_parentPath, localVersion);
            }

            // Download the newer version and update our installed version to it.
            Update(availableVersion);
            File.WriteAllText(_localVersionFile.FullName, availableVersion);
            return Path.Combine(_parentPath, availableVersion);
        }

        /// <summary>
        /// Returns the latest version available from the Internet.
        /// </summary>
        protected abstract string AvailableVersion();

        /// <summary>
        /// Downloads and installs the <paramref name="availableVersion"/> into the directory <c>Path.Combine(ParentPath, availableVersion)</c>.
        /// </summary>
        /// <param name="availableVersion"></param>
        protected abstract void Update(string availableVersion);

        /// <summary>
        /// Gets the newest installed version, or <c>null</c> if there is no installed version.
        /// </summary>
        private string LocalVersion()
        {
            return !_localVersionFile.Exists ? null : File.ReadAllText(_localVersionFile.FullName);
        }

        /// <summary>
        /// Gets the timestamp when the last version check was made, or <c>DateTimeOffset.MinValue</c> if no version check has been made yet.
        /// </summary>
        private DateTimeOffset LatestLocalVersionUpdate()
        {
            return !_localVersionFile.Exists ? DateTimeOffset.MinValue : _localVersionFile.LastWriteTimeUtc;
        }
    }
}