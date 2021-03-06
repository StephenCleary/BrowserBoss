﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace Nito.BrowserBoss.WebDrivers
{
    /// <summary>
    /// Installs/updates the Chrome web driver.
    /// </summary>
    public sealed class FirefoxWebDriverSetup : IWebDriverSetup
    {
        /// <summary>
        /// Starts a new instance of the web driver, installing or updating it as necessary.
        /// </summary>
        /// <param name="hideCommandWindow">Whether to hide the Selenium command window.</param>
        public IWebDriver Start(bool hideCommandWindow = true)
        {
            return new FirefoxDriver();
        }
    }
}
