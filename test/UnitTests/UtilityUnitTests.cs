using Nito.BrowserBoss;
using System;
using Xunit;

namespace UnitTests
{
    public class UtilityUnitTests
    {
        [Theory]
        [InlineData("a", "'a'")]
        [InlineData("a'", "\"a'\"")]
        [InlineData("a\"", "\'a\"'")]
        [InlineData("a\"'", "\"a\\\"'\"")]
        [InlineData("a\"'\\", "\"a\\\"'\\\\\"")]
        public void CssString_QuotesAndEscapes(string input, string expected)
        {
            Assert.Equal(expected, Utility.CssString(input));
        }

        [Theory]
        [InlineData("a", "'a'")]
        [InlineData("a'", "\"a'\"")]
        [InlineData("a\"", "'a\"'")]
        public void XPathString_QuotesAndEscapes(string input, string expected)
        {
            Assert.Equal(expected, Utility.XPathString(input));
        }
    }
}
