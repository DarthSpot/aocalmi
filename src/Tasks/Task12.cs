using System;
using System.Collections.Generic;
using System.Linq;
using AoCCommon;

namespace AoC2021.Tasks
{
    public class Task12 : ITask
    {
        public override TaskResult RunPartOne()
        {
            var input = InitTaskLines()
                .Select(x => x.Split("-").ToArray()).ToList();

            var ways = new List<(string l, string r)>();
            foreach (var (l,r) in input.Select(x => (x[0], x[1])))
            {
                ways.Add((l, r));
                ways.Add((r, l));
            }
            var routes = ways
                .Where(x => !string.Equals(x.l, "end") && !string.Equals(x.r, "start"))
                .GroupBy(x => x.l).ToDictionary(x => x.Key, x => x.ToList());

            var start = "start";
            var end = "end";
            
            var routePerm = new List<List<string>>();
            var unfinishedRoutes = new Queue<List<string>>();
            unfinishedRoutes.Enqueue(new List<string> {start});
            while (unfinishedRoutes.Any())
            {
                var route = unfinishedRoutes.Dequeue();
                var current = route.Last();
                
                if (string.Equals(current, end))
                    routePerm.Add(route);
                else
                {
                    var possibleRoutes = routes[current]
                        .Where(x => x.r.All(char.IsUpper) || !route.Contains(x.r))
                        .ToList();
                    if (!possibleRoutes.Any())
                    {
                    
                    }
                    else
                    {
                        foreach (var altRoute in possibleRoutes.Skip(1)
                                     .Select(otherRoute => new List<string>(route.Append(otherRoute.r))))
                            unfinishedRoutes.Enqueue(altRoute);
                        route.Add(possibleRoutes.First().r);
                        unfinishedRoutes.Enqueue(route);
                    }
                }
            }
            
            return GetResult(routePerm.Count);
        }

        public override TaskResult RunPartTwo()
        {
            var input = InitTaskLines()
                .Select(x => x.Split("-").ToArray()).ToList();

            var ways = new List<(string l, string r)>();
            foreach (var (l,r) in input.Select(x => (x[0], x[1])))
            {
                ways.Add((l, r));
                ways.Add((r, l));
            }
            var routes = ways
                .Where(x => !string.Equals(x.l, "end") && !string.Equals(x.r, "start"))
                .GroupBy(x => x.l).ToDictionary(x => x.Key, x => x.ToList());

            var start = "start";
            var end = "end";
            var routePerm = new List<List<string>>();
            var unfinishedRoutes = new Queue<List<string>>();
            unfinishedRoutes.Enqueue(new List<string> {start});

            while (unfinishedRoutes.Any())
            {
                var route = unfinishedRoutes.Dequeue();
                var current = route.Last();
                
                if (string.Equals(current, end))
                    routePerm.Add(route);
                else
                {
                    var hasVistedTwice = route
                        .Where(x => x.All(char.IsLower)).GroupBy(x => x)
                        .Any(x => x.Count() > 1);
                    var possibleRoutes = routes[current]
                        .Where(x => x.r.All(char.IsUpper) || !route.Contains(x.r) || !hasVistedTwice)
                        .ToList();
                    if (possibleRoutes.Any())
                    {
                        foreach (var altRoute in possibleRoutes.Skip(1)
                                     .Select(otherRoute => new List<string>(route.Append(otherRoute.r))))
                            unfinishedRoutes.Enqueue(altRoute);
                        route.Add(possibleRoutes.First().r);
                        unfinishedRoutes.Enqueue(route);
                    }
                }
            }

            return GetResult(routePerm.Count);
        }
    }
}