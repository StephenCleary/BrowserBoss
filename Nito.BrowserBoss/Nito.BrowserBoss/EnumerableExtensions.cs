using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nito.BrowserBoss
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Applies an action to all elements.
        /// </summary>
        /// <param name="elements">The source sequence of elements.</param>
        /// <param name="action">The action to perform on each element.</param>
        public static void Apply<T>(this IEnumerable<T> elements, Action<T> action)
        {
            foreach (var e in elements)
                action(e);
        }
    }
}
