using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml.Xsl;

namespace AoC2021.Tasks
{
    public class Task22 : ITask
    {
        public override TaskResult RunPartOne()
        {
            (string Value, Cube cube)[] input = InitTaskLines()
                .Select(x => Regex.Match(x, "((on)|(off)) x=([0-9-]+)..([0-9-]+),y=([0-9-]+)..([0-9-]+),z=([0-9-]+)..([0-9-]+)"))
                .Select(x => (x.Groups[1].Value, 
                        new Cube(
                            Convert.ToInt32(x.Groups[4].Value),
                            Convert.ToInt32(x.Groups[5].Value),
                            Convert.ToInt32(x.Groups[6].Value),
                            Convert.ToInt32(x.Groups[7].Value),
                            Convert.ToInt32(x.Groups[8].Value),
                            Convert.ToInt32(x.Groups[9].Value)
                        )))
                .ToArray();


            var cubes = new Dictionary<Cube, int>();
            var c = 0;
            
            foreach (var step in input)
            {
                cubes.Add(step.cube, step.Value == "on" ? 1 : 0);
            }

            var xmin = -50;
            var xmax = 50;
            var ymin = -50;
            var ymax = 50;
            var zmin = -50;
            var zmax = 50;
            for (var x = xmin; x <= xmax; x++)
                for (var y = ymin; y <= ymax; y++)
                    for (var z = zmin; z <= zmax; z++)
                    {
                        var lastBlob = cubes.Where(c => IsIn(c.Key, x, y, z)).ToList();
                        if (lastBlob.Any() && lastBlob.Last().Value == 1)
                            c++;
                    }        
            
            return GetResult(c);
        }

        private bool IsIn(Cube cube, int x, int y, int z)
        {
            return cube.xmin <= x && cube.xmax >= x
                                  && cube.ymin <= y && cube.ymax >= y
                                  && cube.zmin <= z && cube.zmax >= z;
        }

        public override TaskResult RunPartTwo()
        {
            var input = InitTaskLines().Select(x => Regex.Match(x, "((on)|(off)) x=([0-9-]+)..([0-9-]+),y=([0-9-]+)..([0-9-]+),z=([0-9-]+)..([0-9-]+)"))
                .Select(x => (x.Groups[1].Value, 
                    new Cube(
                        Convert.ToInt32(x.Groups[4].Value),
                        Convert.ToInt32(x.Groups[5].Value),
                        Convert.ToInt32(x.Groups[6].Value),
                        Convert.ToInt32(x.Groups[7].Value),
                        Convert.ToInt32(x.Groups[8].Value),
                        Convert.ToInt32(x.Groups[9].Value)
                    )))
                .ToArray();

            var cubes = new HashSet<Cube>();
            foreach (var step in input)
            {
                var overlaps = cubes.Select(x => (x, SplitCube(x, step.Item2))).ToList();
                if (overlaps.Any(x => x.Item2.Count > 0))
                {
                    foreach (var overlapper in overlaps)
                    {
                        var splits = overlapper.Item2.ToList();
                        cubes.Remove(overlapper.x);
                        foreach (var split in splits)
                            cubes.Add(split);
                    }
                }
                if (step.Value == "on")
                    cubes.Add(step.Item2);
            }

            return GetResult(cubes.Select(x => x.Size).Sum());
        }

        private bool IsCompletlyIn(Cube a, Cube b)
        {
            return a.xmin >= b.xmin
                   && a.xmax <= b.xmax
                   && a.ymin >= b.ymin
                   && a.ymax <= b.ymax
                   && a.zmin >= b.zmin
                   && a.zmax <= b.zmax;
        } 

        private List<Cube> SplitCube(Cube cubeA, Cube cubeB)
        {
            var splits = new List<Cube>();
            if (IsCompletlyIn(cubeA, cubeB))
                return splits;
            
            var getPairs = new Func<int, int, int, int, List<(int, int)>>((amin, amax, bmin, bmax) =>
            {
                var cmb = new[] { amin, bmin, amax, bmax }.OrderBy(x => x).ToList();
                var res = new List<(int, int)>();

                if (bmin <= amin && bmax >= amax)
                {
                    res.Add((amin, amax));
                }
                if (bmin < amin && bmax < amax)
                {
                    res.Add((amin, bmax));
                    res.Add((bmax + 1, amax));
                } 
                else if (bmin >= amin && bmax <= amax)
                {
                    res.Add((amin, bmin - 1));
                    res.Add((bmin, Math.Min(bmax, amax)));
                    res.Add((bmax + 1, amax));    
                } 
                else if (bmin > amin && bmax > amax)
                {
                    res.Add((amin, bmin-1));
                    res.Add((bmin, amax));
                }

                return res.Where(x => x.Item1 <= x.Item2 && x.Item1 >= amin && x.Item2 <= amax).ToList();
            });

            var xxs = getPairs(cubeA.xmin, cubeA.xmax, cubeB.xmin, cubeB.xmax);
            var yys = getPairs(cubeA.ymin, cubeA.ymax, cubeB.ymin, cubeB.ymax);
            var zzs = getPairs(cubeA.zmin, cubeA.zmax, cubeB.zmin, cubeB.zmax);
            if (!xxs.Any() || !yys.Any() || !zzs.Any())
                return new List<Cube>() { cubeA };
            
            foreach (var x in xxs)
                foreach (var y in yys)
                    foreach (var z in zzs)
                    {
                        var nc = new Cube(x.Item1, x.Item2,
                            y.Item1, y.Item2,
                            z.Item1, z.Item2);
                        
                        if (nc != null)
                            splits.Add(nc);
                    }

            var realParts = splits.Where(x => IsCompletlyIn(x, cubeA)).ToList();
            var exclude = splits.Where(x => IsCompletlyIn(x, cubeB)).ToList();
            var result = realParts.Where(x => !IsCompletlyIn(x, cubeB)).ToList();
            if (result.Sum(x => x.Size) > cubeA.Size)
            {
                Console.WriteLine("oO");
            }
            return result;
        }

        public record Cube(int xmin, int xmax, int ymin, int ymax, int zmin, int zmax)
        {
            public void Deconstruct(out int xmin, out int xmax, out int ymin, out int ymax, out int zmin, out int zmax)
            {
                xmin = this.xmin;
                xmax = this.xmax;
                ymin = this.ymin;
                ymax = this.ymax;
                zmin = this.zmin;
                zmax = this.zmax;
            }

            public long Size => (long)(xmax - xmin + 1) * (long)(ymax - ymin + 1) * (long)(zmax - zmin + 1);
        }
    }

}