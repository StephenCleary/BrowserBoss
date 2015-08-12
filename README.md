# BrowserBoss
Easily write scripts for browser automation.

## Getting Started - Standalone Applications

If you want to use BrowserBoss in a "real" application, just install [the NuGet package](https://www.nuget.org/packages/Nito.BrowserBoss/) into your project and then you can write code like:

````cs
using System;
using Nito.BrowserBoss;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // This is a sample site designed to be difficult to script.
                Boss.Url = "http://newtours.demoaut.com/";

                // You can click links by text
                Boss.Click("REGISTER");
                
                // CSS selectors are commonly used
                Boss.Write("#email", "StephenCleary");
                
                // XPath also works fine
                Boss.Write("//input[@type='password']", "password");
                // Note that the last line matched both password inputs, and filled them both in.

                Boss.Click("input[name='register']");

                Console.WriteLine("Done");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.ReadKey();
        }
    }
}
````

Tip: You may also want to install the [NuGet package Costura.Fody](https://www.nuget.org/packages/Costura.Fody/), which auto-magically embeds your referenced dlls as resources. This makes it easy to write small standalone executables to script common tasks.

## Getting Started - LINQPad

BrowserBoss was specifically designed with LINQPad in mind. If you have LINQPad installed, you can install [the NuGet package](https://www.nuget.org/packages/Nito.BrowserBoss/) into your query. Then you can run code like this:

````cs
// This is a sample site designed to be difficult to script.
Boss.Url = "http://newtours.demoaut.com/";

// You can click links by text
Boss.Click("REGISTER");

// CSS selectors are commonly used
Boss.Write("#email", "StephenCleary");

// XPath also works fine
Boss.Write("//input[@type='password']", "password");
// Note that the last line matched both password inputs, and filled them both in.

Boss.Click("input[name='register']");

Console.WriteLine("Done");
````

## How It Works: Core Concepts

BrowserBoss is built on the excellent [Selenium](http://www.seleniumhq.org/) project, but extends it to make scripting easier. BrowserBoss exposes a simplified API, but the underlying Selenium types are also exposed just in case you need them.

### Search Texts and Finders

BrowserBoss uses "search texts" for most of its browser interaction. A search text is a special string that you pass to BrowserBoss that can select one or more elements on a page. In the following code, "#email" is a search text:

    Boss.Write("#email", "StephenCleary@example.com");

Search texts are evaluated using *finders*. There are 5 finders built-in to BrowserBoss, and you can add your own by implementing the `Nito.BrowserBoss.Finders.IFind` interface and adding it to the `Boss.Config.Finders` collection. A search text is evaluated by each finder one at a time; as soon as a single finder returns a result, then no more finders are evaluated.

The built-in finders are:

1. CSS selectors. The CSS selector engine used also supports the [JQuery CSS selector extensions](https://api.jquery.com/category/selectors/jquery-selector-extensions/).
2. XPath.
3. Value finder. This finder will match any element whose `value` attribute is the search text.
4. Label finder. This finder will first find any label whose `text` attribute matches the search text, and then match whatever element that label is for (using the `for` attribute if present, or the next HTML element if it is a form element).
5. Text finder. This finder will match any element whose `text` attribute matches the search text.

### Retries

BrowserBoss will automatically retry as long as it action isn't doable. This helps in situations where the page is slow to load. By default, BrowserBoss will retry any operation for up to 30 seconds, at which point it will give up and throw an exception. This timeout can be changed by setting `Boss.Config.Timeout`.

### Easy to Get Started

BrowserBoss installs all necessary WebDrivers for you automagically, and will periodically keep them up-to-date. So this means if you develop an exe or LINQPad script for your coworker, you can just hand it to them and not have to worry about setup instructions.

### Default Browser

BrowserBoss will auto-detect your default browser and start it automatically. Browser detection currently works for Chrome, Firefox, and IE. If you want a different browser, you can put `Boss.StartChrome();`, `Boss.StartInternetExplorer();`, or `Boss.StartFirefox();` at the top of your script/program.

### Focused on Scripting, but Also Object-Oriented

The primary BrowserBoss API is designed with scripting in mind, particularly with the `using static` keyword coming in C# 6. However, all of the actual implementation logic (i.e., finders, retries, automatic WebDriver installation) is available using properly-designed OOP classes. To get started, check out the `Session` and `Browser` types.

## Available APIs on the `Boss` Type

`Url = "url";` - set this string value to navigate to the specified page.

`Click("searchText");` - click the matching elements.

`Write("searchText", "textToWrite");` - writes text to the matching elements. For `select` elements, selects the appropriate option rather than writing text.

`var element = Boss.Find("searchText");` - finds a single matching element and returns it. You can then perform actions on the returned element, e.g., reading its `value` or `text` by calling `Read()`.

`var elements = Boss.FindElements("searchText");` - finds multiple matching element and returns them. The returned collection is never empty.

`Check("searchText");` - checks the matching elements (if they are unchecked).

`Uncheck("searchText");` - unchecks the matching elements (if they are checked).

`var result = Boss.Script("javascript", ...);` - execute JavaScript within the browser, optionally passing arguments and retrieving results.

`DoubleClick("searchText");` - double-clicks the matching elements.

`Clear("searchText");` - clears the value/text of the matching elements. Note that `Write` will automatically `Clear` its element first.

`DragDrop("sourceSearchText", "targetSearchText");` - drags a single matching source element onto a single matching target element and drops it there.

`XPathString("string")` - returns an XPath expression that evaluates to the specified string value. Useful if you're dealing with text that may have single and/or double quotes.

`CssString("string")` - returns a CSS literal that evaluates to the specified string value. Useful if you're dealing with text that may have single and/or double quotes.
