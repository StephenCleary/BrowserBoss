using System.Collections.Generic;
using OpenQA.Selenium;

namespace Nito.BrowserBoss.Finders
{
    /// <summary>
    /// Finds elements by their text value.
    /// </summary>
    public sealed class FindByNormalizeSpaceText : IFind
    {
        IReadOnlyCollection<IWebElement> IFind.Find(ISearchContext context, string searchText)
        {
            return context.FindElements(By.XPath(".//*[normalize-space(text()) = normalize-space(" + Utility.XPathString(searchText) + ")]"));
        }
    }
}