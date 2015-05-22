namespace Nito.BrowserBoss.Loggers
{
    public sealed class NullLogger : ILogger
    {
        public void WriteLine(string text)
        {
        }
    }
}
