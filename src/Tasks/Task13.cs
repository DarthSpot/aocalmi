using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2021.Tasks
{
    public class Task13 : ITask
    {
        public override TaskResult RunPartOne()
        {
            var input = InitTaskLines();
            List<(char axis, int num)> folds = input.Select(x => Regex.Match(x, "fold along ([xy])=([0-9]+)"))
                .Where(x => x.Success)
                .Select(x => (x.Groups[1].Value.First(), Convert.ToInt32(x.Groups[2].Value)))
                .ToList();
            List<(int x, int y)> dots = input.Take(input.Length - (folds.Count + 1)).Select(x => x.Split(','))
                .Select(x => (Convert.ToInt32(x[0]), Convert.ToInt32(x[1]))).ToList();
            foreach (var fold in folds.Take(1))
            {
                var dotList = new List<(int x, int y)>();
                foreach (var dot in dots)
                {
                    if ((fold.axis == 'x' ? dot.x : dot.y) > fold.num)
                    {
                        (int x, int y) newDot = fold.axis == 'x' ? (fold.num * 2 - dot.x, dot.y) : (dot.x, fold.num * 2 - dot.y);
                        dotList.Add(newDot);
                    }
                    else
                    {
                        dotList.Add(dot);
                    }
                }
                dots = dotList.Distinct().ToList();
            }
            
            return GetResult(dots.Count);
        }

        private void PrintDots(List<(int x, int y)> dots)
        {
            for (var y = dots.Min(d => d.y); y <= dots.Max(d => d.y); y++)
            {
                for (var x = dots.Min(d => d.x); x <= dots.Max(d => d.x); x++)
                {
                    Console.Write(dots.Any(d => d.x == x && d.y == y) ? '█' : ' ');
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public override TaskResult RunPartTwo()
        {
            var input = InitTaskLines();
            List<(char axis, int num)> folds = input.Select(x => Regex.Match(x, "fold along ([xy])=([0-9]+)"))
                .Where(x => x.Success)
                .Select(x => (x.Groups[1].Value.First(), Convert.ToInt32(x.Groups[2].Value)))
                .ToList();
            List<(int x, int y)> dots = input.Take(input.Length - (folds.Count + 1)).Select(x => x.Split(','))
                .Select(x => (Convert.ToInt32(x[0]), Convert.ToInt32(x[1]))).ToList();
            foreach (var fold in folds)
            {
                var dotList = new List<(int x, int y)>();
                foreach (var dot in dots)
                {
                    if ((fold.axis == 'x' ? dot.x : dot.y) > fold.num)
                    {
                        (int x, int y) newDot = fold.axis == 'x' ? (fold.num * 2 - dot.x, dot.y) : (dot.x, fold.num * 2 - dot.y);
                        dotList.Add(newDot);
                    }
                    else
                    {
                        dotList.Add(dot);
                    }
                }
                dots = dotList.Distinct().ToList();
            }
            
            return GetResult("Read Console Output");
        }
    }
}