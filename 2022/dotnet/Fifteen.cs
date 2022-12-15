using System.Diagnostics;

namespace Advent;

public class Fifteen : AdventDay
{
    public int Day => 15;

    int partOneRound = 1;
    public void PartOne(string input)
    {
        var y = 10;
        var xmin = -3;
        var xmax = 30;
        if (partOneRound++ == 2)
        {
            y = 2_000_000;
            xmin = -1_600_000;
            xmax = 10_000_000;
        }
        var sensors = Parse(input);
        var cantBeBeacons = 0;
        for (var x = xmin; x < xmax; x++)
        {
            foreach (var sensor in sensors)
            {
                var advance = sensor.CanAdvanceBy((x, y));
                if (advance.HasValue)
                {
                    x += advance.Value;
                    cantBeBeacons += advance.Value + 1;
                    break;
                }
            }
        }

        Console.WriteLine(cantBeBeacons);
    }

    int partTwoRound = 1;
    public void PartTwo(string input)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var max = 20;
        if (partTwoRound++ == 2)
        {
            max = 4_000_000;
        }
        var sensors = Parse(input);

        for (var y = 0; y <= max; y++)
        for (var x = 0; x <= max; x++)
        {
            var empty = true;
            foreach (var sensor in sensors)
            {
                var advance = sensor.CanAdvanceBy((x, y));
                if (advance.HasValue)
                {
                    x += advance.Value;
                    empty = false;
                    break;
                }
            }
            if (empty)
            {
                stopwatch.Stop();
                Console.WriteLine($"wow! {x}, {y} {x * 4_000_000L + y} in {stopwatch.ElapsedMilliseconds}ms");
                return;
            }
        }
    }

    record Sensor
    {
        public required (int x, int y) Location { get; set; }
        public required (int x, int y) Beacon { get; set; }
        public required int Radius { get; set; }

        public static int Distance((int x, int y) a, (int x, int y) b) => Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);

        public int? CanAdvanceBy((int x, int y) point)
        {
            var distance = Distance(Location, point);
            if (distance <= Radius)
            {
                //Console.WriteLine($"found edge {point.x} from sensor {Location} advancing by {RightEdgeFrom(point.x, point.y)}");
                return RightEdgeFrom(point.x, point.y);
            }
            return null;
        }

        int RightEdgeFrom(int x, int y)
        {
            var radiusAt = Radius - Math.Abs(Location.y - y);
            return radiusAt + Location.x - x;
        }
    }

    List<Sensor> Parse(string input) => new Regex(@"Sensor at x=(?<sx>-*\d+), y=(?<sy>-*\d+): closest beacon is at x=(?<bx>-*\d+), y=(?<by>-*\d+)")
        .Matches(input)
        .Select(x => ((int.Parse(x.Groups["sx"].Value), int.Parse(x.Groups["sy"].Value)),
                    (int.Parse(x.Groups["bx"].Value), int.Parse(x.Groups["by"].Value))))
        .Select(x => new Sensor {
            Location = (x.Item1.Item1, x.Item1.Item2),
            Beacon = (x.Item2.Item1, x.Item2.Item2),
            Radius = Sensor.Distance((x.Item1.Item1, x.Item1.Item2), (x.Item2.Item1, x.Item2.Item2))
        }).ToList();
}