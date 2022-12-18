namespace Advent;

public class Seventeen : AdventDay
{
    public int Day => 17;

    public void PartOne(string input)
    {
        Cave.Debug = false;
        var cave = new Cave
        {
            Moves = GetMoves(input),
        };

        cave.Simulate(2022);
    }

    IEnumerable<Move> GetMoves(string input) => input.ToCharArray()
        .Select(c => c switch {
            '<' => Move.Left,
            '>' => Move.Right,
            _ => throw new Exception($"bad move {c}"),
        }).RepeatForever();

    class Cave
    {
        public HashSet<(int x, int y)> Points { get; set; } = new();
        public required IEnumerable<Move> Moves { get; set; }
        public IEnumerable<Rock> Rocks { get; }

        public Cave()
        {
            var rockTemplate = """
            ####

            .#.
            ###
            .#.

            ..#
            ..#
            ###

            #
            #
            #
            #

            ##
            ##
            """;

            var rocks = new List<Rock>();
            foreach (var chunk in rockTemplate.Split("\n\n"))
            {
                var rock = new Rock();
                rocks.Add(rock);
                foreach (var (i, line) in chunk.SplitLines().Reverse().Enumerate())
                {
                    foreach (var (j, c) in line.ToCharArray().Enumerate())
                    {
                        if (c == '#')
                        {
                            rock.Points.Add((3 + j, i));
                        }
                    }
                }
            }
            Rocks = rocks.RepeatForever();

            foreach (var floor in Enumerable.Range(0, 9).Select(x => (x, 0)))
            {
                Points.Add(floor);
            }
        }


        public void Simulate(int until)
        {
            var count = 0;
            var moves = Moves.GetEnumerator();
            foreach (var r in Rocks)
            {
                var rock = r.New(Points.Max(x => x.y) + 4);
                while (moves.MoveNext())
                {
                    Print(rock);
                    var move = moves.Current;
                    var movedByJet = rock.MakeMove(move);
                    var maxX = movedByJet.Points.Max(x => x.x);
                    var minX = movedByJet.Points.Min(x => x.x);
                    if (maxX < 8 && minX > 0 && !Collides(movedByJet))
                    {
                        rock = movedByJet;
                        Print(rock);
                    }

                    var movedDown = rock.MakeMove(Move.Down);
                    if (Collides(movedDown))
                    {
                        // hit something on this move, get next rock
                        foreach (var p in rock.Points)
                        {
                            Points.Add(p);
                        }
                        count += 1;
                        if (until == count)
                        {
                            Print();
                            Console.WriteLine(Points.Max(x => x.y));
                            return;
                        }
                        break;
                    }
                    rock = movedDown;
                }
            }
        }

        bool Collides(Rock r) => r.Points.Any(x => Points.Contains(x));

        public static bool Debug = false;

        public void Print(string s)
        {
            if (!Debug) return;
            Console.WriteLine(s);
        }

        public void Print(Rock? rock = null)
        {
            if (!Debug) return;

            var max = rock?.Points.Max(x => x.y) ?? Points.Max(x => x.y);
            for (var i = max; i >= 0; i--)
            {
                Console.Write("|");
                foreach (var p in Enumerable.Range(1, 7))
                {
                    if (i == 0)
                    {
                        Console.Write("-");
                    }
                    else if (Points.Contains((p, i)))
                    {
                        Console.Write("#");
                    }
                    else if (rock?.Points.Contains((p, i)) ?? false)
                    {
                        Console.Write("@");
                    }
                    else
                    {
                        Console.Write(".");
                    }
                }
                Console.WriteLine("|");
                if (i == 0) Console.WriteLine();
            }
        }
    }

    record Rock
    {
        public List<(int x, int y)> Points { get; set; } = new();

        public Rock MakeMove(Move move)
        {
            (int x, int y) step = move switch {
                Move.Left => (-1, 0),
                Move.Right => (1, 0),
                Move.Down => (0, -1),
                _ => throw new Exception(),
            };
            return new Rock {
                Points = Points.Select(p => (p.x + step.x, p.y + step.y)).ToList(),
            };
        }

        public Rock New(int y)
        {
            return new Rock {
                Points = Points.Select(p => (p.x, p.y + y)).ToList(),
            };
        }
    }

    enum Move
    {
        Left,
        Right,
        Down,
    }
}