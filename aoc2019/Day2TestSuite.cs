using Xunit;

namespace aoc2019
{
    public class Day2TestSuite
    {
        [Theory]
        [InlineData(new[] {1, 0, 0, 0, 99}, new[] {2, 0, 0, 0, 99})]
        [InlineData(new[] {2, 3, 0, 3, 99}, new[] {2, 3, 0, 6, 99})]
        [InlineData(new[] {2, 4, 4, 5, 99, 0}, new[] {2, 4, 4, 5, 99, 9801})]
        [InlineData(new[] {1, 1, 1, 4, 99, 5, 6, 0, 99}, new[] {30, 1, 1, 4, 2, 5, 6, 0, 99})]
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

        private static (int actual, int noun, int verb) FindNounAndVerb(int expected)
        {
            var computer = new Day5.IntComputer(Day5.IntComputer.LoadProgram("Day2_input"));

            var actual = 0;
            int noun;
            var verb = 0;

            for (noun = 0; noun <= 99; noun++)
            {
                for (verb = 0; verb <= 99; verb++)
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
