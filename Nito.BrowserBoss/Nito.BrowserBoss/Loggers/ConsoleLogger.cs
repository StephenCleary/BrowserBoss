using System;

namespace Nito.BrowserBoss.Loggers
{
    /// <summary>
    /// A logger that writes all messages to the console window.
    /// </summary>
    public sealed class ConsoleLogger : ILogger
    {
        void ILogger.WriteLine(string text)
        {
            Console.WriteLine(text);
        }
    }
}
