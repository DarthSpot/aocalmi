using System.Text.RegularExpressions;
using AoCCommon;

namespace AoC2022.Tasks;

public class Task4 : ITask
{
    public override TaskResult RunPartOne()
    {
        var input = InitTaskLines();
        var count = 0;
        foreach (var line in input)
        {
            var m = Regex.Match(line, @"(\d+)-(\d+),(\d+)-(\d+)");
            Func<int,int> f = i => int.Parse(m.Groups[i].Value);
            var a = (f(1), f(2));
            var b = (f(3), f(4));

            var rangeA = Enumerable.Range(a.Item1, a.Item2-a.Item1+1).ToList();
            var rangeB = Enumerable.Range(b.Item1, b.Item2-b.Item1+1).ToList();

            if ((rangeA.Contains(b.Item1) && rangeA.Contains(b.Item2)) || (rangeB.Contains(a.Item1) && rangeB.Contains(a.Item2)))
                count++;
        }

        return GetResult(count);
    }

    public override TaskResult RunPartTwo()
    {
        var input = InitTaskLines();
        var count = 0;
        foreach (var line in input)
        {
            var m = Regex.Match(line, @"(\d+)-(\d+),(\d+)-(\d+)");
            Func<int,int> f = i => int.Parse(m.Groups[i].Value);
            var (ax, ay) = (f(1), f(2));
            var (bx, by) = (f(3), f(4));

            var rangeA = Enumerable.Range(ax, ay-ax+1).ToList();
            var rangeB = Enumerable.Range(bx, by-bx+1).ToList();

            if (rangeA.Contains(bx) || rangeA.Contains(by) || rangeB.Contains(ax) || rangeB.Contains(ay))
                count++;
        }

        return GetResult(count);
    }
}