using AoCCommon;

namespace AoC2022.Tasks;

public class Task9 : ITask
{
    public override TaskResult RunPartOne()
    {
        var input = InitTaskLines();
        (int x, int y) h = (0, 0);
        (int x, int y) t = (0, 0);
        var tPos = new HashSet<(int, int)>();
        tPos.Add(t);
        foreach (var line in input)
        {
            var cmd = line.Split(" ");
            for (var i = 0; i < int.Parse(cmd[1]); i++)
            {
                switch (cmd[0])
                {
                    case "R":
                        h = (h.x + 1, h.y);
                        break;
                    case "U":
                        h = (h.x, h.y + 1);
                        break;
                    case "L":
                        h = (h.x - 1, h.y);
                        break;
                    case "D":
                        h = (h.x, h.y - 1);
                        break;
                }

                var (dx, dy) = (h.x - t.x, h.y - t.y);
                var (px, py) = (0, 0);
                if (Math.Abs(dx) > 1 || Math.Abs(dy) > 1)
                {
                    px = dx switch
                    {
                        > 1 => 1,
                        < -1 => -1,
                        _ => dx
                    };

                    py = dy switch
                    {
                        > 1 => 1,
                        < -1 => -1,
                        _ => dy
                    };

                    t = (t.x + px, t.y + py);
                    tPos.Add(t);
                }
            }
        }

        return GetResult(tPos.Distinct().Count());
    }

    public override TaskResult RunPartTwo()
    {
        var input = InitTaskLines();
        var rope = new List<List<(int x, int y)>>();
        for (var i = 0; i < 10; i++)
            rope.Add(new List<(int x, int y)>(){(0,0)});
        foreach (var line in input)
        {
            var cmd = line.Split(" ");
            var h = rope[0].Last();
            for (var i = 0; i < int.Parse(cmd[1]); i++)
            {
                switch (cmd[0])
                {
                    case "R":
                        h = (h.x + 1, h.y);
                        break;
                    case "U":
                        h = (h.x, h.y + 1);
                        break;
                    case "L":
                        h = (h.x - 1, h.y);
                        break;
                    case "D":
                        h = (h.x, h.y - 1);
                        break;
                }
                rope[0].Add(h);

                for (var r = 1; r < 10; r++)
                {
                    var p = rope[r - 1].Last();
                    var t = rope[r].Last();
                    var (dx, dy) = (p.x - t.x, p.y - t.y);
                    var (px, py) = (0, 0);
                    if (Math.Abs(dx) > 1 || Math.Abs(dy) > 1)
                    {
                        px = dx switch
                        {
                            > 1 => 1,
                            < -1 => -1,
                            _ => dx
                        };

                        py = dy switch
                        {
                            > 1 => 1,
                            < -1 => -1,
                            _ => dy
                        };

                        rope[r].Add((t.x + px, t.y + py));
                    }
                }
            }
        }

        return GetResult(rope.Last().Distinct().Count());
    }
}