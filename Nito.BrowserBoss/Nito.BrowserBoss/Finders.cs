using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using SizSelCsZzz;

namespace Nito.BrowserBoss
{
    /// <summary>
    /// A "finder" is an object that knows how to search for matching elements given a search string.
    /// </summary>
    public interface IFind
    {
        /// <summary>
        /// Finds all matching elements. May throw exceptions on error.
        /// </summary>
        /// <param name="context">The context of the search. All results should be within this context.</param>
        /// <param name="searchText">The search string used to match the elements.</param>
        IReadOnlyCollection<IWebElement> Find(ISearchContext context, string searchText);
    }

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
 
    /// <summary>
    /// Finds elements by JQuery-style CSS selectors. This supports JQuery extensions to CSS selectors: https://api.jquery.com/category/selectors/jquery-selector-extensions/
    /// </summary>
    public sealed class FindByJQueryCss : IFind
    {
        IReadOnlyCollection<IWebElement> IFind.Find(ISearchContext context, string searchText)
        {
            return context.FindElements(ByJQuery.CssSelector(searchText));
        }
    }

    /// <summary>
    /// Finds elements by XPath strings.
    /// </summary>
    public sealed class FindByXPath : IFind
    {
        IReadOnlyCollection<IWebElement> IFind.Find(ISearchContext context, string searchText)
        {
            return context.FindElements(By.XPath(searchText));
        }
    }

    /// <summary>
    /// Finds elements by their text value.
    /// </summary>
    public sealed class FindByText : IFind
    {
        IReadOnlyCollection<IWebElement> IFind.Find(ISearchContext context, string searchText)
        {
            return context.FindElements(By.XPath(".//*[text() = " + Utility.XPathString(searchText) + "]"));
        }
    }

    /// <summary>
    /// Finds elements by their value.
    /// </summary>
    public sealed class FindByValue : IFind
    {
        IReadOnlyCollection<IWebElement> IFind.Find(ISearchContext context, string searchText)
        {
            return context.FindElements(By.CssSelector("*[value=" + Utility.CssString(searchText) + "]"));
        }
    }

    /// <summary>
    /// Finds elements by a matching label. The search string is the label text, and the returned element is the form element that label is for.
    /// </summary>
    public sealed class FindByLabel : IFind
    {
        private static IEnumerable<IWebElement> DoFind(ISearchContext context, string searchText)
        {
            foreach (var label in context.FindElements(By.XPath(".//label[text() = " + Utility.XPathString(searchText) + "]")))
            {
                var forAttribute = label.GetAttribute("for");
                if (forAttribute != null)
                {
                    foreach (var e in context.FindElements(By.Id(forAttribute)))
                        yield return e;
                }
                else
                {
                    foreach (var e in label.FindElements(By.XPath("./following-sibling::*[1]")))
                    {
                        if (e.TagName == "select" || e.TagName == "textarea" || (e.TagName == "input" && e.GetAttribute("type") != "hidden"))
                            yield return e;
                    }
                }
            }
        }

        IReadOnlyCollection<IWebElement> IFind.Find(ISearchContext context, string searchText)
        {
            return DoFind(context, searchText).ToArray();
        }
    }
}
