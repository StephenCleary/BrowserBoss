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
        /// Starts the Chrome browser, updating the web driver if necessary.
        /// </summary>
        public static Browser StartChrome()
        {
            var path = new ChromeWebDriverSetup().Install();
            return new Browser(new ChromeDriver(path));
        }

        /// <summary>
        /// Starts the Internet Explorer browser, updating the web driver if necessary.
        /// </summary>
        public static Browser StartInternetExplorer()
        {
            var path = new InternetExplorerWebDriverSetup().Install();
            return new Browser(new InternetExplorerDriver(path));
        }

        /// <summary>
        /// Starts the Firefox browser.
        /// </summary>
        public static Browser StartFirefox()
        {
            return new Browser(new FirefoxDriver());
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
