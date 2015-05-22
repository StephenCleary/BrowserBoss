using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;

namespace Nito.BrowserBoss
{
    /// <summary>
    /// Represents a browser instance.
    /// </summary>
    public sealed class Browser
    {
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
        /// Starts the Chrome browser, updating the Chrome WebDriver if necessary.
        /// </summary>
        public static Browser StartChrome()
        {
            var path = new WebDrivers.ChromeWebDriverSetup().Install();
            return new Browser(new ChromeDriver(path));
        }

        public static Browser StartIE()
        {
            var path = new WebDrivers.IEWebDriverSetup().Install();
            return new Browser(new InternetExplorerDriver(path));
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
