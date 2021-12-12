using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace AoC2021.Tasks
{
    public class Task9 : ITask
    {
        public override TaskResult RunPartOne()
        {
            var input = InitTaskString()
                .Trim()
                .Split("\n")
                .Select(x => x.Select(c => c - '0').ToArray())
                .ToList();
            var w = input[0].Length;
            var h = input.Count;

            List<(int x, int y)> lowPoints = input.SelectMany(x => x)
                .Select((p, i) => (i % w, i / w, p))
                .Where(p => GetNeighbors(p.Item1, p.Item2, w, h).Select(n => input[n.y][n.x]).Min() > p.p)
                .Select(x => (x.Item1, x.Item2))
                .ToList();
            
            return GetResult(lowPoints.Select(x => input[x.y][x.x] + 1).Sum());
        }

        public IEnumerable<(int x, int y)> GetNeighbors(int x, int y, int width, int height)
        {
            return new (int x, int y)[] { (x - 1, y), (x, y - 1), (x + 1, y), (x, y + 1) }
                .Where(p => Enumerable.Range(0, width).Contains(p.x) && Enumerable.Range(0, height).Contains(p.y));
        }


        public override TaskResult RunPartTwo()
        {
            var input = InitTaskString()
                .Trim()
                .Split("\n")
                .Select(x => x.Select(c => c - '0').ToArray())
                .ToList();
            var w = input[0].Length;
            var h = input.Count;

            List<(int x, int y)> lowPoints = input.SelectMany(x => x)
                .Select((p, i) => (i % w, i / w, p))
                .Where(p => GetNeighbors(p.Item1, p.Item2, w, h).Select(n => input[n.y][n.x]).Min() > p.p)
                .Select(x => (x.Item1, x.Item2))
                .ToList();

            var basins = lowPoints.Select(lp =>
            {
                var basin = new List<(int x, int y)>();
                var newItems = new List<(int x, int y)> { lp };
                while (newItems.Any())
                {
                    basin.AddRange(newItems);
                    newItems.Clear();
                    newItems.AddRange(basin.Select(item => GetNeighbors(item.x, item.y, w, h)
                            .Where(x => !basin.Contains(x))
                            .Where(b => input[b.y][b.x] < 9)
                            .Where(b => input[b.y][b.x] > input[item.y][item.x]))
                        .SelectMany(x => x)
                        .Distinct());
                }
                return basin.Distinct().ToList();
            }).ToList();

            PrintBasin(input, basins.SelectMany(x => x).ToList(), w, h);
            return GetResult(basins.Select(x => x.Count).OrderByDescending(x => x).Take(3).Aggregate((x, y) => x * y));
        }

        private void PrintBasin(List<int[]> input, List<(int x, int y)> basinPoints, int width, int height)
        {
            var image = new Image<Rgba32>(width, height);
            
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    if (basinPoints.Contains((x, y)))
                    {
                        var point = input[y][x];
                        image[x,y] =  point switch
                        {
                            0 => Color.Red,
                            1 => Color.DarkRed,
                            2 => Color.OrangeRed,
                            3 => Color.Orange,
                            4 => Color.Gold,
                            5 => Color.Yellow,
                            6 => Color.LightGreen,
                            7 => Color.YellowGreen,
                            8 => Color.GreenYellow,
                            _ => throw new ArgumentOutOfRangeException()
                        };
                    }
                    else
                    {
                        image[x, y] = Color.Green;
                    }
                }
            }

            var path = Path.GetTempFileName() + ".bmp";
            image.SaveAsBmp(path);
            Console.WriteLine(path);
        }
    }
}