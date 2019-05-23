namespace Nito.BrowserBoss.Loggers
{
    /// <summary>
    /// A logger that ignores everything.
    /// </summary>
    public sealed class NullLogger : ILogger
    {
        void ILogger.WriteLine(string text)
        {
        }
    }
}
