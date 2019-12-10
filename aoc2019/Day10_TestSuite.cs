using System.Collections;
using Xunit;

namespace aoc2019
{
    public class Day10_TestSuite
    {
        public static TheoryData<string, (int x, int y), int> GetBestLocationData
            => new TheoryData<string, (int x, int y), int>
            {
                {
                  @".#..#
                    .....
                    #####
                    ....#
                    ...##",
                    (3, 4),
                    8
                },
                {
                  @"......#.#.
                    #..#.#....
                    ..#######.
                    .#.#.###..
                    .#..#.....
                    ..#....#.#
                    #..#....#.
                    .##.#..###
                    ##...#..#.
                    .#....####",
                    (5, 8),
                    33
                },
                {
                  @"#.#...#.#.
                    .###....#.
                    .#....#...
                    ##.#.#.#.#
                    ....#.#.#.
                    .##..###.#
                    ..#...##..
                    ..##....##
                    ......#...
                    .####.###.",
                    (1, 2),
                    35
                },
                {
                  @".#..#..###
                    ####.###.#
                    ....###.#.
                    ..###.##.#
                    ##.##.#.#.
                    ....###..#
                    ..#.#..#.#
                    #..#.#.###
                    .##...##.#
                    .....#.#..",
                    (6, 3),
                    41
                },
                {
                  @".#..##.###...#######
                    ##.############..##.
                    .#.######.########.#
                    .###.#######.####.#.
                    #####.##.#.##.###.##
                    ..#####..#.#########
                    ####################
                    #.####....###.#.#.##
                    ##.#################
                    #####.##.###..####..
                    ..######..##.#######
                    ####.##.####...##..#
                    .#####..#.######.###
                    ##...#.##########...
                    #.##########.#######
                    .####.#.###.###.#.##
                    ....##.##.###..#####
                    .#.#.###########.###
                    #.#.#.#####.####.###
                    ###.##.####.##.#..##",
                    (11, 13),
                    210
                },
            };

        [Theory]
        [MemberData(nameof(GetBestLocationData))]
        public void GetBestLocation(string asteroidMap, (int x, int y) expectedLocation, int expectedDetectedAsteroid)
        {
            // Arrange
            
            // Act
            
            // Assert
        }

        [Fact]
        public void ParseAsteroidMap()
        {
            // Assert
            var asteroidMap = @".#
                                #.";

            var expected10 = true;
            var expected11 = false;
            
            // Act
            var actualAsteroidField = Day10.AsteroidField.ParseMap(asteroidMap);

            var actual10 = actualAsteroidField.Map[(1, 0)];
            var actual11 = actualAsteroidField.Map[(1, 1)];

            // Assert
            Assert.Equal(expected10, actual10);
            Assert.Equal(expected11, actual11);
        }
    }
}
