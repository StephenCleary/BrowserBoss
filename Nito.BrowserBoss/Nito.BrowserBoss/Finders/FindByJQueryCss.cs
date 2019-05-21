using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using OpenQA.Selenium;

namespace Nito.BrowserBoss.Finders
{
    /// <summary>
    /// Finds elements by JQuery-style CSS selectors. This supports JQuery extensions to CSS selectors: https://api.jquery.com/category/selectors/jquery-selector-extensions/
    /// </summary>
    public sealed class FindByJQueryCss : IFind
    {
        public string JQueryUri { get; set; } = "https://code.jquery.com/jquery-3.4.1.slim.min.js";

        IReadOnlyCollection<IWebElement> IFind.Find(ISearchContext context, string searchText)
        {
            return context.FindElements(new ByJQuery(JQueryUri, searchText));
        }

        private sealed class ByJQuery : By
        {
            private readonly string _jQueryUri;
            private readonly string _selector;

            public ByJQuery(string jQueryUri, string selector)
            {
                _jQueryUri = jQueryUri;
                _selector = JsonConvert.SerializeObject(selector);
                Description = $"ByJQuery: {selector}";
            }

            public override ReadOnlyCollection<IWebElement> FindElements(ISearchContext context)
            {
                var scriptExecutor = (IJavaScriptExecutor)context;
                EnsureJQueryIsLoaded(scriptExecutor);

                IEnumerable<object> result;
                if (context is IWebElement)
                    result = (IEnumerable<object>) scriptExecutor.ExecuteScript($"return jQuery.makeArray(jQuery({_selector}, arguments[0]))", context);
                else
                    result = (IEnumerable<object>) scriptExecutor.ExecuteScript($"return jQuery.makeArray(jQuery({_selector}))");

                return new ReadOnlyCollection<IWebElement>(result.Cast<IWebElement>().ToList());
            }

            public override IWebElement FindElement(ISearchContext context)
            {
                var found = FindElements(context);
                if (found.Count == 0)
                    throw new NoSuchElementException($"Could not find element matching jQuery selector {_selector}.");

                return found[0];
            }

            void EnsureJQueryIsLoaded(IJavaScriptExecutor scriptExecutor)
            {
                if (IsJQueryLoaded(scriptExecutor))
                    return;

                var script = new WebClient().DownloadString(_jQueryUri);
                scriptExecutor.ExecuteScript(script);
            }

            bool IsJQueryLoaded(IJavaScriptExecutor scriptExecutor)
            {
                try
                {
                    return (bool) scriptExecutor.ExecuteScript("return typeof(jQuery)==='function'");
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}