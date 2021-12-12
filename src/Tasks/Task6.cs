using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2021.Tasks
{
    public class Task6 : ITask
    {
        public override TaskResult RunPartOne()
        {
            var input = InitTaskString().Split(',')
                .Select(x => Convert.ToInt64(x))
                .ToList();
            
            var swarm = Enumerable.Repeat(0L, 7).ToArray();
            foreach (var fish in input)
                swarm[fish] += 1;
            
            var addBuffer = new Queue<long>(new []{0L, 0L});
            for (var i = 0; i < 80; i++)
            {
                var dayInCicle = i % 7;
                var fishOnDay = swarm[dayInCicle];
                var newFish = addBuffer.Dequeue();
                swarm[dayInCicle] += newFish;
                addBuffer.Enqueue(fishOnDay);
            }
            
            return GetResult((swarm.Sum() + addBuffer.Sum()).ToString());
        }
        
        public override TaskResult RunPartTwo()
        {
            var input = InitTaskString().Split(',')
                .Select(x => Convert.ToInt64(x))
                .ToList();
            var swarm = Enumerable.Repeat(0L, 7).ToArray();
            foreach (var fish in input)
                swarm[fish] += 1;
            
            var addBuffer = new Queue<long>(new []{0L, 0L});
            for (var i = 0; i < 256; i++)
            {
                var dayInCicle = i % 7;
                var fishOnDay = swarm[dayInCicle];
                var newFish = addBuffer.Dequeue();
                swarm[dayInCicle] += newFish;
                addBuffer.Enqueue(fishOnDay);
            }
            
            return GetResult((swarm.Sum() + addBuffer.Sum()).ToString());
        }
    }
}