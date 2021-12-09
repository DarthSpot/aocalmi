using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AoC2021.Tasks
{
    public class Task8 : ITask
    {
        public override TaskResult RunTask()
        {
            var input = InitTaskLines().Select(x => x.Split('|'))
                .Select(x => x[1].Trim().Split(" "))
                .SelectMany(x => x)
                .Select(x => x.Length).ToArray();
            
            return GetResult(input.Count(x => new[] { 2, 3, 4, 7 }.Contains(x)));
        }

        public override TaskResult RunTaskExtended()
        {
            (string[] input, string[] value)[] input =
                InitTaskLines().Select(x => x.Split('|'))
                    .Select(x => (x[0].Trim().Split(" "), 
                        x[1].Trim().Split(" "))).ToArray();

            return GetResult(input.Select(row =>
            {
                var map = new Dictionary<int, string>();
                var sevenDisplay = new Dictionary<char, char>();
                var inputGrp = row.input.GroupBy(x => x.Length)
                    .ToDictionary(x => x.Key,
                        x => x.Select(x => string.Concat(x.OrderBy(c => c))).ToList());
                map[1] = inputGrp[2].Single();
                map[4] = inputGrp[4].Single();
                map[7] = inputGrp[3].Single();
                map[8] = inputGrp[7].Single();
                sevenDisplay.Add('a', map[7].Except(map[1]).Single());
                var middleTopLeft = map[4].Except(map[1]).ToArray();
                map[5] = inputGrp[5].Single(x => x.Intersect(middleTopLeft).Count() == 2);

                sevenDisplay.Add('g', map[5].Except(map[4]).Except(map[7]).Single());
                map[3] = inputGrp[5].Single(x => x.Except(map[7].Append(sevenDisplay['g'])).Count() == 1);
                sevenDisplay.Add('d', map[3].Except(map[7].Append(sevenDisplay['g'])).Single());
                map[2] = inputGrp[5].Single(x => x.Except(map[3]).Except(map[5]).Count() == 1);
                sevenDisplay.Add('e', map[2].Except(map[3]).Single());
                map[0] = inputGrp[6].Single(x => !x.Intersect(new[] { sevenDisplay['d'] }).Any());
                map[9] = inputGrp[6].Single(x => !x.Intersect(new[] { sevenDisplay['e'] }).Any());
                map[6] = inputGrp[6].Single(x => !map.ContainsValue(x));

                var mapToValue = map.ToDictionary(x => x.Value, x => x.Key);
                return Convert.ToInt64(string.Concat(row.value.Select(x =>
                    (char)('0' + mapToValue[string.Concat(x.OrderBy(c => c))]))));
            }).Sum()+"");
        }
    }
}