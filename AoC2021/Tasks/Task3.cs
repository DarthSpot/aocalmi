using System;
using System.Linq;
using AoCCommon;

namespace AoC2021.Tasks
{
    public class Task3 : ITask
    {
        public override TaskResult RunPartOne()
        {
            var input = InitTaskLines();
            var lines = input.Length;
            var res = "";
            for (var i = 0; i < input[0].Length; i++)
            {
                var bits = input.Select(x => x[i]).ToArray();
                var zeros = bits.Count(x => x == '0');
                if (zeros > lines / 2)
                    res += '0';
                else
                    res += '1';
            }

            var epsilon = Convert.ToInt32(res, 2);
            var gamma = Convert.ToInt32(string.Concat(res.Select(x => x == '0' ? '1' : '0')), 2);
            return GetResult(epsilon * gamma);
        }

        public override TaskResult RunPartTwo()
        {
            var input = InitTaskLines();
            var counter = new Func<string[], int, (char lcb,char mcb)>((data, pos) =>
            {
                var grp = data.Select(x => x[pos])
                    .GroupBy(x => x)
                    .ToDictionary(x => x.Key, x => x.Count());
                return grp['0'] == grp['1']
                    ? ('0', '1')
                    : (grp.OrderBy(x => x.Value).First().Key, grp.OrderBy(x => x.Value).Last().Key);
            });

            var oxy = input.ToArray();
            for (var bit = 0; oxy.Length > 1; bit++)
            {
                var pr = counter(oxy, bit).mcb;
                oxy = oxy.Where(x => x[bit] == pr).ToArray();
            }
            
            var co2 = input.ToArray();
            for (var bit = 0; co2.Length > 1; bit++)
            {
                var pr = counter(co2, bit).lcb;
                co2 = co2.Where(x => x[bit] == pr).ToArray();
            }

            return GetResult(Convert.ToInt32(co2.Single(), 2) * Convert.ToInt32(oxy.Single(), 2));
        }
    }
}