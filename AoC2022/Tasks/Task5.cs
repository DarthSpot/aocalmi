using System.Text.RegularExpressions;
using AoCCommon;

namespace AoC2022.Tasks;

public class Task5 : ITask
{
    public override TaskResult RunPartOne()
    {
        var input = InitTaskString();
        var split = input.Split("\n\n");
        var d = new Dictionary<int, Stack<char>>();
        foreach (var line in split[0].Split("\n").Reverse().Skip(1))
        {
            var pointer = 1;
            var i = 1;
            while (pointer < line.Length)
            {
                if (!d.ContainsKey(i))
                    d.Add(i, new Stack<char>());
                var c = line[pointer];
                if (c != ' ')
                    d[i].Push(c);
                i++;
                pointer += 4;
            }
        }

        foreach (var line in split[1].Split("\n"))
        {
            var reg = Regex.Match(line, @"move (\d+) from (\d+) to (\d+)");
            if (reg.Success)
            {
                var count = int.Parse(reg.Groups[1].Value);
                var start = int.Parse(reg.Groups[2].Value);
                var target = int.Parse(reg.Groups[3].Value);
                for (var steps = 0; steps < count; steps++)
                {
                    d[target].Push(d[start].Pop());
                }   
            }
        }
        
        return GetResult(string.Concat(d.Select(x => x.Value.Peek())));
    }

    public override TaskResult RunPartTwo()
    {
        var input = InitTaskString();
        var split = input.Split("\n\n");
        var d = new Dictionary<int, Stack<char>>();
        foreach (var line in split[0].Split("\n").Reverse().Skip(1))
        {
            var pointer = 1;
            var i = 1;
            while (pointer < line.Length)
            {
                if (!d.ContainsKey(i))
                    d.Add(i, new Stack<char>());
                var c = line[pointer];
                if (c != ' ')
                    d[i].Push(c);
                i++;
                pointer += 4;
            }
        }

        foreach (var line in split[1].Split("\n"))
        {
            var reg = Regex.Match(line, @"move (\d+) from (\d+) to (\d+)");
            if (reg.Success)
            {
                var count = int.Parse(reg.Groups[1].Value);
                var start = int.Parse(reg.Groups[2].Value);
                var target = int.Parse(reg.Groups[3].Value);
                var crane = new Stack<char>();
                for (var steps = 0; steps < count; steps++)
                {
                    crane.Push(d[start].Pop());
                }

                while (crane.TryPop(out var crate))
                {
                    d[target].Push(crate);
                }
                    
            }
        }
        
        return GetResult(string.Concat(d.Select(x => x.Value.Peek())));
    }
}