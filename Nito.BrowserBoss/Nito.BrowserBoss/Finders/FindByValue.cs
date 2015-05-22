using System.Collections.Generic;
using OpenQA.Selenium;

namespace Nito.BrowserBoss.Finders
{
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
}