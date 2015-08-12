using Nito.BrowserBoss.WebDrivers;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;

namespace Nito.BrowserBoss
{
    /// <summary>
    /// Represents a browser instance.
    /// </summary>
    public sealed class Browser
    {
        /// <summary>
        /// Creates a browser wrapper for the specified web driver.
        /// </summary>
        /// <param name="webDriver">The web driver to wrap.</param>
        public Browser(IWebDriver webDriver)
        {
            WebDriver = webDriver;
        }

        /// <summary>
        /// The underlying Selenium web driver.
        /// </summary>
        public IWebDriver WebDriver { get; private set; }

        /// <summary>
        /// Gets the current URL or navigates to a new URL.
        /// </summary>
        public string Url
        {
            get { return WebDriver.Url; }
            set { WebDriver.Navigate().GoToUrl(value); }
        }

        /// <summary>
        /// Starts the user's default browser, updating the web driver if necessary.
        /// </summary>
        /// <param name="hideCommandWindow">Whether to hide the Selenium command window.</param>
        public static Browser StartDefault(bool hideCommandWindow = true)
        {
            return new Browser(BrowserUtility.GetSetupForDefaultBrowser().Start(hideCommandWindow));
        }

        /// <summary>
        /// Starts the Chrome browser, updating the web driver if necessary.
        /// </summary>
        /// <param name="hideCommandWindow">Whether to hide the Selenium command window.</param>
        public static Browser StartChrome(bool hideCommandWindow = true)
        {
            return new Browser(new ChromeWebDriverSetup().Start(hideCommandWindow));
        }

        /// <summary>
        /// Starts the Internet Explorer browser, updating the web driver if necessary.
        /// </summary>
        /// <param name="hideCommandWindow">Whether to hide the Selenium command window.</param>
        public static Browser StartInternetExplorer(bool hideCommandWindow = true)
        {
            return new Browser(new InternetExplorerWebDriverSetup().Start(hideCommandWindow));
        }

        /// <summary>
        /// Starts the Firefox browser.
        /// </summary>
        /// <param name="hideCommandWindow">Whether to hide the Selenium command window.</param>
        public static Browser StartFirefox(bool hideCommandWindow = true)
        {
            return new Browser(new FirefoxWebDriverSetup().Start(hideCommandWindow));
        }

        /// <summary>
        /// Executes JavaScript in this browser.
        /// </summary>
        /// <param name="script">The JavaScript to execute.</param>
        /// <param name="args">The arguments to pass to <paramref name="script"/>.</param>
        public dynamic Script(string script, params object[] args)
        {
            return ((IJavaScriptExecutor)WebDriver).ExecuteScript(script, args);
        }
    }
}
