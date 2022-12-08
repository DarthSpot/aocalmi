using AoCCommon;

namespace AoC2022.Tasks;

public class Task8 : ITask
{
    public override TaskResult RunPartOne()
    {
        var rows = InitTaskLines();
        var colList = new List<string>();
        for (var x = 0; x < rows[0].Length; x++)
        {
            colList.Add(rows.Aggregate("", (current, row) => current + row[x]));
        }

        var visible = 0;
        for (var row = 0; row < rows[0].Length; row++)
        {
            for (var col = 0; col < rows.Length; col++)
            {
                var current = rows[col][row];
                var left = string.Concat(rows[col].Take(row));
                var right = string.Concat(rows[col].Skip(row + 1));
                var up = string.Concat(colList[row].Take(col));
                var down = string.Concat(colList[row].Skip(col + 1));
                if (left.All(x => x < current)
                    || right.All(x => x < current)
                    || up.All(x => x < current)
                    || down.All(x => x < current))
                    visible++;
            }
        }
        
        return GetResult(visible);
    }

    public override TaskResult RunPartTwo()
    {
        var rows = InitTaskLines();
        var colList = new List<string>();
        for (var x = 0; x < rows[0].Length; x++)
        {
            colList.Add(rows.Aggregate("", (current, row) => current + row[x]));
        }

        var maxScore = 0;
        for (var row = 0; row < rows[0].Length; row++)
        {
            for (var col = 0; col < rows.Length; col++)
            {
                var current = rows[col][row];
                var left = string.Concat(rows[col].Take(row).Reverse());
                var right = string.Concat(rows[col].Skip(row + 1));
                var up = string.Concat(colList[row].Take(col).Reverse());
                var down = string.Concat(colList[row].Skip(col + 1));
                var dirs = new[] { left, right, up, down };
                var score = dirs.Select(dir =>
                {
                    var hitTree = false;
                    var count = 0;
                    foreach (var tree in dir)
                    {
                        count++;
                        if (tree >= current)
                        {
                            break;
                        }
                    }

                    return count;
                }).Aggregate((x, y) => x * y);
                if (score > maxScore)
                    maxScore = score;
            }
        }
        
        return GetResult(maxScore);
    }
}