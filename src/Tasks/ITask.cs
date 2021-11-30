namespace AoC2021.Tasks
{
    public interface ITask
    {
        public string InputPath { get; set; }
        public TaskResult RunTask();
        public TaskResult RunTaskExtended();
    }
}