using OpenQA.Selenium;

namespace Nito.BrowserBoss.WebDrivers
{
    /// <summary>
    /// Handles automatic WebDriver installation/updating.
    /// </summary>
    public interface IWebDriverSetup
    {
        /// <summary>
        /// Starts a new instance of the web driver, installing or updating it as necessary.
        /// </summary>
        IWebDriver Start();
    }
}
