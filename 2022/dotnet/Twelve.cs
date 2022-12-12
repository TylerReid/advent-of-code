using Newtonsoft.Json;

namespace Advent;

public class Twelve : AdventDay
{
    public int Day => 12;

    public void PartOne(string input)
    {
        Derp = int.MaxValue;
        var walkedPath = new WalkedPath
        {
            Start = FindMarker(input, 'S'),
            End = FindMarker(input, 'E'),
            Path = new List<(int x, int y)> { FindMarker(input, 'S') },
            Map = Parse(input),
        };

        var paths = GetWalkedPaths(walkedPath);

        var best = paths.OrderBy(x => x.Path.Count())
            .First();

        //Console.WriteLine(JsonConvert.SerializeObject(best, Formatting.Indented));
        Console.WriteLine(best.Path.Count() - 1);
    }

    List<WalkedPath> GetWalkedPaths(WalkedPath start)
    {
        var paths = new List<WalkedPath>();
        foreach ((int x, int y) next in PossibleSteps(start.Start))
        if (start.CanWalk(start.Start, next) && TryWalkPath(start, next, out var p))
        {
            paths.AddRange(p);
        }
        return paths;
    }

    private static int Derp = int.MaxValue;
    bool TryWalkPath(WalkedPath path, (int x, int y) next, out List<WalkedPath> paths)
    {
        if (path.Path.Contains(next) || path.Path.Count > Derp)
        {
            paths = new();
            return false;
        }
        if (next == path.End)
        {
            Derp = Derp > path.Path.Count() ? path.Path.Count() : Derp;
            paths = new List<WalkedPath>
            {
                path.MakeClone().WithStep(next),
            };
            return true;
        }

        var previous = path.Path.Last();
        paths = new();
        foreach ((int x, int y) candidate in PossibleSteps(next))
        {
            if (candidate == previous || !path.CanWalk(next, candidate)) continue;

            var nextPath = path.MakeClone().WithStep(next);
            if (TryWalkPath(nextPath, candidate, out var newPaths))
            {
                paths.AddRange(newPaths);
            }
        }
        return paths.Any();
    }

    List<List<int>> Parse(string input)
    {
        var data = new List<List<int>>();
        foreach (var line in input.SplitLines())
        {
            data.Add(line.ToCharArray()
                .Select(x => (int)x)
                .Select(x => x == 83 ? (int)'a' : x)
                .Select(x => x == 69 ? (int)'z' : x)
                .Select(x => x - 96)
                .ToList()
            );
        }
        return data;
    }

    (int x, int y) FindMarker(string input, char marker)
    {
        foreach (var (i, line) in input.SplitLines().Enumerate())
        {
            foreach (var (j, c) in line.ToCharArray().Enumerate())
            {
                if (c == marker)
                {
                    return (i, j);
                }
            }
        }
        throw new Exception("missing marker");
    }

    List<(int x, int y)> PossibleSteps((int x, int y) p) => new() {
        (p.x - 1, p.y),
        (p.x + 1, p.y),
        (p.x, p.y - 1),
        (p.x, p.y + 1),
    };

    record WalkedPath
    {
        public required (int x, int y) Start { get; set; }
        public required (int x, int y) End { get; set; }
        public required List<(int x, int y)> Path { get; set; }
        public required List<List<int>> Map { get; set; }

        public WalkedPath MakeClone() => this with
        {
            Path = new(Path),
        };

        public WalkedPath WithStep((int x, int y) step)
        {
            Path.Add(step);
            return this;
        }

        public bool IsOnMap((int x, int y) p) => 
            p.x >= 0 && p.x < Map.Count() &&
            p.y >= 0 && p.y < Map.First().Count();

        public bool CanWalk((int x, int y) a, (int x, int y) b)
        {
            if (a == b) return false;
            if (!IsOnMap(b)) return false;
            if (!IsOnMap(a)) return false;

            var currentHeight = Map[a.x][a.y];
            var candidateHeight = Map[b.x][b.y];

            return new[]{currentHeight - 1, currentHeight, currentHeight + 1}.Contains(candidateHeight);
        }
    }
}