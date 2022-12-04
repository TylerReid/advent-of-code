namespace Advent;

public class Three : AdventDay
{
    public int Day => 3;

    public void PartOne(string input)
    {
        var result =  Parse(input).Select(x => FindCommonItems(x))
            .Select(Score)
            .Sum();
        Console.WriteLine(result);
    }

    public void PartTwo(string input)
    {
        var result = input.Split("\n")
            .Select(x => x.ToCharArray().ToHashSet())
            .Chunk(3)
            .Select(x => x.Aggregate((a, b) => a.Intersect(b).ToHashSet()))
            .Select(x => Score(x.ToList()))
            .Sum();

        Console.WriteLine(result);
    }

    List<(List<char>, List<char>)> Parse(string input)
    {
        var all = new List<(List<char>, List<char>)>();

        foreach (var line in input.Split("\n"))
        {
            var first = new List<char>();
            var second = new List<char>();
            all.Add((first, second));
            var middle = line.Length / 2;
            var chars = line.ToCharArray();
            foreach (var i in Enumerable.Range(0, chars.Length))
            {
                if (i < middle)
                {
                    first.Add(chars[i]);
                }
                else
                {
                    second.Add(chars[i]);
                }
            }
        }

        return all;
    }

    List<char> FindCommonItems((List<char> a, List<char> b) items) => items.a.Intersect(items.b).ToList();

    int Score(List<char> items) => items.Select(x => (int)x).Select(x => x < 91 ? x - 38 : x - 96).Sum();
}