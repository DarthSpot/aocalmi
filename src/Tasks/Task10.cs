using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AoC2021.Tasks
{
    public class Task10 : ITask
    {
        public override TaskResult RunTask()
        {
            var input = InitTaskLines();
            var match = new Dictionary<char, char>()
            {
                { '(', ')' },
                { '[', ']' },
                { '{', '}' },
                { '<', '>' }
            };
            var errors = new List<char>();
            foreach (var line in input)
            {
                var chunkStack = new Stack<char>();
                foreach (var c in line)
                {
                    if (match.ContainsKey(c))
                        chunkStack.Push(c);
                    else
                    {
                        if (chunkStack.Any())
                        {
                            var top = chunkStack.Pop();
                            if (match[top] != c)
                            {
                                errors.Add(c);
                                break;
                            }
                        }
                        else
                        {
                            errors.Add(c);
                            break;
                        }
                    }
                }
            }

            var points = new Dictionary<char, int>()
            {
                { ')', 3},
                { ']', 57},
                { '}', 1197},
                { '>', 25137},
            };

            return GetResult(errors.Select(x => points[x]).Sum());
        }

        public override TaskResult RunTaskExtended()
        {
            var input = InitTaskLines();
            var match = new Dictionary<char, char>()
            {
                { '(', ')' },
                { '[', ']' },
                { '{', '}' },
                { '<', '>' }
            };

            var remainingLines = input.Where(line =>
            {
                var chunkStack = new Stack<char>();
                foreach (var c in line)
                {
                    if (match.ContainsKey(c))
                        chunkStack.Push(c);
                    else
                    {
                        if (chunkStack.Any())
                        {
                            var top = chunkStack.Pop();
                            if (match[top] != c)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                return true;
            }).ToList();

            var points = new Dictionary<char, long>()
            {
                { ')', 1},
                { ']', 2},
                { '}', 3},
                { '>', 4},
            };

            var score = new List<long>();
            foreach (var line in remainingLines)
            {
                var chunkStack = new Stack<char>();
                foreach (var c in line)
                {
                    if (match.ContainsKey(c))
                        chunkStack.Push(c);
                    else
                    {
                        var top = chunkStack.Pop();
                        if (match[top] != c)
                        {
                            throw new Exception();
                        }
                    }
                }
               
                score.Add(chunkStack.Select(x => points[match[x]]).Prepend(0L).Aggregate((x, y) => x * 5L + y));
            }

            return GetResult(score.OrderBy(x => x).ToArray()[score.Count / 2]+"");
        }
    }
}