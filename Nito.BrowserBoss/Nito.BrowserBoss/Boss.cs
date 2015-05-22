using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using Nito.BrowserBoss.Loggers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;

namespace Nito.BrowserBoss
{
    public static class Boss
    {
        static Boss()
        {
            Logger = new NullLogger();
        }

        public static ILogger Logger { get; set; }

        /// <summary>
        /// The current browser.
        /// </summary>
        public static Browser Browser { get; set; }

        /// <summary>
        /// Gets the current URL or navigates to a new URL.
        /// </summary>
        public static string Url
        {
            get { return Browser.Url; }
            set { Browser.Url = value; }
        }

        /// <summary>
        /// Starts the Chrome driver as <see cref="Browser"/>. Installs/updates the Chrome WebDriver as necessary.
        /// </summary>
        public static async Task StartChromeAsync()
        {
            Browser = await Browser.StartChromeAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Starts the IE driver as <see cref="Browser"/>. Installs/updates the Internet Explorer WebDriver as necessary.
        /// </summary>
        /// <returns></returns>
        public static async Task StartIEAsync()
        {
            Browser = await Browser.StartIEAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Executes JavaScript in <see cref="Browser"/>.
        /// </summary>
        /// <param name="script">The JavaScript to execute.</param>
        /// <param name="args">The arguments to pass to <paramref name="script"/>.</param>
        public static dynamic Script(string script, params object[] args)
        {
            return Browser.Script(script, args);
        }

        internal static void Retry(Func<bool> func)
        {
            var wait = new WebDriverWait(Browser.WebDriver, Config.ElementTimeout);
            wait.Until(_ =>
            {
                try
                {
                    return func();
                }
                catch
                {
                    return false;
                }
            });
        }

        internal static T Retry<T>(Func<T> func) where T : System.Collections.ICollection
        {
            var wait = new WebDriverWait(Browser.WebDriver, Config.ElementTimeout);
            var result = default(T);
            wait.Until(_ =>
            {
                try
                {
                    result = func();
                    return result.Count != 0;
                }
                catch
                {
                    return false;
                }
            });
            return result;
        }

        /// <summary>
        /// Repeatedly attempts to find the elements until the <see cref="Config.ElementTimeout"/> timeout expires. Throws an exception if no matching elements could be found.
        /// </summary>
        /// <param name="context">The context in which to search.</param>
        /// <param name="searchText">The search string.</param>
        internal static IReadOnlyCollection<Element> FindElements(ISearchContext context, string searchText)
        {
            try
            {
                return Retry(() => Config.Finders.TryFind(Browser.WebDriver, context, searchText).Where(x => x.Displayed).Select(x => new Element(x)).ToArray());
            }
            catch (WebDriverTimeoutException ex)
            {
                throw new InvalidDataException("Could not find elements matching " + searchText, ex);
            }
        }

        /// <summary>
        /// Repeatedly attempts to find exactly one element until the <see cref="Config.ElementTimeout"/> timeout expires. Throws an exception if no matching element could be found.
        /// </summary>
        /// <param name="context">The context in which to search.</param>
        /// <param name="searchText">The search string.</param>
        internal static Element FindElement(ISearchContext context, string searchText)
        {
            var result = FindElements(context, searchText);
            if (result.Count != 1)
                throw new InvalidDataException("Multiple elements match " + searchText);
            return result.First();
        }

        /// <summary>
        /// Finds the elements specified by <paramref name="searchText"/>. Repeatedly searches using the finders defined in <see cref="Config.Finders"/> until the <see cref="Config.ElementTimeout"/> timeout expires.
        /// </summary>
        /// <param name="searchText">The search string.</param>
        public static IReadOnlyCollection<Element> FindElements(string searchText)
        {
            return FindElements(Browser.WebDriver, searchText);
        }

        /// <summary>
        /// Finds a single matching element specified by <paramref name="searchText"/>. Repeatedly searches using the finders defined in <see cref="Config.Finders"/> until the <see cref="Config.ElementTimeout"/> timeout expires.
        /// </summary>
        /// <param name="searchText">The search string.</param>
        public static Element Find(string searchText)
        {
            return FindElement(Browser.WebDriver, searchText);
        }

        /// <summary>
        /// Sends text to the matching elements via keystrokes. If an element is a select element, then this selects the appropriate option instead of sending keystrokes.
        /// </summary>
        /// <param name="searchText">The search string.</param>
        /// <param name="text">The text to send.</param>
        public static void Write(string searchText, string text)
        {
            FindElements(searchText).Apply(x => x.Write(text));
        }

        /// <summary>
        /// Clears the value of matching elements. Throws an exception if no elements could be cleared.
        /// </summary>
        /// <param name="searchText">The search string.</param>
        public static void Clear(string searchText)
        {
            FindElements(searchText).Apply(x => x.Clear());
        }

        /// <summary>
        /// Clicks the matching elements. Throws an exception if no elements could be clicked.
        /// </summary>
        /// <param name="searchText">The search string.</param>
        public static void Click(string searchText)
        {
            FindElements(searchText).Apply(x => x.Click());
        }

        /// <summary>
        /// Double-clicks the matching elements. Throws an exception if no elements could be double-clicked.
        /// </summary>
        /// <param name="searchText">The search string.</param>
        public static void DoubleClick(string searchText)
        {
            FindElements(searchText).Apply(x => x.DoubleClick());
        }

        /// <summary>
        /// Ensures the matching elements are checked. Throws an exception if no elements could be checked.
        /// </summary>
        /// <param name="searchText">The search string.</param>
        public static void Check(string searchText)
        {
            FindElements(searchText).Apply(x => x.Check());
        }

        /// <summary>
        /// Ensures the matching elements are unchecked. Throws an exception if no elements could be unchecked.
        /// </summary>
        /// <param name="searchText">The search string.</param>
        public static void Uncheck(string searchText)
        {
            FindElements(searchText).Apply(x => x.Uncheck());
        }

        /// <summary>
        /// Returns an XPath expression that evaluates to the specified string value.
        /// </summary>
        /// <param name="value">The string value to embed in an XPath search text.</param>
        public static string XPathString(string value)
        {
            return Utility.XPathLiteralString(value);
        }

        /// <summary>
        /// Returns a CSS literal that evaluates to the specified string value.
        /// </summary>
        /// <param name="value">The string value to embed in a CSS search text.</param>
        public static string CssString(string value)
        {
            return Utility.CssLiteralString(value);
        }

        /// <summary>
        /// Configuration for the <see cref="Boss"/> class.
        /// </summary>
        public static class Config
        {
            static Config()
            {
                Finders = FinderExtensions.DefaultFinders().ToList();
                ElementTimeout = TimeSpan.FromSeconds(30);
            }

            /// <summary>
            /// The amount of time to wait for browser elements to appear. The default is 30 seconds.
            /// </summary>
            public static TimeSpan ElementTimeout { get; set; }

            /// <summary>
            /// The collection of finders used to evaluate search strings.
            /// </summary>
            public static List<IFinder> Finders { get; private set; }
        }
    }
}
