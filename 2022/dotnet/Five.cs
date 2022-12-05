using System.Text.RegularExpressions;

namespace Advent;

public class Five : AdventDay
{
    public int Day => 5;

    public void PartOne(string input)
    {
        var stacks = ParseStacks(input);
        RunInstructions(stacks, input);
        PrintTop(stacks);
    }

    public void PartTwo(string input)
    {
        var stacks = ParseStacks(input);
        RunInstructionsTwo(stacks, input);
        PrintTop(stacks);
    }

    List<Stack<char>> ParseStacks(string input)
    {
        var data = new List<Stack<char>>();
        var lines = input.SplitLines();
        foreach (var _ in lines.First().Chunk(4))
        {
            data.Add(new Stack<char>());
        }

        foreach (var line in lines.Reverse())
        {
            if (line.StartsWith("move") || line.StartsWith(" 1") || line == "")
            {
                continue;
            }
            var chunks = line.Chunk(4).ToList();
            for (var i = 0; i < chunks.Count; i++)
            {
                if (chunks[i][0] == '[')
                {
                    data[i].Push(chunks[i][1]);
                }
            }
        }

        return data;
    }

    private readonly Regex _moveRegex = new Regex(@"move (\d+) from (\d+) to (\d+)");
    void RunInstructions(List<Stack<char>> stacks, string instructions)
    {
        foreach (var line in instructions.SplitLines().Where(x => x.StartsWith("move")))
        {
            var captures = _moveRegex.Match(line).Groups;
            var numberMoved = int.Parse(captures[1].Value);
            var from = int.Parse(captures[2].Value) - 1;
            var to = int.Parse(captures[3].Value) - 1;
            foreach (var _ in Enumerable.Range(0, numberMoved))
            {
                stacks[to].Push(stacks[from].Pop());
            }
        }
    }

    void RunInstructionsTwo(List<Stack<char>> stacks, string instructions)
    {
        foreach (var line in instructions.SplitLines().Where(x => x.StartsWith("move")))
        {
            var captures = _moveRegex.Match(line).Groups;
            var numberMoved = int.Parse(captures[1].Value);
            var from = int.Parse(captures[2].Value) - 1;
            var to = int.Parse(captures[3].Value) - 1;
            var scratch = new Stack<char>();
            foreach (var _ in Enumerable.Range(0, numberMoved))
            {
                scratch.Push(stacks[from].Pop());
            }
            while (scratch.Any())
            {
                stacks[to].Push(scratch.Pop());
            }
        }
    }

    void PrintTop(List<Stack<char>> stacks)
    {
        foreach (var stack in stacks)
        {
            Console.Write($"{stack.Peek()}");
        }
        Console.WriteLine();
    }
}