using System.Text.RegularExpressions;

namespace Advent;

public class Nine : AdventDay
{
    public int Day => 9;

    public void PartOne(string input)
    {
        var moves = ParseMoves(input);
        var visited = DoMoves(moves, 2);
        Console.WriteLine(visited.Count());
    }

    public void PartTwo(string input)
    {
        var moves = ParseMoves(input);
        var visited = DoMoves(moves, 10);
        Console.WriteLine(visited.Count());
    }

    HashSet<(int x, int y)> DoMoves(List<(int x, int y)> moves, int length)
    {
        var tailVisited = new HashSet<(int x, int y)>{ (0, 0) };
        var rope = new (int x, int y)[length];

        foreach (var move in moves)
        {
            rope[0] = (rope[0].x + move.x, rope[0].y + move.y);

            for (var i = 1; i < rope.Length; i++)
            {
                //todo remove double call
                while (rope[i] != MoveCloser(rope[i - 1], rope[i]))
                {
                    rope[i] = MoveCloser(rope[i - 1], rope[i]);
                    if (i == rope.Length - 1)
                    {
                        tailVisited.Add(rope[i]);
                    }
                    
                }
            }
        }

        return tailVisited;
    }

    (int x, int y) MoveCloser((int x, int y) head, (int x, int y) tail)
    {
        var oldTail = tail;
        if (head.x > tail.x)
        {
            tail.x++;
        }
        if (head.x < tail.x)
        {
            tail.x--;
        }
        if (head.y > tail.y)
        {
            tail.y++;
        }
        if (head.y < tail.y)
        {
            tail.y--;
        }
        return head == tail ? oldTail : tail;
    }

    private readonly Regex _moveRegex = new Regex(@"([L|R|U|D]) (\d+)");
    List<(int x, int y)> ParseMoves(string input)
    {
        var moves = new List<(int x, int y)>();
        foreach (var line in input.SplitLines())
        {
            var match = _moveRegex.Match(line);
            if (!match.Success) throw new Exception($"weird line! {line}");
            var move = match.Groups[1].Value;
            var amount = int.Parse(match.Groups[2].Value);
            moves.Add(move switch 
            {
                "L" => (-amount, 0),
                "R" => (amount, 0),
                "U" => (0, amount),
                "D" => (0, -amount),
                _ => throw new Exception($"unhandled line {line}"),
            });
        }
        return moves;
    }
}