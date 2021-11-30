using System;

namespace AoC2021.Tasks
{
    public record TaskResult
    {
        public TaskResult(string Result, TimeSpan Runtime)
        {
            this.Result = Result;
            this.Runtime = Runtime;
        }

        public string Result { get; init; }
        public TimeSpan Runtime { get; init; }

        public void Deconstruct(out string Result, out TimeSpan Runtime)
        {
            Result = this.Result;
            Runtime = this.Runtime;
        }

        public override string ToString()
        {
            return $"{nameof(Result)}: {Result}, {nameof(Runtime)}: {Runtime}";
        }
    }
}