using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2021.Tasks
{
    public class Task4 : ITask
    {
        public override TaskResult RunTask()
        {
            var input = InitTaskLines();
            var nums = input[0].Split(',').Select(x => Convert.ToInt32(x)).ToArray();
            var boards = new List<int[][]>();
            for (var i = 2; i < input.Length; i += 6)
            {
                boards.Add(Enumerable.Range(i, 5)
                    .Select(x =>
                    {
                        var split = input[x].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        return split.Select(c => Convert.ToInt32(c)).ToArray();
                    }).ToArray());
            }

            var getEntrys = new Func<int[][], List<int[]>>(board =>
            {
                var res = new List<int[]>();
                for (var x = 0; x < 5; x++)
                {
                    res.Add(board[x]);
                    res.Add(Enumerable.Range(0,5).Select(c => board[c][x]).ToArray());
                }
                return res;
            });

            for (var pull = 5; pull < nums.Length; pull++)
            {
                var pullNums = nums.Take(pull).ToArray();
                foreach (var board in boards)
                {
                    var candidates = getEntrys(board);
                    if (candidates.Any(c => c.All(x => pullNums.Contains(x))))
                    {
                        var unused = board.SelectMany(x => x).Where(x => !pullNums.Contains(x)).Sum();
                        return GetResult(unused * pullNums.Last());
                    }
                }
            }

            return GetResult("Error");
        }

        public override TaskResult RunTaskExtended()
        {
            var input = InitTaskLines();
            var nums = input[0].Split(',').Select(x => Convert.ToInt32(x)).ToArray();
            var boards = new List<int[][]>();
            for (var i = 2; i < input.Length; i += 6)
            {
                boards.Add(Enumerable.Range(i, 5)
                    .Select(x =>
                    {
                        var split = input[x].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        return split.Select(c => Convert.ToInt32(c)).ToArray();
                    }).ToArray());
            }

            var getEntrys = new Func<int[][], List<int[]>>(board =>
            {
                var res = new List<int[]>();
                for (var x = 0; x < 5; x++)
                {
                    res.Add(board[x]);
                    res.Add(Enumerable.Range(0,5).Select(c => board[c][x]).ToArray());
                }
                return res;
            });

            var looserBoard = boards.Select(board =>
            {
                var pulls = 5;
                var candidates = getEntrys(board);
                var pullNums = nums.Take(pulls).ToArray();
                while (!candidates.Any(c => c.All(x => pullNums.Contains(x))))
                {
                    pullNums = nums.Take(++pulls).ToArray();
                }

                var unused = board.SelectMany(x => x).Where(x => !pullNums.Contains(x)).Sum();
                var score = unused * pullNums.Last();
                return (pulls, score);
            }).OrderBy(x => x.pulls).Last();
            return GetResult(looserBoard.score);
        }
    }
}