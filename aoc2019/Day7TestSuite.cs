using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace aoc2019
{
    public class Day7TestSuite
    {
        public Day7TestSuite(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        private readonly ITestOutputHelper _testOutputHelper;

        public static TheoryData<long, long[], long[]> GetThrusterSignalData
            => new TheoryData<long, long[], long[]>
            {
                {43210, new[] {4L, 3, 2, 1, 0}, new[] {3L, 15, 3, 16, 1002, 16, 10, 16, 1, 16, 15, 15, 4, 15, 99, 0, 0}},
                {
                    54321, new[] {0L, 1, 2, 3, 4}, new[]
                    {
                        3L, 23, 3, 24, 1002, 24, 10, 24, 1002, 23, -1, 23,
                        101, 5, 23, 23, 1, 24, 23, 23, 4, 23, 99, 0, 0
                    }
                },
                {
                    65210, new[] {1L, 0, 4, 3, 2}, new[]
                    {
                        3L, 31, 3, 32, 1002, 32, 10, 32, 1001, 31, -2, 31, 1007, 31, 0, 33,
                        1002, 33, 7, 33, 1, 33, 31, 31, 1, 32, 31, 31, 4, 31, 99, 0, 0, 0
                    }
                },
            };

        [Theory]
        [MemberData(nameof(GetThrusterSignalData))]
        public void GetThrusterSignal(long expected, long[] expectedSettings, long[] program)
        {
            // Arrange
            var computer = new Day5.IntComputer(program);

            // Act
            var actual = CalculateMaxThrusterSignal(computer);
            _testOutputHelper.WriteLine(
                $"{actual.signal} [{String.Join(", ", actual.settings.Select(i => i.ToString()))}]");

            // Assert
            Assert.Equal(expected, actual.signal);
            Assert.Equal(expectedSettings, actual.settings);
        }

        private (long signal, long[] settings) CalculateMaxThrusterSignal(Day5.IntComputer computer)
        {
            var enumerateSettings = EnumerateSettings(0, 4);
            var signals = enumerateSettings.Select(settings =>
                (signal: CalculateThrusterSignal(computer, settings), settings));
            var orderedEnumerable = signals.OrderBy(e => e.signal);
            return orderedEnumerable.Last();
        }

        private static long CalculateThrusterSignal(Day5.IntComputer computer, long[] phaseSettings)
        {
            var outputA = computer.Compute(phaseSettings[0], 0);
            var outputB = computer.Compute(phaseSettings[1], outputA[0]);
            var outputC = computer.Compute(phaseSettings[2], outputB[0]);
            var outputD = computer.Compute(phaseSettings[3], outputC[0]);
            var outputE = computer.Compute(phaseSettings[4], outputD[0]);
            return outputE[0];
        }

        private static IEnumerable<long[]> EnumerateSettings(long minValue, long maxValue)
        {
            for (var a = minValue; a <= maxValue; a++)
            for (var b = minValue; b <= maxValue; b++)
            for (var c = minValue; c <= maxValue; c++)
            for (var d = minValue; d <= maxValue; d++)
            for (var e = minValue; e <= maxValue; e++)
            {
                var settings = new[] {a, b, c, d, e};
                if (settings.Distinct().Count() == 5)
                    yield return settings;
            }
        }

        [Fact]
        public void MaxThrusterSignal()
        {
            // Arrange
            var computer = new Day5.IntComputer(Day5.IntComputer.LoadProgram("Day7_input"));

            var expected = 34852;

            // Act
            var actual = CalculateMaxThrusterSignal(computer);
            _testOutputHelper.WriteLine(
                $"{actual.signal} [{String.Join(", ", actual.settings.Select(i => i.ToString()))}]");

            // Assert
            Assert.Equal(expected, actual.signal);
        }
    }
}
