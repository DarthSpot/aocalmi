using AoCCommon;

namespace AoC2022.Tasks;

public class Task2 : ITask
{
    public override TaskResult RunPartOne()
    {
        var input = InitTaskLines();
        var points = 0;
        foreach (var line in input)
        {
            var l = line[0] - 'A';
            var r = line[2] - 'X';
            if (r == l + 1 || (r == 0 && l == 2))
                points += 6 + r + 1;
            else if (r == l)
                points += 3 + r + 1;
            else
                points += r + 1;
        }

        return GetResult(points);
    }

    public override TaskResult RunPartTwo()
    {
        var input = InitTaskLines();
        var points = 0;
        foreach (var line in input)
        {
            var l = line[0] - 'A';
            switch (line[2])
            {
                case 'X':
                    points += (3 + (l - 1)) % 3 + 1;
                    break;
                case 'Y':
                    points += l + 3 + 1;
                    break;
                case 'Z':
                    points += 6 + (l + 1) % 3 + 1;
                    break;
            }
        }

        return GetResult(points);
    }
}