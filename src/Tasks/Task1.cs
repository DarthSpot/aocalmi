using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace AoC2021.Tasks
{
    public class Task1 : ITask
    {
        public override TaskResult RunPartOne()
        {
            var data = InitTaskLines().Select(x => Convert.ToInt32(x)).ToArray();
            var result = data.Zip(data.Skip(1), (x, y) => y > x).Count(x => x);
            return GetResult(result);
        }

        public override TaskResult RunPartTwo()
        {
            var input = InitTaskLines();
            var data = input.Select(x => Convert.ToInt32(x)).ToArray();
            var triples = new List<List<int>>();
            for (var i = 0; i < data.Length-2; i++)
                triples.Add(new List<int>{data[i], data[i+1], data[i+2]});
            
            var result = triples.Zip(triples.Skip(1), (x, y) => y.Sum() > x.Sum()).Count(x => x);
            return GetResult(result);
        }
    }
}