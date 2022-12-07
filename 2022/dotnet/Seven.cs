using System.Text.RegularExpressions;

namespace Advent;

public class Seven : AdventDay
{
    public int Day => 7;

    public void PartOne(string input)
    {
        var directories = RunCmds(input);
        var sums = directories
            .Select(x => x.Value.Size())
            .Where(x => x <= 100_000)
            .Sum();
        
        Console.WriteLine(sums);
    }

    public void PartTwo(string input)
    {
        var directories = RunCmds(input);
        var totalUsed = directories.Max(x => x.Value.Size());
        var unused = 70_000_000 - totalUsed;
        var targetDelete = 30_000_000 - unused;
        Console.WriteLine($"total used: {totalUsed} total free: {unused} target delete: {targetDelete}");
        var dirToDelete = directories
            .Where(x => x.Value.Size() >= targetDelete)
            .OrderBy(x => x.Value.Size())
            .First();

        Console.WriteLine($"{dirToDelete.Value.Path()} {dirToDelete.Value.Size()}");
    }

    private Dictionary<string, Dir> RunCmds(string input)
    {
        var root = new Dir { Name = "/" }; 
        var currentDir = root;
        var dirDirectory = new Dictionary<string, Dir>
        {
            {root.Path(), root},
        };

        foreach (var line in input.SplitLines())
        {
            if (Command.TryParse(line, out var cmd))
            {
                if (cmd.Cmd == "cd")
                {
                    if (cmd.Args == "..")
                    {
                        currentDir = currentDir.Parent ?? root;
                    }
                    else if (cmd.Args == "/")
                    {
                        currentDir = root;
                    }
                    else
                    {
                        currentDir = currentDir.ChildDirs.Single(x => x.Name == cmd.Args!);
                    }
                }
            }
            else if (File.TryParse(line, out var file))
            {
                currentDir.Files.Add(file);
            }
            else if (Dir.TryParse(line, currentDir, out var dir))
            {
                dirDirectory[dir.Path()] = dir;
            }
            else
            {
                throw new Exception($"unhandled line `{line}`");
            }
        }

        return dirDirectory;
    }


    class Command
    {
        private static readonly Regex _regex = new Regex(@"^\$ (\w+)\s*(.*)$");
        public string Cmd { get; set; } = "";
        public string? Args { get; set; }

        public static bool TryParse(string input, out Command command)
        {
            command = new();
            var match = _regex.Match(input);
            if (!match.Success) return false;
            command.Cmd = match.Groups[1].Value;
            if (match.Groups.Count == 3)
            {
                command.Args = match.Groups[2].Value;
            }
            return true;
        }
    }

    record File
    {
        private static readonly Regex _regex = new Regex(@"^(\d+) (.+)$");
        public long Size { get; set; }
        public string Name { get; set; } = "";

        public static bool TryParse(string input, out File file)
        {
            file = new File();
            var match = _regex.Match(input);
            if (!match.Success) return false;
            file.Name = match.Groups[2].Value;
            file.Size = long.Parse(match.Groups[1].Value);
            return true;
        }
    }

    class Dir
    {
        private static readonly Regex _regex = new(@"^dir (.+)$");

        public string Name { get; set; } = "";
        public Dir? Parent { get; set; }
        public List<Dir> ChildDirs { get; set; } = new();
        public HashSet<File> Files { get; set; } = new();

        public long Size() => Files.Sum(x => x.Size) + ChildDirs.Sum(x => x.Size());
        public string Path() => $"{Parent?.Path()}/{Name}";

        public static bool TryParse(string input, Dir current, out Dir dir)
        {
            dir = new();
            var match = _regex.Match(input);
            if (!match.Success) return false;
            dir.Name = match.Groups[1].Value;
            dir.Parent = current;
            current.ChildDirs.Add(dir);
            return true;
        }
    }
}