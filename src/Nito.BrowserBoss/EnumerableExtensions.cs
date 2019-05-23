using System;
using System.Collections.Generic;

namespace Nito.BrowserBoss
{
    /// <summary>
    /// Utility methods for sequences.
    /// </summary>
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
