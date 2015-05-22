using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;

namespace Nito.BrowserBoss.Finders
{
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