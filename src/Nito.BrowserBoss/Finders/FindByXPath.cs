using System.Collections.Generic;
using OpenQA.Selenium;

namespace Nito.BrowserBoss.Finders
{
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
}