using System;
using System.Collections.Generic;
using Nito.BrowserBoss.Loggers;

namespace Nito.BrowserBoss
{
    /// <summary>
    /// Provides a single session to control the browser. Begin by calling one of the <c>Start</c> methods.
    /// </summary>
    public static class Boss
    {
        /// <summary>
        /// The current global session. This is <c>null</c> until one of the <c>Start</c> methods is called.
        /// </summary>
        private static Session _session;

        /// <summary>
        /// The current browser.
        /// </summary>
        public static Browser Browser { get { return _session.Browser; } }

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
        public static void StartChrome()
        {
            _session = new Session(Browser.StartChrome());
        }

        /// <summary>
        /// Starts the IE driver as <see cref="Browser"/>. Installs/updates the Internet Explorer WebDriver as necessary.
        /// </summary>
        /// <returns></returns>
        public static void StartInternetExplorer()
        {
            _session = new Session(Browser.StartInternetExplorer());
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

        /// <summary>
        /// Finds the elements specified by <paramref name="searchText"/>. Repeatedly searches using the finders defined in <see cref="Config.Finders"/> until <see cref="Config.Timeout"/> expires. Throws an exception if no matching elements could be found.
        /// </summary>
        /// <param name="searchText">The search string.</param>
        public static IReadOnlyCollection<Element> FindElements(string searchText)
        {
            return _session.FindElements(searchText);
        }

        /// <summary>
        /// Finds a single matching element specified by <paramref name="searchText"/>. Repeatedly searches using the finders defined in <see cref="Config.Finders"/> until <see cref="Config.Timeout"/> expires. Throws an exception if no matching elements could be found.
        /// </summary>
        /// <param name="searchText">The search string.</param>
        public static Element Find(string searchText)
        {
            return _session.Find(searchText);
        }

        /// <summary>
        /// Sends text to the matching elements via keystrokes. If an element is a select element, then this selects the appropriate option instead of sending keystrokes.
        /// </summary>
        /// <param name="searchText">The search string.</param>
        /// <param name="text">The text to send.</param>
        public static void Write(string searchText, string text)
        {
            _session.Write(searchText, text);
        }

        /// <summary>
        /// Clears the value of matching elements. Throws an exception if no elements could be cleared.
        /// </summary>
        /// <param name="searchText">The search string.</param>
        public static void Clear(string searchText)
        {
            _session.Clear(searchText);
        }

        /// <summary>
        /// Clicks the matching elements. Throws an exception if no elements could be clicked.
        /// </summary>
        /// <param name="searchText">The search string.</param>
        public static void Click(string searchText)
        {
            _session.Click(searchText);
        }

        /// <summary>
        /// Double-clicks the matching elements. Throws an exception if no elements could be double-clicked.
        /// </summary>
        /// <param name="searchText">The search string.</param>
        public static void DoubleClick(string searchText)
        {
            _session.DoubleClick(searchText);
        }

        /// <summary>
        /// Ensures the matching elements are checked. Throws an exception if no elements could be checked.
        /// </summary>
        /// <param name="searchText">The search string.</param>
        public static void Check(string searchText)
        {
            _session.Check(searchText);
        }

        /// <summary>
        /// Ensures the matching elements are unchecked. Throws an exception if no elements could be unchecked.
        /// </summary>
        /// <param name="searchText">The search string.</param>
        public static void Uncheck(string searchText)
        {
            _session.Uncheck(searchText);
        }

        /// <summary>
        /// Returns an XPath expression that evaluates to the specified string value.
        /// </summary>
        /// <param name="value">The string value to embed in an XPath search text.</param>
        public static string XPathString(string value)
        {
            return Utility.XPathString(value);
        }

        /// <summary>
        /// Returns a CSS literal that evaluates to the specified string value.
        /// </summary>
        /// <param name="value">The string value to embed in a CSS search text.</param>
        public static string CssString(string value)
        {
            return Utility.CssString(value);
        }

        /// <summary>
        /// Configuration for the <see cref="Boss"/> class. These values are not valid until one of the <c>Start</c> methods is called.
        /// </summary>
        public static class Config
        {
            /// <summary>
            /// Gets or sets the current logger.
            /// </summary>
            public static ILogger Logger
            {
                get { return _session.Logger; }
                set { _session.Logger = value; }
            }

            /// <summary>
            /// The amount of time to wait for browser elements to appear. The default is 30 seconds.
            /// </summary>
            public static TimeSpan Timeout
            {
                get { return _session.Timeout; }
                set { _session.Timeout = value; }
            }

            /// <summary>
            /// The collection of finders used to evaluate search strings.
            /// </summary>
            public static List<IFind> Finders { get { return _session.Finders; } }
        }
    }
}
