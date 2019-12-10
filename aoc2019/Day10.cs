using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace aoc2019
{
    public static class Day10
    {
        public class AsteroidField
        {
            public IDictionary<(int x, int y), bool> Map { get; }

            private AsteroidField(IDictionary<(int x, int y), bool> map)
            {
                Map = new ConcurrentDictionary<(int x, int y), bool>(map);
            }

            public static AsteroidField ParseMap(string asteroidMap)
            {
                var rows = asteroidMap.Split(new []{Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
                var map = new Dictionary<(int x, int y), bool>();
                for (var x = 0; x < rows.Count(); x++)
                {
                    for (var y = 0; y < rows[x].Length; y++)
                    {
                        map[(x, y)] = rows[x][y] == '#';
                    }
                }
                return new AsteroidField(map);
            }
        }
    }
}
