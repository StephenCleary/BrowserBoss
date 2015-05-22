using System.Collections.Generic;
using OpenQA.Selenium;

namespace Nito.BrowserBoss.Finders
{
    /// <summary>
    /// Extension methods for finders.
    /// </summary>
    public static class FindExtensions
    {
        /// <summary>
        /// Finds all matching elements. Does not throw exceptions; an empty enumeration is returned if any errors occur.
        /// </summary>
        /// <param name="this">The finder.</param>
        /// <param name="context">The context of the search. All results should be within this context.</param>
        /// <param name="searchText">The search string used to match the elements.</param>
        public static IReadOnlyCollection<IWebElement> TryFind(this IFind @this, ISearchContext context, string searchText)
        {
            try
            {
                return @this.Find(context, searchText);
            }
            catch
            {
                return new IWebElement[0];
            }
        }

        /// <summary>
        /// Finds all matching elements. Only returns results from the first finder that returns results. Does not throw exceptions; an empty enumeration is returned if any errors occur.
        /// </summary>
        /// <param name="this">The finders.</param>
        /// <param name="context">The context of the search. All results should be within this context.</param>
        /// <param name="searchText">The search string used to match the elements.</param>
        public static IReadOnlyCollection<IWebElement> TryFind(this IEnumerable<IFind> @this, ISearchContext context, string searchText)
        {
            foreach (var finder in @this)
            {
                var result = finder.TryFind(context, searchText);
                if (result.Count != 0)
                    return result;
            }

            return new IWebElement[0];
        }

        /// <summary>
        /// Finds all matching elements, searching child iframes if no matches were found. Only returns results from the first finder that returns results. Does not throw exceptions; an empty enumeration is returned if any errors occur.
        /// </summary>
        /// <param name="this">The finders.</param>
        /// <param name="webDriver">The web driver, representing the top-level context of this search.</param>
        /// <param name="context">The context of the search. All results should be within this context.</param>
        /// <param name="searchText">The search string used to match the elements.</param>
        public static IReadOnlyCollection<IWebElement> TryFind(this IReadOnlyCollection<IFind> @this, IWebDriver webDriver, ISearchContext context, string searchText)
        {
            // First, return all the normal matches.
            webDriver.SwitchTo().DefaultContent();
            var result = @this.TryFind(context, searchText);
            if (result.Count != 0)
                return result;

            // If more results are required, then search the iframes.
            foreach (var iframe in context.FindElements(By.TagName("iframe")))
            {
                webDriver.SwitchTo().Frame(iframe);
                var html = webDriver.FindElement(By.TagName("html"));
                result = @this.TryFind(webDriver, html, searchText);
                if (result.Count != 0)
                    return result;
            }

            return result;
        }

        /// <summary>
        /// The default finders used by BrowserBoss.
        /// </summary>
        public static IEnumerable<IFind> DefaultFinders()
        {
            yield return new FindByJQueryCss();
            yield return new FindByXPath();
            yield return new FindByValue();
            yield return new FindByLabel();
            yield return new FindByText();
        }
    }
}