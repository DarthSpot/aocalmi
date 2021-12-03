using System;

namespace AoC2021.Tasks
{
    public class Task2 : ITask 
    {
        public override TaskResult RunTask()
        {
            var input = InitTaskLines();
            var x = 0;
            var y = 0;
            foreach (var line in input)
            {
                var cmd = line.Split(" ")[0];
                var value = Convert.ToInt32(line.Split(" ")[1]);
                switch (cmd)
                {
                    case "forward":
                        x += value;
                        break;
                    case "up":
                        y -= value;
                        break;
                    case "down":
                        y += value;
                        break;
                }
            }

            return GetResult(x * y);
        }

        public override TaskResult RunTaskExtended()
        {
            var input = InitTaskLines();
            var aim = 0;
            var x = 0;
            var y = 0;
            foreach (var line in input)
            {
                var cmd = line.Split(" ")[0];
                var value = Convert.ToInt32(line.Split(" ")[1]);
                switch (cmd)
                {
                    case "forward":
                        x += value;
                        y += aim * value;
                        break;
                    case "up":
                        aim -= value;
                        break;
                    case "down":
                        aim += value;
                        break;
                }
            }

            return GetResult(x * y);
        }
    }
}