using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using AoCCommon;

namespace AoC2021
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Any(x => !Regex.IsMatch(x, "[0-9]+")))
            {
                Console.WriteLine("AoC 2021 Runner!");
                Console.WriteLine("----------------");
                Console.WriteLine("dotnet run -- <TaskNumber (1-24)> ([<TaskNumber (1-24)>])");
            }

            foreach (var arg in args)
            {
                Console.WriteLine(arg);
                try
                {
                    var instance = Activator.CreateInstance("AoC2021", "AoC2021.Tasks.Task" + arg).Unwrap() as ITask;
                    var inputPath = Path.Combine(Path.Combine(Path.Combine(
                        new DirectoryInfo(Assembly.GetExecutingAssembly().Location).Parent.Parent.Parent.Parent.Parent
                            .FullName,
                        "input"),
                        "2021"
                    ), arg);
                    instance.InputPath = inputPath;
                    Console.WriteLine($"[{arg}] - Result (simple): {instance.RunPartOne()}");
                    Console.WriteLine($"[{arg}] - Result (advanced): {instance.RunPartTwo()}");
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}