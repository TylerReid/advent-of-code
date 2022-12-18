namespace Advent;

public class Eighteen : AdventDay
{
    public int Day => 18;

    public void PartOne(string input)
    {
        var points = Parse(input);
        Console.WriteLine(SurfaceArea(points));
    }

    int SurfaceArea(List<(int x, int y, int z)> points)
    {
        var bag = new List<(int x , int y, int z)>(points);
        var area = points.Count() * 6;
        while (bag.Any())
        {
            var point = bag.First();
            bag.Remove(point);

            foreach (var other in bag)
            {
                if (ShareSide(point, other))
                {
                    area -= 2;
                }
            }
        }
        return area;
    }

    bool ShareSide((int x, int y, int z) a, (int x, int y, int z) b)
    {
        return (Math.Abs(a.x - b.x), Math.Abs(a.y - b.y), Math.Abs(a.z - b.z)) switch {
            (1, 0, 0) => true,
            (0, 1, 0) => true,
            (0, 0, 1) => true,
            _ => false,
        };
    }

    List<(int x, int y, int z)> Parse(string input) => input.SplitLines()
        .Select(s => s.SplitTwice(","))
        .Select(x => x.Convert(p => int.Parse(p)))
        .ToList();
}