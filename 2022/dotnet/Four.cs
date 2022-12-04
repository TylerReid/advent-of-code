namespace Advent;

public class Four : AdventDay
{
    public int Day => 4;

    public void PartOne(string input)
    {
        var derp = input.SplitLines()
            .Select(x => x.SplitOnce(","))
            .Select(x => (ExtractRanges(x.Item1), ExtractRanges(x.Item2)))
            .Where(x => x.Item1.IsSubsetOf(x.Item2) || x.Item2.IsSubsetOf(x.Item1))
            .Count();

        Console.WriteLine(derp);
    }

    public void PartTwo(string input)
    {
        var derp = input.SplitLines()
            .Select(x => x.SplitOnce(","))
            .Select(x => (ExtractRanges(x.Item1), ExtractRanges(x.Item2)))
            .Where(x => x.Item1.Intersect(x.Item2).Count() > 0 || x.Item2.Intersect(x.Item1).Count() > 0)
            .Count();

        Console.WriteLine(derp);
    }

    private HashSet<int> ExtractRanges(string s)
    {
        var split = s.SplitOnce("-");
        var (start, end) = (int.Parse(split.Item1), int.Parse(split.Item2));
        return Enumerable.Range(start, end - start + 1).ToHashSet();
    }
}