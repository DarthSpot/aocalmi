using AoCCommon;

namespace AoC2022.Tasks;

public class Task6 : ITask
{
    public override TaskResult RunPartOne()
    {
        var input = InitTaskString();
        var buffer = new Queue<char>();

        var i = 4;
        foreach (var c in input)
        {
            buffer.Enqueue(c);
            if (buffer.Count == 4)
            {
                if (buffer.Distinct().Count() == 4)
                {
                    return GetResult(i);
                }
                buffer.Dequeue();
            }
            i++;
        }

        return GetResult("");
    }

    public override TaskResult RunPartTwo()
    {
        var input = InitTaskString();
        var buffer = new Queue<char>();

        var i = 1;
        foreach (var c in input)
        {
            buffer.Enqueue(c);
            if (buffer.Count == 14)
            {
                if (buffer.Distinct().Count() == 14)
                {
                    return GetResult(i);
                }
                buffer.Dequeue();
            }
            i++;
        }

        return GetResult("");
    }
}