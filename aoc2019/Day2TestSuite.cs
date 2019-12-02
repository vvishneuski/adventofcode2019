using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using aoc2019.Utils;
using Xunit;
using Xunit.Abstractions;

namespace aoc2019
{
    public class Day2TestSuite
    {
        private ITestOutputHelper TestOutputHelper { get; }

        public Day2TestSuite(ITestOutputHelper testOutputHelper)
        {
            TestOutputHelper = testOutputHelper;
        }

        [Theory]
        [InlineData(new[] {1, 0, 0, 0, 99}, new[] {2, 0, 0, 0, 99})]
        [InlineData(new[] {2, 3, 0, 3, 99}, new[] {2, 3, 0, 6, 99})]
        [InlineData(new[] {2, 4, 4, 5, 99, 0}, new[] {2, 4, 4, 5, 99, 9801})]
        [InlineData(new[] {1, 1, 1, 4, 99, 5, 6, 0, 99}, new[] {30, 1, 1, 4, 2, 5, 6, 0, 99})]
        public void ComputeResult(int[] program, int[] expected)
        {
            // Arrange
            Day2.Memory = program;
            
            // Act
            Day2.Compute();
            var actual = Day2.Memory;
            
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ComputeResultFromInput()
        {
            // Arrange
            Day2.Memory = LoadProgram("Day2_input").ToArray();
            Day2.Memory[1] = 12;
            Day2.Memory[2] = 2;
            
            var expected = 6327510;
            
            // Act
            Day2.Compute();
            var actual = Day2.Memory[0];
            
            // Assert
            Assert.Equal(expected, actual);
            DumpMemory("Day2_output", Day2.Memory);
        }

        private static IEnumerable<int> LoadProgram(string name)
        {
            using (var input = AssetManager.GetAsset(name))
            {
                return input.ReadToEnd().Split(',').Select(Int32.Parse);
            }
        }

        private static void DumpMemory(string name, IEnumerable<int> memory)
        {
            using (var fileStream = AssetManager.OpenAsset(name))
            using (var output = new StreamWriter(fileStream))
            {
                output.WriteLine(String.Join(",", memory));
                output.Close();
            }
        }

        [Fact]
        public void ComputeResultRealFromInput()
        {
            // Arrange
            var expected = 19690720;

            // Act
            var (actual, noun, verb) = FindNounAndVerb(expected);
            
            // Assert
            Assert.Equal(expected, actual);
            
            DumpMemory("Day2_outputReal", Day2.Memory);
            TestOutputHelper.WriteLine($@"{noun} {verb} {actual}");
        }

        private static (int actual, int noun, int verb) FindNounAndVerb(int expected)
        {
            for (var noun = 0; noun <= 99; noun++)
            {
                for (var verb = 0; verb <= 99; verb++)
                {
                    Day2.Memory = LoadProgram("Day2_input").ToArray();
                    Day2.Memory[1] = noun;
                    Day2.Memory[2] = verb;

                    Day2.Compute();
                    var actual = Day2.Memory[0];

                    if (expected == actual)
                    {
                        return (actual, noun, verb);
                    }
                }
            }

            return (0, 0, 0);
        }
    }
}
