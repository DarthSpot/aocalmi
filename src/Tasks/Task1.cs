using System;

namespace AoC2021.Tasks
{
    public class Task1 : ITask
    {
        public string InputPath { get; set; }

        public TaskResult RunTask()
        {
            return new TaskResult("hello world", TimeSpan.FromHours(1));
        }

        public TaskResult RunTaskExtended()
        {
            return new TaskResult("hello world", TimeSpan.FromHours(1));
        }
    }
}