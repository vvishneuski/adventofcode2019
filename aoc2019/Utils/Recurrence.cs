using System;
using System.Collections.Generic;

namespace aoc2019.Utils
{
    public static class Recurrence
    {
        public static IEnumerable<T> Get<T>(T initial, Func<T, T> recurrence, Predicate<T> repeat)
        {
            var current = initial;
            while (repeat(current = recurrence(current)))
            {
                yield return current;
            }
        }
        
    }
}
