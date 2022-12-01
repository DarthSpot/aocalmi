using System;
using System.Collections.Generic;
using System.Linq;
using AoCCommon;

namespace AoC2021.Tasks
{
    public class Task21 : ITask
    {
        public override TaskResult RunPartOne()
        {
            List<(int pos, int score)> input = InitTaskLines().Select(x => (Convert.ToInt32(x.Split(" ").Last())-1, 0)).ToList();
            var rolls = 0;
            var player = 0;
            var cd = 0;
            var dice = Enumerable.Range(1, 100).Select(x => DieRange().Skip((x - 1) * 3).Take(3).ToList())
                .ToList();
            
            while (input.All(x => x.Item2 < 1000))
            {
                var playerInfo = input[player];
                var die = dice[cd%100];
                rolls += 3;
                cd++;
                var newP = (playerInfo.pos + die.Sum()) % 10;
                var score = playerInfo.score + newP + 1;
                input[player] = (newP, score);
                player = 1 - player;
            }

            return GetResult(input.Min(x => x.score) * rolls);
        }

        private IEnumerable<int> DieRange()
        {
            var c = 1;
            while (true)
            {
                yield return c++;
                if (c > 100)
                    c = 1;
            }
        }

        public override TaskResult RunPartTwo()
        {
            List<(int pos, int score)> input = InitTaskLines().Select(x => (Convert.ToInt32(x.Split(" ").Last())-1, 0)).ToList();
            
            var player = 0;
            var map = new Dictionary<(int x, int y, int sx, int sy), long>();
            map.Add((input[0].pos, input[1].pos, 0, 0), 1L);
            var add = new Action<int, int, int, int, long>((a, b, c, d, v) =>
            {
                if (map.ContainsKey((a, b, c, d)))
                    map[(a, b, c, d)] += v;
                else
                    map[(a, b, c, d)] = v;
            });
            var p1Wins = new List<long>();
            var p2Wins = new List<long>();
            while (map.Any(x => x.Value > 0))
            {
                foreach (var item in map.Where(x => x.Value > 0).ToList())
                {
                    for (var i = 1; i <= 3; i++)
                    {
                        for (var j = 1; j <= 3; j++)
                        {
                            for (var k = 1; k <= 3; k++)
                            {
                                var sum = (i + j + k);
                                var p1 = (item.Key.x + sum) % 10;
                                var p2 = (item.Key.y + sum) % 10;
                                var sx = item.Key.sx + p1 + 1;
                                var sy = item.Key.sy + p2 + 1;
                                if (player == 0)
                                    add(p1, item.Key.y, sx, item.Key.sy, item.Value);
                                else
                                    add(item.Key.x, p2, item.Key.sx, sy, item.Value);
                            }
                        }
                    }
                    map[item.Key] -= item.Value;
                    if (map[item.Key] == 0)
                        map.Remove(item.Key);
                }

                foreach (var item in map.Where(x => x.Key.sx >= 21).ToList())
                {
                    p1Wins.Add(item.Value);
                    map.Remove(item.Key);
                }

                foreach (var item in map.Where(x => x.Key.sy >= 21).ToList())
                {
                    p2Wins.Add(item.Value);
                    map.Remove(item.Key);
                }
                player = 1 - player;
            }
            
            return GetResult(p1Wins.Sum() > p2Wins.Sum() ? p1Wins.Sum() : p2Wins.Sum());
        }
    }
}