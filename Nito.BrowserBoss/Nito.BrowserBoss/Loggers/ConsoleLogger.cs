using System;

namespace Nito.BrowserBoss.Loggers
{
    public sealed class ConsoleLogger : ILogger
    {
        public void WriteLine(string text)
        {
            Console.WriteLine(text);
        }
    }
}
