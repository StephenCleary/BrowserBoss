namespace Nito.BrowserBoss.WebDrivers
{
    /// <summary>
    /// Handles automatic WebDriver installation/updating.
    /// </summary>
    public interface IWebDriverSetup
    {
        /// <summary>
        /// Installs or updates the WebDriver. Returns the directory where the most recent version is.
        /// </summary>
        string Install();
    }
}
