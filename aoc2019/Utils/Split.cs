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
