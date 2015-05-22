using System.Collections.Generic;
using OpenQA.Selenium;
using SizSelCsZzz;

namespace Nito.BrowserBoss.Finders
{
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
}