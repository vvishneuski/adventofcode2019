using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace aoc2019.Utils
{
    public static class Split
    {
        public static IEnumerable<IEnumerable<T>> SplitUp<T>(this IEnumerable<T> source, int length)
        {
            var list = source.ToList();
            for (var start = 0; start < list.Count; start += length)
            {
                yield return list.GetRange(start, length);
            }
        }

        public static IEnumerable<string> SplitUp(this string source, int length)
        {
            for (var start = 0; start < source.Length; start += length)
            {
                yield return source.Substring(start, length);
            }
        }

        [Fact]
        public static void SplitUpStringTest()
        {
            // Arrange
            var source = "123456789012";

            var expected3 = "789";

            // Act
            var actual3 = source.SplitUp(3).ToList()[2];

            // Assert
            Assert.Equal(expected3, actual3);
        }

        [Fact]
        public static void SplitUpTest()
        {
            // Arrange
            var source = Enumerable.Range(0, 100);

            var expected50 = Enumerable.Range(50, 10);

            // Act
            var actual50 = source.SplitUp(10).ToList()[5];

            // Assert
            Assert.Equal(expected50, actual50);
        }
    }
}
