using System.Text;

namespace Nito.BrowserBoss
{
    public static class Utility
    {
        /// <summary>
        /// Surrounds the string value with single or double quotes, returning an expression if necessary.
        /// </summary>
        /// <param name="value">The string value.</param>
        public static string XPathString(string value)
        {
            // Quickly handle the common cases.
            if (!value.Contains("'"))
                return "'" + value + "'";
            if (!value.Contains("\""))
                return "\"" + value + "\"";

            // TODO: unit testing. http://stackoverflow.com/questions/1341847/special-character-in-xpath-query
            var sb = new StringBuilder();
            sb.Append("concat(");
            var index = 0;
            while (true)
            {
                var nextDoubleQuote = value.IndexOf('"', index);
                var nextSingleQuote = value.IndexOf('\'', index);

                if (nextDoubleQuote == -1)
                {
                    sb.Append("\"" + value.Substring(index) + "\")");
                    return sb.ToString();
                }

                if (nextSingleQuote == -1)
                {
                    sb.Append("'" + value.Substring(index) + "'");
                    return sb.ToString();
                }

                if (index != 0)
                    sb.Append(",");
                if (nextDoubleQuote > nextSingleQuote)
                {
                    sb.Append("\"" + value.Substring(index, nextDoubleQuote - index - 1) + "\",");
                    index = nextDoubleQuote;
                }
                else
                {
                    sb.Append("'" + value.Substring(index, nextSingleQuote - index - 1) + "',");
                    index = nextSingleQuote;
                }
            }
        }

        /// <summary>
        /// Escapes the string value as necessary and surrounds it with single or double quotes.
        /// </summary>
        /// <param name="value">The string value.</param>
        public static string CssString(string value)
        {
            if (!value.Contains("\\"))
            {
                if (!value.Contains("'"))
                    return "'" + value + "'";
                if (!value.Contains("\""))
                    return "\"" + value + "\"";
            }

            // TODO: unit tests
            var sb = new StringBuilder();
            foreach (var ch in value)
            {
                if (ch == '\\')
                    sb.Append("\\\\");
                else if (ch == '\"')
                    sb.Append("\\\"");
                else
                    sb.Append(ch);
            }
            return sb.ToString();
        }
    }
}
