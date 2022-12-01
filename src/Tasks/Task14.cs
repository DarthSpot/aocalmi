using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using AoCCommon;

namespace AoC2021.Tasks
{
    public class Task14 : ITask
    {
        public override TaskResult RunPartOne()
        {
            var input = InitTaskLines();
            var start = input[0];
            var templates = input.Skip(2).Select(x => x.Split(" -> "))
                .Select(x => (x[0], x[1].Single()))
                .ToDictionary(x => x.Item1, x => x.Item2);
            var steps = 10;
            var poly = start.ToList();
            for (var step = 0; step < steps; step++)
            {
                poly = poly.Zip(poly.Skip(1), (a, b) => (a, b))
                    .Select(x => new[] { templates[$"{x.a}{x.b}"], x.b }).SelectMany(x => x).Prepend(poly.First())
                    .ToList();
                Console.WriteLine(step);
            }

            var groups = poly.GroupBy(x => x).OrderBy(x => x.Count()).ToList();
            return GetResult(groups.Last().Count() - groups.First().Count());
        }

        public override TaskResult RunPartTwo()
        {
            var input = InitTaskLines();
            var start = input[0];
            Dictionary<(char a, char b), char> templates = input.Skip(2).Select(x => x.Split(" -> "))
                .Select(x => ((x[0][0], x[0][1]), x[1].Single()))
                .ToDictionary(x => x.Item1, x => x.Item2);

            var pairs = templates.Select(x => x.Key)
                .ToDictionary(x => x, x => BigInteger.Zero);
            
            foreach (var item in start.Zip(start.Skip(1), (a, b) => (a, b)))
                pairs[item] = BigInteger.One;

            for (var step = 0; step < 40; step++)
            {
                foreach (var (key, count) in pairs.ToList())
                {
                    var conn = templates[key];
                    var l = (key.a, conn);
                    var r = (conn, key.b);
                    pairs[key] -= count;
                    pairs[l] += count;
                    pairs[r] += count;
                }
            }
            pairs.Add((start.Last(), '_'), BigInteger.One);

            var charCount = pairs.Select(x => (x.Key.a, x.Value))
                .GroupBy(x => x.a)
                .Select(x => (x.Key,
                    x.Select(n => n.Value).Aggregate(BigInteger.Zero, (current, val) => current + val)))
                .ToList();
            
            var groups = charCount.OrderBy(x => x.Item2).ToList();
            var result = groups.Last().Item2 - groups.First().Item2;
            return GetResult(result.ToString());
        }
    }
}