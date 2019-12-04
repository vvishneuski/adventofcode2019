using System.Collections.Generic;
using System.Linq;
using aoc2019.Utils;
using Xunit;

namespace aoc2019
{
    public class Day3TestSuite
    {
        public static TheoryData<(int, int), (int, int), int> GetDistanceData
            => new TheoryData<(int, int), (int, int), int>
            {
                {(0, 0), (0, 1), 1},
                {(0, 0), (0, -1), 1},
                {(0, 0), (1, 0), 1},
                {(0, 0), (-1, 0), 1},
                {(0, 0), (1, 1), 2},
                {(0, 0), (1, -1), 2},
                {(0, 0), (-1, -1), 2},
                {(0, 0), (-1, 1), 2},
                {(1, 2), (0, 1), 2},
                {(1, 5), (0, 1), 5}
            };

        public static TheoryData<string, (int, int), IEnumerable<(int, int)>> GetRouteData
            => new TheoryData<string, (int, int), IEnumerable<(int, int)>>
            {
                {"R8", (0, 0), new[] {(0, 1), (0, 2), (0, 3), (0, 4), (0, 5), (0, 6), (0, 7), (0, 8)}},
                {"U5", (0, 8), new[] {(1, 8), (2, 8), (3, 8), (4, 8), (5, 8)}},
                {"L5", (5, 8), new[] {(5, 7), (5, 6), (5, 5), (5, 4), (5, 3)}},
                {"D3", (5, 3), new[] {(4, 3), (3, 3), (2, 3)}}
            };

        public static TheoryData<string, IEnumerable<(int, int)>> GetWireData
            => new TheoryData<string, IEnumerable<(int, int)>>
            {
                {
                    "R8,U5,L5,D3", new[]
                    {
                        (0, 1), (0, 2), (0, 3), (0, 4), (0, 5), (0, 6), (0, 7), (0, 8),
                        (1, 8), (2, 8), (3, 8), (4, 8), (5, 8),
                        (5, 7), (5, 6), (5, 5), (5, 4), (5, 3),
                        (4, 3), (3, 3), (2, 3)
                    }
                }
            };

        private static string[] Paths
        {
            get
            {
                using (var input = AssetManager.GetAsset("Day3_input"))
                {
                    return input.ReadToEnd().Split('\n');
                }
            }
        }

        public static TheoryData<string, string, int> GetDistanceInputData
            => new TheoryData<string, string, int>
            {
                {Paths[0], Paths[1], 5357}
            };

        public static TheoryData<string, string, int> GetMovesInputData
            => new TheoryData<string, string, int>
            {
                {Paths[0], Paths[1], 101956}
            };

        [Theory]
        [MemberData(nameof(GetDistanceData))]
        public void GetDistance((int, int) pointFrom, (int, int) pointTo, int expectedDistance)
        {
            // Act
            var actualDistance = pointFrom.GetDistance(pointTo);

            // Assert
            Assert.Equal(expectedDistance, actualDistance);
        }

        [Theory]
        [MemberData(nameof(GetRouteData))]
        public void GetRoute(string path, (int, int) startPoint, IEnumerable<(int, int)> expectedRoute)
        {
            // Act
            var actualRoute = startPoint.GetRoute(path);

            // Assert
            Assert.Equal(expectedRoute, actualRoute);
        }

        [Theory]
        [MemberData(nameof(GetWireData))]
        public void GetWire(string path, IEnumerable<(int, int)> expectedRoute)
        {
            // Arrange
            var wire = new Day3.Wire(path);

            // Act
            var actualRoute = wire.Route;

            // Assert
            Assert.Equal(expectedRoute, actualRoute);
        }

        [Theory]
        [InlineData("R8,U5,L5,D3", "U7,R6,D4,L4", 6)]
        [InlineData("R75,D30,R83,U83,L12,D49,R71,U7,L72", "U62,R66,U55,R34,D71,R55,D58,R83", 159)]
        [InlineData("R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51", "U98,R91,D20,R16,D67,R40,U7,R15,U6,R7", 135)]
        [MemberData(nameof(GetDistanceInputData))]
        public void GetDistanceToClosestCross(string path1, string path2, int expectedDistance)
        {
            // Arrange
            var wire1 = new Day3.Wire(path1);
            var wire2 = new Day3.Wire(path2);

            // Act
            var actualDistance = wire1.Intersect(wire2).Select(point => point.GetDistance((0, 0))).Min();

            // Assert
            Assert.Equal(expectedDistance, actualDistance);
        }

        [Theory]
        [InlineData("R8,U5,L5,D3", "U7,R6,D4,L4", 30)]
        [InlineData("R75,D30,R83,U83,L12,D49,R71,U7,L72", "U62,R66,U55,R34,D71,R55,D58,R83", 610)]
        [InlineData("R98,U47,R26,D63,R33,U87,L62,D20,R33,U53,R51", "U98,R91,D20,R16,D67,R40,U7,R15,U6,R7", 410)]
        [MemberData(nameof(GetMovesInputData))]
        public void GetMovesToFirstCross(string path1, string path2, int expectedWireLength)
        {
            // Arrange
            var wire1 = new Day3.Wire(path1);
            var wire2 = new Day3.Wire(path2);

            // Act
            var actualWireLength = wire1.Intersect(wire2)
                .Select(cross => wire1.GetLength(cross) + wire2.GetLength(cross)).Min();

            // Assert
            Assert.Equal(expectedWireLength, actualWireLength);
        }
    }
}
