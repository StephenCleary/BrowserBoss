namespace Nito.BrowserBoss.Loggers
{
    /// <summary>
    /// A simple tracing logger.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Writes a line of text to the logger.
        /// </summary>
        /// <param name="text">The text to write.</param>
        void WriteLine(string text);
    }
}
