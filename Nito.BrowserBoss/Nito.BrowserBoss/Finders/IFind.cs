using System.Collections.Generic;
using OpenQA.Selenium;

namespace Nito.BrowserBoss.Finders
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
}
