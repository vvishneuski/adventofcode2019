using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc2019
{
    public static class Day6
    {
        public class SpaceObject
        {
            public SpaceObject(string id, params SpaceObject[] satellites)
            {
                Id = id;
                AddSatellites(satellites);
            }

            public string Id { get; }

            public LinkedList<SpaceObject> Satellites { get; private set; }

            public SpaceObject Orbits { get; private set; }

            private void AddSatellites(IEnumerable<SpaceObject> satellites)
            {
                Satellites = new LinkedList<SpaceObject>(satellites);
                foreach (var satellite in Satellites) satellite.Orbits = this;
            }

            private static int GetOrbitsCount(SpaceObject start, Predicate<SpaceObject> predicate)
            {
                var count = 0;
                var orbits = start;
                while (!predicate(orbits))
                {
                    count++;
                    orbits = orbits.Orbits;
                }

                return count;
            }

            public int GetOrbitsCount()
            {
                return GetOrbitsCount(Orbits, orbits => orbits == null);
            }

            public int GetOrbitsCount(SpaceObject target)
            {
                return GetOrbitsCount(this, target.Equals);
            }

            public int GetOrbitsCount(string id)
            {
                return GetOrbitsCount(this, orbits => id.Equals(orbits.Id));
            }

            private static int GetSatellitesCount(SpaceObject start, int count, Predicate<SpaceObject> predicate)
            {
                if (predicate(start))
                    return count;

                if (!start.Satellites.Any())
                    return -1;

                count++;
                return start.Satellites.Select(satellite => GetSatellitesCount(satellite, count, predicate)).Max();
            }

            public int GetSatellitesCount(SpaceObject target)
            {
                return GetSatellitesCount(this, 0, target.Equals);
            }

            public int GetSatellitesCount(string id)
            {
                return GetSatellitesCount(this, 0, orbits => id.Equals(orbits.Id));
            }

            private static int GetAllRoutesCount(SpaceObject start, int count)
            {
                count++;
                return start.Satellites.Select(satellite => GetAllRoutesCount(satellite, count)).Sum() + count - 1;
            }

            public int GetAllRoutesCount()
            {
                return GetAllRoutesCount(this, 0);
            }

            private static IList<SpaceObject> GetSatellitesRoute(SpaceObject start, string destinationId,
                IList<SpaceObject> route = null)
            {
                if (destinationId.Equals(start.Id))
                    return route;

                if (!start.Satellites.Any())
                    return null;

                route = new List<SpaceObject>(route ?? Enumerable.Empty<SpaceObject>())
                {
                    start
                };
                return start.Satellites.Select(satellite => GetSatellitesRoute(satellite, destinationId, route))
                    .SingleOrDefault(r => r != null);
            }

            public int GetOrbitalTransfers(string startId, string finishId)
            {
                var startRoute = GetSatellitesRoute(this, startId);
                var finishRoute = GetSatellitesRoute(this, finishId);

                var commonRoute = startRoute.Intersect(finishRoute).ToList();

                return startRoute.Count - commonRoute.Count
                       + finishRoute.Count - commonRoute.Count;
            }

            public static SpaceObject ParseSpaceMap(string spaceMap)
            {
                var pairs = spaceMap.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
                    .Select(GetPair);
                var objects = pairs.SelectMany(pair => new[] {pair.orbits, pair.satellite}).Distinct()
                    .ToDictionary(id => id, id => new SpaceObject(id));
                var orbits = pairs.GroupBy(pair => pair.orbits, pair => pair.satellite)
                    .Select(group => GetOrbits(group, objects));
                return orbits.ToList().Single(spaceObject => spaceObject.Orbits == null);
            }

            private static SpaceObject GetOrbits(IGrouping<string, string> group,
                IReadOnlyDictionary<string, SpaceObject> objects)
            {
                var orbits = objects[group.Key];
                orbits.AddSatellites(group.Select(id => objects[id]));
                return orbits;
            }

            private static (string orbits, string satellite) GetPair(string pair)
            {
                var orbitsSatellite = pair.Trim().Split(')');
                return (orbitsSatellite[0], orbitsSatellite[1]);
            }
        }
    }
}
