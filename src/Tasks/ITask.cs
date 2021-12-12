using System.Diagnostics;
using System.IO;

namespace AoC2021.Tasks
{
    public abstract class ITask
    {
        public string InputPath { get; set; }
        public abstract TaskResult RunPartOne();
        public abstract TaskResult RunPartTwo();
        protected Stopwatch Stopwatch { get; } = new Stopwatch();
        protected string[] InitTaskLines()
        {
            Stopwatch.Restart();
            return File.ReadAllLines(InputPath);
        }
        
        protected string InitTaskString()
        {
            Stopwatch.Restart();
            return File.ReadAllText(InputPath);
        }

        protected TaskResult GetResult(int result)
        {
            Stopwatch.Stop();
            return new TaskResult(result.ToString(), Stopwatch.Elapsed);
        }
        
        protected TaskResult GetResult(string result)
        {
            Stopwatch.Stop();
            return new TaskResult(result, Stopwatch.Elapsed);
        }
    }
}