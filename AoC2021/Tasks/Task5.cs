using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AoCCommon;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Metadata;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace AoC2021.Tasks
{
    public class Task5 : ITask
    {
        public override TaskResult RunPartOne()
        {
            var input = InitTaskLines();
            var vents = input
                .Select(x => x.Split(" -> "))
                .Select(x => (x[0].Split(',').Select(c => Convert.ToInt32(c)).ToArray(),
                    x[1].Split(',').Select(c => Convert.ToInt32(c)).ToArray()))
                .Where(x => x.Item1[0] == x.Item2[0] || x.Item1[1] == x.Item2[1])
                .ToList();
            var res = new Dictionary<(int, int), int>();
            var add = new Action<(int, int)>(key =>
            {
                if (res.ContainsKey(key))
                    res[key] = res[key] + 1;
                else
                    res[key] = 1;
            });

            foreach (var vent in vents)
            {
                var xs = new[] { vent.Item1[0], vent.Item2[0] }.OrderBy(x => x).ToArray();
                var ys = new[] { vent.Item1[1], vent.Item2[1] }.OrderBy(x => x).ToArray();
                
                for (var y = ys[0]; y <= ys[1]; y++)
                    for (var x = xs[0]; x <= xs[1]; x++)
                    {
                        add((x, y));
                    }
            }

            // PrintMap(res);

            return GetResult(res.Count(x => x.Value > 1));
        }

        public void PrintMap(Dictionary<(int, int), int> map)
        {
            var keys = map.Keys;
            for (var y = keys.Min(k => k.Item2); y <= keys.Max(k => k.Item2); y++)
            {
                for (var x = keys.Min(k => k.Item1); x <= keys.Max(k => k.Item1); x++)
                {
                    if (map.ContainsKey((x, y)))
                        Console.Write(map[(x, y)]);
                    else
                        Console.Write('.');
                }
                Console.WriteLine();
            }
        }

        public void PrintMapImage(Dictionary<(int, int), int> map, string path)
        {
            var keys = map.Keys;
            var minX = keys.Min(k => k.Item1);
            var maxX = keys.Max(k => k.Item1);
            var minY = keys.Min(k => k.Item2);
            var maxY = keys.Max(k => k.Item2);
            var width = maxX - minX + 1;
            var height = maxY - minY + 1;
            using (var image = new Image<Rgba32>(width, height))
            {
                image.Mutate(ctx =>
                {
                    ctx.BackgroundColor(Color.Black);
                    for (var y = minY; y <= maxY; y++)
                    {
                        for (var x = minX; x <= maxX; x++)
                        {
                            var val = map.ContainsKey((x, y)) ? map[(x, y)] : 0;
                            var col = val switch
                            {
                                1 => Color.Grey,
                                2 => Color.Yellow,
                                > 2 => Color.Red,
                                _ => Color.Black
                            };

                            image[x-minX,y-minY] = col;
                        }
                    }
                });

                image.SaveAsBmp(path);
            }

            Console.WriteLine(path);
        }

        public override TaskResult RunPartTwo()
        {
            var input = InitTaskLines();
            var vents = input
                .Select(x => x.Split(" -> "))
                .Select(x => (x[0].Split(',').Select(c => Convert.ToInt32(c)).ToArray(),
                    x[1].Split(',').Select(c => Convert.ToInt32(c)).ToArray()))
                .ToList();
            var res = new Dictionary<(int, int), int>();
            var add = new Action<(int, int)>(key =>
            {
                if (res.ContainsKey(key))
                    res[key] = res[key] + 1;
                else
                    res[key] = 1;
            });
            
            foreach (var vent in vents)
            {
                var start = vent.Item1;
                var end = vent.Item2;

                var xMov = end[0] > start[0] ? 1 : end[0] == start[0] ? 0 : -1;
                var yMov = end[1] > start[1] ? 1 : end[1] == start[1] ? 0 : -1;
                var length = new[] { end[0] - start[0], end[1] - start[1] }.Select(x => Math.Abs(x)).Max();

                for (var i = 0; i <= length; i++)
                {
                    var x = start[0] + i * xMov;
                    var y = start[1] + i * yMov;
                    add((x, y));
                }
            }
            
            PrintMapImage(res, Path.GetTempFileName() + ".bmp");

            return GetResult(res.Count(x => x.Value > 1));
        }
    }
}