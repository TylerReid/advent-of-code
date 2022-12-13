using Newtonsoft.Json;

namespace Advent;

public class Twelve : AdventDay
{
    public int Day => 12;

    public void PartOne(string input)
    {
        var data = Parse(input);
        var start = data.SelectMany(x => x).Single(x => x.IsStart);
        Console.WriteLine(DoDijkstra(data, start));
    }

    public void PartTwo(string input)
    {
        var data = Parse(input);
        void Reset()
        {
            foreach (var n in data!.SelectMany(x => x))
            {
                n.ComeFrom = null;
                n.Distance = long.MaxValue;
            }
        }

        var min = long.MaxValue;
        foreach (var node in data.SelectMany(x => x).Where(x => x.Level == 1))
        {
            Reset();
            node.Distance = 0;
            var distance = DoDijkstra(data, node);
            if (distance < min)
            {
                min = distance;
            }
        }
        Console.WriteLine(min);
    }

    long DoDijkstra(List<List<Node>> data, Node start, bool printDiagram = false)
    {
        var end = data.SelectMany(x => x).Where(x => x.IsEnd).Single();
        foreach (var (i, line) in data.Enumerate())
        {
            foreach (var (j, node) in line.Enumerate())
            {
                node.Paths = PossibleSteps(data, (i, j));
            }
        }

        var nodes = new List<Node> { start };
        var considered = new HashSet<Node>();

        while (nodes.Any())
        {
            var current = nodes.First();

            if (current == end)
            {
                break;
            }

            foreach (var neighbor in current.Paths.Where(x => !considered.Contains(x)))
            {
                if (neighbor.Distance > (current.Distance + 1))
                {
                    checked {
                        neighbor.Distance = current.Distance + 1;
                    }
                    neighbor.ComeFrom = current;
                    nodes.Add(neighbor);
                }
            }

            nodes.Remove(current);
            considered.Add(current);
            nodes = nodes.OrderBy(x => x.Distance).ToList();
        }

        if (printDiagram)
        {
            var path = new List<Node>();
            var n = end;
            while (n?.ComeFrom != null)
            {
                path.Add(n);
                n = n.ComeFrom;
            }

            for (var x = 0; x < data.Count(); x++)
            for (var y = 0; y < data.First().Count(); y++)
            {
                if (start.Position == (x, y))
                {
                    Console.Write('S');
                }
                else if (end.Position == (x, y))
                {
                    Console.Write('E');
                }
                else if (path.Any(derp => derp.Position == (x, y)))
                {
                    Console.Write('#');
                }
                else
                {
                    Console.Write(' ');
                }
                if (y == data.First().Count() - 1) Console.WriteLine();
            }
        }

        return end.Distance;
    }

    class Node
    {
        public bool IsStart { get; set; }
        public bool IsEnd { get; set; }
        public Node? ComeFrom { get; set; }
        [JsonIgnore]
        public List<Node> Paths { get; set; } = new();
        public long Distance { get; set; }
        public int Level { get; set; }
        public (int x, int y) Position { get; set;}
    }

    List<List<Node>> Parse(string input)
    {
        var data = new List<List<Node>>();
        foreach (var (i, line) in input.SplitLines().Enumerate())
        {
            var next = new List<Node>();
            data.Add(next);
            foreach (var (j, c) in line.ToCharArray().Enumerate())
            {
                next.Add(new Node{
                    IsStart = c == 'S',
                    IsEnd = c == 'E',
                    Level = c == 'S' ? (int)'a' - 96
                        : c == 'E' ? (int)'z' - 96 : (int)c - 96,
                    Distance = c == 'S' ? 0 : long.MaxValue,
                    Position = (i, j),
                });
            }
        }
        return data;
    }

    List<Node> PossibleSteps(List<List<Node>> map, (int x, int y) p) => new List<(int x, int y)>() 
        {
            (p.x - 1, p.y),
            (p.x + 1, p.y),
            (p.x, p.y - 1),
            (p.x, p.y + 1),
        }.Where(x => x.x >= 0 && x.x < map.Count() && x.y >= 0 && x.y < map.First().Count())
        .Where(x => map[x.x][x.y].Level <= map[p.x][p.y].Level + 1)
        .Select(x => map[x.x][x.y])
        .ToList();
}