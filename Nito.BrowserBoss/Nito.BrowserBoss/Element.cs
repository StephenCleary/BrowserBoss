using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Nito.BrowserBoss
{
    /// <summary>
    /// Represents an element on a web page.
    /// </summary>
    public sealed class Element
    {
        /// <summary>
        /// The parent session.
        /// </summary>
        private readonly Session _session;

        /// <summary>
        /// Creates a new element wrapper.
        /// </summary>
        /// <param name="session">The parent session.</param>
        /// <param name="webElement">The underlying web element.</param>
        public Element(Session session, IWebElement webElement)
        {
            WebElement = webElement;
            _session = session;
        }

        /// <summary>
        /// The underlying Selenium web element.
        /// </summary>
        public IWebElement WebElement { get; private set; }

        /// <summary>
        /// Whether the element is selected.
        /// </summary>
        public bool Selected { get { return WebElement.Selected; } }

        /// <summary>
        /// Returns the parent element.
        /// </summary>
        public Element Parent
        {
            get { return Find(".."); }
        }

        /// <summary>
        /// Finds child elements of this element.
        /// </summary>
        /// <param name="searchText">The text to search.</param>
        public IEnumerable<Element> FindElements(string searchText)
        {
            return _session.FindElements(WebElement, searchText);
        }

        /// <summary>
        /// Finds a single matching child element of this element.
        /// </summary>
        /// <param name="searchText">The text to search.</param>
        public Element Find(string searchText)
        {
            return _session.FindElement(WebElement, searchText);
        }

        /// <summary>
        /// Reads the value or text of the element.
        /// </summary>
        public string Read()
        {
            var result = string.Empty;
            if (WebElement.TagName == "input" || WebElement.TagName == "textarea")
                result = WebElement.GetAttribute("value") ?? string.Empty;
            if (WebElement.TagName == "select")
            {
                var value = WebElement.GetAttribute("value");
                var option = WebElement.FindElements(By.TagName("option")).FirstOrDefault(x => x.GetAttribute("value") == value);
                if (option != null)
                    result = option.Text ?? string.Empty;
            }
            if (result == string.Empty)
                result = WebElement.Text ?? string.Empty;
            return result;
        }

        /// <summary>
        /// Sends text to the element via keystrokes. If this element is a select element, then this selects the appropriate option instead of sending keystrokes.
        /// </summary>
        /// <param name="text">The text to send.</param>
        public void Write(string text)
        {
            _session.Logger.WriteLine("Writing " + text + " to " + this);
            if (WebElement.TagName != "select")
            {
                Clear();
                WebElement.SendKeys(text);
                return;
            }

            var option = _session.FindElements(WebElement, "option[text()=" + Utility.CssString(text) + "] | option[@value=" + Utility.CssString(text) + "]").FirstOrDefault();
            if (option == null)
                throw new InvalidDataException("Element " + this + " does not contain option " + text);
            option.WebElement.Click();
        }

        /// <summary>
        /// Clears the value of an element.
        /// </summary>
        public void Clear()
        {
            _session.Logger.WriteLine("Clearing " + this);
            if (WebElement.GetAttribute("readonly") == "true")
                throw new InvalidOperationException("Cannot clear a readonly element.");
            WebElement.Clear();
        }

        /// <summary>
        /// Clicks the element.
        /// </summary>
        public void Click()
        {
            _session.Logger.WriteLine("Clicking " + this);
            WebElement.Click();
        }

        /// <summary>
        /// Double-clicks the element.
        /// </summary>
        public void DoubleClick()
        {
            _session.Logger.WriteLine("Doubleclicking " + this);
            var js = "var evt = document.createEvent('MouseEvents'); evt.initMouseEvent('dblclick',true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0,null); arguments[0].dispatchEvent(evt);";
            _session.Browser.Script(js, WebElement);
        }

        /// <summary>
        /// Ensures the element is checked.
        /// </summary>
        public void Check()
        {
            _session.Logger.WriteLine("Checking " + this);
            if (!WebElement.Selected)
                WebElement.Click();
        }

        /// <summary>
        /// Ensures the element is unchecked.
        /// </summary>
        public void Uncheck()
        {
            _session.Logger.WriteLine("Unhecking " + this);
            if (!WebElement.Selected)
                WebElement.Click();
        }

        /// <summary>
        /// Drags this element onto another element.
        /// </summary>
        /// <param name="target">The element to which to drag this one.</param>
        public void DragDropTo(Element target)
        {
            _session.Retry(() =>
            {
                new Actions(_session.Browser.WebDriver).DragAndDrop(WebElement, target.WebElement).Perform();
                return true;
            });
        }

        /// <summary>
        /// Returns a human-readable interpretation of the element.
        /// </summary>
        public override string ToString()
        {
            var id = WebElement.GetAttribute("id");
            if (!string.IsNullOrEmpty(id))
                return "#" + id;
            var text = WebElement.Text;
            if (!string.IsNullOrEmpty(text))
                return "\"" + text + "\"";
            return "<" + WebElement.TagName + ">";
        }
    }
}
