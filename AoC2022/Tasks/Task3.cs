using AoCCommon;

namespace AoC2022.Tasks;

public class Task3 : ITask
{
    public override TaskResult RunPartOne()
    {
        var input = InitTaskLines();
        var sum = input.Aggregate(0, (current, line) => current + line.Substring(0, line.Length / 2)
            .Intersect(line.Substring(line.Length / 2))
            .Select(c => char.IsUpper(c) ? c - 'A' + 27 : c - 'a' + 1)
            .Sum());

        return GetResult(sum);
    }

    public override TaskResult RunPartTwo()
    {
        var input = InitTaskLines();
        var sum = (from @group in input.Select((x, i) => (x, i)).GroupBy(x => x.i / 3)
            select @group.Select(x => x.x).ToList()
            into items
            select items[0].Intersect(items[1]).Intersect(items[2]).Single()
            into badge
            select (char.IsUpper(badge) ? badge - 'A' + 27 : badge - 'a' + 1)).Sum();

        return GetResult(sum);
    }
}