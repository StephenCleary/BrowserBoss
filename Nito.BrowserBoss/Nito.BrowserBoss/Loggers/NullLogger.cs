using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nito.BrowserBoss.Loggers
{
    public sealed class NullLogger : ILogger
    {
        public void WriteLine(string text)
        {
        }
    }
}
