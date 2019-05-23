using System;
using System.Collections.Generic;
using Nito.BrowserBoss.Finders;
using Nito.BrowserBoss.Loggers;

namespace Nito.BrowserBoss
{
    /// <summary>
    /// Provides a single session to control the browser.
    /// </summary>
    public static class Boss
    {
        private static Session _session;

        /// <summary>
        /// The current global session.
        /// </summary>
        public static Session Session
        {
            get
            {
                if (_session == null)
                    _session = new Session(Browser.StartDefault());
                return _session;
            }
            set { _session = value; }
        }

        /// <summary>
        /// The current browser.
        /// </summary>
        public static Browser Browser { get { return Session.Browser; } }

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
        /// <param name="hideCommandWindow">Whether to hide the Selenium command window.</param>
        public static void StartChrome(bool hideCommandWindow = true)
        {
            Session = new Session(Browser.StartChrome(hideCommandWindow));
        }

        /// <summary>
        /// Starts the IE driver as <see cref="Browser"/>. Installs/updates the Internet Explorer WebDriver as necessary.
        /// </summary>
        /// <param name="hideCommandWindow">Whether to hide the Selenium command window.</param>
        public static void StartInternetExplorer(bool hideCommandWindow = true)
        {
            Session = new Session(Browser.StartInternetExplorer(hideCommandWindow));
        }

        /// <summary>
        /// Starts the Firefox driver as <see cref="Browser"/>.
        /// </summary>
        /// <param name="hideCommandWindow">Whether to hide the Selenium command window.</param>
        public static void StartFirefox(bool hideCommandWindow = true)
        {
            Session = new Session(Browser.StartFirefox(hideCommandWindow));
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
        /// Finds the elements specified by <paramref name="searchText"/>. Repeatedly searches using the finders defined in <see cref="BrowserBoss.Session.Finders"/> until <see cref="BrowserBoss.Session.Timeout"/> expires. Throws an exception if no matching elements could be found.
        /// </summary>
        /// <param name="searchText">The search string.</param>
        public static IReadOnlyCollection<Element> FindElements(string searchText)
        {
            return Session.FindElements(searchText);
        }

        /// <summary>
        /// Finds a single matching element specified by <paramref name="searchText"/>. Repeatedly searches using the finders defined in <see cref="BrowserBoss.Session.Finders"/> until <see cref="BrowserBoss.Session.Timeout"/> expires. Throws an exception if no matching elements could be found.
        /// </summary>
        /// <param name="searchText">The search string.</param>
        public static Element Find(string searchText)
        {
            return Session.Find(searchText);
        }

        /// <summary>
        /// Sends text to the matching elements via keystrokes. If an element is a select element, then this selects the appropriate option instead of sending keystrokes.
        /// </summary>
        /// <param name="searchText">The search string.</param>
        /// <param name="text">The text to send.</param>
        public static void Write(string searchText, string text)
        {
            Session.Write(searchText, text);
        }

        /// <summary>
        /// Clears the value of matching elements. Throws an exception if no elements could be cleared.
        /// </summary>
        /// <param name="searchText">The search string.</param>
        public static void Clear(string searchText)
        {
            Session.Clear(searchText);
        }

        /// <summary>
        /// Clicks the matching elements. Throws an exception if no elements could be clicked.
        /// </summary>
        /// <param name="searchText">The search string.</param>
        public static void Click(string searchText)
        {
            Session.Click(searchText);
        }

        /// <summary>
        /// Double-clicks the matching elements. Throws an exception if no elements could be double-clicked.
        /// </summary>
        /// <param name="searchText">The search string.</param>
        public static void DoubleClick(string searchText)
        {
            Session.DoubleClick(searchText);
        }

        /// <summary>
        /// Ensures the matching elements are checked. Throws an exception if no elements could be checked.
        /// </summary>
        /// <param name="searchText">The search string.</param>
        public static void Check(string searchText)
        {
            Session.Check(searchText);
        }

        /// <summary>
        /// Ensures the matching elements are unchecked. Throws an exception if no elements could be unchecked.
        /// </summary>
        /// <param name="searchText">The search string.</param>
        public static void Uncheck(string searchText)
        {
            Session.Uncheck(searchText);
        }

        /// <summary>
        /// Drags the matching element onto another matching element. Throws an exception if either search text doesn't match exactly one element each.
        /// </summary>
        /// <param name="sourceSearchText">The search string for the element to drag.</param>
        /// <param name="targetSearchText">The search string for the drag target.</param>
        public static void DragDrop(string sourceSearchText, string targetSearchText)
        {
            Session.DragDrop(sourceSearchText, targetSearchText);
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
    }
}
