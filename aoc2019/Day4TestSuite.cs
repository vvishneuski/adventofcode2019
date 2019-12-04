using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace aoc2019
{
    public class Day4TestSuite
    {
        public static TheoryData<int, IEnumerable<int>> GetDigitsData
            => new TheoryData<int, IEnumerable<int>>
            {
                {111111, new[] {1, 1, 1, 1, 1, 1}},
                {123456, new[] {1, 2, 3, 4, 5, 6}},
                {567111, new[] {5, 6, 7, 1, 1, 1}},
                {233456, new[] {2, 3, 3, 4, 5, 6}}
            };

        [Theory]
        [MemberData(nameof(GetDigitsData))]
        public void GetDigits(int number, IEnumerable<int> expectedDigits)
        {
            // Act
            var actualDigits = number.GetDigits();
            
            // Assert
            Assert.Equal(expectedDigits, actualDigits);
        }

        [Theory]
        [InlineData(111111, true)]
        [InlineData(123456, true)]
        [InlineData(567111, false)]
        [InlineData(233456, true)]
        public void DigitsAreNotDecrease(int number, bool expected)
        {
            // Act
            var actual = number.DigitsAreNotDecrease();
            
            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(111111, true)]
        [InlineData(123456, false)]
        [InlineData(567111, true)]
        [InlineData(233456, true)]
        public void HasTwoAdjacentDigitsTheSame(int number, bool expected)
        {
            // Act
            var actual = number.HasTwoAdjacentDigitsTheSame();
            
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPossiblePasswords()
        {
            // Arrange
            var expected = 1650;
            
            // Act
            var actual = Day4.GetPossiblePasswords("178416-676461").Count();
            
            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(112233, true)]
        [InlineData(123444, false)]
        [InlineData(111122, true)]
        public void HasExactTwoDigitsTheSame(int number, bool expected)
        {
            // Act
            var actual = number.HasExactTwoDigitsTheSame();
            
            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void GetPossiblePasswordsAdvanced()
        {
            // Arrange
            var expected = 1129;
            
            // Act
            var passwords = Day4.GetPossiblePasswordsAdvanced("178416-676461");
            var actual = passwords.Count();
            
            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
