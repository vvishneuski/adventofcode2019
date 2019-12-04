using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc2019
{
    public static class Day4
    {
        public static IEnumerable<int> GetDigits(this int number)
        {
            return GetDigitsInternal(number).Reverse();
        }

        private static IEnumerable<int> GetDigitsInternal(int number)
        {
            const int @base = 10;
            while (number > 0)
            {
                yield return number % @base;
                number /= @base;
            }
        }

        public static IEnumerable<int> GetPossiblePasswords(string range)
        {
            return GetRange(range).Where(Applicable);
        }

        private static IEnumerable<int> GetRange(string range)
        {
            var limits = range.Split('-').Select(Int32.Parse).ToArray();
            return Enumerable.Range(limits[0], limits[1] - limits[0] + 1);
        }

        private static bool Applicable(int number)
        {
            return number.DigitsAreNotDecrease() && number.HasTwoAdjacentDigitsTheSame();
        }

        public static bool DigitsAreNotDecrease(this int number)
        {
            return GetAdjacentDigitsPairs(number).All(pair => pair.i <= pair.j);
        }

        public static bool HasTwoAdjacentDigitsTheSame(this int number)
        {
            return GetAdjacentDigitsPairs(number).Any(pair => pair.i == pair.j);
        }

        private static IEnumerable<(int i, int j)> GetAdjacentDigitsPairs(int number)
        {
            var digits = number.GetDigits().ToList();

            var firstDigits = digits.Take(digits.Count - 1);
            var secondDigits = digits.Skip(1);

            return firstDigits.Zip(secondDigits, (i, j) => (i, j));
        }

        public static IEnumerable<int> GetPossiblePasswordsAdvanced(string range)
        {
            return GetRange(range).Where(ApplicableAdvanced);
        }

        private static bool ApplicableAdvanced(int number)
        {
            return number.DigitsAreNotDecrease() && number.HasExactTwoDigitsTheSame();
        }

        public static bool HasExactTwoDigitsTheSame(this int number)
        {
            return number.GetDigits().GroupBy(digit => digit).Any(group => group.Count() == 2);
        }
    }
}
