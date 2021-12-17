using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2021.Tasks
{
    public class Task17 : ITask
    {
        public override TaskResult RunPartOne()
        {
            var input = InitTaskString();
            var regex = Regex.Match(input, "target area: x=([0-9-]+)..([0-9-]+), y=([0-9-]+)..([0-9-]+)");
            var (maxy, miny) = (Convert.ToInt32(regex.Groups[3].Value), Convert.ToInt32(regex.Groups[4].Value));
            var highest = Range(0, Math.Abs(maxy)).Select(y => HitsYCoordinates(y, miny, maxy)).OrderBy(y => y).Last();
            return GetResult(highest);
        }

        private int HitsYCoordinates(int y, int miny, int maxy)
        {
            var range = Range(miny, maxy).ToArray();
            var height = 0;
            var highest = 0;
            while (height > maxy)
            {
                height += y;
                y -= 1;
                if (height > highest)
                    highest = height;
                if (range.Contains(height))
                    return highest;
            }

            return 0;
        }

        private IEnumerable<int> Range(int min, int max)
        {
            for (var i = min; max >= min ? i <= max : i >= max; i += max >= min ? 1 : -1)
            {
                yield return i;
            }
        }

        public override TaskResult RunPartTwo()
        {
            var input = InitTaskString();
            var regex = Regex.Match(input, "target area: x=([0-9-]+)..([0-9-]+), y=([0-9-]+)..([0-9-]+)");
            var (minx, maxx) = (Convert.ToInt32(regex.Groups[1].Value), Convert.ToInt32(regex.Groups[2].Value));
            var (maxy, miny) = (Convert.ToInt32(regex.Groups[3].Value), Convert.ToInt32(regex.Groups[4].Value));

            var xrange = Range(minx, maxx).ToArray();
            var yrange = Range(miny, maxy).ToArray();
            var velocities = Enumerable.Range(1, maxx)
                .Where(x =>
                Enumerable.Range(1, x).Select(d => Range(x, 0).Take(d).Sum()).Any(d => xrange.Contains(d)))
                .ToList();
            var ys = Range(maxy * 2, Math.Abs(maxy) * 2).ToArray();
            var velos = (from velo in velocities from yvelo in ys select (velo, yvelo)).ToList();

            var doesithit = new Func<int, int, bool>((x, y) =>
            {
                var cx = 0;
                var cy = 0;
                while (cx <= maxx && cy >= maxy)
                {
                    if (xrange.Contains(cx) && yrange.Contains(cy))
                        return true;
                    cx += x;
                    cy += y;
                    if (x > 0)
                        x--;
                    y -= 1;
                }

                return false;
            });

            return GetResult(velos.Count(x => doesithit(x.velo, x.yvelo)));
        }
        
        
    }
}