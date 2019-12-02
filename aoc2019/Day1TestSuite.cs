using System;
using System.Collections.Generic;
using aoc2019.Utils;
using Xunit;

namespace aoc2019
{
    public class Day1TestSuite
    {
        [Theory]
        [InlineData(12, 2)]
        [InlineData(14, 2)]
        [InlineData(1969, 654)]
        [InlineData(100756, 33583)]
        public void GetFuelRequired(long mass, long expectedFuel)
        {
            // Act
            var actualFuel = Day1.GetFuelRequired(mass);

            // Assert
            Assert.Equal(expectedFuel, actualFuel);
        }

        [Theory]
        [InlineData(new long[] {12, 14, 1969, 100756}, 2 + 2 + 654 + 33583)]
        public void SumFuelRequired(IEnumerable<long> masses, long expectedFuel)
        {
            // Act
            var actualFuel = Day1.SumFuelRequired(masses, Day1.GetFuelRequired);

            // Assert
            Assert.Equal(expectedFuel, actualFuel);
        }

        private static IEnumerable<long> LoadMasses(string name)
        {
            using (var input = AssetManager.GetAsset(name))
            {
                string inputLine;
                while ((inputLine = input.ReadLine()) != null)
                {
                    yield return Int64.Parse(inputLine);
                }
            }
        }

        [Fact]
        public void SumFuelRequiredFromInput()
        {
            // Arrange
            var expectedFuel = 3161483;
            
            // Act
            var actualFuel = Day1.SumFuelRequired(LoadMasses("Day1_input"), Day1.GetFuelRequired);

            // Assert
            Assert.Equal(expectedFuel, actualFuel);
        }

        [Theory]
        [InlineData(12, 2)]
        [InlineData(14, 2)]
        [InlineData(1969, 966)]
        [InlineData(100756, 50346)]
        public void GetFuelRequiredReal(long mass, long expectedFuel)
        {
            // Act
            var actualFuel = Day1.GetFuelRequiredReal(mass);

            // Assert
            Assert.Equal(expectedFuel, actualFuel);
        }

        [Fact]
        public void SumFuelRequiredRealFromInput()
        {
            // Arrange
            var expectedFuel = 4739374;
            
            // Act
            var actualFuel = Day1.SumFuelRequired(LoadMasses("Day1_input"), Day1.GetFuelRequiredReal);

            // Assert
            Assert.Equal(expectedFuel, actualFuel);
        }
    }
}
