using System.Collections.Generic;
using System.Linq;
using AoCCommon;

namespace AoC2021.Tasks
{
    public class Task11 : ITask
    {
        public override TaskResult RunPartOne()
        {
            var input = InitTaskLines()
                .Select(x => x.Select(c => c - '0').ToArray())
                .ToList();
            var w = input[0].Length;
            var h = input.Count;
            var steps = 100;
            var totalflashes = 0;

            for (var step = 0; step < steps; step++)
            {
                for (var y = 0; y < h; y++)
                {
                    for (var x = 0; x < w; x++)
                    {
                        input[y][x] += 1;
                    }
                }

                var flashes = new HashSet<(int x, int y)>();

                var nf = input.SelectMany(x => x)
                    .Select((p, i) => (i % w, i / w, p))
                    .Where(x => x.p > 9 && !flashes.Contains((x.Item1, x.Item2)))
                    .ToList();
                do
                {
                    foreach (var (x, y, _) in nf)
                    {
                        flashes.Add((x,y));
                        foreach (var (nx,ny) in GetNeighbors(x, y, w, h))
                        {
                            input[ny][nx] += 1;
                        }
                    }
                    
                    nf = input.SelectMany(x => x)
                        .Select((p, i) => (i % w, i / w, p))
                        .Where(x => x.p > 9 && !flashes.Contains((x.Item1, x.Item2)))
                        .ToList();
                } while (nf.Any());

                totalflashes += flashes.Count;
                foreach (var (x,y, _) in input.SelectMany(x => x)
                             .Select((p, i) => (i % w, i / w, p))
                             .Where(x => x.p > 9))
                {
                    input[y][x] = 0;
                }
            }

            return GetResult(totalflashes);
        }

        public override TaskResult RunPartTwo()
        {
            var input = InitTaskLines()
                .Select(x => x.Select(c => c - '0').ToArray())
                .ToList();
            var w = input[0].Length;
            var h = input.Count;
            var octoCount = input.SelectMany(x => x).Count();
            var step = 0;
            
            while (true)
            {
                step++;
                for (var y = 0; y < h; y++)
                {
                    for (var x = 0; x < w; x++)
                    {
                        input[y][x] += 1;
                    }
                }

                var flashes = new HashSet<(int x, int y)>();

                var nf = input.SelectMany(x => x)
                    .Select((p, i) => (i % w, i / w, p))
                    .Where(x => x.p > 9 && !flashes.Contains((x.Item1, x.Item2)))
                    .ToList();
                do
                {
                    foreach (var (x, y, _) in nf)
                    {
                        flashes.Add((x,y));
                        foreach (var (nx,ny) in GetNeighbors(x, y, w, h))
                        {
                            input[ny][nx] += 1;
                        }
                    }
                    
                    nf = input.SelectMany(x => x)
                        .Select((p, i) => (i % w, i / w, p))
                        .Where(x => x.p > 9 && !flashes.Contains((x.Item1, x.Item2)))
                        .ToList();
                } while (nf.Any());

                if (flashes.Count == octoCount)
                    return GetResult(step);
                
                
                foreach (var (x,y, _) in input.SelectMany(x => x)
                             .Select((p, i) => (i % w, i / w, p))
                             .Where(x => x.p > 9))
                {
                    input[y][x] = 0;
                }
            }

        }
        
        public IEnumerable<(int x, int y)> GetNeighbors(int x, int y, int width, int height)
        {
            return new (int x, int y)[] { (x - 1, y), (x, y - 1), (x + 1, y), (x, y + 1), (x+1, y+1), (x-1, y-1), (x+1, y-1), (x-1, y+1) }
                .Where(p => Enumerable.Range(0, width).Contains(p.x) && Enumerable.Range(0, height).Contains(p.y));
        }
    }
}