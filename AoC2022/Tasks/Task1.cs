using AoCCommon;

namespace AoC2022.Tasks;

public class Task1 : ITask
{
    public override TaskResult RunPartOne()
    {
        return GetResult(InitTaskString().Split("\n\n")
            .Select(x => x.Trim().Split("\n")
                .Select(int.Parse).ToList()).ToList().Select(x => x.Sum()).Max());
    }

    public override TaskResult RunPartTwo()
    {
        return GetResult(InitTaskString().Split("\n\n")
            .Select(x => x.Trim().Split("\n")
                .Select(int.Parse).ToList()).Select(x => x.Sum()).OrderDescending().Take(3).Sum());
    }
}