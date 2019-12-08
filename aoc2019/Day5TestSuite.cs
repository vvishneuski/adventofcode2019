using System.Linq;
using Xunit;

namespace aoc2019
{
    public class Day5TestSuite
    {
        [Theory]
        [InlineData(new[] {1, 0, 0, 0, 99}, new[] {2, 0, 0, 0, 99})]
        [InlineData(new[] {2, 3, 0, 3, 99}, new[] {2, 3, 0, 6, 99})]
        [InlineData(new[] {2, 4, 4, 5, 99, 0}, new[] {2, 4, 4, 5, 99, 9801})]
        [InlineData(new[] {1, 1, 1, 4, 99, 5, 6, 0, 99}, new[] {30, 1, 1, 4, 2, 5, 6, 0, 99})]
        [InlineData(new[] {1101, 100, -1, 4, 0}, new[] {1101, 100, -1, 4, 99})]
        [InlineData(new[] {1002, 4, 3, 4, 33}, new[] {1002, 4, 3, 4, 99})]
        public void ComputeResult(int[] program, int[] expected)
        {
            // Arrange
            var computer = new Day5.IntComputer(program);

            // Act
            computer.Compute();
            var actual = computer.Memory;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(new[] {3, 9, 8, 9, 10, 9, 4, 9, 99, -1, 8}, 8, 9)]
        [InlineData(new[] {3, 9, 7, 9, 10, 9, 4, 9, 99, -1, 8}, 7, 8)]
        [InlineData(new[] {3, 3, 1108, -1, 8, 3, 4, 3, 99}, 8, 7)]
        [InlineData(new[] {3, 3, 1107, -1, 8, 3, 4, 3, 99}, 3, 9)]
        [InlineData(new[] {3, 12, 6, 12, 15, 1, 13, 14, 13, 4, 13, 99, -1, 0, 1, 9}, 8, 0)]
        [InlineData(new[] {3, 3, 1105, -1, 9, 1101, 0, 0, 12, 4, 12, 99, 1}, 3, 0)]
        public void ComputeCondition(int[] program, int inputForTrue, int inputForFalse)
        {
            // Arrange
            var computer = new Day5.IntComputer(program);

            const int expectedTrue = 1;
            const int expectedFalse = 0;

            // Act
            var actualTrue = computer.Compute(inputForTrue);
            var actualFalse = computer.Compute(inputForFalse);

            // Assert
            Assert.Equal(expectedTrue, actualTrue[0]);
            Assert.Equal(expectedFalse, actualFalse[0]);
        }

        [Fact]
        public void ComputeConditionAdvanced()
        {
            // Arrange
            var program = new[]
            {
                3, 21, 1008, 21, 8, 20, 1005, 20, 22, 107, 8, 21, 20, 1006, 20, 31,
                1106, 0, 36, 98, 0, 0, 1002, 21, 125, 20, 4, 20, 1105, 1, 46, 104,
                999, 1105, 1, 46, 1101, 1000, 1, 20, 4, 20, 1105, 1, 46, 98, 99
            };
            var computer = new Day5.IntComputer(program);

            const int expectedIfBelowEight = 999;
            const int expectedIfEqualsToEight = 1000;
            const int expectedIfGreaterThanEight = 1001;

            // Act
            var actualIfBelowEight = computer.Compute(7);
            var actualIfEqualsToEight = computer.Compute(8);
            var actualIfGreaterThanEight = computer.Compute(9);

            // Assert
            Assert.Equal(expectedIfBelowEight, actualIfBelowEight[0]);
            Assert.Equal(expectedIfEqualsToEight, actualIfEqualsToEight[0]);
            Assert.Equal(expectedIfGreaterThanEight, actualIfGreaterThanEight[0]);
        }

        [Fact]
        public void ComputeResultFromInput5()
        {
            // Arrange
            var computer = new Day5.IntComputer(Day5.IntComputer.LoadProgram("Day5_input"));

            var expected = 16489636;

            // Act
            var actual = computer.Compute(1);

            // Assert
            computer.DumpMemory("Day5_output");
            Assert.All(actual.Take(actual.Length - 1), i => Assert.Equal(0, i));
            Assert.Equal(expected, actual[actual.Length - 1]);
        }


        [Fact]
        public void ComputeResultFromInputAdvanced()
        {
            // Arrange
            var computer = new Day5.IntComputer(Day5.IntComputer.LoadProgram("Day5_input"));

            var expected = 9386583;

            // Act
            var actual = computer.Compute(5)[0];

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void InputOutput()
        {
            // Arrange
            var computer = new Day5.IntComputer(3, 0, 4, 0, 99);
            var expected = new[] {15};

            // Act
            var actual = computer.Compute(expected);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
