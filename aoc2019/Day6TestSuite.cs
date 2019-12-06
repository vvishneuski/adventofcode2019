using aoc2019.Utils;
using Xunit;

namespace aoc2019
{
    public class Day6TestSuite
    {
        private static string LoadSpaceMap(string name)
        {
            using (var input = AssetManager.GetAsset(name))
            {
                return input.ReadToEnd();
            }
        }

        [Fact]
        public void GetOrbitalTransfers()
        {
            // Arrange
            var spaceMap =
                @"COM)B
                    B)C
                    C)D
                    D)E
                    E)F
                    B)G
                    G)H
                    D)I
                    E)J
                    J)K
                    K)L
                    K)YOU
                    I)SAN";

            var expectedOrbitalTransfers = 4;

            // Act
            var COM = Day6.SpaceObject.ParseSpaceMap(spaceMap);
            var actualOrbitalTransfers = COM.GetOrbitalTransfers("YOU", "SAN");

            // Assert
            Assert.Equal(expectedOrbitalTransfers, actualOrbitalTransfers);
        }

        [Fact]
        public void GetOrbitsCount()
        {
            // Arrange
            var E = new Day6.SpaceObject("E");
            var D = new Day6.SpaceObject("D", E);
            var C = new Day6.SpaceObject("C", D);
            var A = new Day6.SpaceObject("A");
            var B = new Day6.SpaceObject("B", A, C);
            var COM = new Day6.SpaceObject("COM", B);

            var expectedDBOrbits = 2;
            var expectedAOrbits = 2;
            var expectedCBOrbits = 1;
            var expectedEOrbits = 4;

            // Act
            var actualDOrbits = D.GetOrbitsCount("B");
            var actualAOrbits = A.GetOrbitsCount();
            var actualCBOrbits = C.GetOrbitsCount(B);
            var actualEOrbits = COM.GetSatellitesCount(E);

            // Assert
            Assert.Equal(expectedDBOrbits, actualDOrbits);
            Assert.Equal(expectedAOrbits, actualAOrbits);
            Assert.Equal(expectedCBOrbits, actualCBOrbits);
            Assert.Equal(expectedEOrbits, actualEOrbits);
        }

        [Fact]
        public void ParseSpaceMap()
        {
            // Arrange
            var spaceMap =
                @"COM)B
                    B)C
                    C)D
                    D)E
                    E)F
                    B)G
                    G)H
                    D)I
                    E)J
                    J)K
                    K)L";

            var expectedId = "COM";
            var expectedFOrbits = 5;
            var expectedAllRoutes = 42;

            // Act
            var COM = Day6.SpaceObject.ParseSpaceMap(spaceMap);
            var actualId = COM.Id;
            var actualFOrbits = COM.GetSatellitesCount("F");
            var actualAllRoutes = COM.GetAllRoutesCount();

            // Assert
            Assert.Equal(expectedId, actualId);
            Assert.Equal(expectedFOrbits, actualFOrbits);
            Assert.Equal(expectedAllRoutes, actualAllRoutes);
        }

        [Fact]
        public void ParseSpaceMapFromInput()
        {
            // Arrange
            var spaceMap = LoadSpaceMap("Day6_input");
            var expectedAllRoutes = 223251;
            var expectedOrbitalTransfers = 430;

            // Act
            var COM = Day6.SpaceObject.ParseSpaceMap(spaceMap);
            var actualAllRoutes = COM.GetAllRoutesCount();
            var actualOrbitalTransfers = COM.GetOrbitalTransfers("YOU", "SAN");

            // Assert
            Assert.Equal(expectedAllRoutes, actualAllRoutes);
            Assert.Equal(expectedOrbitalTransfers, actualOrbitalTransfers);
        }
    }
}
