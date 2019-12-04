using System;
using System.Collections.Generic;
using System.Linq;

namespace aoc2019
{
    public static class Day3
    {
        public static int GetDistance(this (int x, int y) pointFrom, (int x, int y) pointTo)
        {
            return Math.Abs(pointTo.x - pointFrom.x) + Math.Abs(pointTo.y - pointFrom.y);
        }

        public static IEnumerable<(int, int)> GetRoute(this (int, int) startPoint, string move)
        {
            var direction = move[0];
            var distance = short.Parse(move.Substring(1));

            return Enumerable.Range(1, distance).Select(step => startPoint.Move(direction, step));
        }

        private static (int, int) Move(this (int, int) point, char direction, int step)
        {
            var (x, y) = point;

            switch (direction)
            {
                case 'R':
                    y += step;
                    break;
                case 'L':
                    y -= step;
                    break;
                case 'U':
                    x += step;
                    break;
                case 'D':
                    x -= step;
                    break;
            }

            return (x, y);
        }

        public class Wire
        {
            public Wire(string path)
            {
                Path = path;
            }

            public string Path { get; }
            public IEnumerable<(int, int)> Route => Parse(Path);

            private static IEnumerable<(int, int)> Parse(string path)
            {
                var moves = path.Split(',');
                var startPoint = (0, 0);
                foreach (var move in moves)
                {
                    var route = startPoint.GetRoute(move).ToList();
                    foreach (var point in route) yield return point;

                    startPoint = route.Last();
                }
            }

            public IEnumerable<(int x, int y)> Intersect(Wire wire2)
            {
                return Route.Intersect(wire2.Route);
            }

            public int GetLength((int, int) cutPoint)
            {
                return 1 + Route.TakeWhile(point => point != cutPoint).Count();
            }
        }
    }
}
