using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aoc2019.Utils;

namespace aoc2019
{
    public static class Day1
    {
        public static long GetFuelRequired(long mass)
        {
            return mass/3 - 2;
        }

        public static long GetFuelRequiredReal(long mass)
        {
            return Recurrence.Get(mass, GetFuelRequired, fuel => fuel > 0).Sum();
        }

        public static long SumFuelRequired(IEnumerable<long> masses, Func<long, long> getFuel)
        {
            return masses.AsParallel().Sum(getFuel);
        }
    }
}
