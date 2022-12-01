using System;
using System.Collections.Generic;
using System.Linq;
using AoCCommon;

namespace AoC2021.Tasks
{
    public class Task15 : ITask
    {
        public override TaskResult RunPartOne()
        {
            var input = InitTaskLines().Select(x => x.Select(c => c - '0').ToArray()).ToArray();
            var w = input[0].Length;
            var h = input.Length;
            var closed = new HashSet<(int x, int y)>();
            var openWays = new Dictionary<(int x, int y), int> { { (0, 0), 0 } };
            var getVal = new Func<(int x, int y), int>(dot => input[dot.y][dot.x]);

            while (!openWays.ContainsKey((w - 1, h - 1)))
            {
                var shortest = openWays
                    .OrderBy(x => x.Value)
                    .First();

                foreach (var (neighbor, len) in GetNeighbors(shortest.Key.x, shortest.Key.y, w, h)
                             .Where(n => !closed.Contains(n))
                             .Select(n => (n, getVal(n) + shortest.Value)))
                {
                    if (!openWays.ContainsKey(neighbor))
                    {
                        openWays.Add(neighbor, len);
                    }
                    else
                    {
                        if (openWays[neighbor] <= len) 
                            continue;
                        openWays[neighbor] = len;
                    }
                }

                closed.Add(shortest.Key);
                openWays.Remove(shortest.Key);
            }
            return GetResult(openWays[(w - 1, h - 1)]);
        }

        public override TaskResult RunPartTwo()
        {
            var input = InitTaskLines().Select(x => x.Select(c => c - '0').ToArray()).ToArray();
            var w = input[0].Length;
            var h = input.Length;
            var rw = w * 5;
            var rh = h * 5;
            var closed = new HashSet<(int x, int y)>();
            var openWays = new Dictionary<(int x, int y), int> { { (0, 0), 0 } };
            var getVal = new Func<(int x, int y), int>(dot =>
            {
                var xx = dot.x % w;
                var yy = dot.y % h;
                var nx = dot.x / w;
                var ny = dot.y / h;
                var val = input[yy][xx] + nx + ny;
                return val > 9 ? val % 9 : val;
            });

            while (!openWays.ContainsKey((rw - 1, rh - 1)))
            {
                var shortest = openWays
                    .OrderBy(x => x.Value)
                    .First();
                foreach (var (neighbor, len) in GetNeighbors(shortest.Key.x, shortest.Key.y, rw, rh)
                             .Where(n => !closed.Contains(n))
                             .Select(n => (n, getVal(n) + shortest.Value)))
                {
                    if (!openWays.ContainsKey(neighbor))
                    {
                        openWays.Add(neighbor, len);
                    }
                    else
                    {
                        if (openWays[neighbor] <= len) 
                            continue;
                        openWays[neighbor] = len;
                    }
                }
                closed.Add(shortest.Key);
                openWays.Remove(shortest.Key);
            }
            
            return GetResult(openWays[(rw - 1, rh - 1)]);
        }
        
        public IEnumerable<(int x, int y)> GetNeighbors(int x, int y, int width, int height)
        {
            return new (int x, int y)[] { (x - 1, y), (x, y - 1), (x + 1, y), (x, y + 1) }
                .Where(p => Enumerable.Range(0, width).Contains(p.x) && Enumerable.Range(0, height).Contains(p.y));
        }
    }
}