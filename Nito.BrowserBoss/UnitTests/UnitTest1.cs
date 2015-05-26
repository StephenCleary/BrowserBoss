using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nito.BrowserBoss;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            CssStrings("a", "'a'");
            CssStrings("a'", "\"a'\"");
            CssStrings("a\"", "\'a\"'");
            CssStrings("a\"'", "\"a\\\"'\"");
            CssStrings("a\"'\\", "\"a\\\"'\\\\\"");
        }

        public void CssStrings(string input, string expected)
        {
            Assert.AreEqual(expected, Utility.CssString(input));
        }

        [TestMethod]
        public void TestMethod2()
        {
            XPathStrings("a", "'a'");
            XPathStrings("a'", "\"a'\"");
            XPathStrings("a\"", "'a\"'");
        }

        public void XPathStrings(string input, string expected)
        {
            Assert.AreEqual(expected, Utility.XPathString(input));
        }
    }
}
