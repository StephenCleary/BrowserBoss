using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Nito.BrowserBoss.Loggers;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Nito.BrowserBoss
{
    /// <summary>
    /// A browser session.
    /// </summary>
    public class Session
    {
        private ILogger _logger;

        /// <summary>
        /// Creates a session around the specified browser.
        /// </summary>
        public Session(Browser browser)
        {
            Browser = browser;
            Finders = FinderExtensions.DefaultFinders().ToList();
            Timeout = TimeSpan.FromSeconds(30);
            Logger = new NullLogger();
        }

        /// <summary>
        /// The current browser.
        /// </summary>
        public Browser Browser { get; private set; }

        /// <summary>
        /// The amount of time to wait for browser elements to appear. The default is 30 seconds.
        /// </summary>
        public TimeSpan Timeout { get; set; }

        /// <summary>
        /// The current logger, which can help with debugging.
        /// </summary>
        public ILogger Logger
        {
            get { return _logger; }
            set { _logger = value ?? new NullLogger(); }
        }

        /// <summary>
        /// The collection of finders used to evaluate search strings.
        /// </summary>
        public List<IFinder> Finders { get; private set; }

        /// <summary>
        /// Repeatedly executes <paramref name="func"/> until it returns <c>true</c> or until <see cref="Timeout"/> expires.
        /// </summary>
        /// <param name="func">The function to execute.</param>
        public void Retry(Func<bool> func)
        {
            var wait = new WebDriverWait(Browser.WebDriver, Timeout);
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

        /// <summary>
        /// Repeatedly executes <paramref name="func"/> until it returns a non-empty collection or until <see cref="Timeout"/> expires.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection returned by <paramref name="func"/>.</typeparam>
        /// <param name="func">The function to execute.</param>
        public T Retry<T>(Func<T> func) where T : System.Collections.ICollection
        {
            var wait = new WebDriverWait(Browser.WebDriver, Timeout);
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
        /// Repeatedly attempts to find the elements until <see cref="Timeout"/> expires. Throws an exception if no matching elements could be found.
        /// </summary>
        /// <param name="context">The context in which to search.</param>
        /// <param name="searchText">The search string.</param>
        public IReadOnlyCollection<Element> FindElements(ISearchContext context, string searchText)
        {
            try
            {
                return Retry(() => Finders.TryFind(Browser.WebDriver, context, searchText).Where(x => x.Displayed).Select(x => new Element(this, x)).ToArray());
            }
            catch (WebDriverTimeoutException ex)
            {
                throw new InvalidDataException("Could not find elements matching " + searchText, ex);
            }
        }

        /// <summary>
        /// Repeatedly attempts to find exactly one element until <see cref="Timeout"/> expires. Throws an exception if no matching element could be found.
        /// </summary>
        /// <param name="context">The context in which to search.</param>
        /// <param name="searchText">The search string.</param>
        public Element FindElement(ISearchContext context, string searchText)
        {
            var result = FindElements(context, searchText);
            if (result.Count != 1)
                throw new InvalidDataException("Multiple elements match " + searchText);
            return result.First();
        }

        /// <summary>
        /// Finds the elements specified by <paramref name="searchText"/>. Repeatedly searches using the finders defined in <see cref="Finders"/> until <see cref="Timeout"/> expires. Throws an exception if no matching elements could be found.
        /// </summary>
        /// <param name="searchText">The search string.</param>
        public IReadOnlyCollection<Element> FindElements(string searchText)
        {
            return FindElements(Browser.WebDriver, searchText);
        }

        /// <summary>
        /// Finds a single matching element specified by <paramref name="searchText"/>. Repeatedly searches using the finders defined in <see cref="Finders"/> until <see cref="Timeout"/> expires. Throws an exception if no matching element could be found.
        /// </summary>
        /// <param name="searchText">The search string.</param>
        public Element Find(string searchText)
        {
            return FindElement(Browser.WebDriver, searchText);
        }

        /// <summary>
        /// Sends text to the matching elements via keystrokes. If an element is a select element, then this selects the appropriate option instead of sending keystrokes.
        /// </summary>
        /// <param name="searchText">The search string.</param>
        /// <param name="text">The text to send.</param>
        public void Write(string searchText, string text)
        {
            FindElements(searchText).Apply(x => x.Write(text));
        }

        /// <summary>
        /// Clears the value of matching elements. Throws an exception if no elements could be cleared.
        /// </summary>
        /// <param name="searchText">The search string.</param>
        public void Clear(string searchText)
        {
            FindElements(searchText).Apply(x => x.Clear());
        }

        /// <summary>
        /// Clicks the matching elements. Throws an exception if no elements could be clicked.
        /// </summary>
        /// <param name="searchText">The search string.</param>
        public void Click(string searchText)
        {
            FindElements(searchText).Apply(x => x.Click());
        }

        /// <summary>
        /// Double-clicks the matching elements. Throws an exception if no elements could be double-clicked.
        /// </summary>
        /// <param name="searchText">The search string.</param>
        public void DoubleClick(string searchText)
        {
            FindElements(searchText).Apply(x => x.DoubleClick());
        }

        /// <summary>
        /// Ensures the matching elements are checked. Throws an exception if no elements could be checked.
        /// </summary>
        /// <param name="searchText">The search string.</param>
        public void Check(string searchText)
        {
            FindElements(searchText).Apply(x => x.Check());
        }

        /// <summary>
        /// Ensures the matching elements are unchecked. Throws an exception if no elements could be unchecked.
        /// </summary>
        /// <param name="searchText">The search string.</param>
        public void Uncheck(string searchText)
        {
            FindElements(searchText).Apply(x => x.Uncheck());
        }
    }
}
