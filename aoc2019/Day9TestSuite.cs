using System.Security.Cryptography;
using Xunit;

namespace aoc2019
{
    public class Day9TestSuite
    {
        [Theory]
        [InlineData(new long[] {109, 1, 204, -1, 1001, 100, 1, 100, 1008, 100, 16, 101, 1006, 101, 0, 99},
                    new long[] {109, 1, 204, -1, 1001, 100, 1, 100, 1008, 100, 16, 101, 1006, 101, 0, 99})]
        [InlineData(new long[] {1102, 34915192, 34915192, 7, 4, 7, 99, 0}, new long[] {1219070632396864})]
        [InlineData(new long[] {104, 1125899906842624, 99}, new long[] {1125899906842624})]
        public void ComputeOutput(long[] program, long[] expectedOutput)
        {
            // Act
            var actualOutput = new Day5.IntComputer(program).Compute();
            
            // Assert
            Assert.Equal(expectedOutput, actualOutput);
        }

        [Fact]
        public void TestBoostProgram()
        {
            // Arrange
            var computer = new Day5.IntComputer(Day5.IntComputer.LoadProgram("Day9_input"));

            var expectedOutput = new[] {2350741403L};
            
            // Act
            var actualOutput = computer.Compute(1);
            
            // Assert
            Assert.Equal(expectedOutput, actualOutput);
        }

        [Fact]
        public void RunBoostProgram()
        {
            // Arrange
            var computer = new Day5.IntComputer(Day5.IntComputer.LoadProgram("Day9_input"));

            var expectedOutput = new[] {53088L};
            
            // Act
            var actualOutput = computer.Compute(2);
            
            // Assert
            Assert.Equal(expectedOutput, actualOutput);
        }
    }
}
