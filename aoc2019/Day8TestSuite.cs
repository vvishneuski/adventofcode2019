using System.Linq;
using aoc2019.Utils;
using Xunit;
using Xunit.Abstractions;

namespace aoc2019
{
    public class Day8TestSuite
    {
        public Day8TestSuite(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        private readonly ITestOutputHelper _testOutputHelper;

        private static string LoadImageData(string name)
        {
            using (var input = AssetManager.GetAsset(name))
            {
                return input.ReadToEnd();
            }
        }

        private static (int[][] layer, int count0, int count1, int count2) CalculateLayer(int[][] layer)
        {
            return (layer, CountPixels(0), CountPixels(1), CountPixels(2));

            int CountPixels(int pixelColor)
            {
                return layer.SelectMany(row => row).Count(pixel => pixel == pixelColor);
            }
        }

        private void OutputDisplay(int[][] display)
        {
            foreach (var row in display)
            {
                _testOutputHelper.WriteLine(string.Join(string.Empty, row.Select(pixel => pixel == 0 ? " " : "*")));
            }
        }

        [Fact]
        public void DecodeImage()
        {
            // Arrange
            var imageData = "123456789012";

            var expectedL1 = new[] {new[] {1, 2, 3}, new[] {4, 5, 6}};
            var expectedL2 = new[] {new[] {7, 8, 9}, new[] {0, 1, 2}};

            // Act
            var image = Day8.Image.Decode(imageData, width: 3, height: 2);

            var actualL1 = image.Layers[0];
            var actualL2 = image.Layers[1];

            // Assert
            Assert.Equal(expectedL1, actualL1);
            Assert.Equal(expectedL2, actualL2);
        }

        [Fact]
        public void LoadImage()
        {
            // Arrange
            var image = Day8.Image.Decode(LoadImageData("Day8_input"), 25, 6);

            var expected = 1742;
            var expectedDisplay = new[]
            {
                new[] {0, 1, 1, 0, 0, 0, 0, 1, 1, 0, 1, 0, 0, 0, 1, 1, 1, 1, 1, 0, 0, 1, 1, 0, 0},
                new[] {1, 0, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1, 0, 0, 1, 0},
                new[] {1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0, 1, 0, 1, 1, 1, 0, 0, 1, 0, 0, 1, 0},
                new[] {1, 0, 1, 1, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 1, 1, 1, 1, 0},
                new[] {1, 0, 0, 1, 0, 1, 0, 0, 1, 0, 0, 0, 1, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 1, 0},
                new[] {0, 1, 1, 1, 0, 0, 1, 1, 0, 0, 0, 0, 1, 0, 0, 1, 1, 1, 1, 0, 1, 0, 0, 1, 0},
            };

            // Act
            var layers = image.Layers.Select(CalculateLayer).ToList();
            var min = layers.Min(l => l.count0);
            var (_, _, count1, count2) = layers.Single(l => l.count0 == min);
            var actual = count1 * count2;

            var actualDisplay = image.Display();
            OutputDisplay(actualDisplay);

            // Assert
            Assert.Equal(expected, actual);
            Assert.Equal(expectedDisplay, actualDisplay);
        }
    }
}
