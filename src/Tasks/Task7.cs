using System;
using System.Linq;

namespace AoC2021.Tasks
{
    public class Task7 : ITask
    {
        public override TaskResult RunTask()
        {
            var input = InitTaskString().Split(',').Select(x => Convert.ToInt32(x)).ToList();
            return GetResult(Enumerable.Range(input.Min(), input.Max() - input.Min())
                .Select(x => input.Select(d => Math.Abs(x - d)).Sum()).Min());
        }

        public override TaskResult RunTaskExtended()
        {
            var input = InitTaskString().Split(',').Select(x => Convert.ToInt32(x)).ToList();
            return GetResult(Enumerable.Range(input.Min(), input.Max() - input.Min())
                .Select(x => input.Select(d => (Math.Abs(x - d)+1)*Math.Abs(x - d)/2).Sum()).Min());
        }
    }
}