using System;
using Microsoft.Win32;

namespace Nito.BrowserBoss.WebDrivers
{
    /// <summary>
    /// Helper methods for working with browsers.
    /// </summary>
    public static class BrowserUtility
    {
        /// <summary>
        /// Gets the WebDriver setup for the user's default browser.
        /// </summary>
        public static IWebDriverSetup GetSetupForDefaultBrowser()
        {
            var defaultBrowserIdentifier = GetDefaultBrowserIdentifier();
            if (defaultBrowserIdentifier.IndexOf("IE", StringComparison.InvariantCultureIgnoreCase) != -1)
                return new InternetExplorerWebDriverSetup();
            if (defaultBrowserIdentifier.IndexOf("Chrome", StringComparison.InvariantCultureIgnoreCase) != -1)
                return new ChromeWebDriverSetup();
            if (defaultBrowserIdentifier.IndexOf("Firefox", StringComparison.InvariantCultureIgnoreCase) != -1)
                return new FirefoxWebDriverSetup();

            // If it's an unrecognized browser type, just punt and start IE.
            return new InternetExplorerWebDriverSetup();
        }

        private static string GetDefaultBrowserIdentifier()
        {
            using (var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice"))
            {
                if (key == null)
                    return string.Empty;
                var identifier = key.GetValue("Progid") as string;
                return identifier ?? string.Empty;
            }
        }
    }
}
