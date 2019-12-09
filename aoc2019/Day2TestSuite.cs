using System.Linq;
using Xunit;

namespace aoc2019
{
    public class Day2TestSuite
    {
        [Theory]
        [InlineData(new[] {1L, 0, 0, 0, 99}, new[] {2L, 0, 0, 0, 99})]
        [InlineData(new[] {2L, 3, 0, 3, 99}, new[] {2L, 3, 0, 6, 99})]
        [InlineData(new[] {2L, 4, 4, 5, 99, 0}, new[] {2L, 4, 4, 5, 99, 9801})]
        [InlineData(new[] {1L, 1, 1, 4, 99, 5, 6, 0, 99}, new[] {30L, 1, 1, 4, 2, 5, 6, 0, 99})]
        public void ComputeResult(long[] program, long[] expected)
        {
            // Arrange
            var computer = new Day5.IntComputer(program);

            // Act
            computer.Compute();
            var actual = computer.Memory.Select(cell => cell.Value);

            // Assert
            Assert.Equal(expected, actual);
        }

        private static (long actual, long noun, long verb) FindNounAndVerb(long expected)
        {
            var computer = new Day5.IntComputer(Day5.IntComputer.LoadProgram("Day2_input"));

            var actual = 0L;
            var noun = 0L;
            var verb = 0L;

            for (noun = 0L; noun <= 99; noun++)
            {
                for (verb = 0L; verb <= 99; verb++)
                {
                    computer.Program[1] = noun;
                    computer.Program[2] = verb;
                    computer.Compute();

                    actual = computer.Memory[0];

                    if (expected == actual) break;
                }

                if (expected == actual) break;
            }

            return (actual, noun, verb);
        }

        [Fact]
        public void ComputeResultFromInput()
        {
            // Arrange

            var computer = new Day5.IntComputer(Day5.IntComputer.LoadProgram("Day2_input"));
            computer.Program[1] = 12;
            computer.Program[2] = 2;

            var expected = 6327510;

            // Act
            computer.Compute();
            var actual = computer.Memory[0];

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ComputeResultRealFromInput()
        {
            // Arrange
            var expected = 19690720;
            var expectedNoun = 41;
            var expectedVerb = 12;

            // Act
            var (actual, actualNoun, actualVerb) = FindNounAndVerb(expected);

            // Assert
            Assert.Equal(expected, actual);
            Assert.Equal(expectedNoun, actualNoun);
            Assert.Equal(expectedVerb, actualVerb);
        }
    }
}
