using System.Collections;
using System.Text.RegularExpressions;
using AoCCommon;

namespace AoC2022.Tasks;

public class Task7 : ITask
{
    private AoCDirectory BuildDirectoryTree()
    {
        var input = InitTaskString()
            .Split("$")
            .Select(x => x.Trim().Split("\n"))
            .ToList();
        var root = new AoCDirectory(null, "/");
        AoCDirectory current = null;
        foreach (var line in input.Skip(1))
        {
            var cmd = line[0].Split(" ");
            if (cmd[0] == "cd")
            {
                switch (cmd[1])
                {
                    case "/":
                        current = root;
                        break;
                    case "..":
                        current = current.Parent != null ? current.Parent : current;
                        break;
                    default:
                    {
                        if (current.Directories.ContainsKey(cmd[1]))
                            current = current.Directories[cmd[1]];
                        else
                        {
                            var dir = new AoCDirectory(current, cmd[1]);
                            current.Directories.Add(cmd[1], dir);
                            current = dir;
                        }

                        break;
                    }
                }
            } 
            else if (cmd[0] == "ls")
            {
                foreach (var item in line.Skip(1))
                {
                    var data = item.Split(" ");
                    if (data[0] != "dir")
                    {
                        current.Files[data[1]] = long.Parse(data[0]);
                    }
                }
            }
        }

        return root;
    }
    
    public override TaskResult RunPartOne()
    {
        var root = BuildDirectoryTree();

        var dirs = root.Enumerate().Where(x => x.DirectorySize <= 100000).ToList();

        return GetResult(dirs.Select(x => x.DirectorySize).Sum());
    }

    public override TaskResult RunPartTwo()
    {
        var root = BuildDirectoryTree();
        var remaining = 70000000 - root.DirectorySize;
        var diff = 30000000 - remaining;
        return GetResult(root.Enumerate().Where(x => x.DirectorySize >= diff)
            .Select(x => x.DirectorySize).Min());
    }

    private class AoCDirectory
    {
        public AoCDirectory Parent { get; }
        public string Name { get; }
        public Dictionary<string, long> Files { get; } = new();
        public Dictionary<string, AoCDirectory> Directories { get; } = new();

        public AoCDirectory(AoCDirectory parent, string name)
        {
            Parent = parent;
            Name = name;
        }

        public long DirectorySize => Files.Values.Sum() 
                                     + Directories.Values.Select(x => x.DirectorySize).Sum();

        public IEnumerable<AoCDirectory> Enumerate()
        {
            yield return this;
            foreach (var dir in Directories.SelectMany(item => item.Value.Enumerate()))
            {
                yield return dir;
            }
        }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Parent)}: {Parent}, {nameof(DirectorySize)}: {DirectorySize}";
        }
    }
}