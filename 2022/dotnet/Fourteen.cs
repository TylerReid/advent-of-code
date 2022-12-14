namespace Advent;

public class Fourteen : AdventDay
{
    public int Day => 14;

    public void PartOne(string input)
    {
        var rocks = Parse(input);

        var sandCount = 0;
        while (DropSand(rocks))
        {
            sandCount++;
        }

        Console.WriteLine(sandCount);
    }

    public void PartTwo(string input)
    {
        var rocks = Parse(input);
        var bottom = rocks.Max(x => x.y);
        var sandCount = 0;
        while (DropSandTwo(rocks, bottom))
        {
            sandCount++;
        }

        Console.WriteLine(sandCount);
    }

    bool DropSand(HashSet<(int x, int y)> map)
    {
        (int x, int y) sand = (500, 0);
        var bottom = map.Select(x => x.y).Max();
        while (sand.y <= bottom)
        {
            var down = (sand.x, sand.y + 1);
            if (!map.Contains(down))
            {
                sand = down;
                continue;
            }
            var left = (sand.x - 1, sand.y + 1);
            if (!map.Contains(left))
            {
                sand = left;
                continue;
            }

            var right = (sand.x + 1, sand.y + 1);
            if (!map.Contains(right))
            {
                sand = right;
                continue;
            }
            map.Add(sand);
            return true;
        }
        return false;
    }

    bool DropSandTwo(HashSet<(int x, int y)> map, int bottom)
    {
        (int x, int y) sand = (500, 0);
        while (!map.Contains((500, 0)))
        {
            if (sand.y == bottom + 1)
            {
                map.Add(sand);
                return true;
            }

            var down = (sand.x, sand.y + 1);
            if (!map.Contains(down))
            {
                sand = down;
                continue;
            }
            var left = (sand.x - 1, sand.y + 1);
            if (!map.Contains(left))
            {
                sand = left;
                continue;
            }

            var right = (sand.x + 1, sand.y + 1);
            if (!map.Contains(right))
            {
                sand = right;
                continue;
            }
            map.Add(sand);
            return true;
        }
        return false;
    }

    HashSet<(int x, int y)> Parse(string input)
    {
        var rocks = new HashSet<(int x, int y)>();
        foreach (var line in input.SplitLines())
        {
            (int x, int y)? previous = null;
            foreach (var point in line.Split(" -> "))
            {
                var (a, b) = point.SplitOnce(",")
                    .Convert(x => int.Parse(x));
                if (previous == null)
                {
                    previous = (a, b);
                    continue;
                }
                var points = ConnectPoints(previous.Value, (a, b));
                foreach (var p in points)
                {
                    rocks.Add(p);
                }
                previous = (a, b);
            }
        }
        return rocks;
    }

    HashSet<(int x, int y)> ConnectPoints((int x, int y) a, (int x, int y) b)
    {
        (int x, int y) step = (a.x - b.x, a.y - b.y) switch {
            (> 0, 0) => (-1, 0),
            (< 0, 0) => (1, 0),
            (0, > 0) => (0, -1),
            (0, < 0) => (0, 1),
            _ => throw new Exception($"bad step with points {a} {b}"),
        };

        var points = new HashSet<(int x, int y)> { a };
        var next = a;
        while (next != b)
        {
            next = (next.x + step.x, next.y + step.y);
            points.Add(next);
        }

        return points;
    }
}