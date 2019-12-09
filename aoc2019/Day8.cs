using System;
using System.Collections.Generic;
using System.Linq;
using aoc2019.Utils;

namespace aoc2019
{
    public static class Day8
    {
        public class Image
        {
            private readonly int _height;
            private readonly int _width;

            private Image(IEnumerable<int[][]> layers, int width, int height)
            {
                _width = width;
                _height = height;
                Layers = layers.ToList();
            }

            public IList<int[][]> Layers { get; }

            public static Image Decode(string imageData, int width, int height)
            {
                var rows = imageData.Select(Char.GetNumericValue).Select(Convert.ToInt32).SplitUp(width);
                var rawLayers = rows.SplitUp(height);
                return new Image(rawLayers.Select(GetLayer), width, height);
            }

            private static int[][] GetLayer(IEnumerable<IEnumerable<int>> rawLayer)
            {
                return rawLayer.Select(row => row.ToArray()).ToArray();
            }

            public int[][] Display()
            {
                int[][] display = new int[_height][];
                for (int y = 0; y < _height; y++)
                {
                    display[y] = new int[_width];
                    for (int x = 0; x < _width; x++)
                    {
                        display[y][x] = Layers.First(layer => layer[y][x] != 2)[y][x];
                    }
                }

                return display;
            }
        }
    }
}
