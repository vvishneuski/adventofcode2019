using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace aoc2019
{
    public class Day7TestSuite2
    {
        public Day7TestSuite2(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        private readonly ITestOutputHelper _testOutputHelper;

        public static TheoryData<int, int[], int[]> GetThrusterSignalData
            => new TheoryData<int, int[], int[]>
            {
                {
                    139629729, new[] {9, 8, 7, 6, 5}, new[]
                    {
                        3, 26, 1001, 26, -4, 26, 3, 27, 1002, 27, 2, 27, 1, 27, 26,
                        27, 4, 27, 1001, 28, -1, 28, 1005, 28, 6, 99, 0, 0, 5
                    }
                },
                {
                    18216, new[] {9, 7, 8, 5, 6}, new[]
                    {
                        3, 52, 1001, 52, -5, 52, 3, 53, 1, 52, 56, 54, 1007, 54, 5, 55, 1005, 55, 26, 1001, 54,
                        -5, 54, 1105, 1, 12, 1, 53, 54, 53, 1008, 54, 0, 55, 1001, 55, 1, 55, 2, 53, 55, 53, 4,
                        53, 1001, 56, -1, 56, 1005, 56, 6, 99, 0, 0, 0, 0, 10
                    }
                },
            };

        [Theory]
        [MemberData(nameof(GetThrusterSignalData))]
        public void GetThrusterSignal(int expected, int[] expectedSettings, int[] program)
        {
            // Act
            var actual = CalculateMaxThrusterSignal(program);
            _testOutputHelper.WriteLine(
                $"{actual.signal} [{String.Join(", ", actual.settings.Select(i => i.ToString()))}]");

            // Assert
            Assert.Equal(expected, actual.signal);
            Assert.Equal(expectedSettings, actual.settings);
        }

        private (int signal, int[] settings) CalculateMaxThrusterSignal(int[] program)
        {
            var enumerateSettings = EnumerateSettings(5, 9);
            var signals =
                enumerateSettings.Select(settings => (signal: CalculateThrusterSignal(program, settings), settings));
            var orderedEnumerable = signals.OrderBy(e => e.signal);
            return orderedEnumerable.Last();
        }

        private static int CalculateThrusterSignal(int[] program, int[] phaseSettings)
        {
            var outputE = new ConcurrentQueue<int>(new[] {phaseSettings[0], 0});

            var inputA = outputE;
            var outputA = new ConcurrentQueue<int>(new[] {phaseSettings[1]});
            var computerA = Task.Run(() => new Day5.IntComputer(program).Compute(inputA, outputA));

            var inputB = outputA;
            var outputB = new ConcurrentQueue<int>(new[] {phaseSettings[2]});
            var computerB = Task.Run(() => new Day5.IntComputer(program).Compute(inputB, outputB));

            var inputC = outputB;
            var outputC = new ConcurrentQueue<int>(new[] {phaseSettings[3]});
            var computerC = Task.Run(() => new Day5.IntComputer(program).Compute(inputC, outputC));

            var inputD = outputC;
            var outputD = new ConcurrentQueue<int>(new[] {phaseSettings[4]});
            var computerD = Task.Run(() => new Day5.IntComputer(program).Compute(inputD, outputD));

            var inputE = outputD;
            var computerE = Task.Run(() => new Day5.IntComputer(program).Compute(inputE, outputE));

            Task.WaitAll(computerA, computerB, computerC, computerD, computerE);

            outputE.TryDequeue(out var result);
            return result;
        }

        private static IEnumerable<int[]> EnumerateSettings(int minValue, int maxValue)
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
            var expected = 44282086;

            // Act
            var actual = CalculateMaxThrusterSignal(Day5.IntComputer.LoadProgram("Day7_input"));
            _testOutputHelper.WriteLine(
                $"{actual.signal} [{String.Join(", ", actual.settings.Select(i => i.ToString()))}]");

            // Assert
            Assert.Equal(expected, actual.signal);
        }
    }
}
